#include "customMeshRenderer.h"

static const char *g_szVertexShader =
"attribute vec4 aPosition;\n"
"uniform   mat4 uMVP;\n"
"uniform   vec4 uParam;\n"
"uniform   vec4 uColor;\n"
"varying   vec4 vColor;\n"
"\n"
"void main()\n"
"{\n"
"	vec4 pos = aPosition;\n"
"	if(uParam.w < 0.0) {\n"
"		// Cone ( top radius - uParam.x   ;   bottom radius - uParam.y )\n"
"		float r  = mix( uParam.y, uParam.x, aPosition.y + 0.5 );\n"
"		pos = vec4( aPosition.x * r, aPosition.y, aPosition.z * r, aPosition.w );\n"
"	}\n"
"	else if(uParam.w > 0.0) {\n"
"		// Torus ( mean radius - uParam.x    ;    tube radius - uParam.y     ; torus ratio - uParam.z (0.5 - half ring, 1.0 - complete torus) )\n"
"		float x = aPosition.x * uParam.y + uParam.x;\n"
"		pos = vec4( x * cos(aPosition.z  * uParam.z), aPosition.y * uParam.y, x * sin(aPosition.z * uParam.z), aPosition.w );\n"
"	}\n"
"	else {\n"
"		pos = aPosition; // by pass\n"
"	}\n"
"\n"
"	vColor = uColor;\n"
"	gl_Position = uMVP * pos;\n"
"}\n"
;

static const char *g_szFragmentShader =
"varying vec4 vColor;\n"
"\n"
"void main() {\n"
"	gl_FragColor = vColor;\n"
"}\n"
;

static struct
{
	/* OpenGL Shader Program ID */
	GLuint pid;

	/* OpenGL Shader Attribute & Uniform IDs */
	GLint aPosition_id; /* attribute vec4   aPosition  */
	GLint uMVP_id;      /* uniform   mat4   uMVP       */
	GLint uParam_id;    /* uniform   float4 uParam     */
	GLint uColor_id;    /* uniform   float4 uColor     */
}CustomMeshShader = { 0, }, *g_pCMS = 0;

int createCustomMeshRenderer()
{
	GLuint pid = 0;
	GLint  aPos = -1;
	GLint  uMVP = -1;
	GLint  uParam = -1;
	GLint  uColor = -1;

	if (g_pCMS) return 0; /* Already be created */

	pid = glCompileProgram(g_szVertexShader, g_szFragmentShader);
	if (!pid) return -2; /* Failed to Create Shader */

	aPos = glGetAttribLocation(pid, "aPosition");
	uMVP = glGetUniformLocation(pid, "uMVP");
	uParam = glGetUniformLocation(pid, "uParam");
	uColor = glGetUniformLocation(pid, "uColor");

	if (aPos < 0 || uMVP < 0 || uParam < 0 || uColor < 0) {
		glDeleteProgram(pid);
		return -3;
	}

	CustomMeshShader.pid = pid;
	CustomMeshShader.aPosition_id = aPos;
	CustomMeshShader.uMVP_id = uMVP;
	CustomMeshShader.uParam_id = uParam;
	CustomMeshShader.uColor_id = uColor;
	g_pCMS = &CustomMeshShader;

	return 0;
}

void releaseCustomMeshRenderer()
{
	if (g_pCMS) {
		if (g_pCMS->pid) {
			glDeleteProgram(g_pCMS->pid);
			g_pCMS->pid = 0;
		}
		g_pCMS->aPosition_id = -1;
		g_pCMS->uMVP_id = -1;
		g_pCMS->uParam_id = -1;
		g_pCMS->uColor_id = -1;

		g_pCMS = 0;
	}
}

void beginCustomMeshRenderer()
{
	glUseProgram(g_pCMS->pid);
	glEnableVertexAttribArray(g_pCMS->aPosition_id);
}

void endCustomMeshRenderer()
{
	glDisableVertexAttribArray(g_pCMS->aPosition_id);
	glUseProgram(0);
}

void drawMeshRenderObject(const RENDER_OBJECT *pObj, const FLOAT_MATRIX *pVP)
{
	if (pObj->mode == GL_TRIANGLES) 
	{
		FLOAT_MATRIX mvp;
		MatrixMulGL(&mvp, pVP, &(pObj->model));

		glBindBuffer(GL_ARRAY_BUFFER, pObj->vb_id);
		glVertexAttribPointer(g_pCMS->aPosition_id, 3, GL_FLOAT, GL_FALSE, 0, 0);

		glUniform4fv(g_pCMS->uParam_id, 1, pObj->params);
		glUniform4fv(g_pCMS->uColor_id, 1, pObj->colors);
		glUniformMatrix4fv(g_pCMS->uMVP_id, 1, GL_FALSE, &(mvp.arr));

		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, pObj->ib_id);
		glDrawElements(GL_TRIANGLES, pObj->element_count, GL_UNSIGNED_SHORT, 0);

		glBindBuffer(GL_ARRAY_BUFFER, 0);
	}
}
