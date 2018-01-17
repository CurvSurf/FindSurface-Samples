#ifndef _RENDER_OBJECT_FACTORY_H_
#define _RENDER_OBJECT_FACTORY_H_

#include "renderObject.h"

/* Generate pre-built unit mesh object */
int  initRenderObjectFactory();
void releaseRenderObjectFactory();

/* Generate Point Object Buffer - input assuemd as tightly packed point position (as float) buffer */
int genPointRenderObject(RENDER_OBJECT *pOut, const float *pPointList, unsigned int nPointCount);
void deletePointRenderObject(RENDER_OBJECT *pObj);

/* Get pre-built unit Mesh object */
int getPlaneRenderObject(RENDER_OBJECT *pOut, float ll[3], float lr[3], float ur[3], float ul[3]);
int getSphereRenderObject(RENDER_OBJECT *pOut, float c[3], float r);
int getCylinderRenderObject(RENDER_OBJECT *pOut, float t[3], float b[3], float r);
int getConeRenderObject(RENDER_OBJECT *pOut, float t[3], float b[3], float tr, float br);
int getTorusRenderObject(RENDER_OBJECT *pOut, float c[3], float n[3], float mr, float tr);

#endif
