#include "renderObjectFactory.h"
#include <stdlib.h>
#include <string.h>

#define _USE_MATH_DEFINES
#include <math.h>
#include "glwin/floatVector3.h"

#ifndef M_PI
#define M_PI       3.14159265358979323846   // pi
#endif

#define SAFE_DELETE(id) if(id){glDeleteBuffers(1,&(id));}

typedef struct _tagBufferProperties { GLuint vb_id; GLuint ib_id; GLsizei idx_cnt; } BUFFER_PROPERTIES;
static struct 
{
	int isInitialized;
	BUFFER_PROPERTIES unit_plane;
	BUFFER_PROPERTIES unit_sphere;
	BUFFER_PROPERTIES unit_cylinder;
	BUFFER_PROPERTIES unit_cone_element;
	BUFFER_PROPERTIES unit_torus_element;
} _pre_built_robj = { 0, };

static int _genUnitPlane(GLuint *pOutVB, GLuint *pOutIB, GLsizei *pOutIdxCnt);
static int _genUnitSphere(GLuint *pOutVB, GLuint *pOutIB, GLsizei *pOutIdxCnt);
static int _genUnitConeStatic(const int nSubHeight, const int nSubDiv, GLuint * pOutVB, GLuint * pOutIB, GLsizei *pOutIdxCnt);
static int _genUnitTorusElement(GLuint *pOutVB, GLuint *pOutIB, GLsizei *pOutIdxCnt);

int initRenderObjectFactory()
{
	if (!(_pre_built_robj.isInitialized)) 
	{
		GLuint vb_id = 0;
		GLuint ib_id = 0;
		GLsizei idx_cnt = 0;

		memset(&_pre_built_robj, 0x00, sizeof(_pre_built_robj));

		if (!_genUnitPlane(&vb_id, &ib_id, &idx_cnt)) {
			_pre_built_robj.unit_plane.vb_id = vb_id;
			_pre_built_robj.unit_plane.ib_id = ib_id;
			_pre_built_robj.unit_plane.idx_cnt = idx_cnt;
		}
		else { return -1; }

		if (!_genUnitSphere(&vb_id, &ib_id, &idx_cnt) ) {
			_pre_built_robj.unit_sphere.vb_id = vb_id;
			_pre_built_robj.unit_sphere.ib_id = ib_id;
			_pre_built_robj.unit_sphere.idx_cnt = idx_cnt;
		}
		else { return -1; }

		if (!_genUnitConeStatic(3, 24, &vb_id, &ib_id, &idx_cnt)) {
			_pre_built_robj.unit_cylinder.vb_id = vb_id;
			_pre_built_robj.unit_cylinder.ib_id = ib_id;
			_pre_built_robj.unit_cylinder.idx_cnt = idx_cnt;
		}
		else { return -1; }

		if (!_genUnitConeStatic(2, 24, &vb_id, &ib_id, &idx_cnt)) {
			_pre_built_robj.unit_cone_element.vb_id = vb_id;
			_pre_built_robj.unit_cone_element.ib_id = ib_id;
			_pre_built_robj.unit_cone_element.idx_cnt = idx_cnt;
		}
		else { return -1; }

		if (!_genUnitTorusElement(&vb_id, &ib_id, &idx_cnt)) {
			_pre_built_robj.unit_torus_element.vb_id = vb_id;
			_pre_built_robj.unit_torus_element.ib_id = ib_id;
			_pre_built_robj.unit_torus_element.idx_cnt = idx_cnt;
		}
		else { return -1; }
		
		_pre_built_robj.isInitialized = 1;
	}
	return 0;
}

