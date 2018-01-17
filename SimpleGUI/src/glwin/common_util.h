#ifndef _COMMON_UTIL_H_
#define _COMMON_UTIL_H_

#include "floatVector3.h"
#include "floatMatrix.h"

/*****************************************************************
 * crx - normalized client position x ( -1.0 ~ 1.0 )             *
 * cry - normalized client position y ( -1.0 ~ 1.0 )             *
 *                                                               *
 *   ########## 1.0 ########## -> client window                  *
 *   #           |           #									 *
 *   #           |           #									 *
 * -1.0 ---------+--------- 1.0									 *
 *   #           |           #									 *
 *   #           |           #									 *
 *   ######### -1.0 ##########									 *
 *																 *
 *****************************************************************/
void clientToWorldRayLH(FLOAT_VECTOR3 *pOutRayOrig, FLOAT_VECTOR3 *pOutRayDir, float crx, float cry, const FLOAT_MATRIX *pViewMat, const FLOAT_MATRIX *pProjMat);
void clientToWorldRayRH(FLOAT_VECTOR3 *pOutRayOrig, FLOAT_VECTOR3 *pOutRayDir, float crx, float cry, const FLOAT_MATRIX *pViewMat, const FLOAT_MATRIX *pProjMat);

/*
 * stride - byte size per point (if this value is 0, pPointList is assumed that tightly packed position only list (=sizeof(float)*3)
 * pPointList - First 3 components must be position (x, y, z)
 */
int rayPickPoint(const FLOAT_VECTOR3 *pOrig, const FLOAT_VECTOR3 *pDir, const float picking_radius, const void* pPointList, int nCnt, int stride);

#endif
