#ifndef _GL_UTIL_H_
#define _GL_UTIL_H_

#if defined(_WIN32) || defined(_WIN64)
	#include <GL/glew.h>
#else
	#include <GL/gl.h>
	#include <GL/glext.h>
#endif

#define BUFFER_OFFSET(i) ((GLvoid*)(i))

void glCheckError(const char *operation);
GLuint glCompileProgram(const char *szVtxSrc, const char *szFragSrc);
GLuint glGenVertexBuffer(GLsizeiptr size, const void *pVtxList);
GLuint glGenIndexBuffer(GLsizeiptr size, const void *pIdxList);

#endif