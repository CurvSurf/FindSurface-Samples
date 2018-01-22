//#define _USE_UNITY_ANDROID_
//#define _USE_UNITY_IOS_

using System;
using System.Runtime.InteropServices;
#if _USE_UNITY_ANDROID_ || _USE_UNITY_IOS_
using UnityEngine;
#else
using System.Numerics;
#endif

/*
 * .NET 3.X 버전 이상부터는 DllImport 시 CallingConvention, Cdecl을 명시해 줘야 함
 * Unity의 경우 .NET 2.0을 쓰는 것으로 알고 있어서 필요없을 것 같음...
 * 문제는 .NET 3.X버전 이상에서도 Debug 중에만 Warning으로 PInvokeStackImbalacne가 잡힐 뿐... 정상적으로 동작함...
 * 심지어 해당 Warning 검사는 끌 수 있다고...
 */

// Unity-Android  [DllImport ("FindSurface")]   // libFindSurface.so
// Unity-iOS      [DllImport ("__Internal")]
// Win32          [DllImport ("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]

namespace CurvSurf
{
    public enum FS_FEATURE_TYPE: UInt32 {
		FS_TYPE_ANY      = 0,
		FS_TYPE_PLANE    = 1,
		FS_TYPE_SPHERE   = 2,
		FS_TYPE_CYLINDER = 3,
		FS_TYPE_CONE     = 4,
		FS_TYPE_TORUS    = 5,
		FS_TYPE_BOX      = 6,
        FS_TYPE_NONE     = 0xFFFFFFFF
	};

	public enum FS_PARAMS {
		FS_PARAM_ACCURACY      = 0x101,
		FS_PARAM_MEAN_DIST     = 0x102,
		FS_PARAM_TOUCH_R       = 0x103,
		FS_PARAM_CONE2CYLINDER = 0x104,
	/*	
		FS_PARAM_RMS_QUAD      = 0x105,
		FS_PARAM_R12_PLANE     = 0x106,
		FS_PARAM_R12_CYLINDER  = 0x107,
	*/
		FS_PARAM_RAD_EXP       = 0x108,
		FS_PARAM_LAT_EXT       = 0x109
	};

    public class FS_RESULT_PARAMS { /* Empty - Just for Grouping */ }
    public class FS_PLANE_PARAMS : FS_RESULT_PARAMS
    {
        private Vector3 lower_left;
        private Vector3 lower_right;
        private Vector3 upper_right;
        private Vector3 upper_left;

        public Vector3 LowerLeft { get { return lower_left; } }
        public Vector3 LowerRight { get { return lower_right; } }
        public Vector3 UpperRight {  get { return upper_right; } }
        public Vector3 UpperLeft { get { return upper_left; } }

        public float Width {
            get { return Vector3.Distance(lower_left, lower_right); }
        }

        public float Height {
            get { return Vector3.Distance(lower_left, upper_left); }
        }

        public Vector3 Normal {
            get { return Vector3.Cross(lower_right - lower_left, upper_left - lower_left); }
        }

        public Vector3 Center {
            get { return (lower_left + lower_right + upper_right + upper_left) / 4.0f; }
        }

        public FS_PLANE_PARAMS(Vector3 ll, Vector3 lr, Vector3 ur, Vector3 ul)
        {
            lower_left  = ll;
            lower_right = lr;
            upper_right = ur;
            upper_left  = ul;
        }

    }

    public class FS_SPHERE_PARAMS : FS_RESULT_PARAMS
    {
        private Vector3 center; /* center of sphere */
        private float radius;

        public Vector3 Center { get { return center; } }
        public float Radius { get { return radius; } }

        public FS_SPHERE_PARAMS(Vector3 c, float r)
        {
            center = c;
            radius = r;
        }
    }

    public class FS_CYLINDER_PARAMS : FS_RESULT_PARAMS
    {
        private Vector3 bottom;
        private Vector3 top;
        private float radius;

        public Vector3 Bottom { get { return bottom; } }
        public Vector3 Top { get { return top; } }
        public float Radius { get { return radius; } }

        public float Height {
            get { return Vector3.Distance(bottom, top); }
        }

        public Vector3 Center { get { return (bottom + top) / 2.0f; } }
        public Vector3 Axis { get { return Vector3.Normalize(top - bottom); } }

        public FS_CYLINDER_PARAMS(Vector3 b, Vector3 t, float r)
        {
            bottom = b;
            top = t;
            radius = r;
        }
    }

