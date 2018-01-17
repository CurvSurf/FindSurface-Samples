/*
 * Event Handler for MS Windows System
 */
#if defined(_WIN32) || defined(_WIN64)
#include "../../proc.h"
#include <Windowsx.h>

LRESULT CALLBACK EventProc(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	switch(uMsg)
	{
		case WM_LBUTTONDOWN: { OnMouseButtonPress(_KEY_LBTN_, GET_X_LPARAM(lParam), GET_Y_LPARAM(lParam)); } break;
		case WM_LBUTTONUP: { OnMouseButtonRelease(_KEY_LBTN_, GET_X_LPARAM(lParam), GET_Y_LPARAM(lParam)); } break;

		case WM_MBUTTONDOWN: { OnMouseButtonPress(_KEY_MBTN_, GET_X_LPARAM(lParam), GET_Y_LPARAM(lParam)); } break;
		case WM_MBUTTONUP: { OnMouseButtonRelease(_KEY_MBTN_, GET_X_LPARAM(lParam), GET_Y_LPARAM(lParam)); } break;

		case WM_RBUTTONDOWN: { OnMouseButtonPress(_KEY_RBTN_, GET_X_LPARAM(lParam), GET_Y_LPARAM(lParam)); } break;
		case WM_RBUTTONUP: { OnMouseButtonRelease(_KEY_RBTN_, GET_X_LPARAM(lParam), GET_Y_LPARAM(lParam)); } break;

		case WM_MOUSEMOVE: { OnMouseMove(wParam, GET_X_LPARAM(lParam), GET_Y_LPARAM(lParam)); } break;
		case WM_MOUSEWHEEL: { 
			POINT pt = { GET_X_LPARAM(lParam), GET_Y_LPARAM(lParam) };
			ScreenToClient(hWnd, &pt);
			OnMouseWheel(GET_WHEEL_DELTA_WPARAM(wParam) / WHEEL_DELTA, GET_KEYSTATE_WPARAM(wParam), pt.x, pt.y); 
		} break;

		case WM_KEYDOWN:
		{
			if (wParam == VK_ESCAPE) {
				DestroyWindow(hWnd);
			}
			else {
				OnKeyPress((unsigned int)wParam);
			}
		}
		break;

		case WM_KEYUP: { OnKeyRelease((unsigned int)wParam); } break;

		case WM_SIZE:
		{
			static int prev_width  = -1;
			static int prev_height = -1;

			int new_width  = LOWORD(lParam);
			int new_height = HIWORD(lParam);

			if(new_width != prev_width || new_height != prev_height ) {
				prev_width  = new_width;
				prev_height = new_height;
				OnResizeWindow( new_width, new_height );
			}
		}
		break;

		case WM_DESTROY:
		{
			PostQuitMessage(0);
		}
		break;

		default:
			return DefWindowProcA(hWnd, uMsg, wParam, lParam);
	}

	return 0;
}

#endif