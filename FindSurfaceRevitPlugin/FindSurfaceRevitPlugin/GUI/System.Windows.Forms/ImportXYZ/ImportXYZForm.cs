using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
//using Autodesk.Revit.UI;

namespace FindSurfaceRevitPlugin
{
	public partial class ImportXYZForm : Form
	{
		#region Form Component Parameters
		private static bool s_use_intensity = true;
		private static bool s_use_rgb = false;
		private static bool s_use_rgb_bytes_encoding = false;
		private static bool s_use_no_color = false;
		private static bool s_use_normalization = false;
		private static decimal s_denominator = 32767;
		private static int s_number_split = 10;
		private static string s_number_points_approx_text = "N/A";

		private static string s_last_file_source_text = string.Empty;
		private static string s_last_file_name_text = string.Empty;
		#endregion

		private static SteadyPositioning s_positioning=new SteadyPositioning();

		#region Class Member Variables
		ImportXYZData m_importData;
		#endregion

		#region Class Member Methods
		public void LoadParameters()
		{
			textBoxFileSource.Text=s_last_file_source_text;
			radioButtonIntensity.Checked=s_use_intensity;
			radioButtonRGB.Checked=s_use_rgb;
			radioButtonRGBBytes.Checked=s_use_rgb_bytes_encoding;
			radioButtonNoColor.Checked=s_use_no_color;
			checkBoxNormalization.Checked=s_use_normalization;
			textBoxNormalizationValue.Text=s_denominator.ToString();
			numericUpDownSubdivision.Value=s_number_split;
			labelNumPointsValue.Text=s_number_points_approx_text;
		}
		public void SaveParameters()
		{
			s_last_file_source_text=textBoxFileSource.Text;
			s_use_intensity=radioButtonIntensity.Checked;
			s_use_rgb=radioButtonRGB.Checked;
			s_use_rgb_bytes_encoding=radioButtonRGBBytes.Checked;
			s_use_no_color=radioButtonNoColor.Checked;
			s_use_normalization=checkBoxNormalization.Checked;
			s_denominator=decimal.Parse( textBoxNormalizationValue.Text );
			s_number_split=Convert.ToInt32( numericUpDownSubdivision.Value );
			s_number_points_approx_text=labelNumPointsValue.Text;
		}
		public ImportXYZForm( ImportXYZData importData )
		{
			m_importData=importData;

			InitializeComponent();
			this.FormClosing+=new FormClosingEventHandler( s_positioning.OnFormClosing );
			this.Shown+=new System.EventHandler( s_positioning.OnShown );
			LoadParameters();
		}
		#endregion

		#region Class Event Callbacks
		delegate void labelNumPointsValue_Text( string text );
		private void SetText( string text )
		{
			if( this.IsDisposed ) return;
			if( labelNumPointsValue.InvokeRequired )
			{
				labelNumPointsValue_Text d = new labelNumPointsValue_Text(SetText);
				labelNumPointsValue.Invoke( d, new object[] { text } );
			}
			else { labelNumPointsValue.Text=text; }
		}
		private void buttonBrowser_Click( object sender, EventArgs e )
		{
			string file_name=string.Empty;
			string file_full_name = string.Empty;
			if( ShowXYZFileOpenDialog( m_importData, out file_name, out file_full_name )!=DialogResult.Cancel )
			{
				textBoxFileSource.Text=file_full_name;
				s_last_file_name_text=file_name;

				if( IsFileSizeLessThan250M( file_full_name ) )
					MessageBox.Show(
						"The input file size is greater than 250MBytes. "+
						"Loading massive point cloud data may cause serious performance degradation. "+
						"We recommends you to downsample the input point cloud, "+
						"because FindSurface does not require such huge amount of points to find primitives."
						, "Warning: Loading massive point cloud data" );

				new System.Threading.Thread( new System.Threading.ThreadStart( () =>
				{
					SetText( "Counting" );
					int line_count = File.ReadLines(file_full_name).Count();
					SetText( line_count.ToString() );
				} ) ).Start();
			}
		}

		private void radioButtonIntensity_CheckedChanged( object sender, EventArgs e )
		{
			groupBoxNormalization.Enabled=radioButtonIntensity.Checked;
		}

		private void radioButtonRGB_CheckedChanged( object sender, EventArgs e )
		{
			groupBoxNormalization.Enabled=!radioButtonRGB.Checked;
		}

		private void radioButtonRGBBytes_CheckedChanged( object sender, EventArgs e )
		{
			groupBoxNormalization.Enabled=!radioButtonRGBBytes.Checked;
		}

		private void radioButtonNoColor_CheckedChanged( object sender, EventArgs e )
		{
			groupBoxNormalization.Enabled=!radioButtonNoColor.Checked;
		}

		private void checkBoxNormalization_CheckedChanged( object sender, EventArgs e )
		{
			textBoxNormalizationValue.Enabled=checkBoxNormalization.Checked;
		}