void releaseRenderObjectFactory()
{
	if (_pre_built_robj.isInitialized)
	{
		SAFE_DELETE(_pre_built_robj.unit_plane.vb_id);
		SAFE_DELETE(_pre_built_robj.unit_plane.ib_id);

		SAFE_DELETE(_pre_built_robj.unit_sphere.vb_id);
		SAFE_DELETE(_pre_built_robj.unit_sphere.ib_id);

		SAFE_DELETE(_pre_built_robj.unit_cylinder.vb_id);
		SAFE_DELETE(_pre_built_robj.unit_cylinder.ib_id);

		SAFE_DELETE(_pre_built_robj.unit_cone_element.vb_id);
		SAFE_DELETE(_pre_built_robj.unit_cone_element.ib_id);

		SAFE_DELETE(_pre_built_robj.unit_torus_element.vb_id);
		SAFE_DELETE(_pre_built_robj.unit_torus_element.ib_id);
	}
	memset(&_pre_built_robj, 0x00, sizeof(_pre_built_robj));
}

int genPointRenderObject(RENDER_OBJECT *pOut, const float *pPointList, unsigned int nPointCount)
{
	GLuint bid = 0;
	if (!pOut || !pPointList || nPointCount < 1) return -1; /* Invalid Arguments */

	bid = glGenVertexBuffer(sizeof(float) * 3 * nPointCount, pPointList);
	if (!bid) return -2; /* Failed to create buffer */

	memset(pOut, 0x00, sizeof(RENDER_OBJECT));

	pOut->mode = GL_POINTS;
	pOut->vb_id = bid;
	pOut->element_count = (GLsizei)nPointCount;

	/* ib_id & params are not used */

	pOut->colors[0] = 1.0f;
	pOut->colors[1] = 1.0f;
	pOut->colors[2] = 1.0f;
	pOut->colors[3] = 1.0f; /* Point Size */

	MatrixIdentity(&(pOut->model));

	return 0; /* Success */
}

void deletePointRenderObject(RENDER_OBJECT *pObj) {
	if (pObj && pObj->mode == GL_POINTS) {
		SAFE_DELETE(pObj->vb_id);
		memset(pObj, 0x00, sizeof(RENDER_OBJECT));
	}
}

int getPlaneRenderObject(RENDER_OBJECT *pOut, float ll[3], float lr[3], float ur[3], float ul[3])
{
	FLOAT_VECTOR3 normal, right, front;
	float width, height;

	if (!pOut) return -1;
	memset(pOut, 0x00, sizeof(RENDER_OBJECT));

	pOut->mode = GL_TRIANGLES;
	pOut->vb_id = _pre_built_robj.unit_plane.vb_id;
	pOut->ib_id = _pre_built_robj.unit_plane.ib_id;
	pOut->element_count = _pre_built_robj.unit_plane.idx_cnt;

	/* params are not used */

	pOut->colors[0] = 1.0f;
	pOut->colors[1] = 0.0f;
	pOut->colors[2] = 0.0f;
	pOut->colors[3] = 1.0f;

	right.x = ur[0] - ul[0];
	right.y = ur[1] - ul[1];
	right.z = ur[2] - ul[2];

	Vec3Normalize2(&right, &width, &right);

	front.x = ll[0] - ul[0];
	front.y = ll[1] - ul[1];
	front.z = ll[2] - ul[2];

	Vec3Normalize2(&front, &height, &front);

	Vec3Normalize(&normal, Vec3Cross(&normal, &front, &right));

	pOut->model.arr[0] = width * right.x;
	pOut->model.arr[1] = width * right.y;
	pOut->model.arr[2] = width * right.z;
	pOut->model.arr[3] = 0.0f;

	pOut->model.arr[4] = normal.x;
	pOut->model.arr[5] = normal.y;
	pOut->model.arr[6] = normal.z;
	pOut->model.arr[7] = 0.0f;

	pOut->model.arr[8]  = height * front.x;
	pOut->model.arr[9]  = height * front.y;
	pOut->model.arr[10] = height * front.z;
	pOut->model.arr[11] = 0.0f;

	pOut->model.arr[12] = (ll[0] + lr[0] + ul[0] + ur[0]) / 4.0f;
	pOut->model.arr[13] = (ll[1] + lr[1] + ul[1] + ur[1]) / 4.0f;
	pOut->model.arr[14] = (ll[2] + lr[2] + ul[2] + ur[2]) / 4.0f;
	pOut->model.arr[15] = 1.0f;

	return 0;
}

