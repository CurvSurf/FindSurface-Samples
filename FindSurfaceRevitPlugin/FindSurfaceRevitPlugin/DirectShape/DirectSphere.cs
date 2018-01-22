using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

namespace FindSurfaceRevitPlugin
{
	/// <summary>
	/// A wrapper of DirectShape of which shape is Sphere.
	/// </summary>
	public class DirectSphere : DirectShapeBase
	{
		public XYZ Center { get; private set; }
		public double Radius { get; private set; }
		public double SurfaceArea { get { return Math.PI*4.0*Radius*Radius; } }
		public double Volume { get { return Math.PI*4.0*Radius*Radius*Radius/3; } }

		/// <summary>
		/// creates a DirectShape instance of which shape is Sphere.
		/// Sphere is defined by its center and radius.
		/// </summary>
		/// <param name="document">The Revit document where the instance to be drawn</param>
		/// <param name="name">The name of this instance</param>
		/// <param name="center">Position of the center</param>
		/// <param name="radius">Radius of the circle</param>
		/// <param name="line_color">Outline color of Circle</param>
		/// <param name="surface_transparency">Surface transparency; ranged from 0 (transparent) to 100 (opaque)</param>
		public DirectSphere( Document document, string name, XYZ center, double radius, Color line_color, int surface_transparency ) : base( document, name )
		{
			m_shape_type=ShapeTypes.Sphere;
			Center=center;
			Radius=radius;

			XYZ top = center + radius * XYZ.BasisZ;
			XYZ bottom = center - radius * XYZ.BasisZ;
			XYZ right = center + radius * XYZ.BasisX;

			Frame frame = new Frame(center, XYZ.BasisX, XYZ.BasisY, XYZ.BasisZ);

			List<Curve> profile = new List<Curve>();
			profile.Add( Line.CreateBound( top, bottom ) );
			profile.Add( Arc.Create( bottom, top, right ) );

			CurveLoop curve_loop = CurveLoop.Create(profile);
			SolidOptions options = new SolidOptions(ElementId.InvalidElementId, ElementId.InvalidElementId);
			
			if( Frame.CanDefineRevitGeometry( frame )==true )
			{
				Solid sphere = GeometryCreationUtilities.CreateRevolvedGeometry(frame, new CurveLoop[] { curve_loop }, 0, 2*Math.PI, options);
				using( Transaction t = new Transaction( document, "Create sphere direct shape." ) )
				{
					t.Start();
					DirectShape shape=DirectShape.CreateElement( document, new ElementId( BuiltInCategory.OST_GenericModel ) );
					shape.SetShape( new GeometryObject[] { sphere } );
					shape.SetName( name );
					m_element_id=shape.Id;
					document.ActiveView.SetElementOverrides( shape.Id, new OverrideGraphicSettings().SetProjectionLineColor( line_color ).SetSurfaceTransparency( surface_transparency ) );
					t.Commit();
				}
			}
		}
	}
}
