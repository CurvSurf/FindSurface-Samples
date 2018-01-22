#include "camera.h"
#include <stdlib.h> /* for malloc(), free() */
#include <math.h> /* atanf() sinf()*/
#define IMPL(p) ((CAMERA_CONTEXT_IMPL *)(p))

typedef struct _tagImplCameraContext
{
	FLOAT_VECTOR3 eye;
	FLOAT_VECTOR3 at;

	float fovY;
	float aspect;
	float near;
	float far;

	FLOAT_MATRIX view;
	FLOAT_MATRIX proj;
}CAMERA_CONTEXT_IMPL;


CAMERA_CONTEXT createCamera(float aspect) {
	const FLOAT_VECTOR3 def_pos = { 0, 0, 0 };
	const FLOAT_VECTOR3 def_at = { 0, 0, -1 };
	const FLOAT_VECTOR3 def_up = { 0, 1, 0 };

	return createCameraWithParam(&def_pos, &def_at, &def_up, _TO_RADIAN(45.0f), aspect, 0.1f, 1000.0f);
}

CAMERA_CONTEXT createCameraWithParam(
	const FLOAT_VECTOR3 *pPos, const FLOAT_VECTOR3 *pAt, const FLOAT_VECTOR3 *pBaseUp,
	float fovY, float aspect, float near, float far
)
{
	CAMERA_CONTEXT_IMPL *pImpl = (CAMERA_CONTEXT_IMPL *)malloc(sizeof(CAMERA_CONTEXT_IMPL));
	if (pImpl) {
		updateCameraWithParam( pImpl, pPos, pAt, pBaseUp, fovY, aspect, near, far );
	}
	return pImpl;
}

void releaseCamera(CAMERA_CONTEXT pCtx) {
	if (pCtx) { free(pCtx); }
}

void updateCameraViewMatrix(CAMERA_CONTEXT pCtx, const FLOAT_VECTOR3 *pPos, const FLOAT_VECTOR3 *pAt, const FLOAT_VECTOR3 *pBaseUp)
{
	if (pCtx) {
		IMPL(pCtx)->eye = *pPos;
		IMPL(pCtx)->at = *pAt;

		MatrixLookAtRH(&(IMPL(pCtx)->view),
			pPos->x, pPos->y, pPos->z,
			pAt->x, pAt->y, pAt->z,
			pBaseUp->x, pBaseUp->y, pBaseUp->z);
	}
}

void updateCameraProjMatrix(CAMERA_CONTEXT pCtx, float fovY, float aspect, float near, float far)
{
	if (pCtx) {
		IMPL(pCtx)->fovY = fovY;
		IMPL(pCtx)->aspect = aspect;
		IMPL(pCtx)->near = near;
		IMPL(pCtx)->far = far;

		MatrixPerspectiveFovRH(&(IMPL(pCtx)->proj), fovY, aspect, near, far);
	}
}

void updateCameraWithParam(
	CAMERA_CONTEXT pCtx,
	const FLOAT_VECTOR3 *pPos, const FLOAT_VECTOR3 *pAt, const FLOAT_VECTOR3 *pBaseUp,
	float fovY, float aspect, float near, float far
)
{
	if (pCtx) {
		updateCameraViewMatrix(pCtx, pPos, pAt, pBaseUp);
		updateCameraProjMatrix(pCtx, fovY, aspect, near, far);
	}
}

void updateCameraProjectionFov(CAMERA_CONTEXT pCtx, float fovY)
{
	if (pCtx) {
		IMPL(pCtx)->fovY = fovY;
		MatrixPerspectiveFovRH(&(IMPL(pCtx)->proj), fovY, IMPL(pCtx)->aspect, IMPL(pCtx)->near, IMPL(pCtx)->far);
	}
}

void updateCameraProjectionRatio(CAMERA_CONTEXT pCtx, float aspect)
{
	if (pCtx) {
		IMPL(pCtx)->aspect = aspect;
		MatrixPerspectiveFovRH(&(IMPL(pCtx)->proj), IMPL(pCtx)->fovY, aspect, IMPL(pCtx)->near, IMPL(pCtx)->far);
	}
}

void updateCameraViewWithBoundingBoxAxis(CAMERA_CONTEXT pCtx, const FLOAT_VECTOR3 *pMin, const FLOAT_VECTOR3 *pMax, const FLOAT_VECTOR3 *pAxis, const FLOAT_VECTOR3 *pUpAxis)
{
	FLOAT_VECTOR3 tmp = { pMax->x - pMin->x, pMax->y - pMin->y, pMax->z - pMin->z };
	FLOAT_VECTOR3 center = { (pMax->x + pMin->x) / 2.0f, (pMax->y + pMin->y) / 2.0f , (pMax->z + pMin->z) / 2.0f };
	float r = Vec3Length(&tmp) / 2.0f;
	float s = sinf(atanf(1.0f / (IMPL(pCtx)->proj.arr[5] > IMPL(pCtx)->proj.arr[0] ? IMPL(pCtx)->proj.arr[5] : IMPL(pCtx)->proj.arr[0])));
	float l = r / s;

	Vec3Add(&tmp, &center, Vec3Scale(&tmp, pAxis, l));
	updateCameraViewMatrix(pCtx, &tmp, &center, pUpAxis);
}

