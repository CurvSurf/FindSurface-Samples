#include <stdio.h>
#include <stdlib.h>
#include <math.h>

#include <float.h>
#include "glwin/glutil.h"
#include "glwin/common_util.h"
#include "proc.h"
#include "opt_parser.h"

#include "dataManager.h"
#include "sceneManager.h"

#include <FindSurface.h>

#define _WIN_WIDTH_  800
#define _WIN_HEIGHT_ 600

typedef enum {
	AS_IDLE = 0,
	AS_PICKING
} APP_STATE;

/*
 * Global Variables
 */
int WIN_WIDTH  = _WIN_WIDTH_;
int WIN_HEIGHT = _WIN_HEIGHT_;
APP_STATE g_state = AS_IDLE;

/*
 * Global Variables For Event Handling
 */
struct {
	unsigned int prev_btn;
	int prev_x;
	int prev_y;
	int moved;
}mouse_event_param = { 0, 0, 0, 0 };

struct {
	FIND_SURFACE_CONTEXT ctx;
	FS_FEATURE_TYPE which;
	float touch_r;
	float touch_r_step;
} find_surface_param = { 0, FS_TYPE_NONE, 0.1f, 0.01f };

/*
 * User Defined Functions
 */
void PrintUsage();
int PickingProgress(int cx, int cy);
void UpdateTouchRadiusRenderObject(int idx);
void RunFindSurface(int idx);
TORUS_EXTRA_PARAM * CheckTorusParam( TORUS_EXTRA_PARAM *pOut, const FS_FEATURE_RESULT *pResult, const float *pInliers, unsigned int nInCnt);

/*
 * Before Create Windows
 */
int InitApp(CREATE_WINDOW_PARAM *pParam, int argc, const char **argv)
{
	/* Additional Application Initialization Sequence Here */
	FS_OPT_PARAM opt;
	if (parseArguments(&opt, argc, argv) < 0) return -1;

	/* Read XYZ File First */
	if (dmLoadPointCloudFromFile(opt.szFileName) < 0) { /* By default, use sample.xyz for test file (if file name is not specified) */
		fprintf(stderr, "Failed to Load Point Cloud Data From <%s>\n", opt.szFileName);
		return -1;
	}

	/* Initialize Find Surface */
	if (createFindSurface(&(find_surface_param.ctx)) < 0) {
		fprintf(stderr, "Failed to create FindSurface Context\n");
		return -1;
	}

	/* Initialize Find Surface Parameters */
	find_surface_param.touch_r = opt.md * 10.0f;
	find_surface_param.touch_r_step = opt.md;

	setFindSurfaceParamFloat(find_surface_param.ctx, FS_PARAM_ACCURACY, opt.ma);
	setFindSurfaceParamFloat(find_surface_param.ctx, FS_PARAM_MEAN_DIST, opt.md);
	setFindSurfaceParamFloat(find_surface_param.ctx, FS_PARAM_TOUCH_R, find_surface_param.touch_r);

	/* Windows Title & Size */
	pParam->szTitle = "Simple GUI Demo";
	pParam->nWidth = _WIN_WIDTH_;
	pParam->nHeight = _WIN_HEIGHT_;

	return 0;
}

/*
 * After Create Windows, Initialize OpenGL Related Objects or OpenGL itself.
 */
int InitGL() 
{
	int ret = 0;
	ret = smInitSceneManager((float)WIN_WIDTH / (float)WIN_HEIGHT);
	if (ret < 0) {
		fprintf(stderr, "Failed to initialize Scene Manager (%d)\n", ret);
		return -1;
	}

	ret = smSetOutliers(dmGetOriginalData(), dmGetOriginalCount());
	if (ret < 0) {
		fprintf(stderr, "Failed to create opengl vertex buffer\n");
		return -1;
	}

	smSetFrontView();

	/* OpenGL Related Initialization Sequence Here */
	glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
	glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);
	glEnable(GL_PROGRAM_POINT_SIZE);
	glEnable(GL_BLEND);
	glEnable(GL_DEPTH_TEST);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	PrintUsage();

	return 0;
}

/* Timer : UpdateTick() from outside */
void FrameStep(const Timer *pTimer)
{
	/* Render Frame */
	glClear( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT );
	smRender();
}

void ReleaseGL()
{
	/* Release OpenGL related objects */
	smReleaseSceneManager();
}

void ReleaseApp()
{
	dmClear();
}