		private void textBoxNormalizationValue_KeyPress( object sender, KeyPressEventArgs e )
		{
			if( char.IsControl( e.KeyChar )==false&&
				char.IsDigit( e.KeyChar )==false&&
				e.KeyChar!='.' )
			{
				e.Handled=true;
			}

			if( e.KeyChar=='.'&&textBoxNormalizationValue.Text.IndexOf( '.' )>-1 )
			{
				e.Handled=true;
			}
		}
		private void textBoxNormalizationValue_TextChanged( object sender, EventArgs e )
		{
			TextBox textBox = textBoxNormalizationValue;
			if( System.Text.RegularExpressions.Regex.IsMatch( textBoxNormalizationValue.Text, "^[0-9]*.[0-9]*$" )==false )
			{
				MessageBox.Show( "Please enter only numbers." );
				textBox.Text="0";
			}
		}

		private void numericUpDownSubdivision_ValueChanged( object sender, EventArgs e )
		{
			int value = Convert.ToInt32(numericUpDownSubdivision.Value);
			int cube = value*value*value;
			textBoxTotalSubdivision.Text=cube.ToString();
		}
		private void numericUpDownSubdivision_KeyPress( object sender, KeyPressEventArgs e )
		{
			if( char.IsControl( e.KeyChar )==false&&
				char.IsDigit( e.KeyChar )==false )
			{
				e.Handled=true;
				numericUpDownSubdivision.Value=Math.Min( numericUpDownSubdivision.Maximum, numericUpDownSubdivision.Value );
			}
		}
		private void numericUpDownSubdivision_KeyDown( object sender, KeyEventArgs e )
		{
			numericUpDownSubdivision.DownButton();
		}
		private void numericUpDownSubdivision_KeyUp( object sender, KeyEventArgs e )
		{
			numericUpDownSubdivision.UpButton();
		}

		private void buttonOpen_Click( object sender, EventArgs e )
		{
			if( checkBoxNormalization.Checked&&textBoxNormalizationValue.Text==string.Empty )
			{
				MessageBox.Show( "Denominator must be specified when it is enabled." );
				return;
			}

			m_importData.SubdivisionFactor=s_number_split;
			if( ValidateFileName() )
			{
				SetImportColorMode();

				try
				{
					m_importData.Import();
				}
				catch( Exception ex )
				{
					MessageBox.Show( "Import Failed: "+ex.ToString() );
					return;
				}

				this.DialogResult=DialogResult.OK;
				this.Close();
			}
		}
		private void buttonCancel_Click( object sender, EventArgs e )
		{
			this.DialogResult=DialogResult.Cancel;
			this.Close();
		}

		public static DialogResult ShowXYZFileOpenDialog( ImportXYZData import_data, out string file_name, out string file_full_name )
		{
			file_name=string.Empty;
			file_full_name=string.Empty;
			using( OpenFileDialog open_file_dialog = new OpenFileDialog() )
			{
				open_file_dialog.Title=import_data.Title;
				open_file_dialog.InitialDirectory=import_data.ImportFolder;
				open_file_dialog.Filter=import_data.Filter;
				open_file_dialog.RestoreDirectory=true;

				DialogResult result = open_file_dialog.ShowDialog();
				if( result!=DialogResult.Cancel )
				{
					file_name=open_file_dialog.SafeFileName;
					file_full_name=open_file_dialog.FileName;
				}
				return result;
			}
		}

		private static bool IsFileSizeLessThan250M( string file_full_path )
		{
			return (250000000U<new FileInfo( file_full_path ).Length);
		}
		#endregion

		#region Class Implementation
		private bool ValidateFileName()
		{
			string fileNameFull=textBoxFileSource.Text;

			if( string.IsNullOrEmpty( fileNameFull ) )
			{
				MessageBox.Show( "Please specify a valid file path." );
				buttonBrowser.Focus();
				return false;
			}

			if( !File.Exists( fileNameFull ) )
			{
				MessageBox.Show( "Please specify an existing file." );
				buttonBrowser.Focus();
				return false;
			}

			m_importData.ImportFileFullName=fileNameFull;
			m_importData.ImportFileName=s_last_file_name_text;
			return true;
		}

		private void SetImportColorMode()
		{
			if( s_use_intensity )
			{
				m_importData.ColorMode=ColorMode.Intensity;
				if( s_use_normalization )
					m_importData.NormalizationFactor=(double)s_denominator;
				else
					m_importData.NormalizationFactor=1.0;
			}
			else if( s_use_rgb_bytes_encoding )
				m_importData.ColorMode=ColorMode.RGB_Byte;
			else if( s_use_rgb )
				m_importData.ColorMode=ColorMode.RGB;
			else
				m_importData.ColorMode=ColorMode.NoColor;
		}
		#endregion
	}
}
