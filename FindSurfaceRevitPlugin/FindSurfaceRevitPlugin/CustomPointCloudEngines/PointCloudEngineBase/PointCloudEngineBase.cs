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
	/// A base of PointCloudEngine class.
	/// </summary>
	public class PointCloudEngineBase : IPointCloudEngine
	{
		#region Variables
		protected string m_identifier=string.Empty;
		protected List<PointCloudBase> m_pointclouds=null;
		#endregion

		#region Temporary Variables
		protected CloudPoint[] t_pending_points=null;
		#endregion

		#region Properties
		/// <summary>
		/// The engine identifier.
		/// </summary>
		public string Identifier { get { return m_identifier; } }

		/// <summary>
		/// The number of point clouds in the engine.
		/// </summary>
		public int Count { get { return m_pointclouds.Count; } }
		#endregion

		#region Implementation
		/// <summary>
		/// The constructor of PointCloudEngineBase.
		/// </summary>
		/// <param name="engine_identifier">The engine identifier</param>
		protected PointCloudEngineBase( string engine_identifier )
		{
			m_identifier=engine_identifier;
			m_pointclouds=new List<PointCloudBase>();
		}

		/// <summary>
		/// Creates a point cloud.
		/// </summary>
		/// <param name="document">The active document</param>
		/// <param name="identifier">The name of the point cloud</param>
		/// <param name="points">The points</param>
		/// <param name="transform">The transform</param>
		protected void CreatePointCloud( Document document, string identifier, CloudPoint[] points, Transform transform )
		{
			t_pending_points=points;

			using( Transaction t = new Transaction( document, identifier ) )
			{
				t.Start();
				PointCloudType type=PointCloudType.Create( document, m_identifier, identifier );
				PointCloudInstance instance=PointCloudInstance.Create( document, type.Id, transform );
				PointCloudBase new_pc=m_pointclouds.Last();
				new_pc.ElementId=instance.Id;
				new_pc.Document=document;
				new_pc.Engine=this;
				t.Commit();
			}
		}

		/// <summary>
		/// Lists all names of the point clouds.
		/// </summary>
		/// <returns>The array of the names</returns>
		protected string[] GetPointCloudNames() => m_pointclouds.ConvertAll( x => x.GetName() ).ToArray();

		/// <summary>
		/// Determines whether the engine contains the point cloud 
		/// of which name is identical to the <code>identifier</code>.
		/// </summary>
		/// <param name="identifier">The name of the point cloud</param>
		/// <returns>true if the engine contains, or false otherwise</returns>
		protected bool ContainPointCloud( string identifier ) => m_pointclouds.Any( x => x.GetName()==identifier );

		/// <summary>
		/// Determines whether the engine contains the point cloud 
		/// of which element id is identicial to the <code>element_id</code>.
		/// </summary>
		/// <param name="element_id"></param>
		/// <returns>true if the engine contains, or false otherwise</returns>
		protected bool ContainPointCloud( ElementId element_id ) => m_pointclouds.Any( x => x.ElementId==element_id );

		/// <summary>
		/// Returns the transform of the point cloud.
		/// </summary>
		/// <param name="identifier">The name of the point cloud</param>
		/// <returns>The transform</returns>
		protected Transform GetPointCloudTransform( string identifier )
		{
			if( m_pointclouds.Any( x => x.GetName()==identifier )==false ) return Transform.Identity;

			PointCloudBase pc=m_pointclouds.Find( x=>x.GetName()==identifier );
			return pc.Transform;
		}

		/// <summary>
		/// Returns the cloud points of the point cloud.
		/// </summary>
		/// <param name="identifier">The name of the point cloud</param>
		/// <returns>The CloudPoint array of the points</returns>
		protected CloudPoint[] GetCloudPoints( string identifier )
		{
			if( m_pointclouds.Any( x => x.GetName()==identifier )==false ) return null;

			PointCloudBase pc=m_pointclouds.Find( x=>x.GetName()==identifier );
			return pc.Points;
		}

		/// <summary>
		/// Returns the outline of the point cloud.
		/// </summary>
		/// <param name="identifier">The name of the point cloud</param>
		/// <returns>The outline</returns>
		protected Outline GetPointCloudOutline( string identifier )
		{
			if( m_pointclouds.Any( x => x.GetName()==identifier )==false ) return null;

			PointCloudBase pc=m_pointclouds.Find( x=>x.GetName()==identifier );
			return pc.Outline;
		}

		/// <summary>
		/// Hides the point cloud.
		/// </summary>
		/// <param name="identifier">The name of the point cloud</param>
		protected void HidePointCloud( string identifier )
		{
			if( m_pointclouds.Any( x => x.GetName()==identifier )==false ) return;

			PointCloudBase pc=m_pointclouds.Find( x=>x.GetName()==identifier );
			pc.Hide();
		}

		/// <summary>
		/// Unhides the point cloud.
		/// </summary>
		/// <param name="identifier">The name of the point cloud</param>
		protected void UnhidePointCloud( string identifier )
		{
			if( m_pointclouds.Any( x => x.GetName()==identifier )==false ) return;

			PointCloudBase pc=m_pointclouds.Find( x=>x.GetName()==identifier );
			pc.Unhide();
		}

		/// <summary>
		/// Removes the point cloud.
		/// </summary>
		/// <param name="identifier">The name of the point cloud</param>
		protected void RemovePointCloud( string identifier )
		{
			if( m_pointclouds.Any( x => x.GetName()==identifier )==false ) return;

			PointCloudBase pc=m_pointclouds.Find( x => x.GetName()==identifier );
			m_pointclouds.Remove( pc );
			pc.Delete();
		}

		/// <summary>
		/// Removes the point cloud.
		/// </summary>
		/// <param name="element_id">The element id of the point cloud</param>
		protected void RemovePointCloud( ElementId element_id )
		{
			if( m_pointclouds.Any( x => x.ElementId==element_id )==false ) return;

			PointCloudBase pc=m_pointclouds.Find(x=>x.ElementId==element_id );
			m_pointclouds.Remove( pc );
			pc.Delete();
		}
		#endregion
		
		#region IPointCloudEngine Methods
		/// <summary>
		/// Creates the point cloud access of PointCloud instance.
		/// The access is directly used by Revit to read points in it.
		/// </summary>
		/// <param name="identifier">The name of the PointCloud instance</param>
		/// <returns>The point cloud access that is created</returns>
		public virtual IPointCloudAccess CreatePointCloudAccess( string identifier ) { throw new NotImplementedException( "cannot directly call the method of base class." ); }
		public virtual void Free() { throw new NotImplementedException( "cannot directly call the method of base class." ); }
		#endregion
	}
}
