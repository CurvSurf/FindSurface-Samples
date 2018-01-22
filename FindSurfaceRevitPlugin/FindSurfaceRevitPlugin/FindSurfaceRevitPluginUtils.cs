using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.PointClouds;

namespace FindSurfaceRevitPlugin
{
	/// <summary>
	/// Some custom functions that Revit API and c# does not support.
	/// </summary>
	public static class FindSurfaceRevitPluginUtils
	{
		/// <summary>
		/// Calculate an angle between the given vector <code>v0</code> and <code>v1</code>.
		/// The <code>axis</code> gives the direction of measurement according to the Right-hand rule.
		/// The angle is ranged from 0 to 2*PI.
		/// </summary>
		/// <remarks>The result of XYZ.AngleTo() is ranged from 0 to PI.</remarks>
		/// <param name="v0">A vector</param>
		/// <param name="v1">Another vector</param>
		/// <param name="axis">The axis perpendicular to both vectors</param>
		/// <returns>Positive angle between the two vectors</returns>
		public static double GetPositiveAngleBetween( XYZ v0, XYZ v1, XYZ axis )
		{
			double angle=GetAngleBetween( v0, v1, axis );
			return angle<0.0 ? angle+Math.PI*2.0 : angle;
		}

		/// <summary>
		/// Calculate an angle between the given vector <code>v0</code> and <code>v1</code>.
		/// The <code>axis</code> gives the direction of measurement according to the Right-hand rule.
		/// The angle is ranged from -PI to PI.
		/// </summary>
		/// <remarks>The result of XYZ.AngleTo() is ranged from 0 to PI.</remarks>
		/// <param name="v0">A vector</param>
		/// <param name="v1">Another vector</param>
		/// <param name="axis">The axis perpendicular to both vectors</param>
		/// <returns>Angle between the two vectors</returns>
		public static double GetAngleBetween( XYZ v0, XYZ v1, XYZ axis )
		{
			XYZ cross=v0.CrossProduct( v1 );
			if( cross.IsAlmostEqualTo( XYZ.Zero ) ) return 0.0;

			double sign=axis.Normalize().DotProduct( cross.Normalize() );
			
			return sign*v0.AngleTo( v1 );
		}

		/// <summary>
		/// Converts float array of position coordinates and int array of color to an array of CloudPoint.
		/// The length of <code>xyz_array</code> should be exactly 3 times longer than the one of <code>color_array</code>,
		/// otherwise IndexOutOfRangeException will occur.
		/// The length of <code>point_array</code> will be exactly the same as the one of color_array.
		/// </summary>
		/// <param name="xyz_array">The input array of position coordinates</param>
		/// <param name="color_array">The input array of color</param>
		/// <param name="point_array">The output array of CloudPoint instances</param>
		/// <exception cref="IndexOutOfRangeException"></exception>
		public static void Pack( float[] xyz_array, int[] color_array, out CloudPoint[] point_array )
		{
			
			int num_points=xyz_array.Length/3;
			point_array=new CloudPoint[num_points];
			for( int k = 0;k<num_points;k++ )
			{
				int index=k*3;
				float x=xyz_array[index];
				float y=xyz_array[index+1];
				float z=xyz_array[index+2];
				int color=color_array[k];
				point_array[k]=new CloudPoint( x, y, z, color );
			}
		}

		/// <summary>
		/// Converts CloudPoint array to float array of position coordinates and int array of color.
		/// The length of <code>xyz_array</code> will be exactly 3 times longer than the one of <code>point_array</code> and
		/// the length of <code>color_array</code> will be the same as the one of <code>point_array</code>.
		/// </summary>
		/// <param name="point_array">The input array of CloudPoint instances</param>
		/// <param name="xyz_array">The output array of position coordinates</param>
		/// <param name="color_array">The output array of color</param>
		public static void Unpack( CloudPoint[] point_array, out float[] xyz_array, out int[] color_array )
		{
			xyz_array=new float[point_array.Length*3];
			color_array=new int[point_array.Length];

			for( int k = 0;k<point_array.Length;k++ )
			{
				int index=k*3;
				CloudPoint cp = point_array[k];
				xyz_array[index]=cp.X;
				xyz_array[index+1]=cp.Y;
				xyz_array[index+2]=cp.Z;
				color_array[k]=cp.Color;
			}
		}

