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
	public partial class SettingsForm : Form
	{
		private static decimal s_accuracy = 0.005m;
		private static decimal s_touch_radius = 0.05m;
		private static decimal s_mean_distance = 0.005m;
		private static decimal s_cone_2_cyl = 10.0m;
		private static decimal s_rad_expansion = 5;
		private static decimal s_lat_extension = 5;

		private static SteadyPositioning s_positioning=new SteadyPositioning();

		private void LoadParameters()
		{
			this.textBoxAccuracy.Text=s_accuracy.ToString();
			this.textBoxTouchRadius.Text=s_touch_radius.ToString();
			this.textBoxMeanDistance.Text=s_mean_distance.ToString();
			this.textBoxCone2Cyl.Text=s_cone_2_cyl.ToString();
			this.textBoxLatExtension.Text=s_lat_extension.ToString();
			this.trackBarLatExtension.Value=Convert.ToInt32( s_lat_extension );
			this.textBoxRadExpansion.Text=s_rad_expansion.ToString();
			this.trackBarRadExpansion.Value=Convert.ToInt32( s_rad_expansion );
		}
		
		private void SaveParameters()
		{
			s_accuracy=decimal.Parse( textBoxAccuracy.Text );
			s_touch_radius=decimal.Parse( textBoxTouchRadius.Text );
			s_mean_distance=decimal.Parse( textBoxMeanDistance.Text );
			s_cone_2_cyl=decimal.Parse( textBoxCone2Cyl.Text );
			s_rad_expansion=decimal.Parse( textBoxRadExpansion.Text );
			s_lat_extension=decimal.Parse( textBoxLatExtension.Text );
		}

		public SettingsForm()
		{
			InitializeComponent();

			this.FormClosing+=new FormClosingEventHandler( s_positioning.OnFormClosing );
			this.Shown+=new EventHandler( s_positioning.OnShown );

			LoadParameters();
		}

		private void textBoxAccuracy_KeyPress( object sender, KeyPressEventArgs e )
		{
			if( char.IsControl( e.KeyChar )==false&&
				char.IsDigit( e.KeyChar )==false&&
				e.KeyChar!='.' )
			{
				e.Handled=true;
			}

			if( e.KeyChar=='.'&&textBoxAccuracy.Text.IndexOf( '.' )>-1 )
			{
				e.Handled=true;
			}
		}
		private void textBoxAccuracy_TextChanged( object sender, EventArgs e )
		{
			TextBox textBox = textBoxAccuracy;
			if( System.Text.RegularExpressions.Regex.IsMatch( textBoxAccuracy.Text, "^[0-9]*.[0-9]*$" )==false )
			{
				MessageBox.Show( "Please enter only numbers." );
				textBox.Text="0";
			}
		}

		private void textBoxTouchRadius_KeyPress( object sender, KeyPressEventArgs e )
		{
			if( char.IsControl( e.KeyChar )==false&&
				char.IsDigit( e.KeyChar )==false&&
				e.KeyChar!='.' )
			{
				e.Handled=true;
			}

			if( e.KeyChar=='.'&&textBoxTouchRadius.Text.IndexOf( '.' )>-1 )
			{
				e.Handled=true;
			}
		}
		private void textBoxTouchRadius_TextChanged( object sender, EventArgs e )
		{
			TextBox textBox = textBoxTouchRadius;
			if( System.Text.RegularExpressions.Regex.IsMatch( textBox.Text, "^[0-9]*.[0-9]*$" )==false )
			{
				MessageBox.Show( "Please enter only numbers." );
				textBox.Text="0";
			}
		}

		private void textBoxMeanDistance_KeyPress( object sender, KeyPressEventArgs e )
		{
			if( char.IsControl( e.KeyChar )==false&&
				char.IsDigit( e.KeyChar )==false&&
				e.KeyChar!='.' )
			{
				e.Handled=true;
			}

			if( e.KeyChar=='.'&&textBoxMeanDistance.Text.IndexOf( '.' )>-1 )
			{
				e.Handled=true;
			}
		}
		private void textBoxMeanDistance_TextChanged( object sender, EventArgs e )
		{
			TextBox textBox = textBoxMeanDistance;
			if( System.Text.RegularExpressions.Regex.IsMatch( textBox.Text, "^[0-9]*.[0-9]*$" )==false )
			{
				MessageBox.Show( "Please enter only numbers." );
				textBox.Text="0";
			}
		}

		private void textBoxCone2Cyl_KeyPress( object sender, KeyPressEventArgs e )
		{
			if( char.IsControl( e.KeyChar )==false&&
				char.IsDigit( e.KeyChar )==false&&
				e.KeyChar!='.' )
			{
				e.Handled=true;
			}

			if( e.KeyChar=='.'&&textBoxCone2Cyl.Text.IndexOf( '.' )>-1 )
			{
				e.Handled=true;
			}
		}
		private void textBoxCone2Cyl_TextChanged( object sender, EventArgs e )
		{
			TextBox textBox = textBoxCone2Cyl;
			if( System.Text.RegularExpressions.Regex.IsMatch( textBox.Text, "^[0-9]*.[0-9]*$" )==false )
			{
				MessageBox.Show( "Please enter only numbers." );
				textBox.Text="0";
			}
		}

		private void textBoxRadExpansion_KeyPress( object sender, KeyPressEventArgs e )
		{
			TextBox textBox = textBoxRadExpansion;
			if( char.IsControl( e.KeyChar )==false&&
				char.IsDigit( e.KeyChar )==false )
			{
				e.Handled=true;
			}
		}
		private void textBoxRadExpansion_TextChanged( object sender, EventArgs e )
		{
			TextBox textBox = textBoxRadExpansion;
			if( System.Text.RegularExpressions.Regex.IsMatch( textBox.Text, "^[0-9]*.[0-9]*$" )==false )
			{
				MessageBox.Show( "Please enter only numbers." );
				textBox.Text="0";
			}
			textBox.Text=(textBox.Text==string.Empty) ? "0" : textBox.Text;
			textBox.Text=Math.Min( int.Parse( textBox.Text ), trackBarRadExpansion.Maximum ).ToString();
			textBox.Text=Math.Max( int.Parse( textBox.Text ), trackBarRadExpansion.Minimum ).ToString();
		}

		private void trackBarRadExpansion_ValueChanged( object sender, EventArgs e )
		{
			textBoxRadExpansion.Text=trackBarRadExpansion.Value.ToString();
		}

		private void textBoxLatExtension_KeyPress( object sender, KeyPressEventArgs e )
		{
			TextBox textBox = textBoxLatExtension;
			if( char.IsControl( e.KeyChar )==false&&
				char.IsDigit( e.KeyChar )==false )
			{
				e.Handled=true;
			}
		}
		private void textBoxLatExtension_TextChanged( object sender, EventArgs e )
		{
			TextBox textBox = textBoxLatExtension;
			if( System.Text.RegularExpressions.Regex.IsMatch( textBox.Text, "^[0-9]*.[0-9]*$" )==false )
			{
				MessageBox.Show( "Please enter only numbers." );
				textBox.Text="0";
			}
			textBox.Text=Math.Min( int.Parse( textBox.Text ), trackBarLatExtension.Maximum ).ToString();
			textBox.Text=Math.Max( int.Parse( textBox.Text ), trackBarLatExtension.Minimum ).ToString();
		}

		private void trackBarLatExtension_ValueChanged( object sender, EventArgs e )
		{
			textBoxLatExtension.Text=trackBarLatExtension.Value.ToString();
		}

		private void buttonOK_Click( object sender, EventArgs e )
		{
			if( textBoxAccuracy.Text==string.Empty ) { MessageBox.Show( "Accuracy must be specified." ); return; }
			if( textBoxTouchRadius.Text==string.Empty ) { MessageBox.Show( "Touch Radius must be specified." ); return; }
			if( textBoxMeanDistance.Text==string.Empty ) { MessageBox.Show( "Mean Distance must be specified." ); return; }
			if( textBoxCone2Cyl.Text==string.Empty ) { MessageBox.Show( "Cone2Cyl. must be specified." ); return; }
			if( textBoxRadExpansion.Text==string.Empty ) { MessageBox.Show( "Rad. Expansion must be specified." ); return; }
			if( textBoxLatExtension.Text==string.Empty ) { MessageBox.Show( "Lat. Extension must be specified." ); return; }

			SaveParameters();

			FindSurfaceRevitPlugin.FindSurface.Accuracy=(float)s_accuracy;
			FindSurfaceRevitPlugin.FindSurface.TouchRadius=(float)s_touch_radius;
			FindSurfaceRevitPlugin.FindSurface.MeanDistance=(float)s_mean_distance;
			FindSurfaceRevitPlugin.FindSurface.ConeToCylinder=(float)s_cone_2_cyl;
			FindSurfaceRevitPlugin.FindSurface.RadialExpantion=(int)s_rad_expansion;
			FindSurfaceRevitPlugin.FindSurface.LateralExtension=(int)s_lat_extension;

			this.DialogResult=DialogResult.OK;
			this.Close();
		}

		private void buttonCancel_Click( object sender, EventArgs e )
		{
			this.DialogResult=DialogResult.Cancel;
			this.Close();
		}

	}
}