/* ===================================================================== *
 * GL Application Specific Event Handle Functions                        *
 * **these functions will be called from EventProc() in (W|X)EvtProc.c   *
 * ===================================================================== */
void OnResizeWindow(int new_width, int new_height)
{
	/* Update OpenGL view port & projection matrix */
	glViewport(0, 0, new_width, new_height);
	smOnUpdateSceneRatio((float)new_width / (float)new_height);
	WIN_WIDTH = new_width;
	WIN_HEIGHT = new_height;
}

void OnKeyPress(unsigned int key_code)
{
}

void OnKeyRelease(unsigned int key_code)
{
	if (g_state == AS_IDLE) {
		switch (key_code)
		{
			case _KEY_1_: printf("> Find Plane    : Please Pick the Seed Point\n"); g_state = AS_PICKING; find_surface_param.which = FS_TYPE_PLANE;    break;
			case _KEY_2_: printf("> Find Sphere   : Please Pick the Seed Point\n"); g_state = AS_PICKING; find_surface_param.which = FS_TYPE_SPHERE;   break;
			case _KEY_3_: printf("> Find Cylinder : Please Pick the Seed Point\n"); g_state = AS_PICKING; find_surface_param.which = FS_TYPE_CYLINDER; break;
			case _KEY_4_: printf("> Find Cone     : Please Pick the Seed Point\n"); g_state = AS_PICKING; find_surface_param.which = FS_TYPE_CONE;     break;
			case _KEY_5_: printf("> Find Torus    : Please Pick the Seed Point\n"); g_state = AS_PICKING; find_surface_param.which = FS_TYPE_TORUS;    break;

			case _KEY_Z_: printf("> Front View\n");smSetFrontView(); break;
			case _KEY_X_: printf("> Side View\n");smSetSideView(); break;
			case _KEY_C_: printf("> Top View\n");smSetTopView(); break;

			case _KEY_R_ :
				printf("> Reset All\n");
				dmResetOutliers();
				smClearRenderList();
				smSetOutliers(dmGetOutliersData(), dmGetOutliersCount());
				break;
		}
	}
}

void OnMouseButtonPress(unsigned int which, int x, int y)
{
	if (!mouse_event_param.prev_btn) {
		mouse_event_param.prev_btn = which;
		mouse_event_param.prev_x = x;
		mouse_event_param.prev_y = y;
		mouse_event_param.moved = 0;
	}
}

void OnMouseButtonRelease(unsigned int which, int x, int y)
{
	if (which == mouse_event_param.prev_btn) {
		mouse_event_param.prev_btn = 0;
	}

	if (g_state == AS_PICKING) {
		if (!mouse_event_param.moved) {
			if (which == _KEY_LBTN_) {
				int idx = PickingProgress(x, y);
				if (idx >= 0) {
					RunFindSurface(idx);

					smSetTouchRadius(0, 0);
					g_state = AS_IDLE;
				}
				else {
					fprintf(stderr, ">> Nothing picked\n");
				}
			}
			else if(which == _KEY_RBTN_) {
				smSetTouchRadius(0, 0);
				g_state = AS_IDLE;
				printf("> Cancel Find Surface\n");
			}
		}
	}
}

void OnMouseMove(unsigned int key_state, int x, int y)
{
	if (mouse_event_param.prev_btn)
	{
		if (key_state & _KS_CTRL_) 
		{
			float rx = ((float)(x - mouse_event_param.prev_x)/(float)_WIN_WIDTH_);
			float ry = ((float)(y - mouse_event_param.prev_y)/(float)_WIN_HEIGHT_);

			if (mouse_event_param.prev_btn == _KEY_LBTN_) {
				smRotateView(-rx * 2.0f * 3.141592f, -ry * 2.0f * 3.141592f);
			}
			else if (mouse_event_param.prev_btn == _KEY_RBTN_) {
				smMoveView(-rx, ry);
			}
		}
		mouse_event_param.prev_x = x;
		mouse_event_param.prev_y = y;
		mouse_event_param.moved = 1;
	}

	if (g_state == AS_PICKING) {
		UpdateTouchRadiusRenderObject(PickingProgress(x, y));
	}
}

void OnMouseWheel(int zDelta, unsigned int key_state, int x, int y)
{
	if (key_state & _KS_CTRL_) {
		smZoomView(zDelta > 0 ? -0.1f : 0.1f);
	}
	else if (g_state == AS_PICKING) {
		if (zDelta > 0) { find_surface_param.touch_r += find_surface_param.touch_r_step; }
		else if (find_surface_param.touch_r > find_surface_param.touch_r_step) {
			find_surface_param.touch_r -= find_surface_param.touch_r_step;
		}
		UpdateTouchRadiusRenderObject(PickingProgress(x, y));
	}
}

