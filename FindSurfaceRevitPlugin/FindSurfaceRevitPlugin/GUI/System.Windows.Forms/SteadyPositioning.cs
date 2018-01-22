using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace FindSurfaceRevitPlugin
{
	/// <summary>
	/// A utility class of window positioning.
	/// This class stores the position of a window before it is closed,
	/// and move the window to the stored position when it is shown.
	/// </summary>
	class SteadyPositioning
	{
		private bool m_first_shown=true;
		private Point m_last_position;

		public void OnShown( object sender, EventArgs e )
		{
			Form form = sender as Form;
			form.Location=m_first_shown ? GetPositionToBeCentered( GetRevitCenterPosition(), form.Size ) : m_last_position;
			m_first_shown=false;
		}

		public void OnFormClosing( object sender, FormClosingEventArgs e )
		{
			m_last_position=(sender as Form).Location;
		}

		private static Point GetRevitCenterPosition()
		{
			Screen revit_screen = Screen.FromHandle(Process.GetProcessesByName("Revit")[0].MainWindowHandle);
			Point location = revit_screen.WorkingArea.Location;
			Size size = revit_screen.WorkingArea.Size;
			return new Point( location.X+size.Width/2, location.Y+size.Height/2 );
		}

		private static Point GetPositionToBeCentered( Point center, Size size )
		{
			return new Point( center.X-size.Width/2, center.Y-size.Height/2 );
		}
	}
}
