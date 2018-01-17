#ifndef _FIND_SURFACE_H_
#define _FIND_SURFACE_H_

#ifndef __DECL__
	#if defined(_WIN32) || defined(_WIN64)
		#define __DECL__ __declspec( dllimport )
	#else
		#define __DECL__
	#endif /* defined(_WIN32) || defined(_WIN64) */
#endif /* __DECL__ */

#ifdef __cplusplus
extern "C" {
#endif

#define FS_NO_ERROR             0
#define FS_OUT_OF_MEMORY       -1
#define FS_INVALID_OPERATION   -2
#define FS_LICENSE_EXPIRED     -3
#define FS_LICENSE_UNKNOWN     -4
#define FS_INVALID_ENUM        -100
#define FS_INVALID_VALUE       -101
#define FS_NOT_FOUND           -200
#define FS_UNACCEPTABLE_RESULT -201


typedef void* FIND_SURFACE_CONTEXT;

typedef enum {
	FS_TYPE_ANY      = 0,
	FS_TYPE_PLANE    = 1,
	FS_TYPE_SPHERE   = 2,
	FS_TYPE_CYLINDER = 3,
	FS_TYPE_CONE     = 4,
	FS_TYPE_TORUS    = 5,
/*
	* > Reservation
	FS_TYPE_BOX      = 6,
	FS_TYPE_SOR      = 7
*/
	FS_TYPE_NONE     = 0xFFFFFFFF
} FS_FEATURE_TYPE;

typedef enum {
	FS_PARAM_ACCURACY      = 0x101,
	FS_PARAM_MEAN_DIST     = 0x102,
	FS_PARAM_TOUCH_R       = 0x103,
	FS_PARAM_CONE2CYLINDER = 0x104,
/*	
 * > Reservation
	FS_PARAM_RMS_QUAD      = 0x105,
	FS_PARAM_R12_PLANE     = 0x106,
	FS_PARAM_R12_CYLINDER  = 0x107,
*/
	FS_PARAM_RAD_EXP       = 0x108,
	FS_PARAM_LAT_EXT       = 0x109
} FS_PARAMS;

typedef struct {
	FS_FEATURE_TYPE type;
	float           rms;

	union {
		float reserved[14];

		struct {
			float ll[3]; /* lower left corner */
			float lr[3]; /* lower right corner */
			float ur[3]; /* upper right corner */
			float ul[3]; /* upper left corner */
		}plane_param;

		struct {
			float c[3]; /* center */
			float r;    /* radius of the sphere */
		}sphere_param;

		struct {
			float b[3]; /* center of bottom circle */
			float t[3]; /* center of top circle */
			float r;    /* radius of the cylinder */
		}cylinder_param;

		struct {
			float b[3]; /* center of bottom circle */
			float t[3]; /* center of top circle */
			float br;   /* bottom (larger) radius */
			float tr;   /* top (smaller) radius of the cone */
		}cone_param;

		struct {
			float c[3]; /* center */
			float n[3]; /* axis (normal vector) */
			float mr;   /* mean radius */
			float tr;   /* tube (circle) radius of the torus */
		}torus_param;
/*
		struct {
			float o[3]; /* box origin   *//*
			float x[3]; /* box x-corner *//*
			float y[3]; /* box y-corner *//*
			float z[3]; /* box z-corner *//*
		}box_param;
*/
	};
} FS_FEATURE_RESULT;

/**
 * createFindSurface
 *
 * Returns 0 for success, otherwise error
 */
__DECL__ int createFindSurface(FIND_SURFACE_CONTEXT *context);

/**
 * releaseFindSurface
 */
__DECL__ void releaseFindSurface(FIND_SURFACE_CONTEXT context);

/**
 * (set|get)FindSurfaceParam(Float|Int)
 *
 * Float Params - 
 * Int   Params - FS_PARAM_RAD_EXP and FS_PARAM_LAT_EXT are accepted.
 *
 * Returns 0 for success, otherwise error
 */
__DECL__ int setFindSurfaceParamFloat(FIND_SURFACE_CONTEXT context, FS_PARAMS pname, float param);
__DECL__ int getFindSurfaceParamFloat(FIND_SURFACE_CONTEXT context, FS_PARAMS pname, float *param);
__DECL__ int setFindSurfaceParamInt(FIND_SURFACE_CONTEXT context, FS_PARAMS pname, int param);
__DECL__ int getFindSurfaceParamInt(FIND_SURFACE_CONTEXT context, FS_PARAMS pname, int *param);

/**
 * setPointCloud(Float|Double)
 *
 * pointer [in] : Specifies a offset of the first component of the point cloud data in the array.
 *                Number of components per each point cloud data must be 3 (x, y, z).
 * count   [in] : Specifies the number of the point cloud data
 * stride  [in] : Specifies the byte offset between consecutive point cloud data.
 *                If stride is 0, point cloud data are understood to be tightly packed in the array.
 *
 * Returns 0 for success, otherwise error
 */
__DECL__ int setPointCloudFloat(FIND_SURFACE_CONTEXT context, const void *pointer, unsigned int count, unsigned int stride);
__DECL__ int setPointCloudDouble(FIND_SURFACE_CONTEXT context, const void *pointer, unsigned int count, unsigned int stride);

/**
 * getPointCloudCount
 *
 * Returns the number of points currently set
 */
__DECL__ unsigned int getPointCloudCount(FIND_SURFACE_CONTEXT context);

/**
 * findSurface
 *
 * type        [in]  : Specifies what kind of surface to find. Symbolic constants FS_TYPE_ANY, FS_TYPE_PLANE, FS_TYPE_SPHERE, FS_TYPE_CYLINDER, FS_TYPE_CONE, FS_TYPE_TORUS and FS_TYPE_BOX are accepted.
 * start_index [in]  : Specifies the index of point cloud data to start find surface.
 * result      [out] : Pointer to a buffer that receives the feature parameters.
 *
 * Returns ...
 */
__DECL__ int findSurface(FIND_SURFACE_CONTEXT context, FS_FEATURE_TYPE type, unsigned int start_index, FS_FEATURE_RESULT *result );

__DECL__ int findStripPlane(FIND_SURFACE_CONTEXT context, unsigned int index_1, unsigned int index_2, FS_FEATURE_RESULT *result);
__DECL__ int findRodCylinder(FIND_SURFACE_CONTEXT context, unsigned int index_1, unsigned int index_2, FS_FEATURE_RESULT *result);
__DECL__ int findDiskCylinder(FIND_SURFACE_CONTEXT context, unsigned int index_1, unsigned int index_2, unsigned int index_3, FS_FEATURE_RESULT *result);
__DECL__ int findDiskCone(FIND_SURFACE_CONTEXT context, unsigned int index_1, unsigned int index_2, unsigned int index_3, FS_FEATURE_RESULT *result);
__DECL__ int findThinRingTorus(FIND_SURFACE_CONTEXT context, unsigned int index_1, unsigned int index_2, unsigned int index_3, FS_FEATURE_RESULT *result);

/**
 * get(In|Out)liers(Float|Double)
 *
 * pointer [out, option] : Pointer to a buffer that receives the (in|out)lier point cloud data.
 * bufSize [in]          : Specifies the size of the buffer pointer for this function.
 *
 * Returns the number of (in|out)lier point cloud data.
 */                 
__DECL__ unsigned int getInliersFloat(FIND_SURFACE_CONTEXT context, void *pointer, unsigned int bufSize);
__DECL__ unsigned int getOutliersFloat(FIND_SURFACE_CONTEXT context, void *pointer, unsigned int bufSize);
__DECL__ unsigned int getInliersDouble(FIND_SURFACE_CONTEXT context, void *pointer, unsigned int bufSize);
__DECL__ unsigned int getOutliersDouble(FIND_SURFACE_CONTEXT context, void *pointer, unsigned int bufSize);

/**
 * getInOutlierFlags
 *
 * Returns the boolean array ( 0 for inliers, otherwise outliers ) of in/outlier of the input point cloud data.
 */
__DECL__ const unsigned char *getInOutlierFlags(FIND_SURFACE_CONTEXT context);

/**
 * cleanUpFindSurface
 */
__DECL__ void cleanUpFindSurface(FIND_SURFACE_CONTEXT context);

#ifdef __cplusplus
}
#endif

#undef __DECL__

#endif /* _FIND_SURF_H_ */