    public class FS_CONE_PARAMS : FS_RESULT_PARAMS
    {
        private Vector3 bottom;
        private Vector3 top;
        private float bottom_radius;
        private float top_radius;

        public Vector3 Bottom { get { return bottom; } }
        public Vector3 Top { get { return top; } }
        public float BottomRadius { get { return bottom_radius; } }
        public float TopRadius { get { return top_radius; } }

        public float Height {
            get { return Vector3.Distance(bottom, top); }
        }

        public Vector3 Center { get { return (bottom + top) / 2.0f; } }
        public Vector3 Axis { get { return Vector3.Normalize(top - bottom); } }

        public float VertexAngle {
            //get { return (float)Math.Atan((bottom_radius - top_radius) / height); }
            get { return (float)Math.Atan2(bottom_radius - top_radius, Height); }
        }

        public Vector3 Vertex {
            get { return top + Axis * ((top_radius * Height) / (bottom_radius - top_radius)); }
        }

        public FS_CONE_PARAMS(Vector3 b, Vector3 t, float br, float tr)
        {
            bottom = b;
            top = t;
            bottom_radius = br;
            top_radius = tr;
        }
    }

    public class FS_TORUS_PARAMS : FS_RESULT_PARAMS
    {
        private Vector3 center;
        private Vector3 normal;
        private float mean_radius;
        private float tube_radius;

        public Vector3 Center { get { return center; } }
        public Vector3 Normal { get { return normal; } }
        public float MeanRadius { get { return mean_radius; } }
        public float TubeRadius { get { return tube_radius; } }

        public FS_TORUS_PARAMS(Vector3 c, Vector3 n, float mr, float tr)
        {
            center = c;
            normal = n;
            mean_radius = mr;
            tube_radius = tr;
        }
    }

	public struct FS_FEATURE_RESULT
	{
		public FS_FEATURE_TYPE type;
		public float rms;
        public FS_RESULT_PARAMS param;

        public FS_PLANE_PARAMS    GetParamAsPlane()    { return (FS_PLANE_PARAMS)param;    }
        public FS_SPHERE_PARAMS   GetParamAsSphere()   { return (FS_SPHERE_PARAMS)param;   }
        public FS_CYLINDER_PARAMS GetParamAsCylinder() { return (FS_CYLINDER_PARAMS)param; }
        public FS_CONE_PARAMS     GetParamAsCone()     { return (FS_CONE_PARAMS)param;     }
        public FS_TORUS_PARAMS    GetParamAsTorus()    { return (FS_TORUS_PARAMS)param;    }
    }

	public class FindSurface 
	{
        [StructLayout(LayoutKind.Sequential)]
        private struct _FS_FEATURE_RESULT
        {
            public FS_FEATURE_TYPE type;
            public float rms;
            public float param1;
            public float param2;
            public float param3;
            public float param4;
            public float param5;
            public float param6;
            public float param7;
            public float param8;
            public float param9;
            public float param10;
            public float param11;
            public float param12;
            public float param13;
            public float param14;
        }


        private IntPtr context = IntPtr.Zero;

		public static FindSurface instance = null;
		public static FindSurface GetInstance() {
			if( instance == null ) {
				instance = new FindSurface();
				if( instance.context == IntPtr.Zero ) {
					instance = null; // Failed to Initialized...
				}
			}
			return instance;
		}

		private FindSurface() {
			IntPtr _tmp = new IntPtr();
			if( createFindSurface(ref _tmp) == 0 ) {
				context = _tmp;
			}
		}

		~FindSurface() {
			if( context != IntPtr.Zero ) {
				releaseFindSurface(context);
				context = IntPtr.Zero;
			}
		}

		public float Accuracy { get { return GetParamFloat(FS_PARAMS.FS_PARAM_ACCURACY); } set { SetParamFloat(FS_PARAMS.FS_PARAM_ACCURACY, value); } }
		public float MeanDistance { get { return GetParamFloat(FS_PARAMS.FS_PARAM_MEAN_DIST); } set { SetParamFloat(FS_PARAMS.FS_PARAM_MEAN_DIST, value); } }
		public float TouchRadius { get { return GetParamFloat(FS_PARAMS.FS_PARAM_TOUCH_R); } set { SetParamFloat(FS_PARAMS.FS_PARAM_TOUCH_R, value); } }
		public float ConeToCylinder { get { return GetParamFloat(FS_PARAMS.FS_PARAM_CONE2CYLINDER); } set { SetParamFloat(FS_PARAMS.FS_PARAM_CONE2CYLINDER, value); } }
		public int RadialExpantion { get { return GetParamInt(FS_PARAMS.FS_PARAM_RAD_EXP); } set { SetParamInt(FS_PARAMS.FS_PARAM_RAD_EXP, value); } }
		public int LateralExtension { get { return GetParamInt(FS_PARAMS.FS_PARAM_LAT_EXT); } set { SetParamInt(FS_PARAMS.FS_PARAM_LAT_EXT, value); } }

