using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.PointClouds;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;

namespace FindSurfaceRevitPlugin
{
	[TransactionAttribute(TransactionMode.Manual)]
	[RegenerationAttribute(RegenerationOption.Manual)]
	public class RepeatCommand : IExternalCommand
	{
		private static IExternalCommand s_last_executed_command = null;
		public static IExternalCommand LastExecutedCommand { set { OnSetLastExecutedCommand(); s_last_executed_command=value; } }
		private static void OnSetLastExecutedCommand()
		{
			PushButton repeatButton = FindSurfaceRevitPluginUI.GetPushButton(FindSurfaceRevitPluginUI.PushButtonRepeatClassName);
			if(s_last_executed_command!=null&&repeatButton.Enabled) return;
			FindSurfaceRevitPluginUI.GetPushButton(FindSurfaceRevitPluginUI.PushButtonRepeatClassName).Enabled=true;
		}

		public Result Execute(
			ExternalCommandData commandData,
			ref string message,
			ElementSet elements)
		{
			UIApplication uiApp = commandData.Application;
			UIDocument uiDoc = uiApp.ActiveUIDocument;

			s_last_executed_command.Execute(commandData, ref message, elements);

			return Result.Succeeded;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	[TransactionAttribute( TransactionMode.Manual )]
	[RegenerationAttribute( RegenerationOption.Manual )]
	public class ViewListCommand : CustomRevitChildDialog, IExternalCommand
	{
		public Result Execute(
			ExternalCommandData commandData,
			ref string message,
			ElementSet elements )
		{
			UIApplication uiApp = commandData.Application;
			UIDocument uiDoc = uiApp.ActiveUIDocument;

			ViewListForm form = new ViewListForm();
			form.ShowDialog( s_hwndRevit );
			
			return Result.Succeeded;
		}
	}

	[TransactionAttribute( TransactionMode.Manual )]
	[RegenerationAttribute( RegenerationOption.Manual )]
	public class SettingsCommand : CustomRevitChildDialog, IExternalCommand
	{
		public Result Execute(
			ExternalCommandData commandData,
			ref string message,
			ElementSet elements )
		{
			UIApplication uiApp = commandData.Application;
			UIDocument uiDoc = uiApp.ActiveUIDocument;
			
			SettingsForm fsParamForm = new SettingsForm();
			fsParamForm.ShowDialog( s_hwndRevit );

			return Result.Succeeded;
		}
	}

	[TransactionAttribute( TransactionMode.Manual )]
	[RegenerationAttribute( RegenerationOption.Manual )]
	public class InspectorCommand : CustomRevitChildDialog, IExternalCommand
	{
		public Result Execute(
			ExternalCommandData commandData,
			ref string message,
			ElementSet elements )
		{
			UIApplication uiApp = commandData.Application;
			UIDocument uiDoc = uiApp.ActiveUIDocument;

			InspectorForm.UIDocument=uiDoc;
			InspectorForm form=InspectorForm.Instance;
			if( form.Created==false ) form.Show( s_hwndRevit );
			else form.Focus();

			return Result.Succeeded;
		}
	}
}