int getSphereRenderObject(RENDER_OBJECT *pOut, float c[3], float r)
{
	if (!pOut) return -1;
	memset(pOut, 0x00, sizeof(RENDER_OBJECT));

	pOut->mode = GL_TRIANGLES;
	pOut->vb_id = _pre_built_robj.unit_sphere.vb_id;
	pOut->ib_id = _pre_built_robj.unit_sphere.ib_id;
	pOut->element_count = _pre_built_robj.unit_sphere.idx_cnt;


	/* params are not used */

	pOut->colors[0] = 1.0f;
	pOut->colors[1] = 1.0f;
	pOut->colors[2] = 0.0f;
	pOut->colors[3] = 1.0f;

	pOut->model.arr[0] = r;
	pOut->model.arr[1] = 0.0f;
	pOut->model.arr[2] = 0.0f;
	pOut->model.arr[3] = 0.0f;

	pOut->model.arr[4] = 0.0f;
	pOut->model.arr[5] = r;
	pOut->model.arr[6] = 0.0f;
	pOut->model.arr[7] = 0.0f;

	pOut->model.arr[8] = 0.0f;
	pOut->model.arr[9] = 0.0f;
	pOut->model.arr[10] = r;
	pOut->model.arr[11] = 0.0f;

	pOut->model.arr[12] = c[0];
	pOut->model.arr[13] = c[1];
	pOut->model.arr[14] = c[2];
	pOut->model.arr[15] = 1.0f;

	return 0;
}

int getCylinderRenderObject(RENDER_OBJECT *pOut, float t[3], float b[3], float r)
{
	FLOAT_VECTOR3 axis, right, front = { 0, 0, 1 };
	float height;

	if (!pOut) return -1;
	memset(pOut, 0x00, sizeof(RENDER_OBJECT));

	pOut->mode = GL_TRIANGLES;
	pOut->vb_id = _pre_built_robj.unit_cylinder.vb_id;
	pOut->ib_id = _pre_built_robj.unit_cylinder.ib_id;
	pOut->element_count = _pre_built_robj.unit_cylinder.idx_cnt;

	/* params are not used */

	pOut->colors[0] = 0.0f;
	pOut->colors[1] = 1.0f;
	pOut->colors[2] = 0.0f;
	pOut->colors[3] = 1.0f;

	axis.x = t[0] - b[0];
	axis.y = t[1] - b[1];
	axis.z = t[2] - b[2];

	height = Vec3Length(&axis);

	axis.x /= height;
	axis.y /= height;
	axis.z /= height;

	// Calculate Right & Front Vector
	if (axis.z != 1.0f) { /* Not equal (0, 0, 1) */
		Vec3Cross(&right, &axis, &front);
		Vec3Normalize(&right, &right);
		Vec3Cross(&front, &right, &axis);
	}
	else {
		right.x = 0; right.y = 1; right.z = 0;
		front.x = 1; front.y = 0; front.z = 0;
	}

	pOut->model.arr[0] = r * right.x;
	pOut->model.arr[1] = r * right.y;
	pOut->model.arr[2] = r * right.z;
	pOut->model.arr[3] = 0.0f;

	pOut->model.arr[4] = height * axis.x;
	pOut->model.arr[5] = height * axis.y;
	pOut->model.arr[6] = height * axis.z;
	pOut->model.arr[7] = 0.0f;

	pOut->model.arr[8]  = r * front.x;
	pOut->model.arr[9]  = r * front.y;
	pOut->model.arr[10] = r * front.z;
	pOut->model.arr[11] = 0.0f;

	pOut->model.arr[12] = (t[0] + b[0]) / 2.0f;
	pOut->model.arr[13] = (t[1] + b[1]) / 2.0f;
	pOut->model.arr[14] = (t[2] + b[2]) / 2.0f;
	pOut->model.arr[15] = 1.0f;

	return 0;
}

