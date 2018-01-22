using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Media.Imaging;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.PointClouds;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;

namespace FindSurfaceRevitPlugin
{
	[TransactionAttribute( TransactionMode.Manual )]
	[RegenerationAttribute( RegenerationOption.Manual )]
	public partial class FindSurfaceRevitPluginUI : IExternalApplication
	{
		private static List<RibbonPanel> s_ribbon_panel_list=new List<RibbonPanel>();
		private static List<PushButton> s_push_button_list=new List<PushButton>();

		public static RibbonPanel GetRibbonPanel( string panel_name ) => s_ribbon_panel_list.Find( x => x.Name==panel_name );
		public static PushButton GetPushButton( string button_class_name ) => s_push_button_list.Find( x => x.Name==button_class_name );

		private static void AddPushButtonOnRibbonPanel( RibbonPanel ribbon_panel, string button_name, string class_name, string tool_tip, string icon_uri )
		{
			PushButtonData push_button_data = new PushButtonData(
				class_name,
				button_name,
				Assembly.GetExecutingAssembly().Location,
				class_name);

			PushButton push_button = ribbon_panel.AddItem(push_button_data) as PushButton;
			push_button.ToolTip=tool_tip;
			
			if( icon_uri==null ) return;
			if( Uri.IsWellFormedUriString( icon_uri, UriKind.RelativeOrAbsolute )==false ) TaskDialog.Show( "debug", "it's not well formed Uri string: "+Environment.NewLine+icon_uri );
			push_button.LargeImage=new BitmapImage( new Uri( icon_uri ) );

			s_push_button_list.Add( push_button );
		}

		private void SetupUI( UIControlledApplication application )
		{
			// Create a custom ribbon tab
			application.CreateRibbonTab( s_ribbon_tab_name );

			// Add a new ribbon panel
			for( int s = 0;s<s_ribbon_panel_name_list.Count;s++ )
			{
				RibbonPanel target_ribbon_panel = application.CreateRibbonPanel( s_ribbon_tab_name, s_ribbon_panel_name_list[s] );
				s_ribbon_panel_list.Add( target_ribbon_panel );

				string[] button_names = s_button_class_names[s].Keys.ToArray();
				string[] class_names = s_button_class_names[s].Values.ToArray();
				string[] tooltips = s_button_tooltips[s].Values.ToArray();
				string[] icon_uris = s_button_icon_uris[s].Values.ToArray();
				for( int k = 0;k<button_names.Length;k++ )
					AddPushButtonOnRibbonPanel( target_ribbon_panel, button_names[k], class_names[k], tooltips[k], icon_uris[k] );
			}

			// disable find surface ribbon panel until a point cloud file is imported by Open push button.
			RibbonPanel find_surface_ribbon_panel = GetRibbonPanel( RibbonPanelFindSurfaceName );
			find_surface_ribbon_panel.Enabled=false;

			// disable find surface 2pts ribbon panel until a point cloud file is imported by Open push button.
			RibbonPanel find_surface_2pts_ribbon_panel = GetRibbonPanel( RibbonPanelFindSurface2ptsName );
			find_surface_2pts_ribbon_panel.Enabled=false;

			// disable find surface 3pts ribbon panel until a point cloud file is imported by Open push button.
			RibbonPanel find_surface_3pts_ribbon_panel = GetRibbonPanel( RibbonPanelFindSurface3ptsName );
			find_surface_3pts_ribbon_panel.Enabled=false;

			// disable repeat push button until a shape is found once by find command push buttons (e.g., plane, sphere, etc.).
			PushButton repeat_push_button = GetPushButton( PushButtonRepeatClassName );
			repeat_push_button.Enabled=false;

			// disable view list push button until a point cloud file is imported by Open push button.
			PushButton view_list_push_button = GetPushButton( PushButtonViewListClassName );
			view_list_push_button.Enabled=false;
		}

		public Result OnStartup( UIControlledApplication application )
		{
			SetupUI( application );

			FindSurfaceRevitPlugin.OnPluginStartUp();

			// subscribe Revit internal event callback
			application.Idling+=new EventHandler<Autodesk.Revit.UI.Events.IdlingEventArgs>( (sender, e)=>FindSurfaceRevitPlugin.OnPluginIdling() );
			application.ControlledApplication.DocumentChanged+=new EventHandler<Autodesk.Revit.DB.Events.DocumentChangedEventArgs>( ( sender, e ) => FindSurfaceRevitPlugin.OnContentChanged( e ) );
			application.ControlledApplication.DocumentClosing+=new EventHandler<Autodesk.Revit.DB.Events.DocumentClosingEventArgs>( ( sender, e ) => FindSurfaceRevitPlugin.OnDocumentClosing( e ) );

			NoticeForm form = new NoticeForm();
			form.ShowDialog();

			return Result.Succeeded;
		}

		public Result OnShutdown( UIControlledApplication application )
		{
			FindSurfaceRevitPlugin.OnPluginShutdown();

			return Result.Succeeded;
		}

	}

	public class CustomRevitChildDialog
	{
		/* reference: http://thebuildingcoder.typepad.com/blog/2009/02/revit-window-handle-and-modeless-dialogues.html */
		public class WindowHandle : System.Windows.Forms.IWin32Window
		{
			IntPtr m_hwnd;

			public WindowHandle( IntPtr h ) { m_hwnd=h; }
			public IntPtr Handle { get { return m_hwnd; } }
		}

		protected static WindowHandle s_hwndRevit = new WindowHandle(System.Diagnostics.Process.GetProcessesByName("Revit")[0].MainWindowHandle);
	}
	
}
