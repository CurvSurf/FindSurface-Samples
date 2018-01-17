#ifndef _RENDER_OBJECT_H_
#define _RENDER_OBJECT_H_

#include "glwin/glutil.h"
#include "glwin/floatMatrix.h"

typedef struct _tagRenderObject
{
	GLenum mode;  /* GL_POINTS or GL_TRIANGLES */
	GLuint vb_id; /* Vertex Buffer id */
	GLuint ib_id; /* Index  Buffer id - not used for Points object */
	GLsizei element_count;

	GLfloat params[4]; /* Only for Mesh Object*/
	GLfloat colors[4]; /* alpha (colors[3]) is not used for Mesh object
					   but, Points object use this value as point size */

	FLOAT_MATRIX model; /* Model Matrix for Mesh Object*/
} RENDER_OBJECT;

#endif
