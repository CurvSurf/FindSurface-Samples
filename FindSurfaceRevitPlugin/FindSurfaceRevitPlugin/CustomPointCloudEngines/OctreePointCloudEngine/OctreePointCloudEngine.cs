using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.PointClouds;

namespace FindSurfaceRevitPlugin
{
	/// <summary>
	/// PointCloudEngine of OctreePointCloud
	/// </summary>
	public class OctreePointCloudEngine : PointCloudEngineBase
	{
		#region Methods
		/// <summary>
		/// The constructor of OctreePointCloudEngine
		/// </summary>
		/// <param name="engine_identifier">The engine name</param>
		public OctreePointCloudEngine( string engine_identifier ) : base( engine_identifier ) { }

		/// <summary>
		/// Creates a point cloud.
		/// </summary>
		/// <param name="document">The active document</param>
		/// <param name="identifier">The name of the point cloud</param>
		/// <param name="points">The points</param>
		/// <param name="transform">The transform</param>
		public new void CreatePointCloud( Document document, string identifier, CloudPoint[] points, Transform transform ) => base.CreatePointCloud( document, identifier, points, transform );

		/// <summary>
		/// Lists all names of the point clouds.
		/// </summary>
		/// <returns>The array of the names</returns>
		public new string[] GetPointCloudNames() => base.GetPointCloudNames();

		/// <summary>
		/// Determines whether the engine contains the point cloud 
		/// of which name is identical to the <code>identifier</code>.
		/// </summary>
		/// <param name="identifier">The name of the point cloud</param>
		/// <returns>true if the engine contains, or false otherwise</returns>
		public new bool ContainPointCloud( string identifier ) => base.ContainPointCloud( identifier );

		/// <summary>
		/// Determines whether the engine contains the point cloud 
		/// of which element id is identicial to the <code>element_id</code>.
		/// </summary>
		/// <param name="element_id">The element id of the point cloud</param>
		/// <returns>true if the engine contains, or false otherwise</returns>
		public new bool ContainPointCloud( ElementId element_id ) => base.ContainPointCloud( element_id );

		/// <summary>
		/// Returns the transform of the point cloud.
		/// </summary>
		/// <param name="identifier">The name of the point cloud</param>
		/// <returns>The transform</returns>
		public new Transform GetPointCloudTransform( string identifier ) => base.GetPointCloudTransform( identifier );

		/// <summary>
		/// Returns the cloud points of the point cloud.
		/// </summary>
		/// <param name="identifier">The name of the point cloud</param>
		/// <returns>The CloudPoint array of the points</returns>
		public new CloudPoint[] GetCloudPoints( string identifier ) => base.GetCloudPoints( identifier );

		/// <summary>
		/// Returns the outline of the point cloud.
		/// </summary>
		/// <param name="identifier">The name of the point cloud</param>
		/// <returns>The outline</returns>
		public new Outline GetPointCloudOutline( string identifier ) => base.GetPointCloudOutline( identifier );

		/// <summary>
		/// Hides the point cloud.
		/// </summary>
		/// <param name="identifier">The name of the point cloud</param>
		public new void HidePointCloud( string identifier ) => base.HidePointCloud( identifier );

		/// <summary>
		/// Unhides the point cloud.
		/// </summary>
		/// <param name="identifier">The name of the point cloud</param>
		public new void UnhidePointCloud( string identifier ) => base.UnhidePointCloud( identifier );

		/// <summary>
		/// Removes the point cloud.
		/// </summary>
		/// <param name="identifier">The name of the point cloud</param>
		public new void RemovePointCloud( string identifier ) => base.RemovePointCloud( identifier );

		/// <summary>
		/// Removes the point cloud.
		/// </summary>
		/// <param name="element_id">The element id of the point cloud.</param>
		public new void RemovePointCloud( ElementId element_id ) => base.RemovePointCloud( element_id );

		/// <summary>
		/// Removes all the point clouds that are created by this engine.
		/// </summary>
		public void RemoveAllPointClouds()
		{
			foreach( string identifier in GetPointCloudNames() )
			{
				RemovePointCloud( identifier );
			}
		}
		#endregion
		
		#region IPointCloudEngine Methods
		/// <summary>
		/// Creates the point cloud access of PointCloud instance.
		/// The access is directly used by Revit to read points in it.
		/// </summary>
		/// <param name="identifier">The name of the PointCloud instance</param>
		/// <returns>The point cloud access that is created</returns>
		public override IPointCloudAccess CreatePointCloudAccess( string identifier )
		{
			if( m_pointclouds.Any( x => x.GetName()==identifier ) ) return m_pointclouds.Find( x => x.GetName()==identifier );

			OctreePointCloud pc=new OctreePointCloud( identifier, t_pending_points );
			m_pointclouds.Add( pc );
			t_pending_points=null;
			
			return pc;
		}
		public override void Free() { }
		#endregion
	}
}
