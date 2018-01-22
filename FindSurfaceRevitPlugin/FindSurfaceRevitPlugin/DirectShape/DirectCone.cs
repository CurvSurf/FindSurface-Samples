using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

namespace FindSurfaceRevitPlugin
{
	/// <summary>
	/// A wrapper of DirectShape of which shape is Cone.
	/// </summary>
	public class DirectCone : DirectShapeBase
	{
		public XYZ Top { get; private set; }
		public XYZ Bottom { get; private set; }
		public XYZ Center { get { return (Top+Bottom)*0.5; } }
		public double Length { get { return (Top-Bottom).GetLength(); } }
		public double TopRadius { get; private set; }
		public double BottomRadius { get; private set; }
		public double Aperture { get { return Math.Atan2( Length, BottomRadius-TopRadius )*2.0; } }
		public double LateralArea { get { return Math.PI*(TopRadius+BottomRadius)*Math.Sqrt( (BottomRadius-TopRadius)*(BottomRadius-TopRadius)+Length*Length ); } }
		public double SurfaceArea { get { return LateralArea+Math.PI*(BottomRadius*BottomRadius+TopRadius*TopRadius); } }
		public double Volume { get { return Math.PI*Length*(BottomRadius*BottomRadius+BottomRadius*TopRadius+TopRadius*TopRadius)/3; } }
		
		/// <summary>
		/// creates a DirectShape instance of which shape is Cone.
		/// Cone is defined by two circles (top, bottom) with different radii.
		/// </summary>
		/// <param name="document">The Revit document where the instance to be drawn</param>
		/// <param name="name">The name of this instance</param>
		/// <param name="top">Position of center of the top circle</param>
		/// <param name="bottom">Position of center of the bottom circle</param>
		/// <param name="top_radius">Radius of the top center</param>
		/// <param name="bottom_radius">Radius of the bottom center</param>
		/// <param name="line_color">Outline color of Cone</param>
		/// <param name="surface_transparency">Surface transparency; ranged from 0 (transparent) to 100 (opaque)</param>
		public DirectCone( Document document, string name, XYZ top, XYZ bottom, double top_radius, double bottom_radius, Color line_color, int surface_transparency ) : base( document, name )
		{
			m_shape_type=ShapeTypes.Cone;
			Top=top;
			Bottom=bottom;
			TopRadius=top_radius;
			BottomRadius=bottom_radius;

			// defines a reference frame of which origin is at the center of the cone and z-axis is passing through the centers of its top and bottom.
			XYZ center = (top + bottom) / 2;
			XYZ basis_z = (top - bottom).Normalize();
			XYZ basis_x = XYZ.BasisY.CrossProduct(basis_z).Normalize();
			XYZ basis_y = basis_z.CrossProduct(basis_x).Normalize();

			Frame frame = new Frame(center, basis_x, basis_y, basis_z);

			XYZ bottom_right = bottom + bottom_radius * basis_x;
			XYZ top_right = top + top_radius * basis_x;

			// creates a profile that is a cross section of a half of cone (the cone will be made by revolving on the z-axis).
			List<Curve> profile = new List<Curve>();
			profile.Add( Line.CreateBound( top, bottom ) );
			profile.Add( Line.CreateBound( bottom, bottom_right ) );
			profile.Add( Line.CreateBound( bottom_right, top_right ) );
			profile.Add( Line.CreateBound( top_right, top ) );

			CurveLoop curve_loop = CurveLoop.Create(profile);
			SolidOptions options = new SolidOptions(ElementId.InvalidElementId, ElementId.InvalidElementId);

			if( Frame.CanDefineRevitGeometry( frame )==true )
			{
				Solid cone = GeometryCreationUtilities.CreateRevolvedGeometry(frame, new CurveLoop[] { curve_loop }, 0, 2 * Math.PI, options);
				using( Transaction t = new Transaction( document, "Create cone direct shape" ) )
				{
					t.Start();
					DirectShape shape=DirectShape.CreateElement( document, new ElementId( BuiltInCategory.OST_GenericModel ) );
					shape.SetShape( new GeometryObject[] { cone } );
					shape.SetName( name );
					m_element_id=shape.Id;
					document.ActiveView.SetElementOverrides( shape.Id, new OverrideGraphicSettings().SetProjectionLineColor( line_color ).SetSurfaceTransparency( surface_transparency ) );
					t.Commit();
				}
			}
		}
	}
}