		/// <summary>
		/// Finds the element of which makes the result of <code>predicate</code> maximum in the <code>array</code>.
		/// </summary>
		/// <typeparam name="Elem">The type of the <code>array</code> elements</typeparam>
		/// <typeparam name="Key">The type of the <code>predicate</code> result</typeparam>
		/// <param name="array">The input array</param>
		/// <param name="predicate">The predicate</param>
		/// <param name="result">The element having maximum result of <code>predicate</code></param>
		/// <returns>false if the array is empty or true otherwise</returns>
		public static bool MaxElement<Elem, Key>( IEnumerable<Elem> array, Func<Elem, Key> predicate, out Elem result ) where Key : IComparable<Key>
		{
			if( array.Any()==false ) { result=default( Elem ); return false; }

			Elem max_element = array.First();
			Key max_key = predicate(max_element);
			foreach( var element in array )
			{
				Key key = predicate(element);
				if( key.CompareTo( max_key )>0 )
				{
					max_key=key;
					max_element=element;
				}
			}
			result=max_element;
			return true;
		}

		/// <summary>
		/// Finds the element of which makes the result of <code>predicate</code> minimum in the <code>array</code>.
		/// </summary>
		/// <typeparam name="Elem">The type of the <code>array</code> elements</typeparam>
		/// <typeparam name="Key">The type of the <code>predicate</code> result</typeparam>
		/// <param name="array">The input array</param>
		/// <param name="predicate">The predicate</param>
		/// <param name="result">The element having minimum result of <code>predicate</code></param>
		/// <returns>false if the array is empty or true otherwise</returns>
		public static bool MinElement<Elem, Key>( IEnumerable<Elem> array, Func<Elem, Key> predicate, out Elem result ) where Key : IComparable<Key>
		{
			if( array.Any()==false ) { result=default( Elem ); return false; }

			Elem min_element = array.First();
			Key min_key = predicate(min_element);
			foreach( var element in array )
			{
				Key key = predicate(element);
				if( key.CompareTo( min_key )<0 )
				{
					min_key=key;
					min_element=element;
				}
			}
			result=min_element;
			return true;
		}

		/// <summary>
		/// Finds both elements of which makes the result of <code>predicate</code> minimum and miximum in the <code>array</code>.
		/// </summary>
		/// <typeparam name="Elem">The type of the <code>array</code> elements</typeparam>
		/// <typeparam name="Key">The type of the <code>predicate</code> result</typeparam>
		/// <param name="array">The input array</param>
		/// <param name="predicate">The predicate</param>
		/// <param name="min_result">The element having minimum result of <code>predicate</param>
		/// <param name="max_result">The element having maximum result of <code>predicate</param>
		/// <returns>false if the array is empty or true otherwise</returns>
		public static bool MinMaxElement<Elem, Key>( IEnumerable<Elem> array, Func<Elem, Key> predicate, out Elem min_result, out Elem max_result ) where Key : IComparable<Key>
		{
			if( array.Any()==false ) { min_result=default( Elem ); max_result=default( Elem ); return false; }

			Elem min_element = array.First();
			Elem max_element = array.First();
			Key min_key=predicate( min_element );
			Key max_key=predicate( max_element );
			foreach( var element in array )
			{
				Key key = predicate(element);
				if( key.CompareTo( min_key )<0 )
				{
					min_key=key;
					min_element=element;
				}
				if( key.CompareTo( max_key )>0 )
				{
					max_key=key;
					max_element=element;
				}
			}
			min_result=min_element;
			max_result=max_element;
			return true;
		}

		/// <summary>
		/// Interpolates two ARGB color values.
		/// </summary>
		/// <param name="c0">One of the color values</param>
		/// <param name="c1">One of the color values</param>
		/// <returns>Interpolated color</returns>
		public static int MixARGB( int c0, int c1 )
		{
			Func<int,int,int> fetch=(x,offset)=>((x>>offset)&0xFF);
			
			// fetch a component
			int[] argb0=new int[] { fetch(c0,24), fetch(c0,16), fetch(c0,8), fetch(c0,0) };
			int[] argb1=new int[] { fetch(c1,24), fetch(c1,16), fetch(c1,8), fetch(c1,0) };

			// interpolate and combine to integer value.
			int result=0;
			for( int k = 0;k<4;k++ )
				result+=((argb0[k]+argb1[k])/2)<<(24-8*k);

			return result;
		}

