using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB.PointClouds;

namespace FindSurfaceRevitPlugin
{
	/// <summary>
	/// A wrapper class of the buffer used to deliver the points to Revit.
	/// </summary>
	public unsafe class IntPtrCloudPointBuffer
	{
		private CloudPoint *m_buffer;
		private int m_buffer_size;
		private int m_point_count=0;

		public bool IsFull { get { return m_buffer_size==m_point_count; } }
		public int Size { get { return m_buffer_size; } }
		public int Remaining { get { return m_buffer_size-m_point_count; } }

		public IntPtrCloudPointBuffer( IntPtr buffer, int buffer_size )
		{
			m_buffer=(CloudPoint*)buffer;
			m_buffer_size=buffer_size;
		}

		/// <summary>
		/// Copies the cloud point to the buffer.
		/// </summary>
		/// <param name="cp">The cloud point to copy</param>
		public void AddCloudPoint( CloudPoint cp )
		{
			*(m_buffer+m_point_count)=cp;
			m_point_count++;
		}
	}
}
