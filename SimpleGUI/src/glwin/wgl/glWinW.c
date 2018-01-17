#if defined(_WIN32) || defined(_WIN64)
#include "../glWin.h"
#include <GL/wglew.h>
#include <string.h>

#define _DEBUG_PRINT
#include "../dbg_print.h"

#define GLWIN_CLASS_NAME "GL_WINDOW"

/* Reference from
 https://www.khronos.org/opengl/wiki/Tutorial:_OpenGL_3.1_The_First_Triangle_(C%2B%2B/Win) */
HGLRC CreateGLContext(HDC hDC)
{
	HGLRC hRC = NULL, tmpRC = NULL;
	int attribs[] =
	{
		WGL_CONTEXT_MAJOR_VERSION_ARB, 3,
		WGL_CONTEXT_MINOR_VERSION_ARB, 0,
		WGL_CONTEXT_FLAGS_ARB, 0,
		0
	};

	tmpRC = wglCreateContext(hDC);
	if (tmpRC == NULL || !wglMakeCurrent(hDC, tmpRC)) {
		DBGP_ERR("Failed to create or initialize base OpenGL Rendering Context\n");
		return NULL;
	}

	if (glewInit() != GLEW_OK) {
		DBGP_ERR("Failed to initialize glew\n");
		wglMakeCurrent(NULL, NULL);
		wglDeleteContext(tmpRC);
		return NULL;
	}

	if (wglewIsSupported("WGL_ARB_create_context")) {
		hRC = wglCreateContextAttribsARB(hDC, NULL, attribs);
	}
	
	wglMakeCurrent(NULL, NULL);
	if (hRC == NULL) {
		/* Not supported or Failed to create... -> Use default Rendering Context */
		hRC = tmpRC;
	}
	else {
		/* Delete temp context */
		wglDeleteContext(tmpRC);
	}

	return hRC;
}

/* reference from NeHe's OpenGL Tutorial 
 http://nehe.gamedev.net/tutorial/creating_an_opengl_window_(win32)/13001/ */
int CreateGLWindow( GL_WIN *pOut, const char *szTitle, int width, int height, EVENT_PROC fnEventProc )
{
	static const PIXELFORMATDESCRIPTOR pfd =
	{
		sizeof(PIXELFORMATDESCRIPTOR),  // Size Of This Pixel Format Descriptor
		1,                              // Version Number
		PFD_DRAW_TO_WINDOW |            // Format Must Support Window
		PFD_SUPPORT_OPENGL |            // Format Must Support OpenGL
		PFD_DOUBLEBUFFER,               // Must Support Double Buffering
		PFD_TYPE_RGBA,                  // Request An RGBA Format
		32,                             // Select Our Color Depth
		0, 0, 0, 0, 0, 0,               // Color Bits Ignored
		0,                              // No Alpha Buffer
		0,                              // Shift Bit Ignored
		0,                              // No Accumulation Buffer
		0, 0, 0, 0,                     // Accumulation Bits Ignored
		24,                             // 24Bit Z-Buffer (Depth Buffer)
		8,                              // 8Bit Stencil Buffer
		0,                              // No Auxiliary Buffer
		PFD_MAIN_PLANE,                 // Main Drawing Layer
		0,                              // Reserved
		0, 0, 0                         // Layer Masks Ignored
	};

	HINSTANCE hInstance = GetModuleHandleA(NULL);
	HWND      hWnd = NULL;
	HDC       hDC  = NULL;
	HGLRC     hRC  = NULL;
	GLuint    pixel_fmt = 0;

	RECT      client_area = { 0, 0, width, height };
	DWORD     dwMyWinStyle = WS_OVERLAPPED | WS_CAPTION | WS_MINIMIZEBOX | WS_SYSMENU | WS_VISIBLE;
	WNDCLASSA wc = {
		CS_HREDRAW | CS_VREDRAW | CS_DBLCLKS,
		(WNDPROC)fnEventProc,
		0, 0,
		hInstance,
		LoadIconA(NULL, IDI_APPLICATION),
		LoadCursorA(NULL, IDC_ARROW),
		(HBRUSH)GetStockObject(WHITE_BRUSH),
		NULL,
		GLWIN_CLASS_NAME
	};

	if (!RegisterClassA(&wc)) {
		DBGP_ERR("Failed to register the window class\n");
		exit(EXIT_FAILURE);
	}

	/* Calculate new width and height */
	AdjustWindowRect(&client_area, dwMyWinStyle, FALSE);
	width  = client_area.right  - client_area.left;
	height = client_area.bottom - client_area.top;

	/* Create Window */
	hWnd = CreateWindowA( GLWIN_CLASS_NAME, szTitle, dwMyWinStyle,
		CW_USEDEFAULT, CW_USEDEFAULT, width, height,
		NULL, NULL, hInstance, NULL );
	if (hWnd == NULL) {
		DBGP_ERR("Failed to create window\n");
		UnregisterClassA(GLWIN_CLASS_NAME, hInstance);
		exit(EXIT_FAILURE);
	}

	/* Create OpenGL rendering context */
	hDC = GetDC(hWnd);
	if (hDC == NULL) {
		DBGP_ERR("Failed - GetDC()\n");
		DestroyWindow(hWnd);
		exit(EXIT_FAILURE);
	}

	pixel_fmt = ChoosePixelFormat(hDC, &pfd);
	if (!SetPixelFormat(hDC, pixel_fmt, &pfd)) {
		DBGP_ERR("Failed - Choose & Set Pixel Format\n");
		ReleaseDC(hWnd, hDC);
		DestroyWindow(hWnd);
		exit(EXIT_FAILURE);
	}

	hRC = CreateGLContext(hDC);
	if ( hRC == NULL || !wglMakeCurrent(hDC, hRC) ) {
		DBGP_ERR("Failed to create or initialize base OpenGL Rendering Context\n");
		if( hRC ) wglDeleteContext(hRC);
		ReleaseDC(hWnd, hDC);
		DestroyWindow(hWnd);
		exit(EXIT_FAILURE);
	}

	/* Assign to output buffer */
	pOut->hInstance = hInstance;
	pOut->hWnd      = hWnd;
	pOut->hDC       = hDC;
	pOut->hRC       = hRC;

	return 0;
}

void DestroyGLWindow( GL_WIN *pGLWin )
{
	if(pGLWin) {
		wglMakeCurrent( NULL, NULL );
		if( pGLWin->hRC )  wglDeleteContext( pGLWin->hRC );
		if( pGLWin->hDC )  ReleaseDC( pGLWin->hWnd, pGLWin->hDC );
		if( pGLWin->hWnd ) DestroyWindow( pGLWin->hWnd );
		if( pGLWin->hInstance ) UnregisterClassA(GLWIN_CLASS_NAME, pGLWin->hInstance);

		memset( pGLWin, 0x00, sizeof(GL_WIN) );
	}
}

int ProcessGLWindowEvent( GL_WIN *pGLWin )
{
	MSG msg;
	while( PeekMessageA(&msg, NULL, 0, 0, PM_REMOVE) ) {
		if( msg.message == WM_QUIT ) return -1;
		TranslateMessage( &msg );
		DispatchMessageA( &msg );
	}
	return 0;
}

void SetGLWindowTitle(GL_WIN *pGLWin, const char *szTitle)
{
	if (pGLWin && pGLWin->hWnd) {
		SetWindowTextA(pGLWin->hWnd, szTitle);
	}
}

#endif