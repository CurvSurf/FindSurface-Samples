#include "sceneManager.h"
#include "renderObjectFactory.h"
#include "pointCloudRenderer.h"
#include "customMeshRenderer.h"
#include "glwin/camera.h"
#include "glwin/floatVector3.h"
#include "glwin/floatMatrix.h"

#include "dataManager.h"

#include <stdlib.h> /* malloc() / free() */
#include <string.h> /* memset() memcpy() */

typedef struct _tagRenderList
{
	RENDER_OBJECT inliers;
	RENDER_OBJECT mesh;

	struct _tagRenderList *pNext;
}RenderList;

static struct
{
	CAMERA_CONTEXT current_view;
	struct {
		RENDER_OBJECT outliers;
		RENDER_OBJECT toruch_radius;
		RENDER_OBJECT *pTouchRadius;

		RenderList *pListHead, *pListTail;
	} render_target;
}SceneManager = { 0, }, *g_pSM = 0;

int smInitSceneManager(float scene_aspect_ratio)
{
	int ret = 0;
	if (g_pSM) return 0;

	g_pSM = &SceneManager;

	ret = initRenderObjectFactory();
	if (ret < 0) { smReleaseSceneManager(); return -1; }

	ret = createPointCloudRenderer();
	if (ret < 0) { smReleaseSceneManager(); return -1; }

	ret = createCustomMeshRenderer();
	if (ret < 0) { smReleaseSceneManager(); return -1; }

	g_pSM->current_view = createCamera(scene_aspect_ratio);
	if (g_pSM->current_view == 0) { smReleaseSceneManager(); return -1; }

	memset(&SceneManager.render_target, 0x00, sizeof(SceneManager.render_target));
	{
		float c[] = { 0, 0, 0 };
		getSphereRenderObject(&(SceneManager.render_target.toruch_radius), &c, 1.0f);

		SceneManager.render_target.toruch_radius.colors[0] = 1.0f;
		SceneManager.render_target.toruch_radius.colors[1] = 1.0f;
		SceneManager.render_target.toruch_radius.colors[2] = 1.0f;
		SceneManager.render_target.toruch_radius.colors[3] = 0.5f;
	}

	return 0;
}

void smReleaseSceneManager()
{
	if (g_pSM) {
		smClearRenderList();
		releaseRenderObjectFactory();
		releasePointCloudRenderer();
		releaseCustomMeshRenderer();
		if (SceneManager.current_view) {
			releaseCamera(SceneManager.current_view);
		}
		memset(&SceneManager, 0x00, sizeof(SceneManager));
		g_pSM = 0;
	}
}

const FLOAT_MATRIX *smGetViewMatrix()
{
	return getCameraViewMatrix(g_pSM->current_view);
}

const FLOAT_MATRIX *smGetProjMatrix()
{
	return getCameraProjMatrix(g_pSM->current_view);
}

int smSetOutliers(const float *pData, unsigned int nCnt)
{
	RENDER_OBJECT *pTarget = &(g_pSM->render_target.outliers);
	if (!(pTarget->vb_id)) {
		if (genPointRenderObject(pTarget, pData, nCnt) < 0 ) return -1;
		pTarget->colors[0] = 1.0f; /* R */
		pTarget->colors[1] = 1.0f; /* G */
		pTarget->colors[2] = 1.0f; /* B */
		pTarget->colors[3] = 1.0f; /* Point Size */

	}
	else {
		glBindBuffer(GL_ARRAY_BUFFER, pTarget->vb_id);
		glBufferData(GL_ARRAY_BUFFER, sizeof(float) * 3 * nCnt, pData, GL_STATIC_DRAW);
		glBindBuffer(GL_ARRAY_BUFFER, 0);
		pTarget->element_count = (GLsizei)nCnt;
	}

	return 0;
}