		public void SetParamFloat(FS_PARAMS pname, float param) {
			setFindSurfaceParamFloat( context, pname, param );
		}

		public float GetParamFloat(FS_PARAMS pname) {
			float val = 0.0f;
			getFindSurfaceParamFloat( context, pname, ref val );
			return val;
		}

		public void SetParamInt(FS_PARAMS pname, int param) {
			setFindSurfaceParamInt( context, pname, param );
		}

		public int GetParamInt(FS_PARAMS pname) {
			int val = 0;
			getFindSurfaceParamInt( context, pname, ref val );
			return val;
		}
        
		public bool SetPointCloud(float[] tigtly_packed_xyz_list) {
			uint count = (uint)tigtly_packed_xyz_list.Length / 3;

            int bufSize = Marshal.SizeOf(tigtly_packed_xyz_list[0]) * tigtly_packed_xyz_list.Length;
            IntPtr pointer = Marshal.AllocHGlobal(bufSize);
            Marshal.Copy(tigtly_packed_xyz_list, 0, pointer, tigtly_packed_xyz_list.Length);

            int ret = setPointCloudFloat(context, pointer, count, 0);

            Marshal.FreeHGlobal(pointer);

            return ret == 0 ? true : false;
		}

        private FS_FEATURE_RESULT? _GetResult(_FS_FEATURE_RESULT result)
        {
            FS_FEATURE_TYPE type;
            FS_RESULT_PARAMS param = null;
            switch(result.type)
            {
                case FS_FEATURE_TYPE.FS_TYPE_PLANE:
                    {
                        type = FS_FEATURE_TYPE.FS_TYPE_PLANE;
                        FS_PLANE_PARAMS p = new FS_PLANE_PARAMS(
                            new Vector3(result.param1, result.param2, result.param3),
                            new Vector3(result.param4, result.param5, result.param6),
                            new Vector3(result.param7, result.param8, result.param9),
                            new Vector3(result.param10, result.param11, result.param12)
                        );

                        param = p;
                    }
                    break;
                case FS_FEATURE_TYPE.FS_TYPE_SPHERE:
                    {
                        type = FS_FEATURE_TYPE.FS_TYPE_SPHERE;
                        FS_SPHERE_PARAMS p = new FS_SPHERE_PARAMS(
                            new Vector3(result.param1, result.param2, result.param3),
                            result.param4
                        );

                        param = p;
                    }
                    break;
                case FS_FEATURE_TYPE.FS_TYPE_CYLINDER:
                    {
                        type = FS_FEATURE_TYPE.FS_TYPE_CYLINDER;
                        FS_CYLINDER_PARAMS p = new FS_CYLINDER_PARAMS(
                            new Vector3(result.param1, result.param2, result.param3),
                            new Vector3(result.param4, result.param5, result.param6),
                            result.param7
                        );

                        param = p;
                    }
                    break;
                case FS_FEATURE_TYPE.FS_TYPE_CONE:
                    {
                        if(result.param7 == result.param8)
                        {
                            type = FS_FEATURE_TYPE.FS_TYPE_CYLINDER;
                            FS_CYLINDER_PARAMS p = new FS_CYLINDER_PARAMS(
                                new Vector3(result.param1, result.param2, result.param3),
                                new Vector3(result.param4, result.param5, result.param6),
                                result.param7
                            );

                            param = p;
                        }
                        else
                        {
                            type = FS_FEATURE_TYPE.FS_TYPE_CONE;
                            FS_CONE_PARAMS p = new FS_CONE_PARAMS(
                                new Vector3(result.param1, result.param2, result.param3),
                                new Vector3(result.param4, result.param5, result.param6),
                                result.param7, result.param8
                            );

                            param = p;
                        }
                    }
                    break;
                case FS_FEATURE_TYPE.FS_TYPE_TORUS:
                    {
                        if(result.param7 == 0.0f)
                        {
                            type = FS_FEATURE_TYPE.FS_TYPE_SPHERE;
                            FS_SPHERE_PARAMS p = new FS_SPHERE_PARAMS(
                                new Vector3(result.param1, result.param2, result.param3),
                                result.param8
                            );

                            param = p;
                        }
                        else if(result.param7 == float.MaxValue)
                        {
                            type = FS_FEATURE_TYPE.FS_TYPE_CYLINDER;
                            FS_CYLINDER_PARAMS p = new FS_CYLINDER_PARAMS(
                                new Vector3(result.param1, result.param2, result.param3),
                                new Vector3(result.param4, result.param5, result.param6),
                                result.param8
                            );

                            param = p;
                        }
                        else
                        {
                            type = FS_FEATURE_TYPE.FS_TYPE_TORUS;
                            FS_TORUS_PARAMS p = new FS_TORUS_PARAMS(
                                new Vector3(result.param1, result.param2, result.param3),
                                new Vector3(result.param4, result.param5, result.param6),
                                result.param7, result.param8
                            );

                            param = p;
                        }
                    }
                    break;

                default:
                    return null;
            }

            FS_FEATURE_RESULT ret = new FS_FEATURE_RESULT();
            ret.type  = type;
            ret.rms   = result.rms;
            ret.param = param;

            return ret;
        }
        
