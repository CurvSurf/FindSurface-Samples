#include "floatVector3.h"
#include <math.h>

FLOAT_VECTOR3 *Vec3Add(FLOAT_VECTOR3 *pOut, const FLOAT_VECTOR3 *v1, const FLOAT_VECTOR3 *v2)
{
	pOut->x = v1->x + v2->x;
	pOut->y = v1->y + v2->y;
	pOut->z = v1->z + v2->z;

	return pOut;
}

FLOAT_VECTOR3 *Vec3Sub(FLOAT_VECTOR3 *pOut, const FLOAT_VECTOR3 *v1, const FLOAT_VECTOR3 *v2)
{
	pOut->x = v1->x - v2->x;
	pOut->y = v1->y - v2->y;
	pOut->z = v1->z - v2->z;

	return pOut;
}

FLOAT_VECTOR3 *Vec3Scale(FLOAT_VECTOR3 *pOut, const FLOAT_VECTOR3 *v1, float scale)
{
	pOut->x = v1->x * scale;
	pOut->y = v1->y * scale;
	pOut->z = v1->z * scale;

	return pOut;
}

FLOAT_VECTOR3 *Vec3InvScale(FLOAT_VECTOR3 *pOut, const FLOAT_VECTOR3 *v1, float scale)
{
	pOut->x = v1->x / scale;
	pOut->y = v1->y / scale;
	pOut->z = v1->z / scale;

	return pOut;
}

FLOAT_VECTOR3 *Vec3Lerp(FLOAT_VECTOR3 *pOut, const FLOAT_VECTOR3 *v1, const FLOAT_VECTOR3 *v2, float s)
{
	pOut->x = ( v1->x * ( 1.0f - s ) ) + ( v2->x * s );
	pOut->y = ( v1->y * ( 1.0f - s ) ) + ( v2->y * s );
	pOut->z = ( v1->z * ( 1.0f - s ) ) + ( v2->z * s );

	return pOut;
}

float Vec3Dot(const FLOAT_VECTOR3 *v1, const FLOAT_VECTOR3 *v2)
{
	return ( v1->x * v2->x ) + ( v1->y * v2->y ) + ( v1->z * v2->z );
}

FLOAT_VECTOR3 *Vec3Cross(FLOAT_VECTOR3 *pOut, const FLOAT_VECTOR3 *v1, const FLOAT_VECTOR3 *v2)
{
	FLOAT_VECTOR3 tmp = {
		(v1->y * v2->z) - (v1->z * v2->y),
		(v1->z * v2->x) - (v1->x * v2->z),
		(v1->x * v2->y) - (v1->y * v2->x)
	};

	*pOut = tmp;
	return pOut;
}

FLOAT_VECTOR3 *Vec3Normalize(FLOAT_VECTOR3 *pOut, const FLOAT_VECTOR3 *v1)
{
	float len = sqrtf((v1->x * v1->x) + (v1->y * v1->y) + (v1->z * v1->z));
	if (len < 0.001f) { /* len == 0 */
		if (pOut != v1) {
			*pOut = *v1;
		}
	} 
	else {
		pOut->x = v1->x / len;
		pOut->y = v1->y / len;
		pOut->z = v1->z / len;
	}
	return pOut;
}

FLOAT_VECTOR3 *Vec3Normalize2(FLOAT_VECTOR3 *pOut, float *pOutLen, const FLOAT_VECTOR3 *v1)
{
	float len = sqrtf((v1->x * v1->x) + (v1->y * v1->y) + (v1->z * v1->z));
	if (len < 0.001f) { /* len == 0 */
		if (pOut != v1) {
			*pOut = *v1;
		}
		if (pOutLen) *pOutLen = 0.0f;
	}
	else {
		pOut->x = v1->x / len;
		pOut->y = v1->y / len;
		pOut->z = v1->z / len;
		if (pOutLen) *pOutLen = len;
	}
	return pOut;
}

float Vec3Length(const FLOAT_VECTOR3 *v1)
{
	return sqrtf( (v1->x * v1->x) + (v1->y * v1->y) + (v1->z * v1->z) );
}

float Vec3SquaredLength(const FLOAT_VECTOR3 *v1)
{
	return (v1->x * v1->x) + (v1->y * v1->y) + (v1->z * v1->z);
}

float Vec3Distance(const FLOAT_VECTOR3 *v1, const FLOAT_VECTOR3 *v2)
{
	float dx = v1->x - v2->x;
	float dy = v1->y - v2->y;
	float dz = v1->z - v2->z;

	return sqrtf((dx * dx) + (dy * dy) + (dz * dz));
}