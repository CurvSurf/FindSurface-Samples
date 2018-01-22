using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindSurfaceRevitPlugin
{
	public partial class FindSurfaceRevitPluginUI
	{
		private static readonly string s_ribbon_tab_name                     = "FindSurface";
		private static readonly string s_ribbon_panel_point_cloud_name       = "Point cloud";
		private static readonly string s_ribbon_panel_find_surface_name      = "Find surface";
		private static readonly string s_ribbon_panel_find_surface_2pts_name = "Find surface 2pts";
		private static readonly string s_ribbon_panel_find_surface_3pts_name = "Find surface 3pts";
		private static readonly string s_ribbon_panel_configuration_name     = "Configuration";

		public static string RibbonTabName { get { return s_ribbon_tab_name; } }
		public static string RibbonPanelPointCloudName { get { return s_ribbon_panel_point_cloud_name; } }
		public static string RibbonPanelFindSurfaceName { get { return s_ribbon_panel_find_surface_name; } }
		public static string RibbonPanelFindSurface2ptsName { get { return s_ribbon_panel_find_surface_2pts_name; } }
		public static string RibbonPanelFindSurface3ptsName { get { return s_ribbon_panel_find_surface_3pts_name; } }
		public static string RibbonPanelConfigurationName { get { return s_ribbon_panel_configuration_name; } }

		private static readonly string s_push_button_open_name               = "Open";
		private static readonly string s_push_button_clean_up_name           = "Clean up";
		private static readonly string s_push_button_reset_name              = "Reset";
		private static readonly string s_push_button_plane_name              = "Plane";
		private static readonly string s_push_button_sphere_name             = "Sphere";
		private static readonly string s_push_button_cylinder_name           = "Cylinder";
		private static readonly string s_push_button_cone_name               = "Cone";
		private static readonly string s_push_button_torus_name              = "Torus";
		private static readonly string s_push_button_any_shape_name          = "Any shape";
		private static readonly string s_push_button_strip_plane_name        = "Strip plane";
		private static readonly string s_push_button_rod_cylinder_name       = "Rod cylinder";
		private static readonly string s_push_button_disk_cylinder_name      = "Disk cylinder";
		private static readonly string s_push_button_disk_cone_name          = "Disk cone";
		private static readonly string s_push_button_thin_ring_torus_name    = "Thin ring torus";
		private static readonly string s_push_button_repeat_name             = "Repeat";
		private static readonly string s_push_button_view_list_name          = "View list";
		private static readonly string s_push_button_inspector_name			 = "Inspector";
		private static readonly string s_push_button_settings_name           = "Settings";

		public static string PushButtonOpenName { get { return s_push_button_open_name; } }
		public static string PushButtonCleanUpName { get { return s_push_button_clean_up_name; } }
		public static string PushButtonResetName { get { return s_push_button_reset_name; } }
		public static string PushButtonPlaneName { get { return s_push_button_plane_name; } }
		public static string PushButtonSphereName { get { return s_push_button_sphere_name; } }
		public static string PushButtonCylinderName { get { return s_push_button_cylinder_name; } }
		public static string PushButtonConeName { get { return s_push_button_cone_name; } }
		public static string PushButtonTorusName { get { return s_push_button_torus_name; } }
		public static string PushButtonStripPlaneName { get { return s_push_button_strip_plane_name; } }
		public static string PushButtonRodCylinder { get { return s_push_button_rod_cylinder_name; } }
		public static string PushButtonDiskCylinderName { get { return s_push_button_disk_cylinder_name; } }
		public static string PushButtonDiskConeName { get { return s_push_button_disk_cone_name; } }
		public static string PushButtonThinRingTorusName { get { return s_push_button_thin_ring_torus_name; } }
		public static string PushButtonAnyShapeName { get { return s_push_button_any_shape_name; } }
		public static string PushButtonRepeatName { get { return s_push_button_repeat_name; } }
		public static string PushButtonViewListName { get { return s_push_button_view_list_name; } }
		public static string PushButtonInspectorName { get { return s_push_button_inspector_name; } }
		public static string PushButtonSettingsName { get { return s_push_button_settings_name; } }

		private static readonly string s_push_button_open_class_name         = "FindSurfaceRevitPlugin.OpenCommand";
		private static readonly string s_push_button_clean_up_class_name     = "FindSurfaceRevitPlugin.CleanUpCommand";
		private static readonly string s_push_button_reset_class_name        = "FindSurfaceRevitPlugin.ResetCommand";
		private static readonly string s_push_button_plane_class_name        = "FindSurfaceRevitPlugin.FindPlaneCommand";
		private static readonly string s_push_button_sphere_class_name       = "FindSurfaceRevitPlugin.FindSphereCommand";
		private static readonly string s_push_button_cylinder_class_name     = "FindSurfaceRevitPlugin.FindCylinderCommand";
		private static readonly string s_push_button_cone_class_name         = "FindSurfaceRevitPlugin.FindConeCommand";
		private static readonly string s_push_button_torus_class_name        = "FindSurfaceRevitPlugin.FindTorusCommand";
		private static readonly string s_push_button_any_shape_class_name    = "FindSurfaceRevitPlugin.FindAnyCommand";
		private static readonly string s_push_button_strip_plane_class_name  = "FindSurfaceRevitPlugin.FindStripPlaneCommand";
		private static readonly string s_push_button_rod_cylinder_class_name = "FindSurfaceRevitPlugin.FindRodCylinderCommand";
		private static readonly string s_push_button_disk_cylinder_class_name= "FindSurfaceRevitPlugin.FindDiskCylinderCommand";
		private static readonly string s_push_button_disk_cone_class_name    = "FindSurfaceRevitPlugin.FindDiskConeCommand";
		private static readonly string s_push_button_thin_ring_torus_class_name = "FindSurfaceRevitPlugin.FindThinRingTorusCommand";
		private static readonly string s_push_button_repeat_class_name       = "FindSurfaceRevitPlugin.RepeatCommand";
		private static readonly string s_push_button_view_list_class_name    = "FindSurfaceRevitPlugin.ViewListCommand";
		private static readonly string s_push_button_inspector_class_name	 = "FindSurfaceRevitPlugin.InspectorCommand";
		private static readonly string s_push_button_settings_class_name     = "FindSurfaceRevitPlugin.SettingsCommand";

		public static string PushButtonOpenClassName { get { return s_push_button_open_class_name; } }
		public static string PushButtonCleanUpClassName { get { return s_push_button_clean_up_class_name; } }
		public static string PushButtonResetClassName { get { return s_push_button_reset_class_name; } }
		public static string PushButtonPlaneClassName { get { return s_push_button_plane_class_name; } }
		public static string PushButtonSphereClassName { get { return s_push_button_sphere_class_name; } }
		public static string PushButtonCylinderClassName { get { return s_push_button_cylinder_class_name; } }
		public static string PushButtonConeClassName { get { return s_push_button_cone_class_name; } }
		public static string PushButtonTorusClassName { get { return s_push_button_torus_class_name; } }
		public static string PushButtonAnyShapeClassName { get { return s_push_button_any_shape_class_name; } }
		public static string PushButtonStripPlaneClassName { get { return s_push_button_strip_plane_class_name; } }
		public static string PushButtonRodCylinderClassName { get { return s_push_button_rod_cylinder_class_name; } }
		public static string PushButtonDiskCylinderClassName { get { return s_push_button_disk_cylinder_class_name; } }
		public static string PushButtonDiskConeClassName { get { return s_push_button_disk_cone_class_name; } }
		public static string PushButtonThinRingTorusClassName { get { return s_push_button_thin_ring_torus_class_name; } }
		public static string PushButtonRepeatClassName { get { return s_push_button_repeat_class_name; } }
		public static string PushButtonViewListClassName { get { return s_push_button_view_list_class_name; } }
		public static string PushButtonInspectorClassName { get { return s_push_button_inspector_class_name; } }
		public static string PushButtonSettingsClassName { get { return s_push_button_settings_class_name; } }

		private static readonly string s_push_button_open_tooltip            = "Opens a XYZ file.";
		private static readonly string s_push_button_clean_up_tooltip        = "Removes all point cloud instances created by FindSurface point cloud engine.";
		private static readonly string s_push_button_reset_tooltip           = "Reset the point cloud in the first state loaded by Open command.";
		private static readonly string s_push_button_plane_tooltip           = "Finds plane near a selected point."+Environment.NewLine+"Click to select the point.";
		private static readonly string s_push_button_sphere_tooltip          = "Finds sphere near a selected point."+Environment.NewLine+"Click to select the point.";
		private static readonly string s_push_button_cylinder_tooltip        = "Finds cylinder near a selected point."+Environment.NewLine+"Click to select the point.";
		private static readonly string s_push_button_cone_tooltip            = "Finds cone near a selected point."+Environment.NewLine+"Click to select the point.";
		private static readonly string s_push_button_torus_tooltip           = "Finds torus near a selected point."+Environment.NewLine+"Click to select the point.";
		private static readonly string s_push_button_any_shape_tooltip       = "Finds any of the applicable shape near a selected point;"+Environment.NewLine+"plane, sphere, cylinder, cone, torus"+Environment.NewLine+"Click to select the point.";
		private static readonly string s_push_button_strip_plane_tooltip	 = "Finds strip plane near selected 2 points."+Environment.NewLine+"Click to select the points.";
		private static readonly string s_push_button_rod_cylinder_tooltip	 = "Finds rod cylinder near selected 2 points."+Environment.NewLine+"Click to select the points.";
		private static readonly string s_push_button_disk_cylinder_tooltip	 = "Finds disk cylinder near selected 3 points."+Environment.NewLine+"Click to select the points.";
		private static readonly string s_push_button_disk_cone_tooltip		 = "Finds disk cone near selected 3 points."+Environment.NewLine+"Click to select the points.";
		private static readonly string s_push_button_thin_ring_torus_tooltip = "Finds thin ring torus near selected 3 points."+Environment.NewLine+"Click to select the points.";
		private static readonly string s_push_button_repeat_tooltip          = "Repeats the Find command that is previously executed.";
		private static readonly string s_push_button_view_list_tooltip       = "Controls the visibility of point clouds and shapes.";
		private static readonly string s_push_button_inspector_tooltip		 = "Shows a dialog that tells you about the currently selected target's information.";
		private static readonly string s_push_button_settings_tooltip        = "Controls the parameters of FindSurface.";

		public static string PushButtonOpenTooltip { get { return s_push_button_open_tooltip; } }
		public static string PushButtonCleanUpTooltip { get { return s_push_button_clean_up_tooltip; } }
		public static string PushButtonResetTooltip { get { return s_push_button_reset_tooltip; } }
		public static string PushButtonPlaneTooltip { get { return s_push_button_plane_tooltip; } }
		public static string PushButtonSphereTooltip { get { return s_push_button_sphere_tooltip; } }
		public static string PushButtonCylinderTooltip { get { return s_push_button_cylinder_tooltip; } }
		public static string PushButtonConeTooltip { get { return s_push_button_cone_tooltip; } }
		public static string PushButtonTorusTooltip { get { return s_push_button_torus_tooltip; } }
		public static string PushButtonAnyShapeTooltip { get { return s_push_button_any_shape_tooltip; } }
		public static string PushButtonStripPlaneTooltip { get { return s_push_button_strip_plane_tooltip; } }
		public static string PushButtonRodCylinderTooltip { get { return s_push_button_rod_cylinder_tooltip; } }
		public static string PushButtonDiskCylinderTooltip { get { return s_push_button_disk_cylinder_tooltip; } }
		public static string PushButtonDiskConeTooltip { get { return s_push_button_disk_cone_tooltip; } }
		public static string PushButtonThinRingTorusTooltip { get { return s_push_button_thin_ring_torus_tooltip; } }
		public static string PushButtonRepeatTooltip { get { return s_push_button_repeat_tooltip; } }
		public static string PushButtonViewListTooltip { get { return s_push_button_view_list_tooltip; } }
		public static string PushButtonInspectorTooltip { get { return s_push_button_inspector_tooltip; } }
		public static string PushButtonSettingsTooltip { get { return s_push_button_settings_tooltip; } }

		private static readonly string s_push_button_open_icon_uri           = "pack://application:,,,/FindSurfaceRevitPlugin;component/Resources/ic_note_add_black_32.png";
		private static readonly string s_push_button_clean_up_icon_uri       = "pack://application:,,,/FindSurfaceRevitPlugin;component/Resources/ic_delete_forever_black_32.png";
		private static readonly string s_push_button_reset_icon_uri          = "pack://application:,,,/FindSurfaceRevitPlugin;component/Resources/ic_restore_black_32.png";
		private static readonly string s_push_button_plane_icon_uri          = "pack://application:,,,/FindSurfaceRevitPlugin;component/Resources/icon_plane_32.png";
		private static readonly string s_push_button_sphere_icon_uri         = "pack://application:,,,/FindSurfaceRevitPlugin;component/Resources/icon_sphere_32.png";
		private static readonly string s_push_button_cylinder_icon_uri       = "pack://application:,,,/FindSurfaceRevitPlugin;component/Resources/icon_cylinder_32.png";
		private static readonly string s_push_button_cone_icon_uri           = "pack://application:,,,/FindSurfaceRevitPlugin;component/Resources/icon_cone_32.png";
		private static readonly string s_push_button_torus_icon_uri          = "pack://application:,,,/FindSurfaceRevitPlugin;component/Resources/icon_torus_32.png";
		private static readonly string s_push_button_any_shape_icon_uri      = "pack://application:,,,/FindSurfaceRevitPlugin;component/Resources/icon_auto_32.png";
		private static readonly string s_push_button_strip_plane_icon_uri	 = "pack://application:,,,/FindSurfaceRevitPlugin;component/Resources/icon_plane_32.png";
		private static readonly string s_push_button_rod_cylinder_icon_uri	 = "pack://application:,,,/FindSurfaceRevitPlugin;component/Resources/icon_cylinder_32.png";
		private static readonly string s_push_button_disk_cylinder_icon_uri  = "pack://application:,,,/FindSurfaceRevitPlugin;component/Resources/icon_cylinder_32.png";
		private static readonly string s_push_button_disk_cone_icon_uri		 = "pack://application:,,,/FindSurfaceRevitPlugin;component/Resources/icon_cone_32.png";
		private static readonly string s_push_button_thin_ring_torus_icon_uri= "pack://application:,,,/FindSurfaceRevitPlugin;component/Resources/icon_torus_32.png";
		private static readonly string s_push_button_repeat_icon_uri         = "pack://application:,,,/FindSurfaceRevitPlugin;component/Resources/ic_replay_black_32.png";
		private static readonly string s_push_button_view_list_icon_uri      = "pack://application:,,,/FindSurfaceRevitPlugin;component/Resources/ic_view_list_black_32.png";
		private static readonly string s_push_button_inspector_icon_uri		 = "pack://application:,,,/FindSurfaceRevitPlugin;component/Resources/ic_info_outline_black_32.png";
		private static readonly string s_push_button_settings_icon_uri       = "pack://application:,,,/FindSurfaceRevitPlugin;component/Resources/ic_settings_black_32.png";

		public static string PushButtonOpenIconUri { get { return s_push_button_open_icon_uri; } }
		public static string PushButtonCleanUpIconUri { get { return s_push_button_clean_up_icon_uri; } }
		public static string PushButtonResetIconUri { get { return s_push_button_reset_icon_uri; } }
		public static string PushButtonPlaneIconUri { get { return s_push_button_plane_icon_uri; } }
		public static string PushButtonSphereIconUri { get { return s_push_button_sphere_icon_uri; } }
		public static string PushButtonCylinderIconUri { get { return s_push_button_cylinder_icon_uri; } }
		public static string PushButtonConeIconUri { get { return s_push_button_cone_icon_uri; } }
		public static string PushButtonTorusIconUri { get { return s_push_button_torus_icon_uri; } }
		public static string PushButtonAnyShapeIconUri { get { return s_push_button_any_shape_icon_uri; } }
		public static string PushButtonStripPlaneIconUri { get { return s_push_button_strip_plane_icon_uri; } }
		public static string PushButtonRodCylinderIconUri { get { return s_push_button_rod_cylinder_icon_uri; } }
		public static string PushButtonDiskCylinderIconUri { get { return s_push_button_disk_cylinder_icon_uri; } }
		public static string PushButtonDiskConeIconUri { get { return s_push_button_disk_cone_icon_uri; } }
		public static string PushButtonThinRingTorusIconUri { get { return s_push_button_thin_ring_torus_icon_uri; } }
		public static string PushButtonRepeatIconUri { get { return s_push_button_repeat_icon_uri; } }
		public static string PushButtonViewListIconUri { get { return s_push_button_view_list_icon_uri; } }
		public static string PushButtonInspectorIconUri { get { return s_push_button_inspector_icon_uri; } }
		public static string PushButtonSettingsIconUri { get { return s_push_button_settings_icon_uri; } }

		public static readonly List<string> s_ribbon_panel_name_list = new List<string>()
		{
			s_ribbon_panel_point_cloud_name,
			s_ribbon_panel_find_surface_name,
			s_ribbon_panel_find_surface_2pts_name,
			s_ribbon_panel_find_surface_3pts_name,
			s_ribbon_panel_configuration_name
		};

		public static readonly List<Dictionary<string,string>> s_button_class_names = new List<Dictionary<string,string>>()
		{
			new Dictionary<string, string>()
			{
				{ s_push_button_open_name,      s_push_button_open_class_name },
				{ s_push_button_clean_up_name,  s_push_button_clean_up_class_name },
				{ s_push_button_reset_name,     s_push_button_reset_class_name },
			},
			new Dictionary<string, string>()
			{
				{ s_push_button_plane_name,     s_push_button_plane_class_name },
				{ s_push_button_sphere_name,    s_push_button_sphere_class_name },
				{ s_push_button_cylinder_name,  s_push_button_cylinder_class_name },
				{ s_push_button_cone_name,      s_push_button_cone_class_name },
				{ s_push_button_torus_name,     s_push_button_torus_class_name },
				{ s_push_button_any_shape_name, s_push_button_any_shape_class_name },
			},
			new Dictionary<string, string>()
			{
				{ s_push_button_strip_plane_name, s_push_button_strip_plane_class_name },
				{ s_push_button_rod_cylinder_name, s_push_button_rod_cylinder_class_name },
			},
			new Dictionary<string, string>()
			{
				{ s_push_button_disk_cylinder_name, s_push_button_disk_cylinder_class_name },
				{ s_push_button_disk_cone_name, s_push_button_disk_cone_class_name },
				{ s_push_button_thin_ring_torus_name, s_push_button_thin_ring_torus_class_name },
			},
			new Dictionary<string, string>()
			{
				{ s_push_button_repeat_name,    s_push_button_repeat_class_name },
				{ s_push_button_view_list_name, s_push_button_view_list_class_name },
				{ s_push_button_inspector_name, s_push_button_inspector_class_name },
				{ s_push_button_settings_name,  s_push_button_settings_class_name },
			}
		};

		public static readonly List<Dictionary<string,string>> s_button_tooltips = new List<Dictionary<string,string>>()
		{
			new Dictionary<string, string>()
			{
				{ s_push_button_open_name,      s_push_button_open_tooltip },
				{ s_push_button_clean_up_name,  s_push_button_clean_up_tooltip },
				{ s_push_button_reset_name,     s_push_button_reset_tooltip },
			},
			new Dictionary<string, string>()
			{
				{ s_push_button_plane_name,     s_push_button_plane_tooltip },
				{ s_push_button_sphere_name,    s_push_button_sphere_tooltip },
				{ s_push_button_cylinder_name,  s_push_button_cylinder_tooltip },
				{ s_push_button_cone_name,      s_push_button_cone_tooltip },
				{ s_push_button_torus_name,     s_push_button_torus_tooltip },
				{ s_push_button_any_shape_name, s_push_button_any_shape_tooltip },
			},
			new Dictionary<string, string>()
			{
				{ s_push_button_strip_plane_name, s_push_button_strip_plane_tooltip },
				{ s_push_button_rod_cylinder_name, s_push_button_rod_cylinder_tooltip },
			},
			new Dictionary<string, string>()
			{
				{ s_push_button_disk_cylinder_name, s_push_button_disk_cylinder_tooltip },
				{ s_push_button_disk_cone_name, s_push_button_disk_cone_tooltip },
				{ s_push_button_thin_ring_torus_name, s_push_button_thin_ring_torus_tooltip },
			},
			new Dictionary<string, string>()
			{
				{ s_push_button_repeat_name,    s_push_button_repeat_tooltip },
				{ s_push_button_view_list_name, s_push_button_view_list_tooltip },
				{ s_push_button_inspector_name, s_push_button_inspector_tooltip },
				{ s_push_button_settings_name,  s_push_button_settings_tooltip },
			}
		};

		public static readonly List<Dictionary<string,string>> s_button_icon_uris = new List<Dictionary<string, string>>()
		{
			new Dictionary<string, string>()
			{
				{ s_push_button_open_name,      s_push_button_open_icon_uri },
				{ s_push_button_clean_up_name,  s_push_button_clean_up_icon_uri },
				{ s_push_button_reset_name,     s_push_button_reset_icon_uri },
			},
			new Dictionary<string, string>()
			{
				{ s_push_button_plane_name,     s_push_button_plane_icon_uri },
				{ s_push_button_sphere_name,    s_push_button_sphere_icon_uri },
				{ s_push_button_cylinder_name,  s_push_button_cylinder_icon_uri },
				{ s_push_button_cone_name,      s_push_button_cone_icon_uri },
				{ s_push_button_torus_name,     s_push_button_torus_icon_uri },
				{ s_push_button_any_shape_name, s_push_button_any_shape_icon_uri },
			},
			new Dictionary<string, string>()
			{
				{ s_push_button_strip_plane_name, s_push_button_strip_plane_icon_uri },
				{ s_push_button_rod_cylinder_name, s_push_button_rod_cylinder_icon_uri },
			},
			new Dictionary<string, string>()
			{
				{ s_push_button_disk_cylinder_name, s_push_button_disk_cylinder_icon_uri },
				{ s_push_button_disk_cone_name, s_push_button_disk_cone_icon_uri },
				{ s_push_button_thin_ring_torus_name, s_push_button_thin_ring_torus_icon_uri },
			},
			new Dictionary<string, string>()
			{
				{ s_push_button_repeat_name,    s_push_button_repeat_icon_uri },
				{ s_push_button_view_list_name, s_push_button_view_list_icon_uri },
				{ s_push_button_inspector_name, s_push_button_inspector_icon_uri },
				{ s_push_button_settings_name,  s_push_button_settings_icon_uri },
			}
		};
	}
}
