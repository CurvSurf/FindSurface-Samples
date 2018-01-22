#include <stdio.h>
#include <math.h>
#include <float.h>
#include "FindSurface.h"
#include "xyz_reader.h"

#if defined(_WIN32) || defined(_WIN64)
#pragma comment(lib, "FindSurface.lib")
#endif

#define RETRY_COUNT     10
#define SPHERE_INDEX    7811
#define CYLINDER_INDEX  3437
#define CONE_INDEX      6637
#define TORUS_INDEX     7384

static struct {
	FS_FEATURE_TYPE find_type;
	int             seed_index;
}
FIND_NORMAL_LIST[] = {
	/* Normal Case */
	{ FS_TYPE_SPHERE,   SPHERE_INDEX },
	{ FS_TYPE_CYLINDER, CYLINDER_INDEX },
	{ FS_TYPE_CONE,     CONE_INDEX },
	{ FS_TYPE_TORUS,    TORUS_INDEX },
	/* Special Case */
	{ FS_TYPE_CONE,     CYLINDER_INDEX },
	{ FS_TYPE_TORUS,    SPHERE_INDEX },
	{ FS_TYPE_TORUS,    CYLINDER_INDEX },
},
FIND_AUTO_LIST[] = {
	{ FS_TYPE_ANY,      SPHERE_INDEX },
	{ FS_TYPE_ANY,      CYLINDER_INDEX },
	{ FS_TYPE_ANY,      CONE_INDEX },
	{ FS_TYPE_ANY,      TORUS_INDEX },
};
const int FIND_NORMAL_LIST_SIZE = sizeof(FIND_NORMAL_LIST) / sizeof(FIND_NORMAL_LIST[0]);
const int FIND_AUTO_LIST_SIZE = sizeof(FIND_AUTO_LIST) / sizeof(FIND_AUTO_LIST[0]);

static void printFindType(FS_FEATURE_TYPE type);
static void printResult(const FS_FEATURE_RESULT *pResult);
int main(void)
{
	const char *FILE_NAME = "sample.xyz";
	int itor;

	FIND_SURFACE_CONTEXT ctx = NULL;
	FS_FEATURE_RESULT result;
	int ret = 0;

	float *pPointCloudList = NULL;
	int nPointCloudCount = 0;

	/* In this example, we use "sample.xyz" for target point cloud data */
	nPointCloudCount = ReadXYZ(&pPointCloudList, FILE_NAME);
	if (nPointCloudCount <= 0) {
		fprintf(stderr, "Fail to Read File <%s>\n", FILE_NAME);
		return -1; /* File Read Error */
	}

	ret = createFindSurface(&ctx);
	if (ret != 0) {
		fprintf(stderr, "Failed: createFindSurface(): ERR(%d)\n", ret);
		return -1;
	}


	/* Set target point cloud data */
	ret = setPointCloudFloat(ctx, pPointCloudList, nPointCloudCount, 0);
	if (ret != 0) {
		fprintf(stderr, "Failed: setPointCloudFloat(): ERR(%d)\n", ret);
		return -1;
	}

	/**************************************************************
	* CASE 1: Type of surface (to be found) is specified by user *
	**************************************************************/

	/* Individual parameters depend on the sensor used */
	setFindSurfaceParamFloat(ctx, FS_PARAM_ACCURACY, 0.010f); /* Set Sensor Measurement Accuracy */
	setFindSurfaceParamFloat(ctx, FS_PARAM_MEAN_DIST, 0.01f); /* Set Mean Distance of Neighboring Points */
	setFindSurfaceParamFloat(ctx, FS_PARAM_TOUCH_R, 0.025f);  /* Touch Size of the Seed Region */

														   /* Find Specific Type of the Surface */
	for (itor = 0; itor < FIND_NORMAL_LIST_SIZE; itor++)
	{
		printFindType(FIND_NORMAL_LIST[itor].find_type);
		ret = findSurface(ctx, FIND_NORMAL_LIST[itor].find_type, FIND_NORMAL_LIST[itor].seed_index, &result);

		if (ret == FS_NO_ERROR) {
			printResult(&result);
		}
		else {
			fprintf(stderr, "\tNot found (%d)\n", ret);
		}
		printf("\n");

		/*
		* In this case we use the same point cloud data for finding a surface
		* so, you don't need to cleanup the context
		*/
	}

	/*******************************************************
	* CASE 2: Type of surface is determined automatically *
	*******************************************************/

	setFindSurfaceParamFloat(ctx, FS_PARAM_ACCURACY, 0.003f); /* More accurate value needed */
	setFindSurfaceParamFloat(ctx, FS_PARAM_MEAN_DIST, 0.01f);
	setFindSurfaceParamFloat(ctx, FS_PARAM_TOUCH_R, 0.065f);  /* Large enough value needed */

														   /* Find Any Type of Surface */
	for (itor = 0; itor < FIND_AUTO_LIST_SIZE; itor++)
	{
		int retry = 0;

		printFindType(FS_TYPE_ANY);

		/*
		* Notice>
		* The sample data "sample.xyz" is "too noisy" and target object is "too small" to perform automation
		* So, there is no guarantee of finding the surface every time
		* Therefore, we try "findSurface()" several times (but still it can be failed)
		*/
		for (; retry < RETRY_COUNT; retry++)
		{
			ret = findSurface(ctx, FS_TYPE_ANY, FIND_AUTO_LIST[itor].seed_index, &result);

			if (ret == FS_NO_ERROR) {
				printResult(&result);
				printf("** try %d times\n", retry + 1);
				break;
			}
		}
		if (ret != FS_NO_ERROR) {
			fprintf(stderr, "\tNot found (%d)\n", ret);
		}
		printf("\n");
	}


	/* Clean Up and Release Context */
	cleanUpFindSurface(ctx);
	releaseFindSurface(ctx);

	return 0;
}

