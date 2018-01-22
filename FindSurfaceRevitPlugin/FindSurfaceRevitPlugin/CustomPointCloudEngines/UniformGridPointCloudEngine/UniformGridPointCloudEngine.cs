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
	/// PointCloudEngine of UniformGridPointCloud
	/// </summary>
	public class UniformGridPointCloudEngine : PointCloudEngineBase
	{
		#region Variables
		private string m_outlier_identifier=string.Empty;
		#endregion

		#region Temporary Variables
		/// <summary>
		/// This temporary variable is used to pass the parameter to CreatePointCloudAccess(),
		/// because there is no way to pass the value using an argument of the method.
		/// </summary>
		private int t_subdivision=0;
		#endregion
		
		#region Methods
		/// <summary>
		/// The constructor of UniformGridPointCloudEngine
		/// </summary>
		/// <param name="engine_identifier">The engine name</param>
		public UniformGridPointCloudEngine( string engine_identifier ) : base( engine_identifier ) { }

		/// <summary>
		/// Creates a point cloud.
		/// </summary>
		/// <remarks>The old one will be removed.</remarks>
		/// <param name="document">The active document</param>
		/// <param name="identifier">The name of the point cloud</param>
		/// <param name="points">The points</param>
		/// <param name="transform">The transform</param>
		/// <param name="subdivision">The subdivision number</param>
		public void CreatePointCloud( Document document, string identifier, CloudPoint[] points, Transform transform, int subdivision )
		{
			t_subdivision=subdivision;

			if( m_pointclouds.Any() ) RemovePointCloud();
			CreatePointCloud( document, identifier, points, transform );
			m_outlier_identifier=identifier;
		}

		/// <summary>
		/// Lists all names of the point clouds.
		/// </summary>
		/// <returns>The array of the names</returns>
		public new string[] GetPointCloudNames() => new string[] { m_outlier_identifier };

		/// <summary>
		/// Determines whether the engine contains the point cloud 
		/// of which name is identical to the <code>identifier</code>.
		/// </summary>
		/// <param name="identifier">The name of the point cloud</param>
		/// <returns>true if the engine contains, false otherwise</returns>
		public new bool ContainPointCloud( string identifier ) => m_outlier_identifier==identifier;

		/// <summary>
		/// Determines whether the engine contains the point cloud 
		/// of which element id is identicial to the <code>element_id</code>.
		/// </summary>
		/// <param name="element_id">The element id of the point cloud</param>
		/// <returns>true if the engine contains, false otherwise</returns>
		public new bool ContainPointCloud( ElementId element_id ) => base.ContainPointCloud( element_id );

		/// <summary>
		/// Returns the transform of the point cloud.
		/// </summary>
		/// <returns>The transform</returns>
		public Transform GetPointCloudTransform() => GetPointCloudTransform( m_outlier_identifier );

		/// <summary>
		/// Returns the cloud points of the point cloud.
		/// </summary>
		/// <returns>The CloudPoint array of the points</returns>
		public CloudPoint[] GetCloudPoint() => base.GetCloudPoints( m_outlier_identifier );

		/// <summary>
		/// Returns the outline of the point cloud.
		/// </summary>
		/// <returns>The outline</returns>
		public Outline GetPointCloudOutline() => base.GetPointCloudOutline( m_outlier_identifier );

		/// <summary>
		/// Hides the point cloud.
		/// </summary>
		public void HidePointCloud() => HidePointCloud( m_outlier_identifier );

		/// <summary>
		/// Unhide the point cloud.
		/// </summary>
		public void UnhidePointCloud() => UnhidePointCloud( m_outlier_identifier );

		/// <summary>
		/// Removes the point cloud.
		/// </summary>
		public void RemovePointCloud()
		{
			RemovePointCloud( m_outlier_identifier );
			m_outlier_identifier=null;
		}

		/// <summary>
		/// Performs ray casting to the point cloud with a given ray.
		/// The ray casting sweeps the points and filter out the points that are farther than the ray radius.
		/// Among the rest of the points, the one that is nearest to the ray origin will be picked.
		/// </summary>
		/// <param name="ray_origin">The origin of the ray</param>
		/// <param name="ray_direction">The direction of the ray</param>
		/// <param name="ray_radius">The radius of ray sweeping</param>
		/// <param name="idx">The index of the point that is picked</param>
		/// <returns>true if there is a point that is picked, false otherwise</returns>
		public bool Cast( XYZ ray_origin, XYZ ray_direction, float ray_radius, out uint idx ) => (m_pointclouds[0] as UniformGridPointCloud).Cast( ray_origin, ray_direction, ray_radius, out idx );
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

			UniformGridPointCloud pc=new UniformGridPointCloud( identifier, t_pending_points, t_subdivision );
			m_pointclouds.Add( pc );
			t_pending_points=null;
			t_subdivision=0;

			return pc;
		}
		public override void Free() { }
		#endregion
	}
}
