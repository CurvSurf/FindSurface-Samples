#include "pointCloudRenderer.h"

static const char *g_szVertexShader =
"attribute vec4   aPosition;\n"
"uniform   mat4   uMVP;\n"
"uniform   vec4   uParam;\n"
"varying   vec4   vColor;\n"
"\n"
"void main() {\n"
"	vColor = vec4(uParam.xyz, 1);\n"
"	gl_Position = uMVP * aPosition;\n"
"	gl_PointSize = uParam.a;\n"
"}\n"
;

static const char *g_szFragmentShader =
"varying vec4 vColor;\n"
"\n"
"void main() {\n"
"	gl_FragColor = vColor;\n"
"}\n"
;

static struct {
	/* OpenGL Shader Program ID */
	GLuint pid;

	/* OpenGL Shader Attribute & Uniform IDs */
	GLint aPosition_id; /* attribute vec4   aPosition  */
	GLint uMVP_id;      /* uniform   mat4   uMVP       */
	GLint uParam_id;    /* uniform   float4 uParam     */
} PointCloudShader = { 0, }, *g_pPCS = 0;

int createPointCloudRenderer()
{
	GLuint pid    =  0;
	GLint  aPos   = -1;
	GLint  uMVP   = -1;
	GLint  uParam = -1;

	if (g_pPCS) return 0; /* Already be created */

	pid = glCompileProgram(g_szVertexShader, g_szFragmentShader);
	if (!pid) return -2; /* Failed to Create Shader */

	aPos   = glGetAttribLocation (pid, "aPosition");
	uMVP   = glGetUniformLocation(pid, "uMVP");
	uParam = glGetUniformLocation(pid, "uParam");

	if (aPos < 0 || uMVP < 0 || uParam < 0) {
		glDeleteProgram(pid);
		return -3;
	}

	PointCloudShader.pid           = pid;
	PointCloudShader.aPosition_id  = aPos;
	PointCloudShader.uMVP_id       = uMVP;
	PointCloudShader.uParam_id     = uParam;

	g_pPCS = &PointCloudShader;

	return 0;
}

void releasePointCloudRenderer()
{
	if (g_pPCS) {
		if (g_pPCS->pid) {
			glDeleteProgram(g_pPCS->pid);
			g_pPCS->pid = 0;
		}
		g_pPCS->aPosition_id = -1;
		g_pPCS->uMVP_id      = -1;
		g_pPCS->uParam_id    = -1;

		g_pPCS = 0;
	}
}

void beginPointCloudRenderer()
{
	glUseProgram(g_pPCS->pid);
	glEnableVertexAttribArray(g_pPCS->aPosition_id);
}

void endPointCloudRenderer()
{
	glDisableVertexAttribArray(g_pPCS->aPosition_id);
	glUseProgram(0);
}

void drawPointRenderObject(const RENDER_OBJECT *pObj, const FLOAT_MATRIX *pVP)
{
	/* we assume that point cloud data has no model matrix */
	if (pObj->mode == GL_POINTS) {
		glBindBuffer(GL_ARRAY_BUFFER, pObj->vb_id);
		glVertexAttribPointer(g_pPCS->aPosition_id, 3, GL_FLOAT, GL_FALSE, 0, 0);

		glUniform4fv(g_pPCS->uParam_id, 1, pObj->colors);
		glUniformMatrix4fv(g_pPCS->uMVP_id, 1, GL_FALSE, &(pVP->arr));

		glDrawArrays(GL_POINTS, 0, pObj->element_count);

		glBindBuffer(GL_ARRAY_BUFFER, 0);
	}
}