		/// <summary>
		/// Halton sequence generator
		/// </summary>
		public static class HaltonSequence
		{
			/// <summary>
			/// Creates a bit mask of which each bit is set to 1.
			/// Among 32 bits of integer result, the bits of <code>bit_length</code> number will be set to 1, starting from LSB.
			/// </summary>
			/// <param name="bit_length">The number of bits to be set to 1</param>
			/// <returns>The bit mask</returns>
			public static int BitMask( int bit_length )
			{
				return (int)Math.Pow( 2, bit_length )-1;
			}

			/// <summary>
			/// Reverse the bit sequence of the <code>bits</code>.
			/// <code>bit_length</code> defines where the LSB will be placed when reversing it.
			/// </summary>
			/// <param name="bits">The bit sequence to be reversed</param>
			/// <param name="bit_length">The bit length</param>
			/// <returns>The int value of reversed bit sequence</returns>
			public static uint ReverseBit( uint bits, int bit_length = 32 )
			{
				/* reference: http://graphics.stanford.edu/~seander/bithacks.html#ReverseParallel */
				bits=((bits>>1)&0x55555555)|((bits&0x55555555)<<1);
				bits=((bits>>2)&0x33333333)|((bits&0x33333333)<<2);
				bits=((bits>>4)&0x0f0f0f0f)|((bits&0x0f0f0f0f)<<4);
				bits=((bits>>8)&0x00ff00ff)|((bits&0x00ff00ff)<<8);
				bits=(bits>>16)|(bits<<16);

				if( bit_length<32 ) bits=(bits>>(32-bit_length))&(uint)BitMask( bit_length );
				return (uint)bits;
			}

			/// <summary>
			/// Calculates the next highest power of 2 of the input number <code>v</code>.
			/// </summary>
			/// <param name="v">The input number</param>
			/// <returns>The next highest power of 2</returns>
			public static int NextHighestPowerOf2( int v )
			{
				/* reference: http://graphics.stanford.edu/~seander/bithacks.html#RoundUpPowerOf2 */
				v--;
				v|=v>>1;
				v|=v>>2;
				v|=v>>4;
				v|=v>>8;
				v|=v>>16;
				v++;
				return v;
			}

			/// <summary>
			/// returns bit distance to MSB being set to 1.
			/// </summary>
			/// <param name="value">The input number</param>
			/// <returns>The bit distance</returns>
			public static int BitLength( int value ) => (int)Math.Ceiling( Math.Log( value )/Math.Log( 2 ) );

			/// <summary>
			/// Generates a halton sequence of which base is the next highest power of 2 of the given <code>seed</code>.
			/// The length of the sequence will be the same as the next highest power of 2 value.
			/// </summary>
			/// <param name="seed">The input seed</param>
			/// <returns>Halton sequence array</returns>
			public static uint[] GenerateHaltonSequence( int seed )
			{
				int bit_length=BitLength(seed);
				int base_number=NextHighestPowerOf2(seed);

				uint[] sequence=new uint[base_number];
				for( uint k = 0;k<base_number;k++ ) sequence[k]=ReverseBit( k, bit_length );
				return sequence;
			}

			/// <summary>
			/// Generates a halton sequence of which base is the next highest power of 2 of the given <code>sequence_length</code>.
			/// But the number that is greater than the <code>sequence_length</code> will be filtered out.
			/// Thus, the length of the sequence will be the same as the <code>sequence_length</code>.
			/// </summary>
			/// <param name="sequence_length">The input seed</param>
			/// <returns>Halton sequence array</returns>
			public static uint[] GenerateIndices( int sequence_length )
			{
				int bit_length = BitLength(sequence_length);
				int base_number=NextHighestPowerOf2(sequence_length);

				uint[] sequence = new uint[base_number];
				for( uint k = 0;k<base_number;k++ )
				{
					sequence[k]=ReverseBit( k, bit_length );
				}
				return sequence.Where( x => x<sequence_length ).ToArray();
			}
		}
	}
}