		public FS_FEATURE_RESULT? RunFindSurface(FS_FEATURE_TYPE type, uint start_index) {
            _FS_FEATURE_RESULT _tmp = new _FS_FEATURE_RESULT();
            return (findSurface(context, type, start_index, ref _tmp) == 0) ? _GetResult(_tmp) : null;
		}

        public FS_FEATURE_RESULT? RunFindStripPlane(uint index_1, uint index_2)
        {
            _FS_FEATURE_RESULT _tmp = new _FS_FEATURE_RESULT();
            return (findStripPlane(context, index_1, index_2, ref _tmp) == 0) ? _GetResult(_tmp) : null;
        }

        public FS_FEATURE_RESULT? RunFindRodCylinder(uint index_1, uint index_2)
        {
            _FS_FEATURE_RESULT _tmp = new _FS_FEATURE_RESULT();
            return (findRodCylinder(context, index_1, index_2, ref _tmp) == 0) ? _GetResult(_tmp) : null;
        }

        public FS_FEATURE_RESULT? RunFindDiskCylinder(uint index_1, uint index_2, uint index_3)
        {
            _FS_FEATURE_RESULT _tmp = new _FS_FEATURE_RESULT();
            return (findDiskCylinder(context, index_1, index_2, index_3, ref _tmp) == 0) ? _GetResult(_tmp) : null;
        }

        public FS_FEATURE_RESULT? RunFindDiskCone(uint index_1, uint index_2, uint index_3)
        {
            _FS_FEATURE_RESULT _tmp = new _FS_FEATURE_RESULT();
            return (findDiskCone(context, index_1, index_2, index_3, ref _tmp) == 0) ? _GetResult(_tmp) : null;
        }

        public FS_FEATURE_RESULT? RunFindThinRingTorus(uint index_1, uint index_2, uint index_3)
        {
            _FS_FEATURE_RESULT _tmp = new _FS_FEATURE_RESULT();
            return (findThinRingTorus(context, index_1, index_2, index_3, ref _tmp) == 0) ? _GetResult(_tmp) : null;
        }

        public float[] GetInliers() {
			uint cnt = getInliersFloat(context, IntPtr.Zero, 0);
			if(cnt > 0) {
				float[] ret = new float[cnt * 3];
                int bufSize = Marshal.SizeOf(ret[0]) * ret.Length;
                IntPtr pointer = Marshal.AllocHGlobal(bufSize);

                getInliersFloat(context, pointer, (uint)bufSize);

                Marshal.Copy(pointer, ret, 0, ret.Length);
                Marshal.FreeHGlobal(pointer);

				return ret;
			}
			else if(cnt == 0) {
				return new float[0];
			}
			return null;
		}

		public float[] GetOutliers() {
            uint cnt = getOutliersFloat(context, IntPtr.Zero, 0);
            if (cnt > 0)
            {
                float[] ret = new float[cnt * 3];
                int bufSize = Marshal.SizeOf(ret[0]) * ret.Length;
                IntPtr pointer = Marshal.AllocHGlobal(bufSize);

                getOutliersFloat(context, pointer, (uint)bufSize);

                Marshal.Copy(pointer, ret, 0, ret.Length);
                Marshal.FreeHGlobal(pointer);

                return ret;
            }
            else if (cnt == 0)
            {
                return new float[0];
            }
            return null;
        }