void smSetTouchRadius(const FLOAT_VECTOR3 *pPos, float r)
{
	if (!pPos || r <= 0.0f) {
		g_pSM->render_target.pTouchRadius = 0;
	}
	else {
		if (!(g_pSM->render_target.pTouchRadius)) {
			g_pSM->render_target.pTouchRadius = &(g_pSM->render_target.toruch_radius);
		}
		g_pSM->render_target.pTouchRadius->model.arr[0]  = r;
		g_pSM->render_target.pTouchRadius->model.arr[5]  = r;
		g_pSM->render_target.pTouchRadius->model.arr[10] = r;

		g_pSM->render_target.pTouchRadius->model.arr[12] = pPos->x;
		g_pSM->render_target.pTouchRadius->model.arr[13] = pPos->y;
		g_pSM->render_target.pTouchRadius->model.arr[14] = pPos->z;
	}
}

int smAppendFindSurfaceResult(const float *inliers_data, unsigned int inliers_count, const FS_FEATURE_RESULT *pResult, const TORUS_EXTRA_PARAM *pParam)
{
	int ret = 0;
	RenderList *pNode = 0;
	pNode = (RenderList *)malloc(sizeof(RenderList));
	if (!pNode) return -1; /* Memory Allocation Failed */

	/* Set Mesh */
	switch (pResult->type)
	{
		case FS_TYPE_PLANE:    getPlaneRenderObject(&(pNode->mesh), pResult->plane_param.ll, pResult->plane_param.lr, pResult->plane_param.ur, pResult->plane_param.ul); break;
		case FS_TYPE_SPHERE:   getSphereRenderObject(&(pNode->mesh), pResult->sphere_param.c, pResult->sphere_param.r); break;
		case FS_TYPE_CYLINDER: getCylinderRenderObject(&(pNode->mesh), pResult->cylinder_param.t, pResult->cylinder_param.b, pResult->cylinder_param.r); break;
		case FS_TYPE_CONE:     getConeRenderObject(&(pNode->mesh), pResult->cone_param.t, pResult->cone_param.b, pResult->cone_param.tr, pResult->cone_param.br); break;
		case FS_TYPE_TORUS:    
			getTorusRenderObject(&(pNode->mesh), pResult->torus_param.c, pResult->torus_param.n, pResult->torus_param.mr, pResult->torus_param.tr);
			if (pParam) {
				FLOAT_VECTOR3 normal = { pResult->torus_param.n[0], pResult->torus_param.n[1], pResult->torus_param.n[2] };
				FLOAT_VECTOR3 front;
				Vec3Normalize(&front, Vec3Cross(&front, &(pParam->right), &normal));

				pNode->mesh.model.arr[0] = pParam->right.x;
				pNode->mesh.model.arr[1] = pParam->right.y;
				pNode->mesh.model.arr[2] = pParam->right.z;
				
				pNode->mesh.model.arr[8] = front.x;
				pNode->mesh.model.arr[9] = front.y;
				pNode->mesh.model.arr[10] = front.z;

				pNode->mesh.params[2] = pParam->ratio;
			}
			break;
		default: free(pNode); return -3;
	}

	/* Set Inliers */
	ret = genPointRenderObject(&(pNode->inliers), inliers_data, inliers_count);
	if (ret < 0) {
		free(pNode);
		return -2; /* Fail to create GL Buffer */
	}
	pNode->inliers.colors[0] = 1.0f;
	pNode->inliers.colors[1] = 0.0f;
	pNode->inliers.colors[2] = 0.0f;
	pNode->inliers.colors[3] = 3.0f; /* Point Size */
	pNode->pNext = 0;

	if (g_pSM->render_target.pListTail) {
		g_pSM->render_target.pListTail->pNext = pNode;
	}
	else {
		g_pSM->render_target.pListHead = pNode;
	}
	g_pSM->render_target.pListTail = pNode;

	return 0;
}