void printFindType(FS_FEATURE_TYPE type)
{
	switch (type)
	{
	case FS_TYPE_ANY:      printf("Find any type of surface\n");        return;
	case FS_TYPE_PLANE:    printf("Find <PLANE> type of surface\n");    return;
	case FS_TYPE_SPHERE:   printf("Find <SPHERE> type of surface\n");   return;
	case FS_TYPE_CYLINDER: printf("Find <Cylinder> type of surface\n"); return;
	case FS_TYPE_CONE:     printf("Find <Cone> type of surface\n");     return;
	case FS_TYPE_TORUS:    printf("Find <Torus> type of surface\n");    return;
	}
}

static float getLength(float x1, float y1, float z1, float x2, float y2, float z2) {
	float dx = x1 - x2;
	float dy = y1 - y2;
	float dz = z1 - z2;

	return sqrtf(dx * dx + dy * dy + dz * dz);
}

void printResult(const FS_FEATURE_RESULT *pResult)
{
	if (pResult == NULL) return;
	printf("> Find Surface [rms: %.3f]\n", pResult->rms);

	switch (pResult->type)
	{
	case FS_TYPE_PLANE:
		printf("\tPLNAE: w(%g), h(%g)\n",
			getLength(
				pResult->plane_param.ll[0], pResult->plane_param.ll[1], pResult->plane_param.ll[2],
				pResult->plane_param.lr[0], pResult->plane_param.lr[1], pResult->plane_param.lr[2]
			),
			getLength(
				pResult->plane_param.ll[0], pResult->plane_param.ll[1], pResult->plane_param.ll[2],
				pResult->plane_param.ul[0], pResult->plane_param.ul[1], pResult->plane_param.ul[2]
			)
		);
		break;

	case FS_TYPE_SPHERE:
		printf("\tSPHERE: r(%g)\n", pResult->sphere_param.r);
		break;

	case FS_TYPE_CYLINDER:
		printf("\tCYLINDER: r(%g), h(%g)\n",
			pResult->cylinder_param.r,
			getLength(
				pResult->cylinder_param.t[0], pResult->cylinder_param.t[1], pResult->cylinder_param.t[2],
				pResult->cylinder_param.b[0], pResult->cylinder_param.b[1], pResult->cylinder_param.b[2]
			)
		);
		break;

	case FS_TYPE_CONE:
		if (pResult->cone_param.tr == pResult->cone_param.br)
		{
			printf("\tCYLINDER: r(%g), h(%g)\n",
				pResult->cone_param.tr,
				getLength(
					pResult->cone_param.t[0], pResult->cone_param.t[1], pResult->cone_param.t[2],
					pResult->cone_param.b[0], pResult->cone_param.b[1], pResult->cone_param.b[2]
				)
			);
		}
		else
		{
			printf("\tCONE: small_r(%g), large_r(%g), h(%g)\n",
				pResult->cone_param.tr,
				pResult->cone_param.br,
				getLength(
					pResult->cone_param.t[0], pResult->cone_param.t[1], pResult->cone_param.t[2],
					pResult->cone_param.b[0], pResult->cone_param.b[1], pResult->cone_param.b[2]
				)
			);
		}
		break;

	case FS_TYPE_TORUS:
		if (pResult->torus_param.mr == 0.0f) /* It is not torus but sphere */
		{
			printf("\tSPHERE: r(%g)\n", pResult->torus_param.tr);
		}
		else if (pResult->torus_param.mr == FLT_MAX) /* It is not torus but cylinder */
		{
			printf("\tCYLINDER: r(%g), h(%g)\n",
				pResult->torus_param.tr,
				getLength(
					pResult->torus_param.c[0], pResult->torus_param.c[1], pResult->torus_param.c[2],
					pResult->torus_param.n[0], pResult->torus_param.n[1], pResult->torus_param.n[2]
				)
			);
		}
		else /* It is torus */
		{
			printf("\tTORI: mean_r(%g), tube_r(%g)\n", pResult->torus_param.mr, pResult->torus_param.tr);
		}
		break;
	}

}