/* ===================================================================== *
 * User defined functions (implementation)                               *
 * ===================================================================== */
void PrintUsage() {
	printf("***************                Usage                ***************\n\n");
	printf("[How to manipulate camera view]\n");
	printf("    > Press 'ctrl' key and mouse button, then move mouse\n");
	printf("     ** L-Button for Rotate, R-Button for Move, Scroll wheel for Zoom\n");
	printf("    > Press 'z', 'x', 'c' key for reset view\n");
	printf("     ** z - front view, x - side view, c - top view\n");
	printf("\n");
	printf("[How to run FindSurface algorithm]\n");
	printf("    > First, select the type of surface by press the one of number keys\n");
	printf("     ** 1 - Plane, 2 - Sphere, 3 - Cylinder, 4 - Cone, 5 - Torus\n");
	printf("    > Pick the seed point\n");
	printf("     ** Mouse L-button for picking, R-button for cancel FindSurface\n");
	printf("        Scroll wheel for adjusting Touch Radius\n");
	printf("\n");
	printf("[Others]\n");
	printf("    > Press 'R' key for reset all\n");
	printf("\n");
}

int PickingProgress(int cx, int cy) {
	FLOAT_VECTOR3 ray_orig, ray_dir;

	/* Normalize Client Coordinates */
	float rx = 2.0f * ((float)cx / (float)WIN_WIDTH - 0.5f);
	float ry = -2.0f * ((float)cy / (float)WIN_HEIGHT - 0.5f);

	clientToWorldRayRH(&ray_orig, &ray_dir, rx, ry, smGetViewMatrix(), smGetProjMatrix());
	return rayPickPoint(&ray_orig, &ray_dir, 0.1f, dmGetOutliersData(), (int)dmGetOutliersCount(), 0);
}

void UpdateTouchRadiusRenderObject(int idx) {
	if (idx < 0) {
		smSetTouchRadius(0, 0);
	}
	else {
		const float *pPtList = dmGetOutliersData();
		FLOAT_VECTOR3 pos = { pPtList[3 * idx], pPtList[3 * idx + 1], pPtList[3 * idx + 2] };
		smSetTouchRadius(&pos, find_surface_param.touch_r);
	}
}

static float _getLength(float x1, float y1, float z1, float x2, float y2, float z2) {
	float dx = x1 - x2;
	float dy = y1 - y2;
	float dz = z1 - z2;

	return sqrtf(dx * dx + dy * dy + dz * dz);
}

static void PrintFindSurfaceResult(const FS_FEATURE_RESULT *pResult)
{
	printf("> Find Surface [rms: %.3f]\n", pResult->rms);

	switch (pResult->type)
	{
	case FS_TYPE_PLANE:
		printf("\tPLNAE: w(%g), h(%g)\n",
			_getLength(
				pResult->plane_param.ul[0], pResult->plane_param.ul[1], pResult->plane_param.ul[2],
				pResult->plane_param.ur[0], pResult->plane_param.ur[1], pResult->plane_param.ur[2]
			),
			_getLength(
				pResult->plane_param.ll[0], pResult->plane_param.ll[1], pResult->plane_param.ll[2],
				pResult->plane_param.ul[0], pResult->plane_param.ul[1], pResult->plane_param.ul[2]
			)
		);
		break;

	case FS_TYPE_SPHERE:
		printf("\tSPHERE: r(%g)\n", pResult->sphere_param.r);
		break;

	case FS_TYPE_CYLINDER:
		printf("\tCYLINDER: r(%g), h(%g)\n",
			pResult->cylinder_param.r,
			_getLength(
				pResult->cylinder_param.t[0], pResult->cylinder_param.t[1], pResult->cylinder_param.t[2],
				pResult->cylinder_param.b[0], pResult->cylinder_param.b[1], pResult->cylinder_param.b[2]
			)
		);
		break;

	case FS_TYPE_CONE:
		printf("\tCONE: small_r(%g), large_r(%g), h(%g)\n",
			pResult->cone_param.tr,
			pResult->cone_param.br,
			_getLength(
				pResult->cone_param.t[0], pResult->cone_param.t[1], pResult->cone_param.t[2],
				pResult->cone_param.b[0], pResult->cone_param.b[1], pResult->cone_param.b[2]
			)
		);
		break;

	case FS_TYPE_TORUS:
		printf("\tTORI: mean_r(%g), tube_r(%g)\n", pResult->torus_param.mr, pResult->torus_param.tr);
		break;
	}
}