void smClearRenderList()
{
	RenderList *pCurr = g_pSM->render_target.pListHead;
	RenderList *pDel;
	while (pCurr) {
		pDel = pCurr;
		pCurr = pCurr->pNext;

		deletePointRenderObject(&(pDel->inliers));
		free(pDel);
	}
	g_pSM->render_target.pListHead = 0;
	g_pSM->render_target.pListTail = 0;
}

void smRotateView(float h_angle, float v_angle)
{
	rotateCamera3(g_pSM->current_view, h_angle, v_angle);
}

void smMoveView(float h_dist, float v_dist)
{
	moveCamera(g_pSM->current_view, h_dist, v_dist);
}

void smZoomView(float delta)
{
	zoomCamera(g_pSM->current_view, delta);
}

void smSetFrontView()
{
	FLOAT_VECTOR3 axis = { 0, 0, 1 };
	FLOAT_VECTOR3 up_axis = { 0, 1, 0 };
	if (dmGetOriginalCount() > 0) {
		updateCameraViewWithBoundingBoxAxis(g_pSM->current_view, dmGetBoundingBoxMin(), dmGetBoundingBoxMax(), &axis, &up_axis);
	}
	else {
		FLOAT_VECTOR3 center = { 0, 0, 0 };
		updateCameraViewMatrix(g_pSM->current_view, &axis, &center, &up_axis);
	}
}

void smSetTopView()
{
	FLOAT_VECTOR3 axis = { 0, 1, 0 };
	FLOAT_VECTOR3 up_axis = { 0, 0, -1 };
	if (dmGetOriginalCount() > 0) {
		updateCameraViewWithBoundingBoxAxis(g_pSM->current_view, dmGetBoundingBoxMin(), dmGetBoundingBoxMax(), &axis, &up_axis);
	}
	else {
		FLOAT_VECTOR3 center = { 0, 0, 0 };
		updateCameraViewMatrix(g_pSM->current_view, &axis, &center, &up_axis);
	}
}

void smSetSideView()
{
	FLOAT_VECTOR3 axis = { 1, 0, 0 };
	FLOAT_VECTOR3 up_axis = { 0, 1, 0 };
	if (dmGetOriginalCount() > 0) {
		updateCameraViewWithBoundingBoxAxis(g_pSM->current_view, dmGetBoundingBoxMin(), dmGetBoundingBoxMax(), &axis, &up_axis);
	}
	else {
		FLOAT_VECTOR3 center = { 0, 0, 0 };
		updateCameraViewMatrix(g_pSM->current_view, &axis, &center, &up_axis);
	}
}

void smOnUpdateSceneRatio(float scene_aspect_ratio)
{
	if (g_pSM) {
		updateCameraProjectionRatio(g_pSM->current_view, scene_aspect_ratio);
	}
}

void smRender()
{
	FLOAT_MATRIX vp;
	const RenderList *pCurr;

	MatrixMulGL(&vp, getCameraProjMatrix(g_pSM->current_view), getCameraViewMatrix(g_pSM->current_view));

	/* Render Point Cloud First */
	beginPointCloudRenderer();
	drawPointRenderObject(&(g_pSM->render_target.outliers), &vp);

	pCurr = g_pSM->render_target.pListHead;
	while (pCurr) {
		drawPointRenderObject(&(pCurr->inliers), &vp);
		pCurr = pCurr->pNext;
	}
	endPointCloudRenderer();

	/* Render Mesh */
	beginCustomMeshRenderer();

	pCurr = g_pSM->render_target.pListHead;
	while (pCurr) {
		drawMeshRenderObject(&(pCurr->mesh), &vp);
		pCurr = pCurr->pNext;
	}

	/* Touch Radius Indicator */
	glDisable(GL_DEPTH_TEST);
	if (g_pSM->render_target.pTouchRadius) {
		glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);
		drawMeshRenderObject(g_pSM->render_target.pTouchRadius, &vp);
		glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);
	}
	glEnable(GL_DEPTH_TEST);

	endCustomMeshRenderer();
}