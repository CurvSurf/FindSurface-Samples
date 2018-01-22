using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

namespace FindSurfaceRevitPlugin
{
	/// <summary>
	/// A wrapper of DirectShape of which shape is Torus
	/// </summary>
	public class DirectTorus : DirectShapeBase
	{
		public XYZ Center { get; private set; }
		public XYZ Axis { get; private set; }
		public double MeanRadius { get; private set; }
		public double TubeRadius { get; private set; }
		public bool HasAnElbow { get; private set; }
		public XYZ TubeBegin { get; private set; }
		public double TubeAngle { get; private set; }
		public double SurfaceArea { get { return 2.0*Math.PI*TubeRadius*(HasAnElbow ? TubeAngle : 2.0*Math.PI)*MeanRadius; } }
		public double Volume { get { return Math.PI*TubeRadius*TubeRadius*(HasAnElbow ? TubeAngle : 2.0*Math.PI)*MeanRadius; } }

		/// <summary>
		/// creates a DirectShape instance of which shape is Torus.
		/// Torus is defined by center, axis, tube radius, and mean radius (the distance between center and tube center).
		/// </summary>
		/// <param name="document">The Revit document where the instance to be drawn</param>
		/// <param name="name">The name of this instance</param>
		/// <param name="center">Position of center of the torus' hole</param>
		/// <param name="axis">Vector passing through the center</param>
		/// <param name="mean_radius">The distance between the center and tube center</param>
		/// <param name="tube_radius">Radius of tube</param>
		/// <param name="line_color">Outline color</param>
		/// <param name="surface_transparency">Surface transparency; ranged from 0 (transparent) to 100 (opaque)</param>
		public DirectTorus( Document document, string name, XYZ center, XYZ axis, double mean_radius, double tube_radius, Color line_color, int surface_transparency ) : base( document, name )
		{
			m_shape_type=ShapeTypes.Torus;
			Center=center;
			Axis=axis;
			MeanRadius=mean_radius;
			TubeRadius=tube_radius;
			HasAnElbow=false;
			TubeBegin=new XYZ();
			TubeAngle=0.0;

			XYZ axis_norm = axis.Normalize();
			XYZ minor_center = ((XYZ.BasisX.CrossProduct(axis_norm).GetLength()<Double.Epsilon) ? XYZ.BasisY : XYZ.BasisX).CrossProduct(axis_norm);
			minor_center=center+minor_center.Normalize()*mean_radius;

			XYZ basis_z = axis.Normalize();
			XYZ basis_x = (minor_center - center).Normalize();
			XYZ basis_y = basis_z.CrossProduct(basis_x).Normalize();

			Frame frame = new Frame(center, basis_x, basis_y, basis_z);

			// model space coordinates
			XYZ near = minor_center - tube_radius * basis_x;
			XYZ far = minor_center + tube_radius * basis_x;
			XYZ back = minor_center + tube_radius * basis_z;
			XYZ front = minor_center - tube_radius * basis_z;

			List<Curve> profile = new List<Curve>();
			profile.Add( Arc.Create( near, far, front ) );
			profile.Add( Arc.Create( far, near, back ) );

			CurveLoop curve_loop = CurveLoop.Create(profile);
			SolidOptions options = new SolidOptions(ElementId.InvalidElementId, ElementId.InvalidElementId);

			if( Frame.CanDefineRevitGeometry( frame )==true )
			{
				Solid torus = GeometryCreationUtilities.CreateRevolvedGeometry(frame, new CurveLoop[] { curve_loop }, 0, 2 * Math.PI, options);
				using( Transaction t = new Transaction( document, "Create torus direct shape" ) )
				{
					t.Start();
					DirectShape shape=DirectShape.CreateElement( document, new ElementId( BuiltInCategory.OST_GenericModel ) );
					shape.SetShape( new GeometryObject[] { torus } );
					shape.SetName( name );
					document.ActiveView.SetElementOverrides( shape.Id, new OverrideGraphicSettings().SetProjectionLineColor( line_color ).SetSurfaceTransparency( surface_transparency ) );
					t.Commit();
				}
			}
		}