int getConeRenderObject(RENDER_OBJECT *pOut, float t[3], float b[3], float tr, float br)
{
	FLOAT_VECTOR3 axis, right, front = { 0, 0, 1 };
	float height;

	if (!pOut) return -1;
	memset(pOut, 0x00, sizeof(RENDER_OBJECT));

	pOut->mode = GL_TRIANGLES;
	pOut->vb_id = _pre_built_robj.unit_cone_element.vb_id;
	pOut->ib_id = _pre_built_robj.unit_cone_element.ib_id;
	pOut->element_count = _pre_built_robj.unit_cone_element.idx_cnt;

	/* params are not used */
	pOut->params[0] = tr;
	pOut->params[1] = br;
	pOut->params[3] = -1.0f;

	pOut->colors[0] = 0.0f;
	pOut->colors[1] = 1.0f;
	pOut->colors[2] = 1.0f;
	pOut->colors[3] = 1.0f;

	axis.x = t[0] - b[0];
	axis.y = t[1] - b[1];
	axis.z = t[2] - b[2];

	height = Vec3Length(&axis);

	axis.x /= height;
	axis.y /= height;
	axis.z /= height;

	// Calculate Right & Front Vector
	if (axis.z != 1.0f) { /* Not equal (0, 0, 1) */
		Vec3Cross(&right, &axis, &front);
		Vec3Normalize(&right, &right);
		Vec3Cross(&front, &right, &axis);
	}
	else {
		right.x = 0; right.y = 1; right.z = 0;
		front.x = 1; front.y = 0; front.z = 0;
	}

	pOut->model.arr[0] = right.x;
	pOut->model.arr[1] = right.y;
	pOut->model.arr[2] = right.z;
	pOut->model.arr[3] = 0.0f;

	pOut->model.arr[4] = height * axis.x;
	pOut->model.arr[5] = height * axis.y;
	pOut->model.arr[6] = height * axis.z;
	pOut->model.arr[7] = 0.0f;

	pOut->model.arr[8] = front.x;
	pOut->model.arr[9] = front.y;
	pOut->model.arr[10] = front.z;
	pOut->model.arr[11] = 0.0f;

	pOut->model.arr[12] = (t[0] + b[0]) / 2.0f;
	pOut->model.arr[13] = (t[1] + b[1]) / 2.0f;
	pOut->model.arr[14] = (t[2] + b[2]) / 2.0f;
	pOut->model.arr[15] = 1.0f;

	return 0;
}

int getTorusRenderObject(RENDER_OBJECT *pOut, float c[3], float n[3], float mr, float tr)
{
	FLOAT_VECTOR3 axis = { n[0], n[1], n[2] }, right, front = { 0, 0, 1 };

	if (!pOut) return -1;
	memset(pOut, 0x00, sizeof(RENDER_OBJECT));

	pOut->mode = GL_TRIANGLES;
	pOut->vb_id = _pre_built_robj.unit_torus_element.vb_id;
	pOut->ib_id = _pre_built_robj.unit_torus_element.ib_id;
	pOut->element_count = _pre_built_robj.unit_torus_element.idx_cnt;

	/* params are not used */
	pOut->params[0] = mr;
	pOut->params[1] = tr;
	pOut->params[2] = 1.0f;
	pOut->params[3] = 1.0f;

	pOut->colors[0] = 1.0f;
	pOut->colors[1] = 0.0f;
	pOut->colors[2] = 1.0f;
	pOut->colors[3] = 1.0f;

	Vec3Normalize(&axis, &axis);

	// Calculate Right & Front Vector
	if (axis.z != 1.0f) { /* Not equal (0, 0, 1) */
		Vec3Cross(&right, &axis, &front);
		Vec3Normalize(&right, &right);
		Vec3Cross(&front, &right, &axis);
	}
	else {
		right.x = 0; right.y = 1; right.z = 0;
		front.x = 1; front.y = 0; front.z = 0;
	}

	pOut->model.arr[0] = right.x;
	pOut->model.arr[1] = right.y;
	pOut->model.arr[2] = right.z;
	pOut->model.arr[3] = 0.0f;

	pOut->model.arr[4] = axis.x;
	pOut->model.arr[5] = axis.y;
	pOut->model.arr[6] = axis.z;
	pOut->model.arr[7] = 0.0f;

	pOut->model.arr[8] = front.x;
	pOut->model.arr[9] = front.y;
	pOut->model.arr[10] = front.z;
	pOut->model.arr[11] = 0.0f;

	pOut->model.arr[12] = c[0];
	pOut->model.arr[13] = c[1];
	pOut->model.arr[14] = c[2];
	pOut->model.arr[15] = 1.0f;

	return 0;
}

