/*
 * Event Handler for X Window System
 */
#if !defined(_WIN32) && !defined(_WIN64)
#include "../../proc.h"
#include <X11/keysym.h>
#include <stdio.h>
#include <string.h>

static void DestroyWindow(Display *pDisp, Window hWnd)
{
	Atom delMsg = XInternAtom( pDisp, "WM_DELETE_WINDOW", False);

	XEvent event = { 0, };
	memset(&event, 0x00, sizeof(XEvent));

	event.type                 = ClientMessage;
	event.xclient.window       = hWnd;
	event.xclient.message_type = delMsg;
	event.xclient.data.l[0]    = delMsg;
	event.xclient.format       = 32;

	XSendEvent( pDisp, hWnd, False, 0, (XEvent *)&event );
}

void EventProc(XEvent *evt)
{
	switch(evt->type)
	{
		case KeyPress:
		{
			KeySym key = XLookupKeysym( &evt->xkey, 0 );
			/* printf("KeyPress: code: 0x%04X, symbol: 0x%04X\n", evt->xkey.keycode, (unsigned int)key); */

			if(key == XK_Escape) DestroyWindow(evt->xkey.display, evt->xkey.window);
			else OnKeyPress((unsigned int)key);
		}	
		break;

		case KeyRelease:
		{
			KeySym key = XLookupKeysym(&evt->xkey, 0);
			OnKeyRelease(key);
		}
		break;

		case ButtonPress:
		{
			switch (evt->xbutton.button) {
				case 1: case 2: case 3: { OnMouseButtonPress(evt->xbutton.button, evt->xbutton.x, evt->xbutton.y); } break;
				/* Scroll Up */
				case 4: { OnMouseWheel(1, evt->xbutton.state, evt->xbutton.x, evt->xbutton.y); } break;
				/* Scroll Down */
				case 5: { OnMouseWheel(-1, evt->xbutton.state, evt->xbutton.x, evt->xbutton.y); } break;
			}
		}
		break;

		case ButtonRelease:
		{
			switch (evt->xbutton.button) {
				case 1: case 2: case 3: { OnMouseButtonRelease(evt->xbutton.button, evt->xbutton.x, evt->xbutton.y); } break;
			}
		}
		break;

		case MotionNotify:
		{
			OnMouseMove(evt->xmotion.state, evt->xmotion.x, evt->xmotion.y);
		}
		break;

		case ConfigureNotify: /* Resize Window Event */
		{
			static int prev_width  = -1;
			static int prev_height = -1;

			if( evt->xconfigure.width != prev_width || evt->xconfigure.height != prev_height ) {
				prev_width  = evt->xconfigure.width;
				prev_height = evt->xconfigure.height;

				OnResizeWindow(evt->xconfigure.width, evt->xconfigure.height);
			}
		}
		break;
	}
}
#endif