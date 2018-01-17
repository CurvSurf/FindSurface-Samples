#if !defined(_WIN32) && !defined(_WIN64)
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include "../glWin.h"

#define _DEBUG_PRINT
#include "../dbg_print.h"

#define GLX_CONTEXT_MAJOR_VERSION_ARB       0x2091
#define GLX_CONTEXT_MINOR_VERSION_ARB       0x2092
typedef GLXContext (*glXCreateContextAttribsARBProc)(Display*, GLXFBConfig, GLXContext, Bool, const int*);

/* Helper to check for extension string presence.  Adapted from:
   http://www.opengl.org/resources/features/OGLextensions/ */
static int isExtensionSupported(const char *extList, const char *extension)
{
	const char *start;
	const char *where, *terminator;

	/* Extension names should not have spaces. */
	where = strchr(extension, ' ');
	if (where || *extension == '\0') return 0;

	/* It takes a bit of care to be fool-proof about parsing the
	   OpenGL extensions string. Don't be fooled by sub-strings,
	   etc. */
	start = extList;
	for (;;) 
	{
		where = strstr(start, extension);
		if (!where) break;

		terminator = where + strlen(extension);
		if ( where == start || *(where - 1) == ' ' ) {
			if ( *terminator == ' ' || *terminator == '\0' ) {
				return 1;
			}
		}

		start = terminator;
	}

	return 0;
}

/* Callback Functions */
static int g_ctxErrorOccurred = 0;
static int ctxErrorHandler( Display *dpy, XErrorEvent *ev )
{
    g_ctxErrorOccurred = 1;
    return 0;
}

static Bool WaitForMapNotify(Display *pDist, XEvent *evt, char *arg)
{
	return ( (evt->type == MapNotify) && (evt->xmap.window == (Window)arg) ) ? GL_TRUE : GL_FALSE;
}

static int QueryVersion(Display *pDisp)
{
	int glx_major, glx_minor;
 
	/* FBConfigs were added in GLX version 1.3. */
	if ( !glXQueryVersion( pDisp, &glx_major, &glx_minor ) || 
		( ( glx_major == 1 ) && ( glx_minor < 3 ) ) || ( glx_major < 1 ) )
	{
		DBGP_ERR("Invalid GLX version (%d.%d)\n", glx_major, glx_minor);
		return -1;
	}
	return 0;
}

static int SelectBestFBConfig(GLXFBConfig *pOut, Display *pDisp, const int *visual_attribs)
{
	int fbCnt = 0;
	GLXFBConfig *pFBC = NULL;

	int best_fbc = -1, worst_fbc = -1, best_num_samp = -1, worst_num_samp = 999;
	int samp_buf, samples;
	int i;

	XVisualInfo *pVI = NULL;

	pFBC = glXChooseFBConfig(pDisp, DefaultScreen(pDisp), visual_attribs, &fbCnt);
	if(pFBC == NULL) {
		DBGP_ERR("Failed to retrieve a famebuffer config\n");
		return -1;
	}
	DBGP( "Found %d matching FB configs.\n", fbCnt );

	/* Pick the FB config/visual with the most samples per pixel */
	DBGP( "Getting XVisualInfos\n" );
	for(i = 0; i < fbCnt; i++) 
	{
		pVI = glXGetVisualFromFBConfig( pDisp, pFBC[i] );
		if(pVI)
		{
			glXGetFBConfigAttrib( pDisp, pFBC[i], GLX_SAMPLE_BUFFERS, &samp_buf );
			glXGetFBConfigAttrib( pDisp, pFBC[i], GLX_SAMPLES       , &samples  );

			DBGP( "  Matching fbconfig %d, visual ID 0x%2x: SAMPLE_BUFFERS = %d,"
              " SAMPLES = %d\n", i, (unsigned int)(pVI->visualid), samp_buf, samples );

			if ( best_fbc < 0 || samp_buf && samples > best_num_samp )
				best_fbc = i, best_num_samp = samples;
			if ( worst_fbc < 0 || !samp_buf || samples < worst_num_samp )
				worst_fbc = i, worst_num_samp = samples;
		}
		XFree(pVI);
	}

	if(best_fbc < 0) {
		DBGP_ERR("No matching fbconfig...\n");
	}
	else {
		*pOut = pFBC[best_fbc];
	}

	XFree(pFBC);

	return best_fbc < 0 ? -1 : 0;
}

