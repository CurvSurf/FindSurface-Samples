using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindSurfaceRevitPlugin
{
	public enum Units
	{
		None, Millimeter, Centimeter, Meter, Kilometer, Inch, Feet, Yards, Miles
	}

	public class XYZUnitConversionData
	{
		private static Dictionary<Units, string> s_length_unit_strings=new Dictionary<Units, string>() { { Units.None, "" }, { Units.Millimeter, "mm" }, { Units.Centimeter, "cm" }, { Units.Meter, "m" }, { Units.Kilometer, "km" }, { Units.Inch, "in" }, { Units.Feet, "ft" }, { Units.Yards, "yd" }, { Units.Miles, "mi" } };
		private static Dictionary<Units, string> s_area_unit_strings=new Dictionary<Units, string>() { { Units.None, "" }, { Units.Millimeter, "mm²" }, { Units.Centimeter, "cm²" }, { Units.Meter, "m²" }, { Units.Kilometer, "km²" }, { Units.Inch, "sq in" }, { Units.Feet, "sq ft" }, { Units.Yards, "sq yd" }, { Units.Miles, "sq mi" } };
		private static Dictionary<Units, string> s_volume_unit_strings=new Dictionary<Units, string>() { { Units.None, "" }, { Units.Millimeter, "mm³" }, { Units.Centimeter, "cm³" }, { Units.Meter, "m³" }, { Units.Kilometer, "km³" }, { Units.Inch, "cu in" }, { Units.Feet, "cu ft" }, { Units.Yards, "cu yd" }, { Units.Miles, "cu mi" } };

		public static Dictionary<Units, string> LengthUnitStrings { get { return s_length_unit_strings; } }
		public static Dictionary<Units, string> AreaUnitStrings { get { return s_area_unit_strings; } }
		public static Dictionary<Units, string> VolumeUnitStrings { get { return s_volume_unit_strings; } }

		private static readonly decimal[,] unit_conversion_table = new decimal[,]
		{
			{       1m,	   0.1m,   0.001m,  0.000001m,    0.0393700787m,       0.0032808399m,       0.0010936133m,    0.0000006213688756m },
			{      10m,	     1m,    0.01m,   0.00001m,    0.3937007874m,        0.032808399m,        0.010936133m,          0.0000062137m },
			{    1000m,	   100m,       1m,     0.001m,     39.37007874m,        3.280839895m,       1.0936132983m,          0.0006213689m },
			{ 1000000m, 100000m,    1000m,         1m,     39370.07874m,        3280.839895m,       1093.6132983m,          0.6213688756m },
			{    25.4m,	  2.54m,  0.0254m, 0.0000254m,               1m, 0.0833333333333333m, 0.0277777777778889m, 0.0000157827694411418m },
			{   304.8m,	 30.48m,  0.3048m, 0.0003048m,              12m,                  1m,  0.333333333334667m,  0.000189393233293702m },
			{   914.4m,	 91.44m,  0.9144m, 0.0009144m, 35.999999999856m,     2.999999999988m,                  1m,  0.000568179699898424m },
			{ 1609350m, 160935m, 1609.35m,   1.60935m, 63360.236220219m,   5280.01968501825m,   1760.00656161911m,                     1m }
		};

		public static string FormatLength( double value, Units baseUnit, string format )
		{
			decimal dvalue=System.Convert.ToDecimal(value);

			switch(baseUnit)
			{
				case Units.Millimeter:
				if( value>10.0 ) return Convert( dvalue, baseUnit, Units.Centimeter ).ToString( format )+" "+LengthUnitStrings[Units.Centimeter];
				if( value>1000.0 ) return Convert( dvalue, baseUnit, Units.Meter ).ToString( format )+" "+LengthUnitStrings[Units.Meter];
				if( value>1000000.0 ) return Convert( dvalue, baseUnit, Units.Kilometer ).ToString( format )+" "+LengthUnitStrings[Units.Kilometer];
				return dvalue.ToString( format )+" "+LengthUnitStrings[baseUnit];

				case Units.Centimeter:
				if( value<1.0 ) return Convert( dvalue, baseUnit, Units.Millimeter ).ToString( format )+" "+LengthUnitStrings[Units.Millimeter];
				if( value>100.0 ) return Convert( dvalue, baseUnit, Units.Meter ).ToString( format )+" "+LengthUnitStrings[Units.Meter];
				if( value>100000.0 ) return Convert( dvalue, baseUnit, Units.Kilometer ).ToString( format )+" "+LengthUnitStrings[Units.Kilometer];
				return dvalue.ToString( format )+" "+LengthUnitStrings[baseUnit];
				 
				case Units.Meter:
				if( value<0.01 ) return Convert( dvalue, baseUnit, Units.Millimeter ).ToString( format )+" "+LengthUnitStrings[Units.Millimeter];
				if( value<1.0 ) return Convert( dvalue, baseUnit, Units.Centimeter ).ToString( format )+" "+LengthUnitStrings[Units.Centimeter];
				if( value>1000.0 ) return Convert( dvalue, baseUnit, Units.Kilometer ).ToString( format )+" "+LengthUnitStrings[Units.Kilometer];
				return dvalue.ToString( format )+" "+LengthUnitStrings[baseUnit];

				case Units.Kilometer:
				if( value<0.00001 ) return Convert( dvalue, baseUnit, Units.Millimeter ).ToString( format )+" "+LengthUnitStrings[Units.Millimeter];
				if( value<0.001 ) return Convert( dvalue, baseUnit, Units.Centimeter ).ToString( format )+" "+LengthUnitStrings[Units.Centimeter];
				if( value<1.0 ) return Convert( dvalue, baseUnit, Units.Meter ).ToString( format )+" "+LengthUnitStrings[Units.Meter];
				return dvalue.ToString( format )+" "+LengthUnitStrings[baseUnit];

				case Units.Inch:
				if( value>12.0 ) return Convert( dvalue, baseUnit, Units.Feet ).ToString( format )+" "+LengthUnitStrings[Units.Feet];
				if( value>36.0 ) return Convert( dvalue, baseUnit, Units.Yards ).ToString( format )+" "+LengthUnitStrings[Units.Yards];
				if( value>63360.0 ) return Convert( dvalue, baseUnit, Units.Miles ).ToString( format )+" "+LengthUnitStrings[Units.Miles];
				return dvalue.ToString( format )+" "+LengthUnitStrings[baseUnit];

				case Units.Feet:
				if( value<1.0 ) return Convert( dvalue, baseUnit, Units.Inch ).ToString( format )+" "+LengthUnitStrings[Units.Inch];
				if( value>3.0 ) return Convert( dvalue, baseUnit, Units.Yards ).ToString( format )+" "+LengthUnitStrings[Units.Yards];
				if( value>5280.0 ) return Convert( dvalue, baseUnit, Units.Miles ).ToString( format )+" "+LengthUnitStrings[Units.Miles];
				return dvalue.ToString( format )+" "+LengthUnitStrings[baseUnit];

				case Units.Yards:
				if( value<0.333334 ) return Convert( dvalue, baseUnit, Units.Inch ).ToString( format )+" "+LengthUnitStrings[Units.Inch];
				if( value<1.0 ) return Convert( dvalue, baseUnit, Units.Feet ).ToString( format )+" "+LengthUnitStrings[Units.Feet];
				if( value>1760 ) return Convert( dvalue, baseUnit, Units.Miles ).ToString( format )+" "+LengthUnitStrings[Units.Miles];
				return dvalue.ToString( format )+" "+LengthUnitStrings[baseUnit];

				case Units.Miles:
				if( value<0.000189393233293702 ) return Convert( dvalue, baseUnit, Units.Inch ).ToString( format )+" "+LengthUnitStrings[Units.Inch];
				if( value<0.000568179699898424 ) return Convert( dvalue, baseUnit, Units.Feet ).ToString( format )+" "+LengthUnitStrings[Units.Feet];
				if( value<1.0 ) return Convert( dvalue, baseUnit, Units.Yards ).ToString( format )+" "+LengthUnitStrings[Units.Yards];
				return dvalue.ToString( format )+" "+LengthUnitStrings[baseUnit];

				default: return value.ToString( format );
			}
		}

		public static string FormatArea( double value, Units baseUnit, string format )
		{
			decimal dvalue=System.Convert.ToDecimal(value);

			switch( baseUnit )
			{
				case Units.Millimeter:
				if( value>10.0 ) return Convert( dvalue, baseUnit, Units.Centimeter ).ToString( format )+" "+AreaUnitStrings[Units.Centimeter];
				if( value>1000.0 ) return Convert( dvalue, baseUnit, Units.Meter ).ToString( format )+" "+AreaUnitStrings[Units.Meter];
				if( value>1000000.0 ) return Convert( dvalue, baseUnit, Units.Kilometer ).ToString( format )+" "+AreaUnitStrings[Units.Kilometer];
				return dvalue.ToString( format )+" "+AreaUnitStrings[baseUnit];

				case Units.Centimeter:
				if( value<1.0 ) return Convert( dvalue, baseUnit, Units.Millimeter ).ToString( format )+" "+AreaUnitStrings[Units.Millimeter];
				if( value>100.0 ) return Convert( dvalue, baseUnit, Units.Meter ).ToString( format )+" "+AreaUnitStrings[Units.Meter];
				if( value>100000.0 ) return Convert( dvalue, baseUnit, Units.Kilometer ).ToString( format )+" "+AreaUnitStrings[Units.Kilometer];
				return dvalue.ToString( format )+" "+AreaUnitStrings[baseUnit];

				case Units.Meter:
				if( value<0.01 ) return Convert( dvalue, baseUnit, Units.Millimeter ).ToString( format )+" "+AreaUnitStrings[Units.Millimeter];
				if( value<1.0 ) return Convert( dvalue, baseUnit, Units.Centimeter ).ToString( format )+" "+AreaUnitStrings[Units.Centimeter];
				if( value>1000.0 ) return Convert( dvalue, baseUnit, Units.Kilometer ).ToString( format )+" "+AreaUnitStrings[Units.Kilometer];
				return dvalue.ToString( format )+" "+AreaUnitStrings[baseUnit];

				case Units.Kilometer:
				if( value<0.00001 ) return Convert( dvalue, baseUnit, Units.Millimeter ).ToString( format )+" "+AreaUnitStrings[Units.Millimeter];
				if( value<0.001 ) return Convert( dvalue, baseUnit, Units.Centimeter ).ToString( format )+" "+AreaUnitStrings[Units.Centimeter];
				if( value<1.0 ) return Convert( dvalue, baseUnit, Units.Meter ).ToString( format )+" "+AreaUnitStrings[Units.Meter];
				return dvalue.ToString( format )+" "+AreaUnitStrings[baseUnit];

				case Units.Inch:
				if( value>12.0 ) return Convert( dvalue, baseUnit, Units.Feet ).ToString( format )+" "+AreaUnitStrings[Units.Feet];
				if( value>36.0 ) return Convert( dvalue, baseUnit, Units.Yards ).ToString( format )+" "+AreaUnitStrings[Units.Yards];
				if( value>63360.0 ) return Convert( dvalue, baseUnit, Units.Miles ).ToString( format )+" "+AreaUnitStrings[Units.Miles];
				return dvalue.ToString( format )+" "+AreaUnitStrings[baseUnit];

				case Units.Feet:
				if( value<1.0 ) return Convert( dvalue, baseUnit, Units.Inch ).ToString( format )+" "+AreaUnitStrings[Units.Inch];
				if( value>3.0 ) return Convert( dvalue, baseUnit, Units.Yards ).ToString( format )+" "+AreaUnitStrings[Units.Yards];
				if( value>5280.0 ) return Convert( dvalue, baseUnit, Units.Miles ).ToString( format )+" "+AreaUnitStrings[Units.Miles];
				return dvalue.ToString( format )+" "+AreaUnitStrings[baseUnit];

				case Units.Yards:
				if( value<0.333334 ) return Convert( dvalue, baseUnit, Units.Inch ).ToString( format )+" "+AreaUnitStrings[Units.Inch];
				if( value<1.0 ) return Convert( dvalue, baseUnit, Units.Feet ).ToString( format )+" "+AreaUnitStrings[Units.Feet];
				if( value>1760 ) return Convert( dvalue, baseUnit, Units.Miles ).ToString( format )+" "+AreaUnitStrings[Units.Miles];
				return dvalue.ToString( format )+" "+AreaUnitStrings[baseUnit];

				case Units.Miles:
				if( value<0.000189393233293702 ) return Convert( dvalue, baseUnit, Units.Inch ).ToString( format )+" "+AreaUnitStrings[Units.Inch];
				if( value<0.000568179699898424 ) return Convert( dvalue, baseUnit, Units.Feet ).ToString( format )+" "+AreaUnitStrings[Units.Feet];
				if( value<1.0 ) return Convert( dvalue, baseUnit, Units.Yards ).ToString( format )+" "+AreaUnitStrings[Units.Yards];
				return dvalue.ToString( format )+" "+AreaUnitStrings[baseUnit];

				default: return value.ToString( format );
			}
		}

		public static string FormatVolume( double value, Units baseUnit, string format )
		{
			decimal dvalue=System.Convert.ToDecimal(value);

			switch( baseUnit )
			{
				case Units.Millimeter:
				if( value>10.0 ) return Convert( dvalue, baseUnit, Units.Centimeter ).ToString( format )+" "+VolumeUnitStrings[Units.Centimeter];
				if( value>1000.0 ) return Convert( dvalue, baseUnit, Units.Meter ).ToString( format )+" "+VolumeUnitStrings[Units.Meter];
				if( value>1000000.0 ) return Convert( dvalue, baseUnit, Units.Kilometer ).ToString( format )+" "+VolumeUnitStrings[Units.Kilometer];
				return dvalue.ToString( format )+" "+VolumeUnitStrings[baseUnit];

				case Units.Centimeter:
				if( value<1.0 ) return Convert( dvalue, baseUnit, Units.Millimeter ).ToString( format )+" "+VolumeUnitStrings[Units.Millimeter];
				if( value>100.0 ) return Convert( dvalue, baseUnit, Units.Meter ).ToString( format )+" "+VolumeUnitStrings[Units.Meter];
				if( value>100000.0 ) return Convert( dvalue, baseUnit, Units.Kilometer ).ToString( format )+" "+VolumeUnitStrings[Units.Kilometer];
				return dvalue.ToString( format )+" "+VolumeUnitStrings[baseUnit];

				case Units.Meter:
				if( value<0.01 ) return Convert( dvalue, baseUnit, Units.Millimeter ).ToString( format )+" "+VolumeUnitStrings[Units.Millimeter];
				if( value<1.0 ) return Convert( dvalue, baseUnit, Units.Centimeter ).ToString( format )+" "+VolumeUnitStrings[Units.Centimeter];
				if( value>1000.0 ) return Convert( dvalue, baseUnit, Units.Kilometer ).ToString( format )+" "+VolumeUnitStrings[Units.Kilometer];
				return dvalue.ToString( format )+" "+VolumeUnitStrings[baseUnit];

				case Units.Kilometer:
				if( value<0.00001 ) return Convert( dvalue, baseUnit, Units.Millimeter ).ToString( format )+" "+VolumeUnitStrings[Units.Millimeter];
				if( value<0.001 ) return Convert( dvalue, baseUnit, Units.Centimeter ).ToString( format )+" "+VolumeUnitStrings[Units.Centimeter];
				if( value<1.0 ) return Convert( dvalue, baseUnit, Units.Meter ).ToString( format )+" "+VolumeUnitStrings[Units.Meter];
				return dvalue.ToString( format )+" "+VolumeUnitStrings[baseUnit];

				case Units.Inch:
				if( value>12.0 ) return Convert( dvalue, baseUnit, Units.Feet ).ToString( format )+" "+VolumeUnitStrings[Units.Feet];
				if( value>36.0 ) return Convert( dvalue, baseUnit, Units.Yards ).ToString( format )+" "+VolumeUnitStrings[Units.Yards];
				if( value>63360.0 ) return Convert( dvalue, baseUnit, Units.Miles ).ToString( format )+" "+VolumeUnitStrings[Units.Miles];
				return dvalue.ToString( format )+" "+VolumeUnitStrings[baseUnit];

				case Units.Feet:
				if( value<1.0 ) return Convert( dvalue, baseUnit, Units.Inch ).ToString( format )+" "+VolumeUnitStrings[Units.Inch];
				if( value>3.0 ) return Convert( dvalue, baseUnit, Units.Yards ).ToString( format )+" "+VolumeUnitStrings[Units.Yards];
				if( value>5280.0 ) return Convert( dvalue, baseUnit, Units.Miles ).ToString( format )+" "+VolumeUnitStrings[Units.Miles];
				return dvalue.ToString( format )+" "+VolumeUnitStrings[baseUnit];

				case Units.Yards:
				if( value<0.333334 ) return Convert( dvalue, baseUnit, Units.Inch ).ToString( format )+" "+VolumeUnitStrings[Units.Inch];
				if( value<1.0 ) return Convert( dvalue, baseUnit, Units.Feet ).ToString( format )+" "+VolumeUnitStrings[Units.Feet];
				if( value>1760 ) return Convert( dvalue, baseUnit, Units.Miles ).ToString( format )+" "+VolumeUnitStrings[Units.Miles];
				return dvalue.ToString( format )+" "+VolumeUnitStrings[baseUnit];

				case Units.Miles:
				if( value<0.000189393233293702 ) return Convert( dvalue, baseUnit, Units.Inch ).ToString( format )+" "+VolumeUnitStrings[Units.Inch];
				if( value<0.000568179699898424 ) return Convert( dvalue, baseUnit, Units.Feet ).ToString( format )+" "+VolumeUnitStrings[Units.Feet];
				if( value<1.0 ) return Convert( dvalue, baseUnit, Units.Yards ).ToString( format )+" "+VolumeUnitStrings[Units.Yards];
				return dvalue.ToString( format )+" "+VolumeUnitStrings[baseUnit];

				default: return value.ToString( format );
			}
		}

		public static decimal Convert(decimal value, Units from, Units to)
		{
			return value*unit_conversion_table[(int)from-1, (int)to-1];
		}

		private Units m_measuring_unit=Units.None;
		private float m_scale=1.0f;
		private float[] m_origin;

		public Units MeasuringUnit { get { return m_measuring_unit; } set { m_measuring_unit=value; } }
		public float Scale { get { return m_scale; } set { m_scale=value; } }
		public float[] OriginOffset { get { return m_origin; } }
		public void SetOriginOffset(float x, float y, float z ) { m_origin=new float[] { x, y, z }; }
	}
}
