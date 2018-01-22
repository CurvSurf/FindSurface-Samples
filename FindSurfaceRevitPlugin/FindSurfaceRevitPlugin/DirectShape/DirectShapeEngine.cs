using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

namespace FindSurfaceRevitPlugin
{
	public class DirectShapeEngine
	{
		#region Variables
		private List<DirectShapeBase> m_direct_shape_list;
		
		private int m_direct_plane_instance_count;
		private int m_direct_sphere_instance_count;
		private int m_direct_cylinder_instance_count;
		private int m_direct_cone_instance_count;
		private int m_direct_torus_instance_count;
		#endregion

		#region Constants
		private static readonly Color S_PLANE_COLOR = new Color( 230, 25, 75 );
		private static readonly Color S_SPHERE_COLOR = new Color( 60, 180, 75 );
		private static readonly Color S_CYLINDER_COLOR = new Color( 0, 130, 200 );
		private static readonly Color S_CONE_COLOR = new Color( 245, 130, 48 );
		private static readonly Color S_TORUS_COLOR = new Color( 240, 50, 230 );
		#endregion

		#region Properties
		/// <summary>
		/// The number of DirectShape instances.
		/// </summary>
		public int Count { get { return m_direct_shape_list.Count; } }

		/// <summary>
		/// The number of the DirectPlane instances that have been created.
		/// </summary>
		public int DirectPlaneInstanceCount { get { return m_direct_plane_instance_count; } }

		/// <summary>
		/// The number of the DirectSphere instances that have been created.
		/// </summary>
		public int DirectSphereInstanceCount { get { return m_direct_sphere_instance_count; } }

		/// <summary>
		/// The number of the DirectCylinder instances that have been created.
		/// </summary>
		public int DirectCylinderInstanceCount { get { return m_direct_cylinder_instance_count; } }

		/// <summary>
		/// The number of the DirectCone instances that have been created.
		/// </summary>
		public int DirectConeInstanceCount { get { return m_direct_cone_instance_count; } }

		/// <summary>
		/// The number of the DirectTorus instances that have been created.
		/// </summary>
		public int DirectTorusInstanceCount { get { return m_direct_torus_instance_count; } }
		#endregion

		#region Methods
		/// <summary>
		/// The constructor of DirectShapeEngine.
		/// </summary>
		public DirectShapeEngine()
		{
			m_direct_shape_list=new List<DirectShapeBase>();

			m_direct_plane_instance_count=0;
			m_direct_sphere_instance_count=0;
			m_direct_cylinder_instance_count=0;
			m_direct_cone_instance_count=0;
			m_direct_torus_instance_count=0;
		}

		/// <summary>
		/// Creates and draws a plane that is defined with the given parameters.
		/// </summary>
		/// <param name="document">The active document</param>
		/// <param name="top_left">The top-left vertex coordinates of the plane</param>
		/// <param name="top_right">The top-right vertex coordinates of the plane</param>
		/// <param name="bottom_left">The bottom-left vertex coordinates of the plane</param>
		/// <param name="bottom_right">The bottom-right vertex coordinates of the plane</param>
		public void DrawPlane( Document document, XYZ top_left, XYZ top_right, XYZ bottom_left, XYZ bottom_right )
		{
			string plane_name="Plane"+m_direct_plane_instance_count++;
			DirectPlane plane=new DirectPlane( document, plane_name, top_left, top_right, bottom_left, bottom_right, S_PLANE_COLOR, 50 );
			m_direct_shape_list.Add( plane );
		}

		/// <summary>
		/// Creates and draws a sphere that is defined with the given parameters.
		/// </summary>
		/// <param name="document">The active document</param>
		/// <param name="center">The center coordinates of the sphere</param>
		/// <param name="radius">The radius of the sphere</param>
		public void DrawSphere( Document document, XYZ center, double radius )
		{
			string sphere_name="Sphere"+m_direct_sphere_instance_count++;
			DirectSphere sphere=new DirectSphere( document, sphere_name, center, radius, S_SPHERE_COLOR, 50 );
			m_direct_shape_list.Add( sphere );
		}

		/// <summary>
		/// Creates and draws a cylinder that is defined with the given parameters.
		/// </summary>
		/// <param name="document">The active document</param>
		/// <param name="top">The top vertex coordinates of the cylinder</param>
		/// <param name="bottom">The bottom vertex coordinates of the cylinder</param>
		/// <param name="radius">The radius of the cylinder</param>
		public void DrawCylinder( Document document, XYZ top, XYZ bottom, double radius )
		{
			string cylinder_name="Cylinder"+m_direct_cylinder_instance_count++;
			DirectCylinder cylinder=new DirectCylinder( document, cylinder_name, top, bottom, radius, S_CYLINDER_COLOR, 50 );
			m_direct_shape_list.Add( cylinder );
		}

