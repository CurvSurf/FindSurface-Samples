#ifndef _CAMERA_H_
#define _CAMERA_H_

#include "floatMatrix.h"
#include "floatVector3.h"

typedef void* CAMERA_CONTEXT;

CAMERA_CONTEXT createCamera(float aspect);
CAMERA_CONTEXT createCameraWithParam (
	const FLOAT_VECTOR3 *pPos, const FLOAT_VECTOR3 *pAt, const FLOAT_VECTOR3 *pBaseUp,
	float fovY, float aspect, float near, float far
);
void releaseCamera(CAMERA_CONTEXT pCtx);

void updateCameraViewMatrix(CAMERA_CONTEXT pCtx, const FLOAT_VECTOR3 *pPos, const FLOAT_VECTOR3 *pAt, const FLOAT_VECTOR3 *pBaseUp);
void updateCameraProjMatrix(CAMERA_CONTEXT pCtx, float fovY, float aspect, float near, float far);
void updateCameraWithParam(
	CAMERA_CONTEXT pCtx,
	const FLOAT_VECTOR3 *pPos, const FLOAT_VECTOR3 *pAt, const FLOAT_VECTOR3 *pBaseUp,
	float fovY, float aspect, float near, float far
);

void updateCameraProjectionFov(CAMERA_CONTEXT pCtx, float fovY);
void updateCameraProjectionRatio(CAMERA_CONTEXT pCtx, float aspect);

void updateCameraViewWithBoundingBoxAxis(CAMERA_CONTEXT pCtx, const FLOAT_VECTOR3 *pMin, const FLOAT_VECTOR3 *pMax, const FLOAT_VECTOR3 *pAxis, const FLOAT_VECTOR3 *pUpAxis);

void rotateCamera1(CAMERA_CONTEXT pCtx, float h_angle, float v_angle);
void rotateCamera3(CAMERA_CONTEXT pCtx, float h_angle, float v_angle);
void moveCamera(CAMERA_CONTEXT pCtx, float h_dist, float v_dist);
void zoomCamera(CAMERA_CONTEXT pCtx, float dist);

const FLOAT_MATRIX *getCameraViewMatrix(CAMERA_CONTEXT pCtx);
const FLOAT_MATRIX *getCameraProjMatrix(CAMERA_CONTEXT pCtx);

#endif