#ifndef _DATA_MANAGER_H_
#define _DATA_MANAGER_H_

#include "glwin/floatVector3.h"

int  dmLoadPointCloudFromFile(const char *szFileName);
void dmResetOutliers();
void dmUpdateOutliers(const float *pOutliers, unsigned int nCnt); /* nCnt must be smaller than original's point count */
void dmClear();

const float *dmGetOriginalData();
unsigned int dmGetOriginalCount();

const FLOAT_VECTOR3 *dmGetBoundingBoxMin();
const FLOAT_VECTOR3 *dmGetBoundingBoxMax();

const float* dmGetOutliersData();
unsigned int dmGetOutliersCount();

#endif
