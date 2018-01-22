#include "dataManager.h"
#include <stdlib.h>
#include <string.h>
#include <float.h>
#include "xyz_reader.h"

#include "glwin/floatVector3.h"

static struct 
{
	/* Manage Origianl Data */
	struct {
		const float *pPointCloudList;
		unsigned int nPointCount;
		struct { FLOAT_VECTOR3 min, max; } bbox;
		FLOAT_VECTOR3 center;
	} original;

	/* Manage Outliers */
	struct {
		float *pPointCloudList;
		unsigned int nPointCount;
	} outliers; /* Source Data For FindSurface */
} DataManager;

int dmLoadPointCloudFromFile(const char *szFileName)
{
	float *pPCL = 0;
	int nCnt = ReadXYZ(&pPCL, szFileName);
	if (nCnt < 0) return nCnt; /* Fail to Read File || Memory Allocation Error */
	else if (nCnt == 0) return 1; /* File does not contain Point Cloud Data */

	DataManager.original.pPointCloudList = pPCL;
	DataManager.original.nPointCount = (unsigned int)nCnt;

	DataManager.outliers.pPointCloudList = (float *)malloc(sizeof(float) * 3 * nCnt);
	if (DataManager.outliers.pPointCloudList == 0) {
		dmClear();
		return -1; /* Memory Allocation Error */
	}
	dmResetOutliers();

	/* calculate bounding box & center (of mass) */
	{
		FLOAT_VECTOR3 min = { FLT_MAX, FLT_MAX, FLT_MAX };
		FLOAT_VECTOR3 max = { -FLT_MAX, -FLT_MAX, -FLT_MAX };
		double acc_x = 0.0, acc_y = 0.0, acc_z = 0.0;
		int i;

		for (i = 0; i < nCnt; i++, pPCL += 3) {
			if (pPCL[0] < min.x) min.x = pPCL[0]; 
			if (pPCL[0] > max.x) max.x = pPCL[0];
			acc_x += pPCL[0];

			if (pPCL[1] < min.y) min.y = pPCL[1];
			if (pPCL[1] > max.y) max.y = pPCL[1];
			acc_y += pPCL[1];

			if (pPCL[2] < min.z) min.z = pPCL[2];
			if (pPCL[2] > max.z) max.z = pPCL[2];
			acc_z += pPCL[2];
		}

		DataManager.original.bbox.min = min;
		DataManager.original.bbox.max = max;
		DataManager.original.center.x = (float)(acc_x / nCnt);
		DataManager.original.center.y = (float)(acc_y / nCnt);
		DataManager.original.center.z = (float)(acc_z / nCnt);
	}

	return 0; /* Success */
}

void dmResetOutliers()
{
	dmUpdateOutliers(DataManager.original.pPointCloudList, DataManager.original.nPointCount);
}

void dmUpdateOutliers(const float *pOutliers, unsigned int nCnt)
{
	if (DataManager.outliers.pPointCloudList && pOutliers) {
		unsigned int min = nCnt < DataManager.original.nPointCount ? nCnt : DataManager.original.nPointCount;
		if (min > 0) {
			memcpy(DataManager.outliers.pPointCloudList, pOutliers, sizeof(float) * 3 * min);
		}
		DataManager.outliers.nPointCount = min;
	}
}

void dmClear()
{
	if (DataManager.original.pPointCloudList) {
		free(DataManager.original.pPointCloudList);
	}
	if (DataManager.outliers.pPointCloudList) {
		free(DataManager.outliers.pPointCloudList);
	}
	memset(&DataManager, 0x00, sizeof(DataManager));
}

const float *dmGetOriginalData()
{
	return DataManager.original.pPointCloudList;
}

unsigned int dmGetOriginalCount()
{
	return DataManager.original.nPointCount;
}

const FLOAT_VECTOR3 *dmGetBoundingBoxMin()
{
	return &(DataManager.original.bbox.min);
}

const FLOAT_VECTOR3 *dmGetBoundingBoxMax()
{
	return &(DataManager.original.bbox.max);
}

const float* dmGetOutliersData()
{
	return DataManager.outliers.pPointCloudList;
}

unsigned int dmGetOutliersCount()
{
	return DataManager.outliers.nPointCount;
}