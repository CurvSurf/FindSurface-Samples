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
	/// A PointCloud wrapper using a uniform grid to contain cloud points.
	/// </summary>
	public class UniformGridPointCloud : PointCloudBase
	{
		#region Variables
		/// <summary>
		/// Uniform-grid cell storages
		/// </summary>
		private UniformGridCell[] m_cells=null;

		/// <summary>
		/// The number of cells in each dimension
		/// </summary>
		private int m_subdivision;
		#endregion

		#region Properties
		/// <summary>
		/// The number of points contained in the instance.
		/// </summary>
		public override int Count { get { return m_points.Length; } }

		/// <summary>
		/// The number of cells in each dimension
		/// </summary>
		public int Subdivision { get { return m_subdivision; } }
		#endregion

		#region Methods
		/// <summary>
		/// The constructor of UniformGridPointCloud.
		/// </summary>
		/// <param name="identifier">The name of the point cloud</param>
		/// <param name="points">CloudPoint array of the points</param>
		/// <param name="subdivision">The subdivision number</param>
		public UniformGridPointCloud( string identifier, CloudPoint[] points, int subdivision ) : base( identifier, points )
		{
			m_subdivision=subdivision;
			Setup( subdivision );
		}

		/// <summary>
		/// Performs ray casting to the point cloud with a given ray.
		/// The ray sweeps the points and collects the points 
		/// of which distance to the ray is less than the ray radius.
		/// Among the points, the one that is nearest to the ray origin will be picked.
		/// </summary>
		/// <param name="ray_origin">The ray origin</param>
		/// <param name="ray_direction">The ray direction</param>
		/// <param name="ray_radius">The radius of ray sweeping</param>
		/// <param name="idx">The index of the point that is picked</param>
		/// <returns>true if there is a point picked, false otherwise</returns>
		public bool Cast( XYZ ray_origin, XYZ ray_direction, float ray_radius, out uint idx )
		{
			
			Transform t=this.Transform.Inverse;
			ray_origin=t.OfPoint( ray_origin );
			ray_direction=t.OfVector( ray_direction );

			idx=0;
			Func<XYZ, XYZ, XYZ, double> Distance=(o,d,p)=>
			{
				XYZ AB=p-o;
				return ((d.DotProduct(AB)/d.DotProduct(d))*d-AB).GetLength();
			};

			List<uint> indices_of_the_points_swept_by_the_ray=new List<uint>();

			// collect indices of the points of which distance 
			// to the ray is less than ray_radius.
			for( int k = 0;k<m_points.Length;k++ )
			{
				CloudPoint cp=m_points[k];
				XYZ cpos=new XYZ(cp.X, cp.Y, cp.Z);

				if( Distance( ray_origin, ray_direction, cpos )<ray_radius )
					indices_of_the_points_swept_by_the_ray.Add( (uint)k );
			}

			if( indices_of_the_points_swept_by_the_ray.Any()==false ) return false;

			// find index of the point having minimum distance to viewer (ray_origin).
			return FindSurfaceRevitPluginUtils.MinElement( indices_of_the_points_swept_by_the_ray, i => ray_origin.DistanceTo( new XYZ( m_points[(int)i].X, m_points[(int)i].Y, m_points[(int)i].Z ) ), out idx );
		}
		#endregion

		#region Implementation
		/// <summary>
		/// Creates the uniform-grid cells.
		/// </summary>
		/// <param name="subdivision"></param>
		private void Setup( int subdivision )
		{
			XYZ lower_left, upper_right;
			CalculateBoundingBox( m_points, out lower_left, out upper_right );
			m_outline=new Outline( lower_left, upper_right );

			// generate index uniform grid structure
			List<UniformGridCell> cell_list=new List<UniformGridCell>();
			XYZ outline_extent=upper_right-lower_left;
			XYZ cell_size=outline_extent/subdivision;
			int subdivision_sq=subdivision*subdivision;
			Func<int,int,int,int> At=( x, y, z )=>subdivision_sq*z+subdivision*y+x;

			for( int z = 0;z<subdivision;z++ )
			{
				double dmz=lower_left.Z+cell_size.Z*z;
				double dMz=(z==subdivision-1) ? upper_right.Z : (lower_left.Z+cell_size.Z*(z+1));
				for( int y = 0;y<subdivision;y++ )
				{
					double dmy=lower_left.Y+cell_size.Y*y;
					double dMy=(y==subdivision-1) ? upper_right.Y : (lower_left.Y+cell_size.Y*(y+1));
					for( int x = 0;x<subdivision;x++ )
					{
						double dmx=lower_left.X+cell_size.X*x;
						double dMx=(x==subdivision-1) ? upper_right.X : (lower_left.X+cell_size.X*(x+1));

						XYZ dm=new XYZ( dmx, dmy, dmz );
						XYZ dM=new XYZ( dMx, dMy, dMz );
						cell_list.Add( new UniformGridCell( dm, dM ) );
					}
				}
			}

			// distribute indices
			List<int>[] indices=new List<int>[subdivision*subdivision*subdivision];
			for( int k = 0;k<m_points.Length;k++ )
			{
				XYZ cp=m_points[k]-lower_left;
				int x=(int)(cp.X/cell_size.X);
				int y=(int)(cp.Y/cell_size.Y);
				int z=(int)(cp.Z/cell_size.Z);

				if( x<0||x>=subdivision||
					y<0||y>=subdivision||
					z<0||z>=subdivision )
					throw new ArgumentException( "Invalid size of input arguments: lower_left, upper_right" );

				int idx=At( x, y, z );
				if( indices[idx]==null ) indices[idx]=new List<int>();
				indices[idx].Add( k );
			}

			for( int k = 0;k<indices.Length;k++ )
				if( indices[k]!=null ) cell_list[k].Indices=indices[k].ToArray();

			m_cells=cell_list.FindAll( x => x.Count!=0 ).ToArray();
		}

		/// <summary>
		/// Reads points from the cells.
		/// </summary>
		/// <param name="filter">The filter that determines which point is to be read</param>
		/// <param name="buffer">The buffer to contain the points</param>
		/// <param name="start_index">The index of points to start fetching</param>
		/// <returns>The number of points that are fetched</returns>
		protected override int ReadPoints( PointCloudFilter filter, IntPtrCloudPointBuffer buffer, int start_index )
		{
			int point_index=0;
			int current_index=start_index;
			int total_points=0;
			int start_cell=0;

			for( int k = 0;k<m_cells.Length;k++ )
			{
				UniformGridCell cell=m_cells[k];

				int filter_result=filter.TestCell( cell.LowerLeft, cell.UpperRight );
				if( filter_result==-1 ) continue;

				total_points+=cell.Count;
				if( current_index<total_points )
				{
					start_cell=k;
					current_index=Math.Max( 0, start_index-total_points );
					break;
				}
			}

			for( int k = start_cell;k<m_cells.Length;k++ )
			{
				UniformGridCell cell=m_cells[k];

				int filter_result=filter.TestCell( cell.LowerLeft, cell.UpperRight );
				if( filter_result==-1 ) continue;

				if( filter_result==0 ) filter.PrepareForCell( m_outline.MinimumPoint, m_outline.MaximumPoint, cell.Count );

				for( int s = current_index;s<cell.Count;s++ )
				{
					if( filter_result==0 )
					{
						if( filter.TestPoint( m_points[cell.Indices[s]] )==false )
							continue;
					}
					buffer.AddCloudPoint( m_points[cell.Indices[s]] );
					point_index++;

					if( point_index>=buffer.Size ) break;
				}

				if( point_index>=buffer.Size ) break;
				current_index=0;
			}
			return point_index;
		}

		#endregion

		//protected class UniformGridPointAccessIterator : IPointSetIterator
		//{
		//	#region Variables
		//	private PointCloudFilter m_filter;
		//	private int m_current_index;
		//	private UniformGridPointCloud m_access;
		//	private bool m_done=false;
		//	#endregion
		//
		//	#region Methods
		//	public UniformGridPointAccessIterator( UniformGridPointCloud access, PointCloudFilter filter )
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
		//		if( found>cp_buffer.Size ) m_done=true;
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
		public override IPointSetIterator CreatePointSetIterator( PointCloudFilter filter, ElementId viewId ) => new PointSetIteratorBase<UniformGridPointCloud>( this, filter );

		/// <summary>
		/// Creates the point set iterator.
		/// </summary>
		/// <param name="filter">The filter that determines which point is to be read</param>
		/// <param name="density">The density of the cloud points</param>
		/// <param name="viewId">The view id</param>
		/// <returns>The point set iterator created</returns>
		public override IPointSetIterator CreatePointSetIterator( PointCloudFilter filter, double density, ElementId viewId ) => new PointSetIteratorBase<UniformGridPointCloud>( this, filter );

		/// <summary>
		/// Reads points from the cells.
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
