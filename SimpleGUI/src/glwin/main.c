#include "glWin.h"
#include "../proc.h"

#define MIN_WINDOW_SIZE 100

int main(int argc, char* argv[])
{
	CREATE_WINDOW_PARAM param = { "noname", MIN_WINDOW_SIZE, MIN_WINDOW_SIZE };
	GL_WIN glWin = { 0, };
	Timer *pTimer = (Timer *)0;
	int ret;

	/* Initialize Program */
	if( InitApp(&param, argc, argv) ) {
		/* Fail to Initialize */
		ReleaseApp();
		return -1;
	}

	if(param.nWidth  < MIN_WINDOW_SIZE) param.nWidth  = MIN_WINDOW_SIZE;
	if(param.nHeight < MIN_WINDOW_SIZE) param.nHeight = MIN_WINDOW_SIZE;

	ret = CreateGLWindow(&glWin, param.szTitle, param.nWidth, param.nHeight, EventProc);
	if( ret < 0 ) return -1;

	if( CreateTimer( &pTimer ) < 0) {
		DestroyGLWindow(&glWin);
		ReleaseApp();
		return -1;
	}

	/* Additional Initalize of GL */
	if( InitGL() ) {
		/* Fail to Initialize Graphical Resource */
		ReleaseTimer(pTimer);
		ReleaseGL();
		DestroyGLWindow(&glWin);
		ReleaseApp();
		return -1;
	}

	pTimer->Reset(pTimer);
	for(;;)
	{
		ret = ProcessGLWindowEvent( &glWin );
		if( ret ) { /* Event has been consumed */
			if( ret < 0 ) break; /* Window Destroy Check */
		}
		else { /* On Idle */
			pTimer->UpdateTick(pTimer);
			FrameStep(pTimer);
			SWAP_BUFFERS(glWin);
		}
	}

	ReleaseGL();

	DestroyGLWindow( &glWin );

	ReleaseApp();

	return 0;
}