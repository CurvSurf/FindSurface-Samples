using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.PointClouds;
using System.Runtime.Serialization;

namespace FindSurfaceRevitPlugin
{
	public enum ColorMode
	{
		Intensity,
		RGB,
		RGB_Byte,
		NoColor
	}

	public class ImportXYZData : ImportDataBase
	{
		#region Class Member Variables
		private ColorMode m_color_mode;
		private int m_subdivision_factor;
		private double m_normalization_factor;
		private float[] m_import_data_xyz=null;
		private int[] m_import_data_color=null;
		private float[] m_import_data_bounding_box_center=null;
		private float[] m_import_data_bounding_box_extent=null;
		#endregion

		#region Class Properties
		public ColorMode ColorMode { get { return m_color_mode; } set { m_color_mode=value; } }
		public int SubdivisionFactor { get { return m_subdivision_factor; } set { m_subdivision_factor=value; } }
		public double NormalizationFactor { get { return m_normalization_factor; } set { m_normalization_factor=value; } }
		public float[] ImportDataXYZ { get { return m_import_data_xyz; } }
		public int[] ImportDataColor { get { return m_import_data_color; } }
		public float[] ImportDataBoundingBoxCenter { get { return m_import_data_bounding_box_center; } }
		public float[] ImportDataBoundingBoxExtent { get { return m_import_data_bounding_box_extent; } }
		#endregion

		#region Class Member Methods
		public ImportXYZData( Document document ) : base( document )
		{
			Initialize();
		}

		public override void Import()
		{
			System.IO.StreamReader file = new System.IO.StreamReader(m_import_file_full_name);
			int line_count=0; string line;

			List<float> import_data_xyz=new List<float>();
			List<int> import_data_color=new List<int>();
			float mx=float.MaxValue, Mx=-float.MaxValue;
			float my=float.MaxValue, My=-float.MaxValue;
			float mz=float.MaxValue, Mz=-float.MaxValue;
			try
			{
				while( (line=file.ReadLine())!=null )
				{
					float x, y, z;
					int color;
					ConsumeStrings( line, out x, out y, out z, out color );
					
					import_data_xyz.Add( x );
					import_data_xyz.Add( y );
					import_data_xyz.Add( z );
					import_data_color.Add( color );

					mx=mx<x ? mx : x; Mx=Mx>x ? Mx : x;
					my=my<y ? my : y; My=My>y ? My : y;
					mz=mz<z ? mz : z; Mz=Mz>z ? Mz : z;
				}

				float cx=(mx+Mx)*0.5f; cx=Math.Abs( cx )<float.Epsilon ? 0.0f : cx;
				float cy=(my+My)*0.5f; cy=Math.Abs( cy )<float.Epsilon ? 0.0f : cy;
				float cz=(mz+Mz)*0.5f; cz=Math.Abs( cz )<float.Epsilon ? 0.0f : cz;

				m_import_data_bounding_box_extent=new float[] { Mx-mx, My-my, Mz-mz };
				m_import_data_bounding_box_center=new float[] { cx, cy, cz };
				//for(int k=0;k<import_data_xyz.Count; k+=3 )
				//{
				//	import_data_xyz[k]-=cx;
				//	import_data_xyz[k+1]-=cy;
				//	import_data_xyz[k+2]-=cz;
				//}
			}
			catch( XYZFileParseException e )
			{
				e.LineNumber=line_count;
				e.FileName=ImportFileName;

				throw e;
			}
			finally
			{
				file.Close();
			}

			m_import_data_xyz=import_data_xyz.ToArray();
			m_import_data_color=import_data_color.ToArray();
		}
		#endregion

		#region Class Implementation
		private void ConsumeStrings( string l, out float x, out float y, out float z, out int color )
		{
			Func<byte,byte,byte,int> byte3_to_int1 = (b0,b1,b2)=>(b2<<16)+(b1<<8)+b0;

			x=0; y=0; z=0; color=0;

			string[] ls = l.Split( ' ' );
			if( float.TryParse( ls[0], out x )==false ) throw new XYZFileParseException( "Failed to parse X coordinate", ls[0] );
			if( float.TryParse( ls[1], out y )==false ) throw new XYZFileParseException( "Failed to parse Y coordinate", ls[1] );
			if( float.TryParse( ls[2], out z )==false ) throw new XYZFileParseException( "Failed to parse Z coordinate", ls[2] );

			if( ls.Count()>3 )
			{
				switch( m_color_mode )
				{
					case ColorMode.Intensity:
					{
						double dc;
						if( double.TryParse( ls[3], out dc )==false ) throw new XYZFileParseException( "Failed to parse Color", ls[3] );
						byte intensity = (byte)((dc/NormalizationFactor)*byte.MaxValue);
						color=byte3_to_int1( intensity, intensity, intensity );
					}
					break;

					case ColorMode.RGB:
					{
						double[] dc=new double[3];
						if( double.TryParse( ls[3], out dc[0] )==false ) throw new XYZFileParseException( "Failed to parse Color", ls[3] );
						if( double.TryParse( ls[4], out dc[1] )==false ) throw new XYZFileParseException( "Failed to parse Color", ls[4] );
						if( double.TryParse( ls[5], out dc[2] )==false ) throw new XYZFileParseException( "Failed to parse Color", ls[5] );
						byte r=(byte)((dc[0]/NormalizationFactor)*byte.MaxValue);
						byte g=(byte)((dc[1]/NormalizationFactor)*byte.MaxValue);
						byte b=(byte)((dc[2]/NormalizationFactor)*byte.MaxValue);
						color=b<<16+g<<8+b;
					}
					break;

					case ColorMode.RGB_Byte:
					if( int.TryParse( ls[3], out color )==false ) throw new XYZFileParseException( $"Failed to parse Color", ls[3] );
					break;

					case ColorMode.NoColor:
					default:
					break;
				}
			}
		}

		private void Initialize()
		{
			m_filter="XYZ Files (*.xyz)|*.xyz";
			m_title="Import XYZ";
		}
		#endregion
	}

	public class XYZFileParseException : ApplicationException
	{
		private int m_line_number;
		private string m_content;
		private string m_file_name;

		public int LineNumber { get { return m_line_number; } set { m_line_number=value; } }
		public string Content { get { return m_content; } set { m_content=value; } }
		public string FileName { get { return m_file_name; } set { m_file_name=value; } }

		public XYZFileParseException( string message, string content ) : base( message )
		{
			m_content=content;
		}

		public override string ToString()
		{
			return $"{Message}: {Content+Environment.NewLine}at {FileName}({LineNumber}).";
		}
	}
}
