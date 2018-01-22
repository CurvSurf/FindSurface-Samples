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
	public class FindDiskCylinderCommand : IExternalCommand
	{
		public Result Execute(
			ExternalCommandData commandData,
			ref string message,
			ElementSet elements)
		{
			UIApplication uiApp = commandData.Application;
			UIDocument uiDoc = uiApp.ActiveUIDocument;

			RepeatCommand.LastExecutedCommand=this;

			return FindSurfaceRevitPlugin.OnFindDiskCylinderCommand(uiDoc);
		}
	}

	[TransactionAttribute(TransactionMode.Manual)]
	[RegenerationAttribute(RegenerationOption.Manual)]
	public class FindDiskConeCommand : IExternalCommand
	{
		public Result Execute(
			ExternalCommandData commandData,
			ref string message,
			ElementSet elements)
		{
			UIApplication uiApp = commandData.Application;
			UIDocument uiDoc = uiApp.ActiveUIDocument;

			RepeatCommand.LastExecutedCommand=this;

			return FindSurfaceRevitPlugin.OnFindDiskConeCommand(uiDoc);
		}
	}

	[TransactionAttribute(TransactionMode.Manual)]
	[RegenerationAttribute(RegenerationOption.Manual)]
	public class FindThinRingTorusCommand : IExternalCommand
	{
		public Result Execute(
			ExternalCommandData commandData,
			ref string message,
			ElementSet elements)
		{
			UIApplication uiApp = commandData.Application;
			UIDocument uiDoc = uiApp.ActiveUIDocument;

			RepeatCommand.LastExecutedCommand=this;

			return FindSurfaceRevitPlugin.OnFindThinRingTorusCommand(uiDoc);
		}
	}
}
