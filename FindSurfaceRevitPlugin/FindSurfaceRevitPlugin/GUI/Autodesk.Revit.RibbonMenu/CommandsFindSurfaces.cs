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
	[TransactionAttribute( TransactionMode.Manual )]
	[RegenerationAttribute( RegenerationOption.Manual )]
	public class FindPlaneCommand : IExternalCommand
	{
		public Result Execute(
			ExternalCommandData commandData,
			ref string message,
			ElementSet elements )
		{
			UIApplication uiApp = commandData.Application;
			UIDocument uiDoc = uiApp.ActiveUIDocument;
			
			RepeatCommand.LastExecutedCommand=this;

			return FindSurfaceRevitPlugin.OnFindPlaneCommand( uiDoc );
		}
	}

	[TransactionAttribute( TransactionMode.Manual )]
	[RegenerationAttribute( RegenerationOption.Manual )]
	public class FindSphereCommand : IExternalCommand
	{
		public Result Execute(
			ExternalCommandData commandData,
			ref string message,
			ElementSet elements )
		{
			UIApplication uiApp = commandData.Application;
			UIDocument uiDoc = uiApp.ActiveUIDocument;

			RepeatCommand.LastExecutedCommand=this;

			return FindSurfaceRevitPlugin.OnFindSphereCommand( uiDoc );
		}
	}

	[TransactionAttribute( TransactionMode.Manual )]
	[RegenerationAttribute( RegenerationOption.Manual )]
	public class FindCylinderCommand : IExternalCommand
	{
		public Result Execute(
			ExternalCommandData commandData,
			ref string message,
			ElementSet elements )
		{
			UIApplication uiApp = commandData.Application;
			UIDocument uiDoc = uiApp.ActiveUIDocument;

			RepeatCommand.LastExecutedCommand=this;

			return FindSurfaceRevitPlugin.OnFindCylinderCommand( uiDoc );
		}
	}

	[TransactionAttribute( TransactionMode.Manual )]
	[RegenerationAttribute( RegenerationOption.Manual )]
	public class FindConeCommand : IExternalCommand
	{
		public Result Execute(
			ExternalCommandData commandData,
			ref string message,
			ElementSet elements )
		{
			UIApplication uiApp = commandData.Application;
			UIDocument uiDoc = uiApp.ActiveUIDocument;

			RepeatCommand.LastExecutedCommand=this;

			return FindSurfaceRevitPlugin.OnFindConeCommand( uiDoc );
		}
	}

	[TransactionAttribute( TransactionMode.Manual )]
	[RegenerationAttribute( RegenerationOption.Manual )]
	public class FindTorusCommand : IExternalCommand
	{
		public Result Execute(
			ExternalCommandData commandData,
			ref string message,
			ElementSet elements )
		{
			UIApplication uiApp = commandData.Application;
			UIDocument uiDoc = uiApp.ActiveUIDocument;

			RepeatCommand.LastExecutedCommand=this;

			return FindSurfaceRevitPlugin.OnFindTorusCommand( uiDoc );
		}
	}

	[TransactionAttribute( TransactionMode.Manual )]
	[RegenerationAttribute( RegenerationOption.Manual )]
	public class FindAnyCommand : IExternalCommand
	{
		public Result Execute(
			ExternalCommandData commandData,
			ref string message,
			ElementSet elements )
		{
			UIApplication uiApp = commandData.Application;
			UIDocument uiDoc = uiApp.ActiveUIDocument;

			RepeatCommand.LastExecutedCommand=this;

			return FindSurfaceRevitPlugin.OnFindAnyShapeCommand( uiDoc );
		}
	}

	
}