        public bool[] GetInOutlierFlags() {
            IntPtr _flags = getInOutlierFlags(context);
            if (_flags == IntPtr.Zero) return null;
            uint cnt = getPointCloudCount(context);

            byte[] _tmp = new byte[cnt];
            Marshal.Copy(_flags, _tmp, 0, _tmp.Length);

            return Array.ConvertAll<byte, bool>(_tmp, b => (b != 0));
        }
        
		public void CleanUp() {
			cleanUpFindSurface(context);
		}

#if _USE_UNITY_ANDORID_
        [DllImport ("FindSurface")]
		private static extern int createFindSurface(ref IntPtr context);

        [DllImport ("FindSurface")]
        private static extern void releaseFindSurface(IntPtr context);

        [DllImport ("FindSurface")]
        private static extern int setFindSurfaceParamFloat(IntPtr context, FS_PARAMS pname, float param);

        [DllImport ("FindSurface")]
        private static extern int getFindSurfaceParamFloat(IntPtr context, FS_PARAMS pname, ref float param);

        [DllImport ("FindSurface")]
        private static extern int setFindSurfaceParamInt(IntPtr context, FS_PARAMS pname, int param);

        [DllImport ("FindSurface")]
        private static extern int getFindSurfaceParamInt(IntPtr context, FS_PARAMS pname, ref int param);

        [DllImport ("FindSurface")]
        private static extern int setPointCloudFloat(IntPtr context, IntPtr pointer, uint count, uint stride);

        [DllImport ("FindSurface")]
        private static extern int setPointCloudDouble(IntPtr context, IntPtr pointer, uint count, uint stride);

        [DllImport ("FindSurface")]
        private static extern uint getPointCloudCount(IntPtr context);

        [DllImport ("FindSurface")]
        private static extern int findSurface(IntPtr context, FS_FEATURE_TYPE type, uint start_index, ref _FS_FEATURE_RESULT result);

        [DllImport ("FindSurface")]
        private static extern int findStripPlane(IntPtr context, uint index_1, uint index_2, ref _FS_FEATURE_RESULT result);

        [DllImport ("FindSurface")]
        private static extern int findRodCylinder(IntPtr context, uint index_1, uint index_2, ref _FS_FEATURE_RESULT result);

        [DllImport ("FindSurface")]
        private static extern int findDiskCylinder(IntPtr context, uint index_1, uint index_2, uint index_3, ref _FS_FEATURE_RESULT result);

        [DllImport ("FindSurface")]
        private static extern int findDiskCone(IntPtr context, uint index_1, uint index_2, uint index_3, ref _FS_FEATURE_RESULT result);

        [DllImport ("FindSurface")]
        private static extern int findThinRingTorus(IntPtr context, uint index_1, uint index_2, uint index_3, ref _FS_FEATURE_RESULT result);

        [DllImport ("FindSurface")]
        private static extern uint getInliersFloat(IntPtr context, IntPtr pointer, uint bufSize);

        [DllImport ("FindSurface")]
        private static extern uint getOutliersFloat(IntPtr context, IntPtr pointer, uint bufSize);

        [DllImport ("FindSurface")]
        private static extern uint getInliersDouble(IntPtr context, IntPtr pointer, uint bufSize);

        [DllImport ("FindSurface")]
        private static extern uint getOutliersDouble(IntPtr context, IntPtr pointer, uint bufSize);

        [DllImport ("FindSurface")]
        private static extern IntPtr getInOutlierFlags(IntPtr context);

        [DllImport ("FindSurface")]
        private static extern void cleanUpFindSurface(IntPtr context);
#elif _USE_UNITY_IOS_
        [DllImport ("__Internal")]
		private static extern int createFindSurface(ref IntPtr context);

        [DllImport ("__Internal")]
        private static extern void releaseFindSurface(IntPtr context);

        [DllImport ("__Internal")]
        private static extern int setFindSurfaceParamFloat(IntPtr context, FS_PARAMS pname, float param);

        [DllImport ("__Internal")]
        private static extern int getFindSurfaceParamFloat(IntPtr context, FS_PARAMS pname, ref float param);

        [DllImport ("__Internal")]
        private static extern int setFindSurfaceParamInt(IntPtr context, FS_PARAMS pname, int param);

        [DllImport ("__Internal")]
        private static extern int getFindSurfaceParamInt(IntPtr context, FS_PARAMS pname, ref int param);

        [DllImport ("__Internal")]
        private static extern int setPointCloudFloat(IntPtr context, IntPtr pointer, uint count, uint stride);