		/// <summary>
		/// creates a DirectShape instance of which shape is a part of a torus (like elbow joint pipe).
		/// Torus is defined by center, axis, tube radius, and mean radius (the distance between center and tube center).
		/// The tube_begin and tube_end defines the angle between the two edges of the piece.
		/// </summary>
		/// <param name="document">The Revit document where the instance to be drawn</param>
		/// <param name="name">The name of this instance</param>
		/// <param name="center">Position of center of the torus' hole</param>
		/// <param name="axis">Vector passing through the center</param>
		/// <param name="mean_radius">The distance between torus center and its tube center</param>
		/// <param name="tube_radius">Radius of tube</param>
		/// <param name="tube_begin">The vector pointing to one of the torus' edge from its center</param>
		/// <param name="torus_angle">The angle between the tube begin and end</param>
		/// <param name="line_color">Outline color of the torus</param>
		/// <param name="surface_transparency">Surface transparency; ranged from 0 (transparent) to 100 (opaque)</param>
		public DirectTorus( Document document, string name, XYZ center, XYZ axis, double mean_radius, double tube_radius, XYZ tube_begin, double torus_angle, Color line_color, int surface_transparency ) : base( document, name )
		{
			m_shape_type=ShapeTypes.Torus;
			Center=center;
			Axis=axis;
			MeanRadius=mean_radius;
			TubeRadius=tube_radius;
			HasAnElbow=true;
			TubeBegin=tube_begin;
			TubeAngle=torus_angle;

			XYZ tilting_axis=XYZ.BasisZ.CrossProduct( axis );
			double tilting_angle=FindSurfaceRevitPluginUtils.GetPositiveAngleBetween( XYZ.BasisZ, axis, tilting_axis );

			bool no_need_to_tilt=tilting_axis.IsAlmostEqualTo(XYZ.Zero);
			Transform tilting_torus=no_need_to_tilt ? Transform.Identity : Transform.CreateRotation( tilting_axis, tilting_angle );
			XYZ tilted_basis_x = tilting_torus.OfVector(XYZ.BasisX);
			
			// model space coordinates
			Frame frame=new Frame(XYZ.Zero, XYZ.BasisX, XYZ.BasisY, XYZ.BasisZ );

			XYZ model_tube_center=XYZ.BasisX*mean_radius;
			XYZ model_tube_top=model_tube_center+tube_radius*XYZ.BasisZ;
			XYZ model_tube_bottom=model_tube_center-tube_radius*XYZ.BasisZ;
			XYZ model_tube_outer=model_tube_center+tube_radius*XYZ.BasisX;
			XYZ model_tube_inner=model_tube_center-tube_radius*XYZ.BasisX;

			List<Curve> tube_circle=new List<Curve>();
			tube_circle.Add( Arc.Create( model_tube_top, model_tube_bottom, model_tube_inner ) );
			tube_circle.Add( Arc.Create( model_tube_bottom, model_tube_top, model_tube_outer ) );

			CurveLoop curve_loop=CurveLoop.Create( tube_circle );
			SolidOptions options=new SolidOptions( ElementId.InvalidElementId, ElementId.InvalidElementId );
			
			if( Frame.CanDefineRevitGeometry( frame ) )
			{
				Solid torus=GeometryCreationUtilities.CreateRevolvedGeometry( frame, new CurveLoop[] { curve_loop }, 0, torus_angle, options );
				using( Transaction t = new Transaction( document, "Create torus direct shape." ) )
				{
					t.Start();
					DirectShape shape=DirectShape.CreateElement( document, new ElementId( BuiltInCategory.OST_GenericModel ) );
					shape.SetShape( new GeometryObject[] { torus } );
					shape.SetName( name );
					m_element_id=shape.Id;

					if( no_need_to_tilt==false ) shape.Location.Rotate( Line.CreateUnbound( XYZ.Zero, tilting_axis ), tilting_angle );
					shape.Location.Rotate( Line.CreateUnbound( XYZ.Zero, axis ), FindSurfaceRevitPluginUtils.GetPositiveAngleBetween( tilted_basis_x, tube_begin, axis ) );
					shape.Location.Move( center );

					document.ActiveView.SetElementOverrides( shape.Id, new OverrideGraphicSettings().SetProjectionLineColor( line_color ).SetSurfaceTransparency( surface_transparency ) );
					t.Commit();
				}
			}
		}
	}
}