static void UpdateFindSurfaceResult(FS_FEATURE_RESULT *pResult) {
	switch (pResult->type) {
	case FS_TYPE_CONE:
	{
		if (pResult->cone_param.tr == pResult->cone_param.br) {
			FS_FEATURE_RESULT new_result;
			new_result.type = FS_TYPE_CYLINDER;
			new_result.rms = pResult->rms;
			new_result.cylinder_param.r = pResult->cone_param.tr;
			new_result.cylinder_param.t[0] = pResult->cone_param.t[0];
			new_result.cylinder_param.t[1] = pResult->cone_param.t[1];
			new_result.cylinder_param.t[2] = pResult->cone_param.t[2];
			new_result.cylinder_param.b[0] = pResult->cone_param.b[0];
			new_result.cylinder_param.b[1] = pResult->cone_param.b[1];
			new_result.cylinder_param.b[2] = pResult->cone_param.b[2];

			*pResult = new_result;
		}
	}
	break;

	case FS_TYPE_TORUS:
	{
		if (pResult->torus_param.mr == 0.0f) /* It is not torus but sphere */
		{
			FS_FEATURE_RESULT new_result;
			new_result.type = FS_TYPE_SPHERE;
			new_result.rms = pResult->rms;
			new_result.sphere_param.r = pResult->torus_param.tr;
			new_result.sphere_param.c[0] = pResult->torus_param.c[0];
			new_result.sphere_param.c[1] = pResult->torus_param.c[1];
			new_result.sphere_param.c[2] = pResult->torus_param.c[2];

			*pResult = new_result;
		}
		else if (pResult->torus_param.mr == FLT_MAX) /* It is not torus but cylinder */
		{
			FS_FEATURE_RESULT new_result;
			new_result.type = FS_TYPE_CYLINDER;
			new_result.rms = pResult->rms;
			new_result.cylinder_param.r = pResult->torus_param.tr;
			new_result.cylinder_param.t[0] = pResult->torus_param.n[0];
			new_result.cylinder_param.t[1] = pResult->torus_param.n[1];
			new_result.cylinder_param.t[2] = pResult->torus_param.n[2];
			new_result.cylinder_param.b[0] = pResult->torus_param.c[0];
			new_result.cylinder_param.b[1] = pResult->torus_param.c[1];
			new_result.cylinder_param.b[2] = pResult->torus_param.c[2];

			*pResult = new_result;
		}
	}
	break;
	}
}

void RunFindSurface(int idx) {
	int ret = 0;
	FS_FEATURE_RESULT result = { 0, };
	TORUS_EXTRA_PARAM param = { 0, }, *pParam = 0;

	unsigned int in_cnt = 0, out_cnt = 0, buf_size;
	float *pTmpPtList = 0;

	setFindSurfaceParamFloat(find_surface_param.ctx, FS_PARAM_TOUCH_R, find_surface_param.touch_r);

	ret = setPointCloudFloat(find_surface_param.ctx, dmGetOutliersData(), dmGetOutliersCount(), 0);
	if (ret < 0) {
		fprintf(stderr, "[FindSurface] setPointCloudFloat() - Memory Allocation Failed\n");
		return;
	}
	
	ret = findSurface(find_surface_param.ctx, find_surface_param.which, (unsigned int)idx, &result);
	if (ret < 0) {
		fprintf(stderr, "[FindSurface] Failed to find surface (type:%d, err:%d)\n", find_surface_param.which, ret);
		return;
	}

	UpdateFindSurfaceResult(&result);
	PrintFindSurfaceResult(&result);

	in_cnt = getInliersFloat(find_surface_param.ctx, 0, 0);
	out_cnt = getOutliersFloat(find_surface_param.ctx, 0, 0);
	buf_size = sizeof(float) * 3 * (in_cnt > out_cnt ? in_cnt : out_cnt);

	pTmpPtList = (float *)malloc(buf_size);
	if (pTmpPtList == 0) {
		fprintf(stderr, "RunFindSurface() - get(In/Out)liers() - Fatal Error: Memory Allocation Failed\n");
		cleanUpFindSurface(find_surface_param.ctx);
		return;
	}

	in_cnt = getInliersFloat(find_surface_param.ctx, pTmpPtList, buf_size);
	if (result.type == FS_TYPE_TORUS) {
		/* Calculate Addtional Torus Parameter ( Axis & Angle ) */
		pParam = CheckTorusParam(&param, &result, pTmpPtList, in_cnt);
	}
	smAppendFindSurfaceResult(pTmpPtList, in_cnt, &result, pParam);

	out_cnt = getOutliersFloat(find_surface_param.ctx, pTmpPtList, buf_size);
	dmUpdateOutliers(pTmpPtList, out_cnt);
	smSetOutliers(dmGetOutliersData(), dmGetOutliersCount());

	free(pTmpPtList);
	cleanUpFindSurface(find_surface_param.ctx);
}

