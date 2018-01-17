#ifndef _CUSTOM_MESH_SHADER_H_
#define _CUSTOM_MESH_SHADER_H_

#include "renderObject.h"

int createCustomMeshRenderer();
void releaseCustomMeshRenderer();

void beginCustomMeshRenderer();
void endCustomMeshRenderer();

void drawMeshRenderObject(const RENDER_OBJECT *pObj, const FLOAT_MATRIX *pVP);

#endif
