using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FindSurfaceRevitPlugin
{
	public partial class ViewListForm : Form
	{
		private static SteadyPositioning s_positioning=new SteadyPositioning();

		public ViewListForm()
		{
			InitializeComponent();

			this.FormClosing+=new FormClosingEventHandler( s_positioning.OnFormClosing );
			this.Shown+=new EventHandler( s_positioning.OnShown );

			if( FindSurfaceRevitPlugin.OutlierPointCloudEngine.Count!=0 )
				CreatePointCloudListBoxItems( FindSurfaceRevitPlugin.OutlierPointCloudEngine.GetPointCloudNames() );
			if( FindSurfaceRevitPlugin.InlierPointCloudEngine.Count!=0 )
				CreatePointCloudListBoxItems( FindSurfaceRevitPlugin.InlierPointCloudEngine.GetPointCloudNames() );
			if( FindSurfaceRevitPlugin.DirectShapeEngine.Count!=0 )
				CreateDirectShapeListBoxItems( FindSurfaceRevitPlugin.DirectShapeEngine.GetDirectShapeNames() );
		}

		private static void CreateListBoxItems( string[] items, Dictionary<string,ObjectVisibility> dic, CheckedListBox checkedListBox )
		{
			foreach( string item in items )
			{
				if( dic.ContainsKey( item )==false )
					dic.Add( item, new ObjectVisibilityChange() );

				checkedListBox.Items.Add( item, dic[item].Visible );
			}
		}
		private void CreatePointCloudListBoxItems( string[] items ) => CreateListBoxItems( items, s_point_cloud_visibility_dic, checkedListBoxPointClouds );
		private void CreateDirectShapeListBoxItems( string[] items ) => CreateListBoxItems( items, s_direct_shape_visibility_dic, checkedListBoxDirectShapes );
		
		private void checkedListBoxPointClouds_ItemCheck( object sender, ItemCheckEventArgs e )
		{
			string item=checkedListBoxPointClouds.Items[e.Index] as string;
			(s_point_cloud_visibility_dic[item] as ObjectVisibilityChange).Visible=(e.NewValue==CheckState.Checked);
		}
		
		private void buttonPointCloudsSetAllVisible_Click( object sender, EventArgs e )
		{
			for( int k = 0;k<checkedListBoxPointClouds.Items.Count;k++ )
				checkedListBoxPointClouds.SetItemChecked( k, true );
		}

		private void buttonPointCloudsHideAll_Click( object sender, EventArgs e )
		{
			for( int k = 0;k<checkedListBoxPointClouds.Items.Count;k++ )
				checkedListBoxPointClouds.SetItemChecked( k, false );
		}

		private void checkedListBoxShapes_ItemCheck( object sender, ItemCheckEventArgs e )
		{
			string item = checkedListBoxDirectShapes.Items[e.Index] as string;
			(s_direct_shape_visibility_dic[item] as ObjectVisibilityChange).Visible=(e.NewValue==CheckState.Checked);
		}
		
		private void buttonShapesSetAllVisible_Click( object sender, EventArgs e )
		{
			for( int k = 0;k<checkedListBoxDirectShapes.Items.Count;k++ )
				checkedListBoxDirectShapes.SetItemChecked( k, true );
		}

		private void buttonShapesHideAll_Click( object sender, EventArgs e )
		{
			for( int k = 0;k<checkedListBoxDirectShapes.Items.Count;k++ )
				checkedListBoxDirectShapes.SetItemChecked( k, false );
		}

		private void buttonClose_Click( object sender, EventArgs e )
		{
			this.DialogResult=DialogResult.OK;
			this.Close();
		}

	}
}
