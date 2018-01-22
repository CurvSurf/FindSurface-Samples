using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

namespace FindSurfaceRevitPlugin
{
	/// <summary>
	/// A cell of uniform grid that contains points.
	/// </summary>
	public class UniformGridCell
	{
		#region Variables
		/// <summary>
		/// Indices of the points
		/// </summary>
		private int[] m_indices=null;

		/// <summary>
		/// Lower-left boundary of the cell
		/// </summary>
		private XYZ m_lower_left=null;

		/// <summary>
		/// Upper-right boundary of the cell
		/// </summary>
		private XYZ m_upper_right=null;
		#endregion

		#region Properties
		/// <summary>
		/// The number of the points that are contained in this subtree.
		/// </summary>
		public int Count { get { return m_indices==null ? 0 : m_indices.Length; } }

		/// <summary>
		/// Indices of the points
		/// </summary>
		public int[] Indices { get { return m_indices; } set { m_indices=value; } }

		/// <summary>
		/// Lower-left boundary of the cell
		/// </summary>
		public XYZ LowerLeft { get { return m_lower_left; } }

		/// <summary>
		/// Upper-right boundary of the cell
		/// </summary>
		public XYZ UpperRight { get { return m_upper_right; } }
		#endregion

		#region Methods
		/// <summary>
		/// The constructor of UniformGridCell
		/// </summary>
		/// <param name="lower_left">Lower-left boundary</param>
		/// <param name="upper_right">Upper-right boundary</param>
		public UniformGridCell( XYZ lower_left, XYZ upper_right )
		{
			m_lower_left=lower_left;
			m_upper_right=upper_right;
		}

		/// <summary>
		/// Creates an index access iterator.
		/// </summary>
		/// <returns>The index access iterator that is created</returns>
		public UniformGridIndexAccessIterator CreateIndexAccessIterator() => new UniformGridIndexAccessIterator( this );
		#endregion

		public class UniformGridIndexAccessIterator
		{
			private UniformGridCell m_access;
			private int m_current_index=0;

			public UniformGridIndexAccessIterator( UniformGridCell access ) { m_access=access; }

			public UniformGridCell Access { get { return m_access; } }
			public bool Done { get { return m_current_index==m_access.Count; } }
			public int NextIndex { get { return m_access.Indices[m_current_index++]; } }
			public void Reset() { m_current_index=0; }
		}
	}
}
