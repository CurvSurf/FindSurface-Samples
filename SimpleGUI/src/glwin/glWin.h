#ifndef _GL_WIN_H_
#define _GL_WIN_H_

#if defined(_WIN32) || defined(_WIN64)
	#include <Windows.h>
	#include <GL/glew.h>

	#define SWAP_BUFFERS(GL_WIN) SwapBuffers((GL_WIN).hDC)

	typedef WNDPROC EVENT_PROC;
	typedef struct {
		HINSTANCE hInstance;
		HWND      hWnd;
		HDC       hDC;
		HGLRC     hRC;
	} GL_WIN;

#else
	#include <unistd.h>
	#include <X11/Xlib.h>
	#include <X11/Xutil.h>
	#include <GL/gl.h>
	#include <GL/glx.h>

	#define SWAP_BUFFERS(GL_WIN) glXSwapBuffers((GL_WIN).pDisp, (GL_WIN).hWnd)

	typedef void (*EVENT_PROC)(XEvent *);
	typedef struct {
		Window     hWnd;
		GLXContext hRC;

		/* X Window Components */
		Display   *pDisp;
		Colormap   cmap;

		/* X Window Event */
		Atom       WM_DELETE_WINDOW;
		EVENT_PROC fnEventProc;
	} GL_WIN;

#endif

/**
 * Create OpenGL Window
 */
int CreateGLWindow( GL_WIN *pOut, const char *szTitle, int width, int height, EVENT_PROC fnEventProc );

/**
 * Destroy OpenGL Window
 */
void DestroyGLWindow( GL_WIN *pGLWin );

int ProcessGLWindowEvent( GL_WIN *pGLWin );

void SetGLWindowTitle(GL_WIN *pGLWin, const char *szTitle);



#endif