static void GetAlginedNormalizedVector(FLOAT_VECTOR3 *pOut, const FLOAT_VECTOR3 *pUp, const FLOAT_VECTOR3 *pTarget) 
{
	FLOAT_VECTOR3 tmp;
	Vec3Normalize(&tmp, Vec3Cross(&tmp, pTarget, pUp));
	Vec3Normalize(pOut, Vec3Cross(&tmp, pUp, &tmp));
}

TORUS_EXTRA_PARAM * CheckTorusParam(TORUS_EXTRA_PARAM *pOut, const FS_FEATURE_RESULT *pResult, const float *pInliers, unsigned int nInCnt)
{
	FLOAT_VECTOR3 center = { pResult->torus_param.c[0], pResult->torus_param.c[1], pResult->torus_param.c[2] };
	FLOAT_VECTOR3 normal = { pResult->torus_param.n[0], pResult->torus_param.n[1], pResult->torus_param.n[2] };
	FLOAT_VECTOR3 dir, tmp;
	float len;
	float l_min_dot = FLT_MAX, r_min_dot = FLT_MAX;
	int l_min_idx = -1, r_min_idx = -1;
	const float *pCurr;
	unsigned int i;

	/* Calculate Center of Mass */
	{
		double acc[3] = { 0, 0, 0 };
		pCurr = pInliers;
		for (i = 0; i < nInCnt; i++) {
			acc[0] += pCurr[0];
			acc[1] += pCurr[1];
			acc[2] += pCurr[2];
			pCurr += 3;
		}
		tmp.x = (float)(acc[0] / nInCnt);
		tmp.y = (float)(acc[1] / nInCnt);
		tmp.z = (float)(acc[2] / nInCnt);
	}
	Vec3Normalize(&tmp, Vec3Sub(&tmp, &tmp, &center));

	if (fabs(Vec3Dot(&normal, &tmp)) > 0.999f) {
		return (TORUS_EXTRA_PARAM *)0; /* It may be complete torus */
	}

	GetAlginedNormalizedVector(&dir, &normal, &tmp);

	pCurr = pInliers;
	for (i = 0; i < nInCnt; i++) {
		tmp.x = pCurr[0]; tmp.y = pCurr[1]; tmp.z = pCurr[2];
		GetAlginedNormalizedVector(&tmp, &normal, Vec3Normalize(&tmp, Vec3Sub(&tmp, &tmp, &center)));
		len = Vec3Dot(&dir, &tmp);

		if (Vec3Dot(&normal, Vec3Cross(&tmp, &dir, &tmp) ) > 0.0f) {
			if (len < l_min_dot) { 
				l_min_dot = len; 
				l_min_idx = i; 
			}
		}
		else {
			if (len < r_min_dot) { 
				r_min_dot = len; 
				r_min_idx = i; 
			}
		}

		pCurr += 3;
	}

	pCurr = &pInliers[3 * r_min_idx];
	tmp.x = pCurr[0]; tmp.y = pCurr[1]; tmp.z = pCurr[2];

	GetAlginedNormalizedVector(&tmp, &normal, Vec3Normalize(&tmp, Vec3Sub(&tmp, &tmp, &center)));
	len = acosf(Vec3Dot(&dir, &tmp));

	pCurr = &pInliers[3 * l_min_idx];
	tmp.x = pCurr[0]; tmp.y = pCurr[1]; tmp.z = pCurr[2];

	GetAlginedNormalizedVector(&tmp, &normal, Vec3Normalize(&tmp, Vec3Sub(&tmp, &tmp, &center)));
	len += acosf(Vec3Dot(&dir, &tmp));

	pOut->right = tmp;
	pOut->ratio = len / (2.0f * _PI_);

	return pOut;
}