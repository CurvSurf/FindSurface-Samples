#ifndef _POINT_CLOUD_RENDERER_H_
#define _POINT_CLOUD_RENDERER_H_

#include "renderObject.h"

int createPointCloudRenderer();
void releasePointCloudRenderer();

void beginPointCloudRenderer();
void endPointCloudRenderer();

void drawPointRenderObject(const RENDER_OBJECT *pObj, const FLOAT_MATRIX *pVP);

#endif
