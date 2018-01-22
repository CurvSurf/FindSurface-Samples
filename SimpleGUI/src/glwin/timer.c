#include "timer.h"
#include <stdlib.h>

#if defined(_WIN32) || defined(_WIN64)
#include <Windows.h>
#else
#include <sys/time.h>
#endif

#define _IMPL_(self) ((TimerImpl*)((self)->pImpl))

typedef struct _tagTimerImpl
{
	double    frequency;

	long long baseTimeStamp;

	long long lastFrameTimeStamp;
	double    lastFrameDurationSec;

	double    avgFrameDurationSec;  /* for last 100 frames */
	double    avgFrameRate;         /* for last 100 frames */

	long long lastPausedTimeStamp;
	long long totalPausedDuration;

	int       isPaused;
} TimerImpl;

static void   ResetImpl         (const struct _tagTimer *self);
static void   ResumeImpl        (const struct _tagTimer *self);
static void   PauseImpl         (const struct _tagTimer *self);
static void   UpdateTickImpl    (const struct _tagTimer *self); 
static double GetElapsedTimeImpl(const struct _tagTimer *self); 
static double GetTotalTimeImpl  (const struct _tagTimer *self); 
static float  GetRWAFPSImpl     (const struct _tagTimer *self); 

int CreateTimer(Timer **pOutTimer)
{
	TimerImpl *pImpl = (TimerImpl *)malloc(sizeof(TimerImpl));
	Timer     *pRtn  = (Timer *)malloc(sizeof(Timer));

	if(!pImpl || !pRtn) {
		free(pImpl);
		free(pRtn);
		return -1; /* Memory Allocation Fail */
	}

	pRtn->pImpl = pImpl;

	/* Initialize Timer */
	pImpl->baseTimeStamp        = 0;
	pImpl->lastFrameTimeStamp   = 0;
	pImpl->lastFrameDurationSec = 0;
	pImpl->lastPausedTimeStamp  = 0;
	pImpl->totalPausedDuration  = 0;
	pImpl->avgFrameDurationSec  = 0;
	pImpl->avgFrameRate         = 0;
	pImpl->isPaused             = 0;

#if defined(_WIN32) || defined(_WIN64)
	{
		LARGE_INTEGER freq;
		QueryPerformanceFrequency( &freq );
		pImpl->frequency = 1.0 / (double)(freq.QuadPart);
	}
#else
	pImpl->frequency = 1.0 / 1000000.0; /* micro secound */
#endif

	pRtn->Reset          = ResetImpl;
	pRtn->Resume         = ResumeImpl;
	pRtn->Pause          = PauseImpl;
	pRtn->UpdateTick     = UpdateTickImpl;
	pRtn->GetElapsedTime = GetElapsedTimeImpl;
	pRtn->GetTotalTime   = GetTotalTimeImpl;
	pRtn->GetRWAFPS      = GetRWAFPSImpl;
	
	*pOutTimer = pRtn;
	return 0;
}

void ReleaseTimer(Timer *pTimer)
{
	if(pTimer) 
	{
		free(pTimer->pImpl);
		free(pTimer);
	}
}

static long long _GetCurrTimeStamp()
{
#if defined(_WIN32) || defined(_WIN64)
	LARGE_INTEGER currTime;
	QueryPerformanceCounter(&currTime);
	return (long long)(currTime.QuadPart);
#else
	struct timeval tv;
	gettimeofday( &tv, NULL );
	return (long long)(tv.tv_sec * 1000000 + tv.tv_usec); /* to microsecond */
#endif
}

/**
 * Implementation of Timer
 */
void ResetImpl(const struct _tagTimer *self)
{
	long long currTime = _GetCurrTimeStamp();

	_IMPL_(self)->baseTimeStamp        = currTime;
	_IMPL_(self)->lastFrameTimeStamp   = currTime;
	_IMPL_(self)->lastFrameDurationSec = 0.0;
	_IMPL_(self)->avgFrameDurationSec  = 0.0;
	_IMPL_(self)->avgFrameRate         = 0.0;

	_IMPL_(self)->isPaused = 0;
	_IMPL_(self)->lastPausedTimeStamp = 0;
	_IMPL_(self)->totalPausedDuration = 0;
}

void ResumeImpl(const struct _tagTimer *self)
{
	if(_IMPL_(self)->isPaused)
	{
		long long currTime = _GetCurrTimeStamp();
		_IMPL_(self)->totalPausedDuration += (currTime - _IMPL_(self)->lastPausedTimeStamp);

		_IMPL_(self)->lastFrameTimeStamp = currTime;
		_IMPL_(self)->isPaused = 0;
	}
}

void PauseImpl(const struct _tagTimer *self)
{
	if( ! _IMPL_(self)->isPaused )
	{
		long long currTime = _GetCurrTimeStamp();

		_IMPL_(self)->lastPausedTimeStamp  = currTime;
		_IMPL_(self)->lastFrameDurationSec = 0.0;
		_IMPL_(self)->isPaused = 1;
	}
}

void UpdateTickImpl(const struct _tagTimer *self)
{
	long long currTime = 0;
	if(_IMPL_(self)->isPaused) { return; }

	currTime = _GetCurrTimeStamp();
	_IMPL_(self)->lastFrameDurationSec = (double)(currTime - _IMPL_(self)->lastFrameTimeStamp)
	                                   * _IMPL_(self)->frequency;
	_IMPL_(self)->lastFrameTimeStamp = currTime;

	if(_IMPL_(self)->lastFrameDurationSec < 0.0) {
		_IMPL_(self)->lastFrameDurationSec = 0.0;
	}

	if(_IMPL_(self)->avgFrameDurationSec <= 0.0) {
		_IMPL_(self)->avgFrameDurationSec = _IMPL_(self)->lastFrameDurationSec;
	}
	else {
		_IMPL_(self)->avgFrameDurationSec = 0.99 * _IMPL_(self)->avgFrameDurationSec + 0.01 * _IMPL_(self)->lastFrameDurationSec;
		_IMPL_(self)->avgFrameRate = 1.0 / _IMPL_(self)->avgFrameDurationSec;
	}
}

double GetElapsedTimeImpl(const struct _tagTimer *self)
{
	return _IMPL_(self)->lastFrameDurationSec;
}

double GetTotalTimeImpl(const struct _tagTimer *self)
{
	return (((_IMPL_(self)->isPaused ? _IMPL_(self)->lastPausedTimeStamp : _IMPL_(self)->lastFrameTimeStamp)
	      - _IMPL_(self)->baseTimeStamp) - _IMPL_(self)->totalPausedDuration) * _IMPL_(self)->frequency;
}

float GetRWAFPSImpl(const struct _tagTimer *self)
{
	return (float)(_IMPL_(self)->avgFrameRate);
}