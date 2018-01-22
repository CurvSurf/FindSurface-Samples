#include "common_util.h"
#include <float.h>
#include <math.h>

static void _clientToWorldRay(FLOAT_VECTOR3 *pOutRayOrig, FLOAT_VECTOR3 *pOutRayDir, float crx, float cry, const FLOAT_MATRIX *pViewMat, const FLOAT_MATRIX *pProjMat, int isLH)
{
	FLOAT_VECTOR3 base_dir = {
		crx / pProjMat->arr[0],
		cry / pProjMat->arr[5],
		isLH ? 1.0f : -1.0f
	};

	FLOAT_MATRIX invView = {
		pViewMat->arr[0], 
		pViewMat->arr[4], 
		pViewMat->arr[8], 
		0.0f,

		pViewMat->arr[1], 
		pViewMat->arr[5], 
		pViewMat->arr[9], 
		0.0f,

		pViewMat->arr[2], 
		pViewMat->arr[6], 
		pViewMat->arr[10], 
		0.0f,

		-( (pViewMat->arr[12] * pViewMat->arr[0]) + (pViewMat->arr[13] * pViewMat->arr[1]) + (pViewMat->arr[14] * pViewMat->arr[2]) ), 
		-( (pViewMat->arr[12] * pViewMat->arr[4]) + (pViewMat->arr[13] * pViewMat->arr[5]) + (pViewMat->arr[14] * pViewMat->arr[6]) ), 
		-( (pViewMat->arr[12] * pViewMat->arr[8]) + (pViewMat->arr[13] * pViewMat->arr[9]) + (pViewMat->arr[14] * pViewMat->arr[10]) ), 
		1.0f
	};

	pOutRayDir->x = (base_dir.x * invView.arr[0]) + (base_dir.y * invView.arr[4]) + (base_dir.z * invView.arr[8]);
	pOutRayDir->y = (base_dir.x * invView.arr[1]) + (base_dir.y * invView.arr[5]) + (base_dir.z * invView.arr[9]);
	pOutRayDir->z = (base_dir.x * invView.arr[2]) + (base_dir.y * invView.arr[6]) + (base_dir.z * invView.arr[10]);
	Vec3Normalize(pOutRayDir, pOutRayDir);

	pOutRayOrig->x = invView.arr[12];
	pOutRayOrig->y = invView.arr[13];
	pOutRayOrig->z = invView.arr[14];
}

void clientToWorldRayLH(FLOAT_VECTOR3 *pOutRayOrig, FLOAT_VECTOR3 *pOutRayDir, float crx, float cry, const FLOAT_MATRIX *pViewMat, const FLOAT_MATRIX *pProjMat)
{
	_clientToWorldRay(pOutRayOrig, pOutRayDir, crx, cry, pViewMat, pProjMat, 1);
}

void clientToWorldRayRH(FLOAT_VECTOR3 *pOutRayOrig, FLOAT_VECTOR3 *pOutRayDir, float crx, float cry, const FLOAT_MATRIX *pViewMat, const FLOAT_MATRIX *pProjMat)
{
	_clientToWorldRay(pOutRayOrig, pOutRayDir, crx, cry, pViewMat, pProjMat, 0);
}

int rayPickPoint(const FLOAT_VECTOR3 *pOrig, const FLOAT_VECTOR3 *pDir, const float picking_radius, const void* pPointList, int nCnt, int stride)
{
	const float PICKING_RANGE  = 0.005f;
	const float DEFAULT_STRIDE = sizeof(float) * 3; /* x, y, z */

	int   itor;
	int   idx = -1;
	float min_length = FLT_MAX;
	float dist = 0.0f;
	float pick_range = picking_radius <= 0.0f ? PICKING_RANGE : picking_radius;
	unsigned char *pCurr = (unsigned char *)pPointList;
	
	if (stride < DEFAULT_STRIDE) { stride = DEFAULT_STRIDE; }
	for (itor = 0; itor < nCnt; itor++)
	{
		/* Get Shortest Length From Ray to Point */
		const FLOAT_VECTOR3 *pPoint = (const FLOAT_VECTOR3 *)pCurr;
		FLOAT_VECTOR3 sub = { pPoint->x - pOrig->x, pPoint->y - pOrig->y, pPoint->z - pOrig->z };
		float len1 = Vec3Dot(pDir, &sub);
		float len2 = Vec3Length(&sub);

		if(len1 < 0.0f) { /* len1 < 0 */
			continue;
		}

		/* shortest length = sqrt( len2*len2 - len1*len1 ) */
		dist = sqrtf((len2 * len2) - (len1 * len1));
		if (dist < pick_range) {
			if (dist < min_length) { /* find the nearest point from the origin of the ray */
				idx = itor;
				min_length = dist;
			}
		}
		pCurr += stride;
	}

	return idx;
}