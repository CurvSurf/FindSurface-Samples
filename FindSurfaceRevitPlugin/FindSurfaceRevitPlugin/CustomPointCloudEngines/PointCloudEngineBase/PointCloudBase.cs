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
	/// A base of PointCloud class as a container of cloud points.
	/// </summary>
	public class PointCloudBase : IPointCloudAccess
	{
		#region Variables
		protected string m_identifier=string.Empty;
		protected CloudPoint[] m_points=null;
		protected Outline m_outline=null;
		protected ElementId m_element_id;
		protected Document m_document;
		protected PointCloudEngineBase m_engine=null;
		#endregion

		#region Properties
		/// <summary>
		/// The number of the points
		/// </summary>
		public virtual int Count { get { return 0; } }

		/// <summary>
		/// CloudPoint array of the points
		/// </summary>
		public CloudPoint[] Points { get { return m_points; } }

		/// <summary>
		/// ElementId that Revit uses to identify the instance of PointCloudInstance class.
		/// </summary>
		public ElementId ElementId { get { return m_element_id; } set { m_element_id=value; } }

		/// <summary>
		/// The document that the point cloud is created.
		/// </summary>
		public Document Document { get { return m_document; } set { m_document=value; } }

		/// <summary>
		/// The outline of this container.
		/// </summary>
		public Outline Outline { get { return m_outline; } }

		public PointCloudEngineBase Engine { get { return m_engine; } set { m_engine=value; } }
		public Transform Transform { get { return GetTransform(); } }
		#endregion

		#region Methods
		/// <summary>
		/// Reads the points in this container.
		/// </summary>
		/// <param name="filter">The filter</param>
		/// <param name="buffer">The buffer to store the points that are read</param>
		/// <param name="start_index">The index of the point to start reading</param>
		/// <returns>The number of points that are read</returns>
		protected virtual int ReadPoints( PointCloudFilter filter, IntPtrCloudPointBuffer buffer, int start_index ) { throw new NotImplementedException(); }

		/// <summary>
		/// Hides the point cloud.
		/// </summary>
		public void Hide() => SetVisible( false );
		
		/// <summary>
		/// Unhides the point cloud.
		/// </summary>
		public void Unhide() => SetVisible( true );

		/// <summary>
		/// Deletes the point cloud.
		/// </summary>
		public void Delete()
		{
			if( GetInstance()==null ) return;

			using( Transaction t = new Transaction( Document, m_identifier ) )
			{
				t.Start();
				Document.Delete( ElementId );
				t.Commit();
			}
		}
		#endregion

		#region Implementation
		/// <summary>
		/// The constructor of PointCloudBase.
		/// </summary>
		/// <param name="identifier">The name of this instance</param>
		/// <param name="points">The points to store in this container</param>
		protected PointCloudBase( string identifier, CloudPoint[] points )
		{
			m_identifier=identifier;
			m_points=points;
		}

		private void SetVisible( bool visible )
		{
			if( GetInstance()==null ) return;

			using( Transaction t = new Transaction( Document, m_identifier ) )
			{
				t.Start();
				if( visible )	Document.ActiveView.UnhideElements( new ElementId[] { ElementId } );
				else			Document.ActiveView.HideElements( new ElementId[] { ElementId } );
				t.Commit();
			}
		}

		/// <summary>
		/// Returns the PointCloudInstance instance of the point cloud.
		/// </summary>
		/// <returns>The instance of PointCloudInstance</returns>
		private PointCloudInstance GetInstance()
		{
			FilteredElementCollector coll=new FilteredElementCollector( Document );
			List<PointCloudInstance> instances=coll.OfClass( typeof( PointCloudInstance ) )
				.ToElements().Cast<PointCloudInstance>().ToList();
			foreach( PointCloudInstance instance in instances )
			{
				PointCloudType type=Document.GetElement( instance.GetTypeId() ) as PointCloudType;
				if( type.EngineIdentifier==m_engine.Identifier&&
					type.Name==m_identifier ) return instance;
			}

			return null;
		}

		private Transform GetTransform()
		{
			PointCloudInstance pci=GetInstance();
			if( pci==null ) return Transform.Identity;

			return pci.GetTotalTransform();
		}
		#endregion

		#region Static Methods
		/// <summary>
		/// Calculates the size of a bounding box, which expands to include the given points.
		/// </summary>
		/// <param name="points">The points in a container of which size is to be calculated</param>
		/// <param name="lower_left">Lower-left boundary that is calculated</param>
		/// <param name="upper_right">Upper-right boundary that is calculated</param>
		public static void CalculateBoundingBox( CloudPoint[] points, out XYZ lower_left, out XYZ upper_right )
		{
			const double Revit_eps=1.0e-9;
			double mx,my,mz; mx=my=mz=double.MaxValue;
			double Mx,My,Mz; Mx=My=Mz=-double.MaxValue;

			foreach( CloudPoint cp in points )
			{
				mx=Math.Min( cp.X, mx ); Mx=Math.Max( cp.X, Mx );
				my=Math.Min( cp.Y, my ); My=Math.Max( cp.Y, My );
				mz=Math.Min( cp.Z, mz ); Mz=Math.Max( cp.Z, Mz );
			}

			double m = Math.Min(mx, Math.Min(my, mz)) - Revit_eps;
			double M = Math.Max(Mx, Math.Max(My, Mz)) + Revit_eps;
			lower_left=new XYZ( m, m, m );
			upper_right=new XYZ( M, M, M );
		}
		#endregion

		#region IPointCloudAccess Methods
		/// <summary>
		/// Gets the name of the PointCloud.
		/// </summary>
		/// <returns>The name</returns>
		public string GetName() => m_identifier;

		/// <summary>
		/// Gets the color encoding.
		/// </summary>
		/// <remarks>The color encoding is fixed, and colors are stored differently instead.</remarks>
		/// <returns>The color encoding</returns>
		public PointCloudColorEncoding GetColorEncoding() => PointCloudColorEncoding.ARGB;

		public virtual IPointSetIterator CreatePointSetIterator( PointCloudFilter filter, ElementId viewId ) { throw new NotImplementedException(); }
		public virtual IPointSetIterator CreatePointSetIterator( PointCloudFilter filter, double density, ElementId viewId ) { throw new NotImplementedException(); }

		// This method does the same thing as what the property "Outline" does. It has to be implemented because of inheritance.
		public Outline GetExtent() => Outline;

		/// <summary>
		/// Gets the offset of the point cloud
		/// </summary>
		/// <returns>The offset</returns>
		public XYZ GetOffset() => XYZ.Zero;

		/// <summary>
		/// Reads the points in this container.
		/// </summary>
		/// <param name="filter">The filter</param>
		/// <param name="viewId">The view id</param>
		/// <param name="buffer">The buffer pointer to store the points that are read</param>
		/// <param name="buffer_size">The size of the buffer</param>
		/// <param name="start_index">The index of the point to start reading</param>
		/// <returns>The number of points that are read</returns>
		public virtual int ReadPoints( PointCloudFilter filter, ElementId viewId, IntPtr buffer, int buffer_size ) { throw new NotImplementedException(); }

		public void Free() { }
		public double GetUnitsToFeetConversionFactor() => 1.0;
		#endregion

		#region IPointSetIterator Implementation
		/// <summary>
		/// A generic base class of PointSetIterator.
		/// A class inherited by this class is expected
		/// to provide points in its access to Revit, using successive calls.
		/// </summary>
		/// <typeparam name="T">The access type</typeparam>
		protected class PointSetIteratorBase<T> : IPointSetIterator where T : PointCloudBase
		{
			#region Variables
			protected PointCloudFilter m_filter;
			protected int m_current_index;
			protected T m_access;
			protected bool m_done=false;
			#endregion

			#region Methods
			/// <summary>
			/// The constructor of PointSetIteratorBase.
			/// </summary>
			/// <param name="access">The access of the iterator</param>
			/// <param name="filter">The filter to select points</param>
			public PointSetIteratorBase( T access, PointCloudFilter filter )
			{
				m_access=access;
				m_filter=filter;
				m_current_index=0;
			}
			#endregion

			#region IPointSetIterator Methods
			/// <summary>
			/// Reads points in the access and store them in the given buffer.
			/// </summary>
			/// <param name="buffer">The buffer pointer to store the points</param>
			/// <param name="buffer_size">The buffer size</param>
			/// <returns>The number of the points that are stored in the buffer</returns>
			public int ReadPoints( IntPtr buffer, int buffer_size )
			{
				int found=0;

				if( m_done ) return 0;

				IntPtrCloudPointBuffer cp_buffer=new IntPtrCloudPointBuffer( buffer, buffer_size );
				found=m_access.ReadPoints( m_filter, cp_buffer, m_current_index );
				m_current_index+=found;

				if( m_current_index==m_access.Count||found==0 ) m_done=true;

				return found;
			}

			public void Free() { m_done=true; }
			#endregion
		}
		#endregion
	}
}
