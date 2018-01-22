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
	[Autodesk.Revit.Attributes.Transaction( Autodesk.Revit.Attributes.TransactionMode.Manual )]
	[Autodesk.Revit.Attributes.Regeneration( Autodesk.Revit.Attributes.RegenerationOption.Manual )]
	[Autodesk.Revit.Attributes.Journaling( Autodesk.Revit.Attributes.JournalingMode.NoCommandData )]
	public class OpenCommand : CustomRevitChildDialog, IExternalCommand
	{
		public Result Execute( 
			ExternalCommandData commandData, 
			ref string message, 
			ElementSet elements )
		{
			Document doc = commandData.Application.ActiveUIDocument.Document;
			
			ImportXYZData import_data = new ImportXYZData( doc );
			try
			{
				using( ImportXYZForm import_form = new ImportXYZForm( import_data ) )
				{
					if( import_form.ShowDialog( s_hwndRevit )==System.Windows.Forms.DialogResult.Cancel )
						return Result.Cancelled;

					FindSurfaceRevitPluginUI.GetRibbonPanel( FindSurfaceRevitPluginUI.RibbonPanelFindSurfaceName ).Enabled=true;
					FindSurfaceRevitPluginUI.GetRibbonPanel(FindSurfaceRevitPluginUI.RibbonPanelFindSurface2ptsName).Enabled=true;
					FindSurfaceRevitPluginUI.GetRibbonPanel(FindSurfaceRevitPluginUI.RibbonPanelFindSurface3ptsName).Enabled=true;
					FindSurfaceRevitPluginUI.GetPushButton( FindSurfaceRevitPluginUI.PushButtonViewListClassName ).Enabled=true;
				}
			}
			catch( Exception ex )
			{
				message=ex.ToString();
				return Result.Failed;
			}

			string import_file_full_name = import_data.ImportFileFullName;
			string import_file_name = import_data.ImportFileName;
			float[] import_data_xyz_array = import_data.ImportDataXYZ;
			int[] import_data_color_array = import_data.ImportDataColor;
			int import_data_subdivision_factor = import_data.SubdivisionFactor;
			float[] import_data_bounding_box_center = import_data.ImportDataBoundingBoxCenter;
			float[] import_data_bounding_box_extent = import_data.ImportDataBoundingBoxExtent;

			//TODO: HERE to insert the working code of XYZUnitConversionForm...
			XYZUnitConversionData conversion_data=new XYZUnitConversionData();
			try
			{
				using(XYZUnitConversionForm conversion_form=new XYZUnitConversionForm(import_data_bounding_box_extent, import_data_bounding_box_center, conversion_data))
				{
					if( conversion_form.ShowDialog( s_hwndRevit )==System.Windows.Forms.DialogResult.Cancel )
						return Result.Cancelled;
				}
			}
			catch( Exception ex )
			{
				message=ex.ToString();
				return Result.Failed;
			}

			float[] offset=conversion_data.OriginOffset;
			float scale=conversion_data.Scale;
			for(int k=0; k<import_data_xyz_array.Length; k+=3)
			{
				import_data_xyz_array[k]-=offset[0];
				import_data_xyz_array[k+1]-=offset[1];
				import_data_xyz_array[k+2]-=offset[2];
				import_data_xyz_array[k]*=scale;
				import_data_xyz_array[k+1]*=scale;
				import_data_xyz_array[k+2]*=scale;
			}
			FindSurfaceRevitPlugin.OnOpenCommand( doc, import_file_name, import_data_xyz_array, import_data_color_array, import_data_subdivision_factor, conversion_data.MeasuringUnit );

			return Result.Succeeded;
		}
	}

	[TransactionAttribute( TransactionMode.Manual )]
	[RegenerationAttribute( RegenerationOption.Manual )]
	public class CleanUpCommand : IExternalCommand
	{
		public Result Execute(
			ExternalCommandData commandData,
			ref string message,
			ElementSet elements )
		{
			Document doc = commandData.Application.ActiveUIDocument.Document;

			FindSurfaceRevitPlugin.OnCleanUpCommand();

			FindSurfaceRevitPluginUI.GetRibbonPanel( FindSurfaceRevitPluginUI.RibbonPanelFindSurfaceName ).Enabled=false;
			FindSurfaceRevitPluginUI.GetRibbonPanel(FindSurfaceRevitPluginUI.RibbonPanelFindSurface2ptsName).Enabled=false;
			FindSurfaceRevitPluginUI.GetRibbonPanel(FindSurfaceRevitPluginUI.RibbonPanelFindSurface3ptsName).Enabled=false;
			FindSurfaceRevitPluginUI.GetPushButton(FindSurfaceRevitPluginUI.PushButtonRepeatClassName).Enabled=false;
			FindSurfaceRevitPluginUI.GetPushButton( FindSurfaceRevitPluginUI.PushButtonViewListClassName ).Enabled=false;
			
			return Result.Succeeded;
		}
	}

	[TransactionAttribute( TransactionMode.Manual )]
	[RegenerationAttribute( RegenerationOption.Manual )]
	public class ResetCommand : IExternalCommand
	{
		public Result Execute(
			ExternalCommandData commandData,
			ref string messge,
			ElementSet elements )
		{
			Document doc = commandData.Application.ActiveUIDocument.Document;

			if( FindSurfaceRevitPlugin.OnResetCommand( doc ) )
			{
				FindSurfaceRevitPluginUI.GetRibbonPanel( FindSurfaceRevitPluginUI.RibbonPanelFindSurfaceName ).Enabled=true;
				FindSurfaceRevitPluginUI.GetRibbonPanel(FindSurfaceRevitPluginUI.RibbonPanelFindSurface2ptsName).Enabled=true;
				FindSurfaceRevitPluginUI.GetRibbonPanel(FindSurfaceRevitPluginUI.RibbonPanelFindSurface3ptsName).Enabled=true;
				FindSurfaceRevitPluginUI.GetPushButton( FindSurfaceRevitPluginUI.PushButtonViewListClassName ).Enabled=true;
			}

			return Result.Succeeded;
		}
	}
}
