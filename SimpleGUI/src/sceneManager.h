#ifndef _SCENE_MANAGER_H_
#define _SCENE_MANAGER_H_

#include "glwin/floatVector3.h"
#include "glwin/floatMatrix.h"
#include <FindSurface.h>

typedef struct {
	FLOAT_VECTOR3 right;
	float ratio;
}TORUS_EXTRA_PARAM;

int smInitSceneManager(float scene_aspect_ratio);
void smReleaseSceneManager();

const FLOAT_MATRIX *smGetViewMatrix();
const FLOAT_MATRIX *smGetProjMatrix();

/* Handling Render Target */
int smSetOutliers(const float *pData, unsigned int nCnt);
void smSetTouchRadius(const FLOAT_VECTOR3 *pPos, float r); /* if pPos == NULL || r <= 0 then hide Touch Radius UI */
int smAppendFindSurfaceResult(const float *inliers_data, unsigned int inliers_count, const FS_FEATURE_RESULT *pResult, const TORUS_EXTRA_PARAM *pParam);
void smClearRenderList();

/* Handling View */
void smRotateView(float h_angle, float v_angle);
void smMoveView(float h_dist, float v_dist);
void smZoomView(float delta);
void smSetFrontView();
void smSetTopView();
void smSetSideView();
void smOnUpdateSceneRatio(float scene_aspect_ratio);

/* Render */
void smRender();

#endif
