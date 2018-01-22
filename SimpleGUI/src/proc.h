#ifndef _PROC_H_
#define _PROC_H_

#include "glwin/timer.h"
#include "glwin/keycode.h"

/* Event Proc.-> impl. in (W|X)EvtProc.c */
#if defined(_WIN32) || defined(_WIN64)
#include <Windows.h>
LRESULT CALLBACK EventProc(HWND, UINT, WPARAM, LPARAM);
#else
#include <X11/Xlib.h>
void EventProc(XEvent *);
#endif

typedef struct _tagCreateWindowParam {
    const char *szTitle; /* title of window  */
    int         nWidth;  /* width of window  */
    int         nHeight; /* height of widnow */
    /*int bFullScreen;*/
} CREATE_WINDOW_PARAM;

/* ===================================================================== *
 * GL Application Related Variables & Functions-> impl. in glmain.c      *
 * These variables & functions are used or called in "main.c"            *
 * ===================================================================== */

/* Initialization sequence, before create GL window & context.
   by default, if this function returns non-zero value,
   program soon terminate.*/
int InitApp( CREATE_WINDOW_PARAM *pParam, int argc, const char **argv );

/* Initialization sequence, after create GL window & context.
   by default, if this function returns non-zero value,
   program soon terminate.*/
int InitGL();

/* Every frame step */
void FrameStep(const Timer *pTimer);

/* Release sequence, related with OpenGL context */
void ReleaseGL();

/* Release sequence, except OpenGL context */
void ReleaseApp();

/* ===================================================================== *
 * GL Application Specific Event Handle Functions                        *
 * **these functions will be called from EventProc() in (W|X)EvtProc.c   *
 * ===================================================================== */
void OnResizeWindow(int new_width, int new_height);
void OnKeyPress(unsigned int key_code); /* escape key is reserved for terminate the application */
void OnKeyRelease(unsigned int key_code);
void OnMouseButtonPress(unsigned int which, int x, int y);
void OnMouseButtonRelease(unsigned int which, int x, int y);
void OnMouseMove(unsigned int key_state, int x, int y);
void OnMouseWheel(int zDelta, unsigned int key_state, int x, int y);


#endif