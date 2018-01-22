using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

namespace FindSurfaceRevitPlugin
{
	/// <summary>
	/// A wrapper of DirectShape of which shape is Cylinder.
	/// </summary>
	public class DirectCylinder : DirectShapeBase
	{
		public XYZ Top { get; private set; }
		public XYZ Bottom { get; private set; }
		public XYZ Center { get { return (Top+Bottom)*0.5; } }
		public double Radius { get; private set; }
		public double Length { get { return (Top-Bottom).GetLength(); } }
		public double SurfaceArea { get { return 2.0*Math.PI*Radius*(Radius+Length); } }
		public double LateralArea { get { return 2.0*Math.PI*Radius*Length; } }
		public double Volume { get { return Math.PI*Radius*Radius*Length; } }

		/// <summary>
		/// creates a DirectShape instance of which shape is Cylinder.
		/// Cylinder is defined by two circles (top, bottom) with the same radius.
		/// </summary>
		/// <param name="document">The Revit document where the instance to be drawn</param>
		/// <param name="name">The name of this instance</param>
		/// <param name="doc">The Revit document where the instance to be drawn</param>
		/// <param name="top">Position of center of the top circle</param>
		/// <param name="bottom">Position of center of the bottom circle</param>
		/// <param name="radius">Radius of the circles</param>
		/// <param name="line_color">Outline color of Cylinder</param>
		/// <param name="surface_transparency">Surface transparency; ranged from 0 (transparent) to 100 (opaque)</param>
		public DirectCylinder( Document document, string name, XYZ top, XYZ bottom, double radius, Color line_color, int surface_transparency ) : base( document, name )
		{
			m_shape_type=ShapeTypes.Cylinder;
			Top=top;
			Bottom=bottom;
			Radius=radius;

			// defines a reference frame of which origin is at the center of the cylinder and z-axis is passing through the centers of its top and bottom.
			XYZ center = (top + bottom) / 2;
			XYZ basis_z = (top - bottom).Normalize();
			XYZ basis_x = XYZ.BasisY.CrossProduct(basis_z).Normalize();
			XYZ basis_y = basis_z.CrossProduct(basis_x).Normalize();

			Frame frame = new Frame(center, basis_x, basis_y, basis_z);

			XYZ bottom_left = bottom - radius * basis_x;
			XYZ bottom_right = bottom + radius * basis_x;
			XYZ bottom_front = bottom - radius * basis_y;
			XYZ bottom_back = bottom + radius * basis_y;

			// creates a profile that is a cross section of a circle (the cylinder will be made by sweeping through the z-axis).
			List<Curve> profile = new List<Curve>();
			profile.Add( Arc.Create( bottom_left, bottom_right, bottom_back ) );
			profile.Add( Arc.Create( bottom_right, bottom_left, bottom_front ) );

			List<Curve> swept_profile = new List<Curve>();
			swept_profile.Add( Line.CreateBound( bottom, top ) );

			CurveLoop curve_loop = CurveLoop.Create(profile);
			CurveLoop sweep_path = CurveLoop.Create(swept_profile);
			SolidOptions options = new SolidOptions(ElementId.InvalidElementId, ElementId.InvalidElementId);

			if( Frame.CanDefineRevitGeometry( frame )==true )
			{
				Solid cylinder = GeometryCreationUtilities.CreateSweptGeometry(sweep_path, 0, 0, new CurveLoop[] { curve_loop }, options);
				using( Transaction t = new Transaction( document, "Create cylinder direct shape" ) )
				{
					t.Start();
					DirectShape shape=DirectShape.CreateElement( document, new ElementId( BuiltInCategory.OST_GenericModel ) );
					shape.SetShape( new GeometryObject[] { cylinder } );
					shape.SetName( name );
					m_element_id=shape.Id;
					document.ActiveView.SetElementOverrides( shape.Id, new OverrideGraphicSettings().SetProjectionLineColor( line_color ).SetSurfaceTransparency( surface_transparency ) );
					t.Commit();
				}
			}
		}
	}
}