/* Implementation of static functions */
int _genUnitPlane(GLuint *pOutVB, GLuint *pOutIB, GLsizei *pOutIdxCnt)
{
	GLuint vb_id = 0;
	GLuint ib_id = 0;

	GLfloat vtxList[3 * 4] = {
		-0.5f, 0.0f, -0.5f,
		-0.5f, 0.0f,  0.5f,
		 0.5f, 0.0f,  0.5f,
		 0.5f, 0.0f, -0.5f
	};

	GLushort idxList[3 * 2] = {
		0, 1, 2,
		0, 2, 3
	};

	vb_id = glGenVertexBuffer(sizeof(vtxList), vtxList);
	ib_id = glGenIndexBuffer(sizeof(idxList), idxList);

	if (vb_id == 0 || ib_id == 0) {
		SAFE_DELETE(vb_id);
		SAFE_DELETE(ib_id);
		return -1; /* Failed to create buffer */
	}

	*pOutVB = vb_id;
	*pOutIB = ib_id;
	*pOutIdxCnt = 6;

	return 0;
}

int _genUnitSphere(GLuint *pOutVB, GLuint *pOutIB, GLsizei *pOutIdxCnt)
{
	const int nSubAxis   = 24;
	const int nSubHeight = 24;

	GLuint vb_id = 0;
	GLuint ib_id = 0;

	int i, k, half;
	float r, h;
	float angle, rad;
	float base_angle, base_div;
	GLfloat  *vtxList, *pCurrVtx, *pEndVtx, *pCurrVtx2;
	GLushort *idxList, *pCurrIdx;

	GLsizei idxCnt = 3 * nSubAxis * 2 * (nSubHeight - 1);
	GLsizei vtxSize = sizeof(GLfloat) * 3 * (nSubAxis * (nSubHeight - 1) + 2);
	GLsizei idxSize = sizeof(GLushort) * idxCnt;

	vtxList = (GLfloat *)malloc(vtxSize);
	idxList = (GLushort *)malloc(idxSize);

	if (vtxList == NULL || idxList == NULL) {
		free(vtxList);
		free(idxList);
		return -1; /* Memory Allocation Failed */
	}

	half = (nSubHeight - 1) / 2;
	base_angle = (float)M_PI / (float)nSubHeight;
	base_div = (float)M_PI * 2.0f / (float)nSubAxis;
	pEndVtx = (GLfloat *)(((GLubyte *)vtxList) + vtxSize);

	/* Fill the Vertex Buffer */
	pCurrVtx = vtxList;
	pCurrVtx[0] = 0.0f; pCurrVtx[1] = 1.0f; pCurrVtx[2] = 0.0f;

	pCurrVtx = pEndVtx - 3;
	pCurrVtx[0] = 0.0f; pCurrVtx[1] = -1.0f; pCurrVtx[2] = 0.0f;
	
	for (k = 0; k < half; k++) {
		angle = base_angle * (k + 1);
		r = sinf(angle);
		h = cosf(angle);

		pCurrVtx = vtxList + 3 + (3 * (nSubAxis * k));
		pCurrVtx2 = pEndVtx - 3 - (3 * (nSubAxis * (k + 1)));

		for (i = 0; i < nSubAxis; i++) {
			rad = base_div * i;

			pCurrVtx[0] = r * -sinf(rad);
			pCurrVtx[1] = h;
			pCurrVtx[2] = r * -cosf(rad);
			pCurrVtx += 3;
			
			
			pCurrVtx2[0] = r * -sinf(rad);
			pCurrVtx2[1] = -h;
			pCurrVtx2[2] = r * -cosf(rad);
			pCurrVtx2 += 3;
		}
	}

	if (nSubHeight % 2 == 0) {
		pCurrVtx = vtxList + (3 * (half * nSubAxis + 1));
		for (i = 0; i < nSubAxis; i++) {
			rad = base_div * i;
			pCurrVtx[0] = -sinf(rad);
			pCurrVtx[1] = 0.0f;
			pCurrVtx[2] = -cosf(rad);
			pCurrVtx += 3;
		}
	}
	
	/* Fill the Index Buffer */
	pCurrIdx = idxList;
	for (i = 1; i <= nSubAxis; i++) {
		pCurrIdx[0] = 0;
		pCurrIdx[1] = i;
		pCurrIdx[2] = (i < nSubAxis ? i : 0) + 1;
		pCurrIdx += 3;
	}

	pCurrIdx = (GLushort *)((((GLubyte *)idxList) + idxSize) - (sizeof(GLushort) * 3 * nSubAxis));
	for (i = 1; i <= nSubAxis; i++) {
		pCurrIdx[0] = nSubAxis * (nSubHeight - 1) + 2 - 1;
		pCurrIdx[1] = pCurrIdx[0] - nSubAxis + (i < nSubAxis ? i : 0);
		pCurrIdx[2] = pCurrIdx[0] - nSubAxis + i - 1;
		pCurrIdx += 3;
	}

	pCurrIdx = idxList + (3 * nSubAxis);
	for (k = 0; k < nSubHeight - 2; k++) {
		for (i = 1; i <= nSubAxis; i++) {
			pCurrIdx[0] = pCurrIdx[3] = nSubAxis * k + ((i < nSubAxis ? i : 0) + 1);
			pCurrIdx[1] = nSubAxis * k + i;
			pCurrIdx[2] = pCurrIdx[4] = pCurrIdx[1] + nSubAxis;
			pCurrIdx[5] = pCurrIdx[0] + nSubAxis;

			pCurrIdx += 6;
		}
	}

	vb_id = glGenVertexBuffer(vtxSize, vtxList);
	ib_id = glGenIndexBuffer(idxSize, idxList);

	free(vtxList);
	free(idxList);

	if (vb_id == 0 || ib_id == 0) {
		SAFE_DELETE(vb_id);
		SAFE_DELETE(ib_id);
		return -1; /* Failed to create buffer */
	}

	*pOutVB = vb_id;
	*pOutIB = ib_id;
	*pOutIdxCnt = idxCnt;

	return 0;
}