static GLXContext CreateGLContext( Display *pDisp, GLXFBConfig fbConf )
{
	GLXContext ctx = NULL;
	const char *glxExts = glXQueryExtensionsString(pDisp, DefaultScreen(pDisp));

	/* NOTE: It is not necessary to create or make current to a context before
       calling glXGetProcAddressARB */
	glXCreateContextAttribsARBProc _glXCreateContextAttribsARB
		= (glXCreateContextAttribsARBProc)glXGetProcAddressARB( (const GLubyte *) "glXCreateContextAttribsARB" );
	
	int (*oldHandler)(Display *, XErrorEvent *) = XSetErrorHandler(&ctxErrorHandler);

	g_ctxErrorOccurred = 0;
	/* Check for the GLX_ARB_create_context extension string and the function.
  	   If either is not present, use GLX 1.3 context creation method. */
	if(!isExtensionSupported( glxExts, "GLX_ARB_create_context") || _glXCreateContextAttribsARB == NULL) 
	{
		DBGP( "glXCreateContextAttribsARB() not found... Using old-style GLX context\n" );
		ctx = glXCreateNewContext( pDisp, fbConf, GLX_RGBA_TYPE, 0, True );
	}
	else
	{
		int context_attribs[] = {
        	GLX_CONTEXT_MAJOR_VERSION_ARB, 3,
        	GLX_CONTEXT_MINOR_VERSION_ARB, 0,
        	None
      	};

		DBGP( "Creating context\n" );
		ctx = _glXCreateContextAttribsARB( pDisp, fbConf, 0, True, context_attribs );

		/* Sync to ensure any errors generated are processed */
		XSync( pDisp, False );

		if( !g_ctxErrorOccurred && ctx ) { DBGP( "Created GL 3.0 context\n" ); }
		else 
		{
			/* Couldn't create GL 3.0 context.  Fall back to old-style 2.x context.
			   When a context version below 3.0 is requested, implementations will
			   return the newest context version compatible with OpenGL versions less
			   than version 3.0. */
			context_attribs[1] = 1; /* GLX_CONTEXT_MAJOR_VERSION_ARB = 1 */
      		context_attribs[3] = 0; /* GLX_CONTEXT_MINOR_VERSION_ARB = 0 */

			g_ctxErrorOccurred = 0;

      		DBGP( "Failed to create GL 3.0 context... using old-style GLX context\n" );
			ctx = _glXCreateContextAttribsARB( pDisp, fbConf, 0, True, context_attribs );
		}
	}

	/* Sync to ensure any errors generated are processed */
	XSync( pDisp, False );

	/* Restore the original error handler */
	XSetErrorHandler( oldHandler );

	if( g_ctxErrorOccurred || ctx == NULL )
	{
		DBGP_ERR( "Failed to create an OpenGL context\n" );
		return NULL;
	}

	/* Verifying that context is a direct context */
	if ( !glXIsDirect ( pDisp, ctx ) )
	{
		DBGP( "Indirect GLX rendering context obtained\n" );
	}
	else
	{
		DBGP( "Direct GLX rendering context obtained\n" );
	}

	return ctx;
}