		/// <summary>
		/// Creates and draws a cone that is defined with the given parameters.
		/// </summary>
		/// <param name="document">The active document</param>
		/// <param name="top">The top vertex coordinates of the cone</param>
		/// <param name="bottom">The bottom vertex coordinates of the cone</param>
		/// <param name="top_radius">The radius of the cone at the top</param>
		/// <param name="bottom_radius">The radius of the cone at the bottom</param>
		public void DrawCone( Document document, XYZ top, XYZ bottom, double top_radius, double bottom_radius )
		{
			string cone_name="Cone"+m_direct_cone_instance_count++;
			DirectCone cone=new DirectCone( document, cone_name, top, bottom, top_radius, bottom_radius, S_CONE_COLOR, 50 );
			m_direct_shape_list.Add( cone );
		}

		/// <summary>
		/// Creates and draws a torus that is defined with the given parameters.
		/// </summary>
		/// <param name="document">The active document</param>
		/// <param name="center">The center coordinates of the torus</param>
		/// <param name="axis">The axis vector of the torus</param>
		/// <param name="mean_radius">The mean radius of the torus</param>
		/// <param name="tube_radius">The tube radius of the torus</param>
		/// <param name="tube_begin">The vector pointing from the center to where the tube begins</param>
		/// <param name="torus_angle">The angle between the tube_begin and another edge of the torus</param>
		public void DrawTorus( Document document, XYZ center, XYZ axis, double mean_radius, double tube_radius, XYZ tube_begin, double torus_angle )
		{
			string torus_name="Torus"+m_direct_torus_instance_count++;
			DirectTorus torus=new DirectTorus( document, torus_name, center, axis, mean_radius, tube_radius, tube_begin, torus_angle, S_TORUS_COLOR, 50 );
			m_direct_shape_list.Add( torus );
		}

		/// <summary>
		/// Returns the names of the shapes that have been created.
		/// </summary>
		/// <returns>The array of the names</returns>
		public string[] GetDirectShapeNames() => m_direct_shape_list.ConvertAll( x => x.Name ).ToArray();

		/// <summary>
		/// Determines whether the engine contains the shape
		/// of which name is identical to the <code>name</code>.
		/// </summary>
		/// <param name="name">The name of the shape</param>
		/// <returns>true if the engine contains, or false otherwise</returns>
		public bool ContainDirectShape( string name ) => m_direct_shape_list.Any( x => x.Name==name );

		/// <summary>
		/// Determines whether the engine contains the shape
		/// of which element id is identical to the <code>element_id</code>.
		/// </summary>
		/// <param name="element_id">The element id of the shape</param>
		/// <returns>true if the engine contains, or false otherwise</returns>
		public bool ContainDirectShape( ElementId element_id ) => m_direct_shape_list.Any( x => x.ElementId==element_id );

		/// <summary>
		/// Hides the shape.
		/// </summary>
		/// <param name="name">The name of the shape</param>
		public void HideDirectShape( string name )
		{
			if( m_direct_shape_list.Any( x => x.Name==name )==false ) return;

			DirectShapeBase ds=m_direct_shape_list.Find( x=>x.Name==name );
			ds.Hide();
		}

		/// <summary>
		/// Unhides the shape.
		/// </summary>
		/// <param name="name">The name of the shape</param>
		public void UnhideDirectShape( string name )
		{
			if( m_direct_shape_list.Any( x => x.Name==name )==false ) return;

			DirectShapeBase ds=m_direct_shape_list.Find( x=>x.Name==name);
			ds.Unhide();
		}

		/// <summary>
		/// Removes the shape.
		/// </summary>
		/// <param name="name">The name of the shape</param>
		public void RemoveDirectShape( string name )
		{
			if( m_direct_shape_list.Any( x => x.Name==name )==false ) return;

			DirectShapeBase ds=m_direct_shape_list.Find( x=>x.Name==name );
			m_direct_shape_list.Remove( ds );
			ds.Delete();
		}

		/// <summary>
		/// Removes the shape.
		/// </summary>
		/// <param name="element_id">The element id of the shape</param>
		public void RemoveDirectShape( ElementId element_id )
		{
			if( m_direct_shape_list.Any( x => x.ElementId==element_id )==false ) return;

			DirectShapeBase ds=m_direct_shape_list.Find( x=>x.ElementId==element_id );
			m_direct_shape_list.Remove( ds );
			ds.Delete();
		}

		/// <summary>
		/// Removes all the shapes that are created by this engine.
		/// </summary>
		public void RemoveAllDirectShapes()
		{
			foreach( string name in GetDirectShapeNames() )
			{
				RemoveDirectShape( name );
			}
		}

		public DirectShapeBase GetShape(ElementId element_id)
		{
			if( m_direct_shape_list.Any( x => x.ElementId==element_id )==false ) return null;

			return m_direct_shape_list.Find( x => x.ElementId==element_id );
		}
		#endregion
	}
}