        [DllImport ("__Internal")]
        private static extern int setPointCloudDouble(IntPtr context, IntPtr pointer, uint count, uint stride);

        [DllImport ("__Internal")]
        private static extern uint getPointCloudCount(IntPtr context);

        [DllImport ("__Internal")]
        private static extern int findSurface(IntPtr context, FS_FEATURE_TYPE type, uint start_index, ref _FS_FEATURE_RESULT result);

        [DllImport ("__Internal")]
        private static extern int findStripPlane(IntPtr context, uint index_1, uint index_2, ref _FS_FEATURE_RESULT result);

        [DllImport ("__Internal")]
        private static extern int findRodCylinder(IntPtr context, uint index_1, uint index_2, ref _FS_FEATURE_RESULT result);

        [DllImport ("__Internal")]
        private static extern int findDiskCylinder(IntPtr context, uint index_1, uint index_2, uint index_3, ref _FS_FEATURE_RESULT result);

        [DllImport ("__Internal")]
        private static extern int findDiskCone(IntPtr context, uint index_1, uint index_2, uint index_3, ref _FS_FEATURE_RESULT result);

        [DllImport ("__Internal")]
        private static extern int findThinRingTorus(IntPtr context, uint index_1, uint index_2, uint index_3, ref _FS_FEATURE_RESULT result);

        [DllImport ("__Internal")]
        private static extern uint getInliersFloat(IntPtr context, IntPtr pointer, uint bufSize);

        [DllImport ("__Internal")]
        private static extern uint getOutliersFloat(IntPtr context, IntPtr pointer, uint bufSize);

        [DllImport ("__Internal")]
        private static extern uint getInliersDouble(IntPtr context, IntPtr pointer, uint bufSize);

        [DllImport ("__Internal")]
        private static extern uint getOutliersDouble(IntPtr context, IntPtr pointer, uint bufSize);

        [DllImport ("__Internal")]
        private static extern IntPtr getInOutlierFlags(IntPtr context);

        [DllImport ("__Internal")]
        private static extern void cleanUpFindSurface(IntPtr context);
#else
        [DllImport("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern int createFindSurface(ref IntPtr context);

        [DllImport("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void releaseFindSurface(IntPtr context);

        [DllImport("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int setFindSurfaceParamFloat(IntPtr context, FS_PARAMS pname, float param);

        [DllImport("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int getFindSurfaceParamFloat(IntPtr context, FS_PARAMS pname, ref float param);

        [DllImport("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int setFindSurfaceParamInt(IntPtr context, FS_PARAMS pname, int param);

        [DllImport("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int getFindSurfaceParamInt(IntPtr context, FS_PARAMS pname, ref int param);

        [DllImport("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int setPointCloudFloat(IntPtr context, IntPtr pointer, uint count, uint stride);

        [DllImport("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int setPointCloudDouble(IntPtr context, IntPtr pointer, uint count, uint stride);

        [DllImport("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint getPointCloudCount(IntPtr context);

        [DllImport("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int findSurface(IntPtr context, FS_FEATURE_TYPE type, uint start_index, ref _FS_FEATURE_RESULT result);

        [DllImport("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int findStripPlane(IntPtr context, uint index_1, uint index_2, ref _FS_FEATURE_RESULT result);

        [DllImport("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int findRodCylinder(IntPtr context, uint index_1, uint index_2, ref _FS_FEATURE_RESULT result);

        [DllImport("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int findDiskCylinder(IntPtr context, uint index_1, uint index_2, uint index_3, ref _FS_FEATURE_RESULT result);

        [DllImport("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int findDiskCone(IntPtr context, uint index_1, uint index_2, uint index_3, ref _FS_FEATURE_RESULT result);

        [DllImport("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int findThinRingTorus(IntPtr context, uint index_1, uint index_2, uint index_3, ref _FS_FEATURE_RESULT result);

        [DllImport("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint getInliersFloat(IntPtr context, IntPtr pointer, uint bufSize);

        [DllImport("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint getOutliersFloat(IntPtr context, IntPtr pointer, uint bufSize);

        [DllImport("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint getInliersDouble(IntPtr context, IntPtr pointer, uint bufSize);

        [DllImport("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint getOutliersDouble(IntPtr context, IntPtr pointer, uint bufSize);

        [DllImport("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr getInOutlierFlags(IntPtr context);

        [DllImport("FindSurface.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void cleanUpFindSurface(IntPtr context);
#endif
    }
}