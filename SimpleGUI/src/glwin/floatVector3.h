#ifndef _FLOAT_VECTOR3_H_
#define _FLOAT_VECTOR3_H_

typedef struct _tagFloatVector3
{
	union 
	{
		struct {
			float x;
			float y;
			float z;
		};
		float v[3];
	};
} FLOAT_VECTOR3;

/* return v1 + v2 */
FLOAT_VECTOR3 *Vec3Add(FLOAT_VECTOR3 *pOut, const FLOAT_VECTOR3 *v1, const FLOAT_VECTOR3 *v2); 
/* return v1 - v2 */
FLOAT_VECTOR3 *Vec3Sub(FLOAT_VECTOR3 *pOut, const FLOAT_VECTOR3 *v1, const FLOAT_VECTOR3 *v2); 
/* return v1 * scale */
FLOAT_VECTOR3 *Vec3Scale(FLOAT_VECTOR3 *pOut, const FLOAT_VECTOR3 *v1, float scale);
/* return v1 / scale */
FLOAT_VECTOR3 *Vec3InvScale(FLOAT_VECTOR3 *pOut, const FLOAT_VECTOR3 *v1, float scale);
/* return v1 * (1-t) + v2 * s , ( 0 <= s <= 1 )*/
FLOAT_VECTOR3 *Vec3Lerp(FLOAT_VECTOR3 *pOut, const FLOAT_VECTOR3 *v1, const FLOAT_VECTOR3 *v2, float s);
/* Dot Product */
float Vec3Dot(const FLOAT_VECTOR3 *v1, const FLOAT_VECTOR3 *v2);
/* Cross Product */
FLOAT_VECTOR3 *Vec3Cross(FLOAT_VECTOR3 *pOut, const FLOAT_VECTOR3 *v1, const FLOAT_VECTOR3 *v2);
/* Normalize */
FLOAT_VECTOR3 *Vec3Normalize(FLOAT_VECTOR3 *pOut, const FLOAT_VECTOR3 *v1);
FLOAT_VECTOR3 *Vec3Normalize2(FLOAT_VECTOR3 *pOut, float *pOutLen, const FLOAT_VECTOR3 *v1);
/* Length of Vector */
float Vec3Length(const FLOAT_VECTOR3 *v1);
/* Squared Length of Vector */
float Vec3SquaredLength(const FLOAT_VECTOR3 *v1);
/* Distance between 2 Vectors */
float Vec3Distance(const FLOAT_VECTOR3 *v1, const FLOAT_VECTOR3 *v2);

#endif
