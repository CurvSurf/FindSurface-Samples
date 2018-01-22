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
	/// A PointCloud wrapper using an oct-tree to contain cloud points.
	/// </summary>
	public class OctreePointCloud : PointCloudBase
	{
		#region Variables
		private OctreeNode m_root=null;
		#endregion

		#region Properties
		/// <summary>
		/// The number of points contained in the instance.
		/// </summary>
		public override int Count { get { return (m_root==null) ? 0 : m_root.Count; } }
		#endregion

		#region Methods
		/// <summary>
		/// The constructor of OctreePointCloud.
		/// </summary>
		/// <param name="identifier">The name of the point cloud</param>
		/// <param name="points">CloudPoint array of the points</param>
		public OctreePointCloud( string identifier, CloudPoint[] points ) : base( identifier, points )
		{
			Setup();
		}
		#endregion

		#region Implementation
		/// <summary>
		/// Creates the octree root.
		/// </summary>
		private void Setup()
		{
			List<int> indices=new List<int>();
			for( int k = 0;k<m_points.Length;k++ ) indices.Add( k );

			// the calculation below is made by approximately optimizing the value using multiple trials.
			// there is no obvious reason to decide to use this way.
			int depth_limit=(int)Math.Floor( Math.Log( Math.Sqrt( 3.0 )*Math.Pow( m_points.Length, 1.0/3.0 )/Math.Log( 2.0 ) ) );

			XYZ lower_left, upper_right;
			CalculateBoundingBox( m_points, out lower_left, out upper_right );
			m_outline=new Outline( lower_left, upper_right );
			m_root=new OctreeNode( lower_left, upper_right, m_points, indices, depth_limit, -1 );
		}

		/// <summary>
		/// Reads points from the root.
		/// </summary>
		/// <param name="filter">The filter that determines which point is to be read</param>
		/// <param name="buffer">The buffer to contain the points</param>
		/// <param name="start_index">The index of points to start fetching</param>
		/// <returns>The number of points that are fetched</returns>
		protected override int ReadPoints( PointCloudFilter filter, IntPtrCloudPointBuffer buffer, int start_index ) => m_root.ReadPoints( filter, buffer, start_index );
		#endregion

		//protected class OctreePointAccessIterator : IPointSetIterator
		//{
		//	#region Variables
		//	private PointCloudFilter m_filter;
		//	private int m_current_index;
		//	private OctreePointCloud m_access;
		//	private bool m_done=false;
		//	#endregion
		//
		//	#region Methods
		//	public OctreePointAccessIterator( OctreePointCloud access, PointCloudFilter filter )
		//	{
		//		m_access=access;
		//		m_filter=filter;
		//		m_current_index=0;
		//	}
		//	#endregion
		//
		//	#region IPointSetIterator Methods
		//	public int ReadPoints( IntPtr buffer, int buffer_size )
		//	{
		//		int found=0;
		//
		//		if( m_done ) return 0;
		//
		//		IntPtrCloudPointBuffer cp_buffer=new IntPtrCloudPointBuffer( buffer, buffer_size );
		//		found=m_access.ReadPoints( m_filter, cp_buffer, m_current_index );
		//		m_current_index+=found;
		//
		//		if( m_current_index==m_access.Count||found==0 ) m_done=true;
		//
		//		return found;
		//	}
		//
		//	public void Free() { m_done=true; }
		//	#endregion
		//}

		#region IPointCloudAccess Methods
		/// <summary>
		/// Creates the point set iterator.
		/// </summary>
		/// <param name="filter">The filter that determines which point is to be read</param>
		/// <param name="viewId">The view id</param>
		/// <returns>The point set iterator created</returns>
		public override IPointSetIterator CreatePointSetIterator( PointCloudFilter filter, ElementId viewId ) => new PointSetIteratorBase<OctreePointCloud>( this, filter );

		/// <summary>
		/// Creates the point set iterator.
		/// </summary>
		/// <param name="filter">The filter that determines which point is to be read</param>
		/// <param name="density">The density of the cloud points</param>
		/// <param name="viewId">The view id</param>
		/// <returns>The point set iterator created</returns>
		public override IPointSetIterator CreatePointSetIterator( PointCloudFilter filter, double density, ElementId viewId ) => new PointSetIteratorBase<OctreePointCloud>( this, filter );

		/// <summary>
		/// Reads points from the root.
		/// </summary>
		/// <param name="filter">The filter that determines which point is to be read</param>
		/// <param name="viewId">The view id</param>
		/// <param name="buffer">The buffer pointer to contain the points</param>
		/// <param name="buffer_size">The size of the buffer</param>
		/// <returns>The number of points that are fetched</returns>
		public override int ReadPoints( PointCloudFilter filter, ElementId viewId, IntPtr buffer, int buffer_size ) => ReadPoints( filter, new IntPtrCloudPointBuffer( buffer, buffer_size ), 0 );
		#endregion
	}
}
