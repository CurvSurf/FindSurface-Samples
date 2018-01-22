using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindSurfaceRevitPlugin
{
	public partial class ViewListForm : System.Windows.Forms.Form
	{
		private class ObjectVisibilityChange : ObjectVisibility
		{
			public new bool Visible { get { return m_visible; } set { m_changed=value^m_visible; m_visible=value; } }
		}

		private static Dictionary<string,ObjectVisibility> s_point_cloud_visibility_dic=new Dictionary<string, ObjectVisibility>();
		private static Dictionary<string,ObjectVisibility> s_direct_shape_visibility_dic=new Dictionary<string, ObjectVisibility>();

		public static KeyValuePair<string,ObjectVisibility>[] GetPointCloudsVisibilityChanged() { return s_point_cloud_visibility_dic.Count( x => x.Value.Changed )!=0 ? s_point_cloud_visibility_dic.Where( x => x.Value.Changed ).ToArray() : new KeyValuePair<string, ObjectVisibility>[0]; }
		public static KeyValuePair<string,ObjectVisibility>[] GetShapesVisibilityChanged() { return s_direct_shape_visibility_dic.Count( x => x.Value.Changed )!=0 ? s_direct_shape_visibility_dic.Where( x => x.Value.Changed ).ToArray() : new KeyValuePair<string, ObjectVisibility>[0]; }
	}

	public class ObjectVisibility
	{
		protected bool m_changed;
		protected bool m_visible;

		public ObjectVisibility() { m_changed=false; m_visible=true; }
		public bool Changed { get { return m_changed; } }
		public bool Visible { get { return m_visible; } }
		public void Expire() { m_changed=false; }
	}
}