static void _updateRotateCamera(CAMERA_CONTEXT pCtx, const FLOAT_MATRIX *pRMat)
{
	FLOAT_VECTOR3 tmp = { IMPL(pCtx)->eye.x - IMPL(pCtx)->at.x, IMPL(pCtx)->eye.y - IMPL(pCtx)->at.y, IMPL(pCtx)->eye.z - IMPL(pCtx)->at.z };
	FLOAT_VECTOR3 new_x, new_y, new_z;

	/* Rotate Camera Position */
	IMPL(pCtx)->eye.x = tmp.x * pRMat->arr[0] + tmp.y * pRMat->arr[4] + tmp.z * pRMat->arr[8] + IMPL(pCtx)->at.x;
	IMPL(pCtx)->eye.y = tmp.x * pRMat->arr[1] + tmp.y * pRMat->arr[5] + tmp.z * pRMat->arr[9] + IMPL(pCtx)->at.y;
	IMPL(pCtx)->eye.z = tmp.x * pRMat->arr[2] + tmp.y * pRMat->arr[6] + tmp.z * pRMat->arr[10] + IMPL(pCtx)->at.z;

	/* Rotate Camera Axis */
	new_x.x = IMPL(pCtx)->view.arr[0] * pRMat->arr[0] + IMPL(pCtx)->view.arr[4] * pRMat->arr[4] + IMPL(pCtx)->view.arr[8] * pRMat->arr[8];
	new_x.y = IMPL(pCtx)->view.arr[0] * pRMat->arr[1] + IMPL(pCtx)->view.arr[4] * pRMat->arr[5] + IMPL(pCtx)->view.arr[8] * pRMat->arr[9];
	new_x.z = IMPL(pCtx)->view.arr[0] * pRMat->arr[2] + IMPL(pCtx)->view.arr[4] * pRMat->arr[6] + IMPL(pCtx)->view.arr[8] * pRMat->arr[10];

	new_y.x = IMPL(pCtx)->view.arr[1] * pRMat->arr[0] + IMPL(pCtx)->view.arr[5] * pRMat->arr[4] + IMPL(pCtx)->view.arr[9] * pRMat->arr[8];
	new_y.y = IMPL(pCtx)->view.arr[1] * pRMat->arr[1] + IMPL(pCtx)->view.arr[5] * pRMat->arr[5] + IMPL(pCtx)->view.arr[9] * pRMat->arr[9];
	new_y.z = IMPL(pCtx)->view.arr[1] * pRMat->arr[2] + IMPL(pCtx)->view.arr[5] * pRMat->arr[6] + IMPL(pCtx)->view.arr[9] * pRMat->arr[10];

	new_z.x = IMPL(pCtx)->view.arr[2] * pRMat->arr[0] + IMPL(pCtx)->view.arr[6] * pRMat->arr[4] + IMPL(pCtx)->view.arr[10] * pRMat->arr[8];
	new_z.y = IMPL(pCtx)->view.arr[2] * pRMat->arr[1] + IMPL(pCtx)->view.arr[6] * pRMat->arr[5] + IMPL(pCtx)->view.arr[10] * pRMat->arr[9];
	new_z.z = IMPL(pCtx)->view.arr[2] * pRMat->arr[2] + IMPL(pCtx)->view.arr[6] * pRMat->arr[6] + IMPL(pCtx)->view.arr[10] * pRMat->arr[10];

	/* Update View Matrix */
	IMPL(pCtx)->view.arr[0] = new_x.x;    IMPL(pCtx)->view.arr[4] = new_x.y;    IMPL(pCtx)->view.arr[8] = new_x.z;
	IMPL(pCtx)->view.arr[1] = new_y.x;    IMPL(pCtx)->view.arr[5] = new_y.y;    IMPL(pCtx)->view.arr[9] = new_y.z;
	IMPL(pCtx)->view.arr[2] = new_z.x;    IMPL(pCtx)->view.arr[6] = new_z.y;    IMPL(pCtx)->view.arr[10] = new_z.z;

	IMPL(pCtx)->view.arr[12] = -Vec3Dot(&(IMPL(pCtx)->eye), &new_x);
	IMPL(pCtx)->view.arr[13] = -Vec3Dot(&(IMPL(pCtx)->eye), &new_y);
	IMPL(pCtx)->view.arr[14] = -Vec3Dot(&(IMPL(pCtx)->eye), &new_z);
}

void rotateCamera1(CAMERA_CONTEXT pCtx, float h_angle, float v_angle)
{
	FLOAT_VECTOR3 h_axis = { IMPL(pCtx)->view.arr[1], IMPL(pCtx)->view.arr[5],  IMPL(pCtx)->view.arr[9] };
	FLOAT_VECTOR3 v_axis = { IMPL(pCtx)->view.arr[0], IMPL(pCtx)->view.arr[4],  IMPL(pCtx)->view.arr[8] };
	FLOAT_MATRIX hMat, vMat, rMat;

	MatrixRotationAxis(&hMat, h_axis.x, h_axis.y, h_axis.z, h_angle);
	MatrixRotationAxis(&vMat, v_axis.x, v_axis.y, v_axis.z, v_angle);
	MatrixMulGL(&rMat, &hMat, &vMat);

	_updateRotateCamera(pCtx, &rMat);
}

