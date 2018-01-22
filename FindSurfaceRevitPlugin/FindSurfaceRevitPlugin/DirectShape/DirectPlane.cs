using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

namespace FindSurfaceRevitPlugin
{
	/// <summary>
	/// A wrapper of DirectShape of which shape is Plane.
	/// </summary>
	public class DirectPlane : DirectShapeBase
	{
		public XYZ TopLeft { get; private set; }
		public XYZ TopRight { get; private set; }
		public XYZ BottomLeft { get; private set; }
		public XYZ BottomRight { get; private set; }
		public XYZ Normal { get { return (TopLeft-TopRight).CrossProduct( BottomLeft-BottomRight ).Normalize(); } }
		public double Area
		{
			get
			{
				double w0=(BottomRight-BottomLeft).GetLength();
				double h0=(TopLeft-BottomLeft).GetLength();
				double w1=(TopRight-TopLeft).GetLength();
				double h1=(TopRight-BottomRight).GetLength();
				double d=(TopRight-BottomLeft).GetLength();
				return CalculateArea( w0, h1, d )+CalculateArea( w1, h0, d );
			}
		}
		/* reference: https://en.wikipedia.org/wiki/Heron%27s_formula */
		private double CalculateArea(double a, double b, double c)
		{
			return Math.Sqrt( (a+b+c)*(-a+b+c)*(a-b+c)*(a+b-c) )*0.25;
		}
		/// <summary>
		/// calculates the transformed location of four vertices (<code>top_left</code>, <code>top_right</code>, <code>bottom_left</code>, <code>bottom_right</code>) defining a plane to be axis-aligned.
		/// </summary>
		/// <param name="top_left"></param>
		/// <param name="top_right"></param>
		/// <param name="bottom_left"></param>
		/// <param name="bottom_right"></param>
		/// <param name="axis_aligned_top_left"></param>
		/// <param name="axis_aligned_top_right"></param>
		/// <param name="axis_aligned_bottom_left"></param>
		/// <param name="axis_aligned_bottom_right"></param>
		/// <param name="rotation_axis"></param>
		/// <param name="rotation_angle"></param>
		/// <param name="translation_offset"></param>
		protected static void GetAxisAlignedPlane( 
			XYZ top_left, XYZ top_right, XYZ bottom_left, XYZ bottom_right, 
			out XYZ axis_aligned_top_left, out XYZ axis_aligned_top_right, out XYZ axis_aligned_bottom_left, out XYZ axis_aligned_bottom_right, 
			out XYZ rotation_axis, out double rotation_angle, out XYZ translation_offset )
		{
			translation_offset=-(top_left+top_right+bottom_left+bottom_right)/4;
			Transform translate = Transform.CreateTranslation(translation_offset);

			XYZ u = (bottom_right - bottom_left).Normalize();
			XYZ v = (top_left - bottom_left).Normalize();
			XYZ normal = u.CrossProduct(v);

			rotation_angle=normal.AngleTo( XYZ.BasisZ );
			rotation_axis=normal.CrossProduct( XYZ.BasisZ );
			Transform rotate = Transform.CreateRotation(rotation_axis, rotation_angle);

			axis_aligned_top_left=rotate.OfPoint( translate.OfPoint( top_left ) );
			axis_aligned_top_right=rotate.OfPoint( translate.OfPoint( top_right ) );
			axis_aligned_bottom_left=rotate.OfPoint( translate.OfPoint( bottom_left ) );
			axis_aligned_bottom_right=rotate.OfPoint( translate.OfPoint( bottom_right ) );
		}

		/// <summary>
		/// creates a DirectShape instance of which shape is Plane.
		/// Plane is defined by four vertices (top-left, top-right, bottom-left, bottom-right) on the plane.
		/// (Actually, it is a rectangle as you already know.)
		/// </summary>
		/// <remarks>The Plane has very small thickness (0.0039) since there is no way to create a plane with no thickness using DirectShape.</remarks>
		/// <param name="document">The Revit document where the instance to be drawn</param>
		/// <param name="name">The name of this instance</param>
		/// <param name="top_left">Position of the top-left vertex</param>
		/// <param name="top_right">Position of the top-right vertex</param>
		/// <param name="bottom_left">Position of the bottom-left vertex</param>
		/// <param name="bottom_right">Position of the bottom-right vertex</param>
		/// <param name="line_color">Outline color of Plane</param>
		/// <param name="surface_transparency">Surface transparency; ranged from 0 (transparent) to 100 (opaque)</param>
		public DirectPlane( Document document, string name, XYZ top_left, XYZ top_right, XYZ bottom_left, XYZ bottom_right, Color line_color, int surface_transparency ) : base( document, name )
		{
			m_shape_type=ShapeTypes.Plane;
			TopLeft=top_left;
			TopRight=top_right;
			BottomLeft=bottom_left;
			BottomRight=bottom_right;

			XYZ rotation_axis, translation_offset;
			double rotation_angle;
			XYZ tl, tr;
			XYZ bl, br;

			// We'll rotates and translates the plane transformed to be axis-aligned, because GeometryCreationUtilities.CreateSweptGeometry may fail to define a plane due to the precision issue.
			GetAxisAlignedPlane(
				top_left, top_right, bottom_left, bottom_right,
				out tl, out tr, out bl, out br,
				out rotation_axis, out rotation_angle, out translation_offset );

			Frame frame = new Frame(XYZ.Zero, XYZ.BasisX, XYZ.BasisY, XYZ.BasisZ);

			List<Curve> profile = new List<Curve>();
			profile.Add( Line.CreateBound( tl, bl ) );
			profile.Add( Line.CreateBound( bl, br ) );
			profile.Add( Line.CreateBound( br, tr ) );
			profile.Add( Line.CreateBound( tr, tl ) );

			List<Curve> swept_profile = new List<Curve>();
			swept_profile.Add( Line.CreateBound( XYZ.Zero, 0.0039*XYZ.BasisZ ) );

			CurveLoop curve_loop = CurveLoop.Create(profile);
			CurveLoop sweep_path = CurveLoop.Create(swept_profile);
			SolidOptions options = new SolidOptions(ElementId.InvalidElementId, ElementId.InvalidElementId);

			if( Frame.CanDefineRevitGeometry( frame )==true )
			{
				Solid thin_box = GeometryCreationUtilities.CreateSweptGeometry(sweep_path, 0, 0, new CurveLoop[] { curve_loop }, options);
				using( Transaction t = new Transaction( document, "Create plane direct shape" ) )
				{
					t.Start();
					DirectShape shape=DirectShape.CreateElement( document, new ElementId( BuiltInCategory.OST_GenericModel ) );
					shape.SetShape( new GeometryObject[] { thin_box } );
					shape.SetName( name );
					m_element_id=shape.Id;
					shape.Location.Rotate( Line.CreateUnbound( XYZ.Zero, rotation_axis ), -rotation_angle );
					shape.Location.Move( -translation_offset );
					document.ActiveView.SetElementOverrides( shape.Id, new OverrideGraphicSettings().SetProjectionLineColor( line_color ).SetSurfaceTransparency( surface_transparency ) );
					t.Commit();
				}
			}
		}
	}
}