int CreateGLWindow(GL_WIN *pOut, const char *szTitle, int width, int height, EVENT_PROC fnEventProc)
{
	static int visual_attribs[] =
	{
		GLX_X_RENDERABLE    , True,
		GLX_DRAWABLE_TYPE   , GLX_WINDOW_BIT,
		GLX_RENDER_TYPE     , GLX_RGBA_BIT,
		GLX_X_VISUAL_TYPE   , GLX_TRUE_COLOR,
		GLX_RED_SIZE        , 8,
		GLX_GREEN_SIZE      , 8,
		GLX_BLUE_SIZE       , 8,
		GLX_ALPHA_SIZE      , 8,
		GLX_DEPTH_SIZE      , 24,
		GLX_STENCIL_SIZE    , 8,
		GLX_DOUBLEBUFFER    , True,
		//GLX_SAMPLE_BUFFERS  , 1,
		//GLX_SAMPLES         , 4,
		None
	};

	GLXFBConfig fbConf;
	XVisualInfo *pVI = NULL;
	Display *pDisp   = NULL;

	XSetWindowAttributes swa;
	Colormap cmap;

	Window root, glWin;
	XEvent evt;
	Atom wmDeleteMessage;
	GLXContext ctx = NULL;

	if(pOut == NULL) return -1;

	pDisp = XOpenDisplay(NULL);

	if(pDisp == NULL) return -1;
	if(QueryVersion(pDisp)) {
		XCloseDisplay( pDisp );
		return -2;
	}
	if(SelectBestFBConfig(&fbConf, pDisp, visual_attribs)) {
		XCloseDisplay( pDisp );
		return -3;
	}

	/* Get a visual */
	pVI = glXGetVisualFromFBConfig( pDisp, fbConf );
	DBGP( "Chosen visual ID = 0x%x\n", (unsigned int)(pVI->visualid) );

	root = RootWindow( pDisp, pVI->screen );

	/* Create Colormap */
	DBGP( "Creating colormap\n" );
	cmap = XCreateColormap( pDisp, root, pVI->visual, AllocNone );

	/* Create Window */
	swa.background_pixel = None;
	swa.border_pixel     = 0;
	swa.colormap         = cmap;
	swa.event_mask       = ExposureMask 
	                     | KeyPressMask 
						 | KeyReleaseMask 
						 | ButtonPressMask 
						 | ButtonReleaseMask 
						 | PointerMotionMask
						 | StructureNotifyMask;
	DBGP( "Creating window\n" );
	glWin = XCreateWindow( pDisp, root,
		0, 0, width, height, 
		0, pVI->depth, InputOutput,
		pVI->visual,
		CWBorderPixel | CWColormap | CWEventMask, &swa
	);

	/* Done with the visual info data */
	XFree( pVI );

	if( !glWin ) {
		DBGP_ERR( "Failed to create window\n" );
		XCloseDisplay( pDisp );
		return -4;
	}

	/* Set Window Title */
	XStoreName( pDisp, glWin, szTitle );

	/* Set Delete Window Message */
	wmDeleteMessage = XInternAtom(pDisp, "WM_DELETE_WINDOW", False);
	XSetWMProtocols(pDisp, glWin, &wmDeleteMessage, 1);

	/* Create OpenGL Context */
	DBGP( "Create OpenGL Rendering Context\n" );
	ctx = CreateGLContext( pDisp, fbConf );

	if( ctx == NULL ) {
		XDestroyWindow( pDisp, glWin );
		XFreeColormap( pDisp, cmap );
		XCloseDisplay( pDisp );
		return -5;
	}

	glXMakeCurrent( pDisp, glWin, ctx );

	/* Show Window */
	DBGP( "Mapping window\n" );
	XMapWindow( pDisp, glWin );
	XIfEvent( pDisp, &evt, WaitForMapNotify, (char *)glWin );

	/* Assign to output buffer */
	pOut->hWnd  = glWin;
	pOut->hRC   = ctx;
	pOut->pDisp = pDisp;
	pOut->cmap  = cmap;
	pOut->WM_DELETE_WINDOW = wmDeleteMessage;
	pOut->fnEventProc = fnEventProc;

	return 0;
}

void DestroyGLWindow( GL_WIN *pGLWin )
{
	if(pGLWin) {
		glXMakeCurrent( pGLWin->pDisp, 0, 0 );
		glXDestroyContext( pGLWin->pDisp, pGLWin->hRC );

		XDestroyWindow( pGLWin->pDisp, pGLWin->hWnd );
		XFreeColormap( pGLWin->pDisp, pGLWin->cmap );
		XCloseDisplay( pGLWin->pDisp );

		memset( pGLWin, 0x00, sizeof(GL_WIN) );
	}
}

int ProcessGLWindowEvent( GL_WIN *pGLWin )
{
	XEvent event;
	while( XPending(pGLWin->pDisp) ) {
		XNextEvent(pGLWin->pDisp, &event);
		if(pGLWin->fnEventProc) pGLWin->fnEventProc( &event );
		/* Quit Window Event ? */
		return (event.type == ClientMessage && 
		        event.xclient.data.l[0] == pGLWin->WM_DELETE_WINDOW) ? -1 : 1;
	}
	return 0;
}

void SetGLWindowTitle(GL_WIN *pGLWin, const char *szTitle)
{
	if (pGLWin && pGLWin->pDisp && pGLWin->hWnd) {
		XStoreName(pGLWin->pDisp, pGLWin->hWnd, szTitle);
	}
}
#endif