void rotateCamera3(CAMERA_CONTEXT pCtx, float h_angle, float v_angle)
{
	FLOAT_VECTOR3 h_axis = { 0, 1, 0 };
	FLOAT_VECTOR3 v_axis = { IMPL(pCtx)->view.arr[0], IMPL(pCtx)->view.arr[4],  IMPL(pCtx)->view.arr[8] };
	FLOAT_MATRIX hMat, vMat, rMat;

	MatrixRotationAxis(&hMat, h_axis.x, h_axis.y, h_axis.z, h_angle);
	MatrixRotationAxis(&vMat, v_axis.x, v_axis.y, v_axis.z, v_angle);
	MatrixMulGL(&rMat, &hMat, &vMat);

	_updateRotateCamera(pCtx, &rMat);
}

void moveCamera(CAMERA_CONTEXT pCtx, float h_dist, float v_dist)
{
	FLOAT_VECTOR3 move = {
		IMPL(pCtx)->view.arr[0] * h_dist + IMPL(pCtx)->view.arr[1] * v_dist,
		IMPL(pCtx)->view.arr[4] * h_dist + IMPL(pCtx)->view.arr[5] * v_dist,
		IMPL(pCtx)->view.arr[8] * h_dist + IMPL(pCtx)->view.arr[9] * v_dist
	};

	FLOAT_VECTOR3 _x = { IMPL(pCtx)->view.arr[0], IMPL(pCtx)->view.arr[4], IMPL(pCtx)->view.arr[8] };
	FLOAT_VECTOR3 _y = { IMPL(pCtx)->view.arr[1], IMPL(pCtx)->view.arr[5], IMPL(pCtx)->view.arr[9] };
	FLOAT_VECTOR3 _z = { IMPL(pCtx)->view.arr[2], IMPL(pCtx)->view.arr[6], IMPL(pCtx)->view.arr[10] };

	Vec3Add(&(IMPL(pCtx)->eye), &(IMPL(pCtx)->eye), &move);
	Vec3Add(&(IMPL(pCtx)->at), &(IMPL(pCtx)->at), &move);

	IMPL(pCtx)->view.arr[12] = -Vec3Dot(&(IMPL(pCtx)->eye), &_x);
	IMPL(pCtx)->view.arr[13] = -Vec3Dot(&(IMPL(pCtx)->eye), &_y);
	IMPL(pCtx)->view.arr[14] = -Vec3Dot(&(IMPL(pCtx)->eye), &_z);
}

void zoomCamera(CAMERA_CONTEXT pCtx, float dist)
{
	FLOAT_VECTOR3 move_dir = { IMPL(pCtx)->view.arr[2], IMPL(pCtx)->view.arr[6], IMPL(pCtx)->view.arr[10] };
	FLOAT_VECTOR3 distVec = { IMPL(pCtx)->eye.x - IMPL(pCtx)->at.x, IMPL(pCtx)->eye.y - IMPL(pCtx)->at.y, IMPL(pCtx)->eye.z - IMPL(pCtx)->at.z };

	if (Vec3Length(&distVec) + dist > 0.1f) {
		FLOAT_VECTOR3 _x = { IMPL(pCtx)->view.arr[0], IMPL(pCtx)->view.arr[4], IMPL(pCtx)->view.arr[8] };
		FLOAT_VECTOR3 _y = { IMPL(pCtx)->view.arr[1], IMPL(pCtx)->view.arr[5], IMPL(pCtx)->view.arr[9] };
		FLOAT_VECTOR3 _z = { IMPL(pCtx)->view.arr[2], IMPL(pCtx)->view.arr[6], IMPL(pCtx)->view.arr[10] };

		Vec3Add( &(IMPL(pCtx)->eye), 
			&(IMPL(pCtx)->eye), 
			Vec3Scale(&move_dir, &move_dir, dist)
		);

		IMPL(pCtx)->view.arr[12] = -Vec3Dot(&(IMPL(pCtx)->eye), &_x);
		IMPL(pCtx)->view.arr[13] = -Vec3Dot(&(IMPL(pCtx)->eye), &_y);
		IMPL(pCtx)->view.arr[14] = -Vec3Dot(&(IMPL(pCtx)->eye), &_z);
	}
}

const FLOAT_MATRIX *getCameraViewMatrix(CAMERA_CONTEXT pCtx)
{
	return pCtx ? &(IMPL(pCtx)->view) : (const FLOAT_MATRIX *)0;
}

const FLOAT_MATRIX *getCameraProjMatrix(CAMERA_CONTEXT pCtx)
{
	return pCtx ? &(IMPL(pCtx)->proj) : (const FLOAT_MATRIX *)0;
}