using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace FindSurfaceRevitPlugin
{
	public partial class InspectorForm : System.Windows.Forms.Form
	{
		public static UIDocument UIDocument { get; set; }

		private static InspectorForm s_instance=null;
		public static InspectorForm Instance { get { if( s_instance==null ) s_instance=new InspectorForm(); return s_instance; } }

		private static SteadyPositioning s_positioning=new SteadyPositioning();
		public InspectorForm()
		{
			s_instance=this;
			InitializeComponent();

			this.FormClosing+=new FormClosingEventHandler( s_positioning.OnFormClosing );
			this.Shown+=new EventHandler( s_positioning.OnShown );
		}
		private void Inspect(object sender, EventArgs e)
		{
			ICollection<ElementId> selectedIds = UIDocument.Selection.GetElementIds();
			if(0==selectedIds.Count)
			{
				this.richTextBoxInspectResult.AppendText( "No selected items.\n" );
			}
			else
			{
				foreach(ElementId id in selectedIds)
				{
					if( FindSurfaceRevitPlugin.DirectShapeEngine.ContainDirectShape(id) )
					{
						DirectShapeBase dsb = FindSurfaceRevitPlugin.DirectShapeEngine.GetShape( id );
						switch(dsb.ShapeType)
						{
							case ShapeTypes.Plane: Write( dsb as DirectPlane ); break;
							case ShapeTypes.Sphere: Write( dsb as DirectSphere ); break;
							case ShapeTypes.Cylinder: Write( dsb as DirectCylinder ); break;
							case ShapeTypes.Cone: Write( dsb as DirectCone ); break;
							case ShapeTypes.Torus: Write( dsb as DirectTorus ); break;
						}
					}
				}
			}
		}
		private void Write(DirectPlane dp)
		{
			string content=$"{dp.Name}\n"+
				$"    .lower-left\n"+
				$"        .X = {dp.BottomLeft.X.ToString("##0.####")}\n"+
				$"        .Y = {dp.BottomLeft.Y.ToString("##0.####")}\n"+
				$"        .Z = {dp.BottomLeft.Z.ToString("##0.####")}\n"+
				$"    .lower-right\n"+
				$"        .X = {dp.BottomRight.X.ToString("##0.####")}\n"+
				$"        .Y = {dp.BottomRight.Y.ToString("##0.####")}\n"+
				$"        .Z = {dp.BottomRight.Z.ToString("##0.####")}\n"+
				$"    .upper-left\n"+
				$"        .X = {dp.TopLeft.X.ToString("##0.####")}\n"+
				$"        .Y = {dp.TopLeft.Y.ToString("##0.####")}\n"+
				$"        .Z = {dp.TopLeft.Z.ToString("##0.####")}\n"+
				$"    .upper-right\n"+
				$"        .X = {dp.TopRight.X.ToString("##0.####")}\n"+
				$"        .Y = {dp.TopRight.Y.ToString("##0.####")}\n"+
				$"        .Z = {dp.TopRight.Z.ToString("##0.####")}\n"+
				$"    .normal\n"+
				$"        .X = {dp.Normal.X.ToString( "##0.####" )}\n"+
				$"        .Y = {dp.Normal.Y.ToString( "##0.####" )}\n"+
				$"        .Z = {dp.Normal.Z.ToString( "##0.####" )}\n"+
				$"    .area = {XYZUnitConversionData.FormatArea(dp.Area, FindSurfaceRevitPlugin.MeasurementUnit, "##0.####")}\n\n";
			this.richTextBoxInspectResult.AppendText( content );
		}
		private void Write(DirectSphere ds)
		{
			string content=$"{ds.Name}\n"+
				$"    .center\n"+
				$"        .X = {ds.Center.X.ToString("##0.####")}\n"+
				$"        .Y = {ds.Center.Y.ToString("##0.####")}\n"+
				$"        .Z = {ds.Center.Z.ToString("##0.####")}\n"+
				$"    .radius = {XYZUnitConversionData.FormatLength(ds.Radius, FindSurfaceRevitPlugin.MeasurementUnit, "##0.####")}\n"+
				$"    .surface-area = {XYZUnitConversionData.FormatArea(ds.SurfaceArea, FindSurfaceRevitPlugin.MeasurementUnit, "##0.####")}\n"+
				$"    .volume = {XYZUnitConversionData.FormatVolume(ds.Volume, FindSurfaceRevitPlugin.MeasurementUnit, "##0.####")}\n\n";
			this.richTextBoxInspectResult.AppendText( content );
		}
		private void Write(DirectCylinder dc)
		{
			string content=$"{dc.Name}\n"+
				$"    .top-center\n"+
				$"        .X = {dc.Top.X.ToString("##0.####")}\n"+
				$"        .Y = {dc.Top.Y.ToString("##0.####")}\n"+
				$"        .Z = {dc.Top.Z.ToString("##0.####")}\n"+
				$"    .bottom-center\n"+
				$"        .X = {dc.Bottom.X.ToString("##0.####")}\n"+
				$"        .Y = {dc.Bottom.Y.ToString("##0.####")}\n"+
				$"        .Z = {dc.Bottom.Z.ToString("##0.####")}\n"+
				$"    .center\n"+
				$"        .X = {dc.Center.X.ToString("##0.####")}\n"+
				$"        .Y = {dc.Center.Y.ToString("##0.####")}\n"+
				$"        .Z = {dc.Center.Z.ToString("##0.####")}\n"+
				$"    .radius = {XYZUnitConversionData.FormatLength(dc.Radius, FindSurfaceRevitPlugin.MeasurementUnit, "##0.####")}\n"+
				$"    .length = {XYZUnitConversionData.FormatLength(dc.Length, FindSurfaceRevitPlugin.MeasurementUnit, "##0.####")}\n"+
				$"    .lateral-area = {XYZUnitConversionData.FormatArea(dc.LateralArea, FindSurfaceRevitPlugin.MeasurementUnit, "##0.####")}\n"+
				$"    .surface-area = {XYZUnitConversionData.FormatArea(dc.SurfaceArea, FindSurfaceRevitPlugin.MeasurementUnit, "##0.####")}\n"+
				$"    .volume = {XYZUnitConversionData.FormatVolume(dc.Volume, FindSurfaceRevitPlugin.MeasurementUnit, "##0.####")}\n\n";
			this.richTextBoxInspectResult.AppendText( content );
		}
		private void Write(DirectCone dc)
		{
			string length_unit_string=" "+XYZUnitConversionData.LengthUnitStrings[FindSurfaceRevitPlugin.MeasurementUnit];
			string area_unit_string=" "+XYZUnitConversionData.AreaUnitStrings[FindSurfaceRevitPlugin.MeasurementUnit];
			string volume_unit_string=" "+XYZUnitConversionData.VolumeUnitStrings[FindSurfaceRevitPlugin.MeasurementUnit];

			string content=$"{dc.Name}\n"+
				$"    .top-center\n"+
				$"        .X = {dc.Top.X.ToString("##0.####")}\n"+
				$"        .Y = {dc.Top.Y.ToString("##0.####")}\n"+
				$"        .Z = {dc.Top.Z.ToString("##0.####")}\n"+
				$"    .bottom-center\n"+
				$"        .X = {dc.Bottom.X.ToString("##0.####")}\n"+
				$"        .Y = {dc.Bottom.Y.ToString("##0.####")}\n"+
				$"        .Z = {dc.Bottom.Z.ToString("##0.####")}\n"+
				$"    .center\n"+
				$"        .X = {dc.Center.X.ToString("##0.####")}\n"+
				$"        .Y = {dc.Center.Y.ToString("##0.####")}\n"+
				$"        .Z = {dc.Center.Z.ToString("##0.####")}\n"+
				$"    .top-radius = {XYZUnitConversionData.FormatLength(dc.TopRadius, FindSurfaceRevitPlugin.MeasurementUnit, "##0.####")}\n"+
				$"    .bottom-radius = {XYZUnitConversionData.FormatLength(dc.BottomRadius, FindSurfaceRevitPlugin.MeasurementUnit, "##0.####")}\n"+
				$"    .aperture = {(dc.Aperture*180/Math.PI).ToString("##0.####")+" deg."}\n"+
				$"    .lateral-area = {XYZUnitConversionData.FormatArea(dc.LateralArea, FindSurfaceRevitPlugin.MeasurementUnit, "##0.####")}\n"+
				$"    .surface-area = {XYZUnitConversionData.FormatArea(dc.SurfaceArea, FindSurfaceRevitPlugin.MeasurementUnit, "##0.####")}\n"+
				$"    .volume = {XYZUnitConversionData.FormatVolume(dc.Volume, FindSurfaceRevitPlugin.MeasurementUnit, "##0.####")}\n\n";
			this.richTextBoxInspectResult.AppendText( content );
		}
		private void Write(DirectTorus dt)
		{
			string length_unit_string=" "+XYZUnitConversionData.LengthUnitStrings[FindSurfaceRevitPlugin.MeasurementUnit];
			string area_unit_string=" "+XYZUnitConversionData.AreaUnitStrings[FindSurfaceRevitPlugin.MeasurementUnit];
			string volume_unit_string=" "+XYZUnitConversionData.VolumeUnitStrings[FindSurfaceRevitPlugin.MeasurementUnit];

			string content=$"{dt.Name}\n"+
				$"    .center\n"+
				$"        .X = {dt.Center.X.ToString("##0.####")}\n"+
				$"        .Y = {dt.Center.Y.ToString("##0.####")}\n"+
				$"        .Z = {dt.Center.Z.ToString("##0.####")}\n"+
				$"    .axis = {dt.Axis.ToString()}\n"+
				$"        .X = {dt.Axis.X.ToString("##0.####")}\n"+
				$"        .Y = {dt.Axis.Y.ToString("##0.####")}\n"+
				$"        .Z = {dt.Axis.Z.ToString("##0.####")}\n"+
				$"    .mean-radius = {dt.MeanRadius.ToString("##0.####")+length_unit_string}\n"+
				$"    .tube-radius = {dt.TubeRadius.ToString("##0.####")+length_unit_string}\n";
			if(dt.HasAnElbow)
			{
				XYZ t=(dt.TubeBegin+dt.Center);
				content+=$"    .tube-begin\n"+
					$"        .X = {t.X.ToString( "##0.####" )}\n"+
					$"        .Y = {t.Y.ToString( "##0.####" )}\n"+
					$"        .Z = {t.Z.ToString( "##0.####" )}\n"+
					$"    .tube-angle = {(dt.TubeAngle*180/Math.PI).ToString("##0.####")+" deg."}\n";
			}
			content+=$"    .surface-area = {dt.SurfaceArea.ToString("##0.####")+area_unit_string}\n"+
				$"    .volume = {dt.Volume.ToString("##0.####")+volume_unit_string}\n\n";
			this.richTextBoxInspectResult.AppendText( content );
		}
		private void Clear(object sender, EventArgs e)
		{
			this.richTextBoxInspectResult.Clear();
		}
	}
}
