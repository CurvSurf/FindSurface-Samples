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
	public partial class XYZUnitConversionForm : Form
	{
		private float extent_x;
		private float extent_y;
		private float extent_z;
		private float center_x;
		private float center_y;
		private float center_z;
		private XYZUnitConversionData m_conversion_data;

		private static string s_comboBoxUnitsItemMillimeter="mm";
		private static string s_comboBoxUnitsItemCentimeter="cm";
		private static string s_comboBoxUnitsItemMeter="m";
		private static string s_comboBoxUnitsItemKilometer="km";
		private static string s_comboBoxUnitsItemInch="in";
		private static string s_comboBoxUnitsItemFeet="ft";
		private static string s_comboBoxUnitsItemYards="yd";
		private static string s_comboBoxUnitsItemMiles="mi";
		private static string[] s_comboBoxUnitsItems=new string[]
		{
			s_comboBoxUnitsItemMillimeter,
			s_comboBoxUnitsItemCentimeter,
			s_comboBoxUnitsItemMeter,
			s_comboBoxUnitsItemKilometer,
			s_comboBoxUnitsItemInch,
			s_comboBoxUnitsItemFeet,
			s_comboBoxUnitsItemYards,
			s_comboBoxUnitsItemMiles
		};

		public XYZUnitConversionForm(float[] extent, float[] center, XYZUnitConversionData conversion_data)
		{
			extent_x=extent[0];
			extent_y=extent[1];
			extent_z=extent[2];
			center_x=center[0];
			center_y=center[1];
			center_z=center[2];
			m_conversion_data=conversion_data;

			InitializeComponent();

			textBoxWidth.Text=extent_x.ToString( "##0.####" );
			textBoxDepth.Text=extent_y.ToString( "##0.####" );
			textBoxHeight.Text=extent_z.ToString( "##0.####" );

			textBoxOriginX.Text=center_x.ToString( "##0.####" );
			textBoxOriginY.Text=center_y.ToString( "##0.####" );
			textBoxOriginZ.Text=center_z.ToString( "##0.####" );

			comboBoxUnits.Items.Add( s_comboBoxUnitsItems[0] );
			comboBoxUnits.Items.Add( s_comboBoxUnitsItems[1] );
			comboBoxUnits.Items.Add( s_comboBoxUnitsItems[2] );
			comboBoxUnits.Items.Add( s_comboBoxUnitsItems[3] );
		
		}

		private void radioButtonUnits_CheckedChanged( object sender, EventArgs e )
		{
			if(radioButtonSIUnits.Checked)
			{
				comboBoxUnits.Items.Clear();
				comboBoxUnits.Items.Add( s_comboBoxUnitsItems[0] );
				comboBoxUnits.Items.Add( s_comboBoxUnitsItems[1] );
				comboBoxUnits.Items.Add( s_comboBoxUnitsItems[2] );
				comboBoxUnits.Items.Add( s_comboBoxUnitsItems[3] );
			}
			else if(radioButtonUSUnits.Checked)
			{
				comboBoxUnits.Items.Clear();
				comboBoxUnits.Items.Add( s_comboBoxUnitsItems[4] );
				comboBoxUnits.Items.Add( s_comboBoxUnitsItems[5] );
				comboBoxUnits.Items.Add( s_comboBoxUnitsItems[6] );
				comboBoxUnits.Items.Add( s_comboBoxUnitsItems[7] );
			}
		}

		private void checkBoxMoveToZeros_CheckedChanged( object sender, EventArgs e )
		{
			if(checkBoxMoveToZeros.Checked)
			{
				textBoxOriginX.Text="0.0000";
				textBoxOriginX.Enabled=false;
				textBoxOriginY.Text="0.0000";
				textBoxOriginY.Enabled=false;
				textBoxOriginZ.Text="0.0000";
				textBoxOriginZ.Enabled=false;
			}
			else
			{
				textBoxOriginX.Text=center_x.ToString("##0.####");
				textBoxOriginX.Enabled=true;
				textBoxOriginY.Text=center_y.ToString("##0.####");
				textBoxOriginY.Enabled=true;
				textBoxOriginZ.Text=center_z.ToString("##0.####");
				textBoxOriginZ.Enabled=true;
			}
			
		}

		private void buttonCancel_Click( object sender, EventArgs e )
		{
			this.DialogResult=DialogResult.Cancel;
			this.Close();
		}

		private void buttonOK_Click( object sender, EventArgs e )
		{
			m_conversion_data.Scale=float.Parse( textBoxScale.Text );
			switch(comboBoxUnits.SelectedIndex)
			{
				case 0:	m_conversion_data.MeasuringUnit=Units.Millimeter; break;
				case 1:	m_conversion_data.MeasuringUnit=Units.Centimeter; break;
				case 2:	m_conversion_data.MeasuringUnit=Units.Meter; break;
				case 3:	m_conversion_data.MeasuringUnit=Units.Kilometer; break;
				case 4:	m_conversion_data.MeasuringUnit=Units.Inch; break;
				case 5:	m_conversion_data.MeasuringUnit=Units.Feet; break;
				case 6:	m_conversion_data.MeasuringUnit=Units.Yards; break;
				case 7:	m_conversion_data.MeasuringUnit=Units.Miles; break;
				default: m_conversion_data.MeasuringUnit=Units.None; break;
			}
			float x=float.Parse(textBoxOriginX.Text);
			float y=float.Parse(textBoxOriginY.Text);
			float z=float.Parse(textBoxOriginZ.Text);
			m_conversion_data.SetOriginOffset( center_x-x, center_y-y, center_z-z );

			this.DialogResult=DialogResult.OK;
			this.Close();
		}

		private void textBoxOriginX_KeyPress( object sender, KeyPressEventArgs e )
		{
			if( char.IsControl( e.KeyChar )==false&&
				char.IsDigit( e.KeyChar )==false&&
				e.KeyChar!='.' )
			{
				e.Handled=true;
			}

			if( e.KeyChar=='.'&&textBoxOriginX.Text.IndexOf('.')>-1 )
			{
				e.Handled=true;
			}
		}

		private void textBoxOriginY_KeyPress( object sender, KeyPressEventArgs e )
		{
			if( char.IsControl( e.KeyChar )==false&&
				char.IsDigit( e.KeyChar )==false&&
				e.KeyChar!='.' )
			{
				e.Handled=true;
			}

			if( e.KeyChar=='.'&&textBoxOriginY.Text.IndexOf( '.' )>-1 )
			{
				e.Handled=true;
			}
		}

		private void textBoxOriginZ_KeyPress( object sender, KeyPressEventArgs e )
		{
			if( char.IsControl( e.KeyChar )==false&&
				char.IsDigit( e.KeyChar )==false&&
				e.KeyChar!='.' )
			{
				e.Handled=true;
			}

			if( e.KeyChar=='.'&&textBoxOriginZ.Text.IndexOf( '.' )>-1 )
			{
				e.Handled=true;
			}
		}

		private void textBoxScale_KeyPress( object sender, KeyPressEventArgs e )
		{
			if( char.IsControl( e.KeyChar )==false&&
				char.IsDigit( e.KeyChar )==false&&
				e.KeyChar!='.' )
			{
				e.Handled=true;
			}

			if( e.KeyChar=='.'&&textBoxScale.Text.IndexOf( '.' )>-1 )
			{
				e.Handled=true;
			}
		}

		private void comboBoxListUnits_KeyPress( object sender, KeyPressEventArgs e )
		{
			e.Handled=true;
		}

		private void comboBoxUnits_SelectedIndexChanged( object sender, EventArgs e )
		{
		}
	}
}
