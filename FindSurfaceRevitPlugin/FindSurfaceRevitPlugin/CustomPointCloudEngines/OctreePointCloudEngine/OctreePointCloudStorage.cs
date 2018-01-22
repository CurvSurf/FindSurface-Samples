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
	/// A node of Oct-tree that contains points.
	/// </summary>
	public class OctreeNode
	{
		#region Variables
		/// <summary>
		/// CloudPoint array of the whole points.
		/// </summary>
		private CloudPoint[] m_points=null;
		
		/// <summary>
		/// Indices of the points
		/// </summary>
		private int[] m_indices=null;

		/// <summary>
		/// Lower-left boundary of the octree node
		/// </summary>
		private XYZ m_lower_left=null;

		/// <summary>
		/// Upper-right boundary of the octree node
		/// </summary>
		private XYZ m_upper_right=null;

		/// <summary>
		/// The depth of the node
		/// </summary>
		private int m_depth=0;

		/// <summary>
		/// The children of the node
		/// </summary>
		private OctreeNode[] m_children=null;
		#endregion

		#region Properties
		/// <summary>
		/// The number of the points that are contained in this subtree.
		/// </summary>
		public int Count { get { return m_indices==null ? 0 : m_indices.Length; } }

		/// <summary>
		/// CloudPoint array of the whole points
		/// </summary>
		public CloudPoint[] Points { get { return m_points; } }

		/// <summary>
		/// Lower-left boundary of the octree node
		/// </summary>
		public XYZ LowerLeft { get { return m_lower_left; } }

		/// <summary>
		/// Upper-right boundary of the octree node
		/// </summary>
		public XYZ UpperRight { get { return m_upper_right; } }

		/// <summary>
		/// The children of the node
		/// </summary>
		public OctreeNode[] Children { get { return m_children; } }
		#endregion

		#region Methods
		/// <summary>
		/// The constructor of OctreePointCloudStorage
		/// </summary>
		/// <param name="lower_left">Lower-left boundary</param>
		/// <param name="upper_right">Upper-right boundary</param>
		/// <param name="points">CloudPoint array of points</param>
		/// <param name="indices">Indices of the points that belong to the node</param>
		/// <param name="depth_limit">Maximum depth of the tree</param>
		/// <param name="parent_depth">The depth of the parent node</param>
		public OctreeNode( XYZ lower_left, XYZ upper_right, CloudPoint[] points, List<int> indices, int depth_limit, int parent_depth )
		{
			m_lower_left=lower_left;
			m_upper_right=upper_right;
			m_depth=parent_depth+1;
			m_points=points;
			m_indices=indices.ToArray();
			
			Setup( points, indices, depth_limit, m_depth );
		}

		/// <summary>
		/// Reads points from the node.
		/// </summary>
		/// <param name="filter">The filter that determines which point is to be read</param>
		/// <param name="buffer">The buffer to contain the points</param>
		/// <param name="start_index">The index of points to start fetching</param>
		/// <returns>The number of points that are fetched</returns>
		public int ReadPoints( PointCloudFilter filter, IntPtrCloudPointBuffer buffer, int start_index )
		{
			if( start_index==0 )
			{
				OctreeIndexSetIteratorList.Initialize( filter );
				OctreeIndexSetIteratorList.PrepareIterators( this );
			}

			return OctreeIndexSetIteratorList.ReadPoints( buffer );
		}
		#endregion

		#region Implementation
		/// <summary>
		/// Subdivides the node to 8 octree children.
		/// </summary>
		/// <param name="lower_left">The lower-left boundary of the node to be divided</param>
		/// <param name="upper_right">The upper-right boundary of the node to be divided</param>
		/// <returns>The array of 16 boundary values of the children, interleaving the lower-left and the upper-right.</returns>
		private static XYZ[] Subdivide( XYZ lower_left, XYZ upper_right )
		{
			XYZ m=lower_left;
			XYZ M=upper_right;
			XYZ c=(m+M)/2;

			XYZ[] sub=new XYZ[16];
			sub[ 0]=new XYZ( m.X, m.Y, m.Z ); sub[ 1]=new XYZ( c.X, c.Y, c.Z );
			sub[ 2]=new XYZ( m.X, m.Y, c.Z ); sub[ 3]=new XYZ( c.X, c.Y, M.Z );
			sub[ 4]=new XYZ( m.X, c.Y, m.Z ); sub[ 5]=new XYZ( c.X, M.Y, c.Z );
			sub[ 6]=new XYZ( m.X, c.Y, c.Z ); sub[ 7]=new XYZ( c.X, M.Y, M.Z );
			sub[ 8]=new XYZ( c.X, m.Y, m.Z ); sub[ 9]=new XYZ( M.X, c.Y, c.Z );
			sub[10]=new XYZ( c.X, m.Y, c.Z ); sub[11]=new XYZ( M.X, c.Y, M.Z );
			sub[12]=new XYZ( c.X, c.Y, m.Z ); sub[13]=new XYZ( M.X, M.Y, c.Z );
			sub[14]=new XYZ( c.X, c.Y, c.Z ); sub[15]=new XYZ( M.X, M.Y, M.Z );

			return sub;
		}

		/// <summary>
		/// Tests if the given point is enclosed by the volume defined by the given [<code>m</code>, <code>M</code>].
		/// </summary>
		/// <param name="p">The position of the point to test</param>
		/// <param name="m">The lower-left boundary of the volume to test</param>
		/// <param name="M">The upper-right boundary of the volume to test</param>
		/// <returns>true if the point is enclosed, false otherwise</returns>
		private static bool Enclose( XYZ p, XYZ m, XYZ M ) => m.X<p.X&&m.Y<p.Y&&m.Z<p.Z&&p.X<M.X&&p.Y<M.Y&&p.Z<M.Z;

		/// <summary>
		/// Finds the index of the child that encloses the point.
		/// </summary>
		/// <param name="p">The position of the point to test</param>
		/// <param name="c">The position of the center of the node</param>
		/// <returns>The index of the child that encloses the points</returns>
		private static int Enclose( XYZ p, XYZ c ) => (p.X<c.X ? 0 : 4)+(p.Y<c.Y ? 0 : 2)+(p.Z<c.Z ? 0 : 1);

		/// <summary>
		/// Setup the octree node and creates its children if needed.
		/// </summary>
		/// <param name="points">CloudPoint array of the whole points</param>
		/// <param name="indices">Indices of the points that belong to the node</param>
		/// <param name="depth_limit">Maximum depth of the tree</param>
		/// <param name="current_depth">The depth of the node</param>
		private void Setup( CloudPoint[] points, List<int> indices, int depth_limit, int current_depth )
		{
			if( indices.Count>=32&&current_depth!=depth_limit )
			{
				XYZ[] sub=Subdivide( m_lower_left, m_upper_right );
				List<int>[] sub_points=new List<int>[8] { new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>() };
				XYZ center=(LowerLeft+UpperRight)*0.5;

				foreach( int index in indices )
				{
					int sub_index=Enclose( points[index], center );
					sub_points[sub_index].Add( index );
				}

				List<OctreeNode> child_cells=new List<OctreeNode>();
				for( int k=0;k<8;k++ )
					if( sub_points[k].Any() ) child_cells.Add( new OctreeNode( sub[k*2], sub[k*2+1], points, sub_points[k], depth_limit, m_depth ) );

				m_children=child_cells.ToArray();
			}
		}
		#endregion

		/// <summary>
		/// IndexSetIteratorList holds the instances of IndexSetIterator.
		/// 
		/// When OctreePointCloudStorage.ReadPoints() is called,
		/// OctreeIndexSetIteratorList recursively traverses the octree, 
		/// and tests the nodes if the nodes passes the filter test.
		/// 
		/// If the result of the test is either Accepted(1) or PartiallyAccepted(0),
		/// then it collects the iterator of the node.
		/// 
		/// When OctreeIndexSetIteratorList.ReadPoints() is called,
		/// it fetches the points for each iterator, and accumulates them in the buffer.
		/// </summary>
		public static class OctreeIndexSetIteratorList
		{
			#region Variables 
			private static List<OctreeIndexSetIterator> s_iterator_list=new List<OctreeIndexSetIterator>();
			private static int s_current_iterator_index=0;
			private static int s_remaining_points=0;
			private static PointCloudFilter s_filter;
			#endregion

			#region Methods
			/// <summary>
			/// Initializes the list.
			/// </summary>
			/// <param name="filter">The filter to test nodes</param>
			public static void Initialize( PointCloudFilter filter )
			{
				s_iterator_list.Clear();
				s_current_iterator_index=0;
				s_remaining_points=0;
				s_filter=filter;
			}

			/// <summary>
			/// Performs the filter test and collects the iterators of the nodes that passes the test.
			/// This method is called recursively with the child nodes.
			/// </summary>
			/// <param name="access">The node to test</param>
			public static void PrepareIterators( OctreeNode access )
			{
				int filter_result=s_filter.TestCell( access.LowerLeft, access.UpperRight );

				// if the node is out of the filter, ignore them.
				if( filter_result==-1 ) return;

				// if the node is partially accepted and has its own children,
				if( filter_result==0&&access.Children!=null )
				{
					// recursively call the methods for each child.
					foreach( OctreeNode child in access.Children )
						if( child!=null ) PrepareIterators( child );
				}
				else // if the node is fully accepted or is partially accepted but having no children, 
				{
					// create and collect the iterators of them.
					s_iterator_list.Add( new OctreeIndexSetIterator( access, s_filter, filter_result==0 ) );
					s_remaining_points+=access.Count;
				}
			}

			/// <summary>
			/// Fetches the points from iterators and accumulates them the buffer.
			/// It calculates the portion of the points based on the buffer size and total number of the points,
			/// so each iterator gives the identical portion of their points (not exactly identical, because the number of points is integer).
			/// This allows us to give the points to Revit in approximately uniform.
			/// </summary>
			/// <param name="buffer">The buffer to contains the points</param>
			/// <returns>The number of the points that are read</returns>
			public static int ReadPoints( IntPtrCloudPointBuffer buffer )
			{
				int point_count=0;
				double quota_ratio=Math.Min( (double)buffer.Size/s_remaining_points, 1.0 );

				while( s_iterator_list.Any()&&buffer.Remaining>0 )
				{
					s_current_iterator_index%=s_iterator_list.Count;
					OctreeIndexSetIterator it=s_iterator_list[s_current_iterator_index];

					int quota=(int)Math.Ceiling( Math.Min( quota_ratio*it.Remaining, buffer.Remaining ) );
					point_count+=it.ReadPoints( buffer, quota );

					if( it.Done ) { s_iterator_list.RemoveAt( s_current_iterator_index ); continue; }
					s_current_iterator_index++;
				}
				s_remaining_points-=point_count;

				return point_count;
			}
			#endregion

		}

		/// <summary>
		/// OctreeIndexSetIterator provides the points of PointCloud instance that is bound to the iterator.
		/// </summary>
		public class OctreeIndexSetIterator
		{
			#region Variables
			private OctreeNode m_access;
			private int m_current_index=0;
			private bool m_partially_accepted;
			private PointCloudFilter m_filter;
			private uint[] m_halton_sequence;
			#endregion

			#region Properties
			/// <summary>
			/// The PointCloudStorage instance that is bound to the iterator.
			/// </summary>
			public OctreeNode Access { get { return m_access; } }

			/// <summary>
			/// The number of the points that have not been provided yet.
			/// </summary>
			public int Remaining { get { return Access.Count-m_current_index; } }

			/// <summary>
			/// It will be true if there is no points to provide, or false otherwise
			/// </summary>
			public bool Done { get { return m_current_index>=Access.Count; } }

			/// <summary>
			/// It will be true if the filter test to the node yields PartiallyAccepted(0).
			/// </summary>
			public bool PartiallyAccepted { get { return m_partially_accepted; } }
			#endregion

			#region Methods
			/// <summary>
			/// The constructor of OctreeIndexSetIterator
			/// </summary>
			/// <param name="access">The node that the iterator represents to</param>
			/// <param name="filter">The filter to test</param>
			/// <param name="partially_accepted">The filter test result</param>
			public OctreeIndexSetIterator( OctreeNode access, PointCloudFilter filter, bool partially_accepted )
			{
				m_access=access;
				m_partially_accepted=partially_accepted;
				m_filter=filter;

				m_halton_sequence=FindSurfaceRevitPluginUtils.HaltonSequence.GenerateIndices( access.Count );
			}

			/// <summary>
			/// Reads the points of the node
			/// </summary>
			/// <param name="buffer">The buffer to contains the points</param>
			/// <param name="quota">The quota of which the number of the points that the invocation of this function should provide</param>
			/// <returns>The number of points that are read</returns>
			public int ReadPoints( IntPtrCloudPointBuffer buffer, int quota )
			{
				int count=0;
				if( m_partially_accepted ) // gives the points that pass the test.
				{
					m_filter.PrepareForCell( Access.LowerLeft, Access.UpperRight, quota );
					while( quota>0 )
					{
						CloudPoint cp=Access.Points[m_halton_sequence[m_current_index++]];
						if( m_filter.TestPoint( cp ) ) { buffer.AddCloudPoint( cp ); count++; }
						quota--;
					}
				}
				else // gives all the points.
				{
					while( quota>0 )
					{
						buffer.AddCloudPoint( Access.Points[m_halton_sequence[m_current_index++]] );
						quota--; count++;
					}
				}
				return count;
			}
			#endregion
		}
	}
}