int _genUnitConeStatic(const int nSubHeight, const int nSubDiv, GLuint *pOutVB, GLuint *pOutIB, GLsizei *pOutIdxCnt)
{
	/* No Hat Cylinder / Cone (top radius = bottom radius = 1) */
	GLuint vb_id = 0;
	GLuint ib_id = 0;

	int i, j, base_idx;
	GLfloat  *vtxList, *pCurrVtx, h, rad;
	GLushort *idxList, *pCurrIdx;

	GLsizei idxCnt = 3 * nSubDiv * 2 * nSubHeight;
	GLsizei vtxSize = sizeof(GLfloat) * 3 * (nSubDiv + 1) * (nSubHeight + 1);
	GLsizei idxSize = sizeof(GLushort) * idxCnt;

	vtxList = (GLfloat *)malloc(vtxSize);
	idxList = (GLushort *)malloc(idxSize);

	if (vtxList == NULL || idxList == NULL) {
		free(vtxList);
		free(idxList);
		return -1; /* Memory Allocation Failed */
	}

	/* Fill the Vertex Buffer */
	pCurrVtx = vtxList;
	for (j = 0; j <= nSubHeight; j++) {
		h = 0.5f - ((float)j / (float)nSubHeight);
		for (i = 0; i <= nSubDiv; i++) {
			rad = i < nSubDiv ? ((float)i * (2.0f * (float)M_PI) / (float)nSubDiv) : 0.0f;

			pCurrVtx[0] = sinf(rad);
			pCurrVtx[1] = h;
			pCurrVtx[2] = cosf(rad);
			pCurrVtx += 3;
		}
	}

	/* Fill the Index Buffer */
	pCurrIdx = idxList;
	for (j = 0; j < nSubHeight; j++) {
		base_idx = j * (nSubDiv + 1);
		for (i = 0; i < nSubDiv; i++) {
			pCurrIdx[0] = pCurrIdx[3] = base_idx + i;
			pCurrIdx[1] = base_idx + i + nSubDiv + 1;
			pCurrIdx[2] = pCurrIdx[4] = base_idx + i + nSubDiv + 2;
			pCurrIdx[5] = base_idx + i + 1;
			pCurrIdx += 6;
		}
	}

	vb_id = glGenVertexBuffer(vtxSize, vtxList);
	ib_id = glGenIndexBuffer(idxSize, idxList);

	free(vtxList);
	free(idxList);

	if (vb_id == 0 || ib_id == 0) {
		SAFE_DELETE(vb_id);
		SAFE_DELETE(ib_id);
		return -1; /* Failed to create buffer */
	}

	*pOutVB = vb_id;
	*pOutIB = ib_id;
	*pOutIdxCnt = idxCnt;

	return 0;
}

int _genUnitTorusElement(GLuint *pOutVB, GLuint *pOutIB, GLsizei *pOutIdxCnt)
{
	const int nSubDiv    = 24;
	const int nSubCircle = 20;

	GLuint vb_id = 0;
	GLuint ib_id = 0;

	int i, j, base_idx;
	GLfloat  *vtxList, *pCurrVtx, m_rad, t_rad;
	GLushort *idxList, *pCurrIdx;

	GLsizei idxCnt = 3 * nSubDiv * 2 * nSubCircle;
	GLsizei vtxSize = sizeof(GLfloat) * 3 * (nSubDiv + 1) * (nSubCircle + 1);
	GLsizei idxSize = sizeof(GLushort) * idxCnt;

	vtxList = (GLfloat *)malloc(vtxSize);
	idxList = (GLushort *)malloc(idxSize);

	/* Fill the Vertex Buffer */
	pCurrVtx = vtxList;
	for (j = 0; j <= nSubDiv; j++) {
		m_rad = (float)j * ((2.0f * (float)M_PI) / (float)nSubDiv);
		for (i = 0; i <= nSubCircle; i++) {
			t_rad = i < nSubCircle ? (float)i * ((2.0f * (float)M_PI) / (float)nSubCircle) : 0.0f;
			pCurrVtx[0] = cosf(t_rad);
			pCurrVtx[1] = sinf(t_rad);
			pCurrVtx[2] = m_rad;

			pCurrVtx += 3;
		}
	}

	/* Fill the Index Buffer */
	pCurrIdx = idxList;
	for (j = 0; j < nSubDiv; j++) {
		base_idx = j * (nSubCircle + 1);
		for (i = 0; i < nSubCircle; i++) {
			pCurrIdx[0] = pCurrIdx[3] = base_idx + i;
			pCurrIdx[1] = base_idx + i + nSubCircle + 1;
			pCurrIdx[2] = pCurrIdx[4] = base_idx + i + nSubCircle + 2;
			pCurrIdx[5] = base_idx + i + 1;
			pCurrIdx += 6;
		}
	}

	vb_id = glGenVertexBuffer(vtxSize, vtxList);
	ib_id = glGenIndexBuffer(idxSize, idxList);

	free(vtxList);
	free(idxList);

	if (vb_id == 0 || ib_id == 0) {
		SAFE_DELETE(vb_id);
		SAFE_DELETE(ib_id);
		return -1; /* Failed to create buffer */
	}

	*pOutVB = vb_id;
	*pOutIB = ib_id;
	*pOutIdxCnt = idxCnt;

	return 0;
}
