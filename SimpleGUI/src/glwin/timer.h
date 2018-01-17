#ifndef _TIMER_H_
#define _TIMER_H_

typedef struct _tagTimer
{
	/* capsuled member variable */
	void *pImpl;

	/* member functions */
	void   (*Reset)         (const struct _tagTimer *self);
	void   (*Resume)        (const struct _tagTimer *self);
	void   (*Pause)         (const struct _tagTimer *self);
	void   (*UpdateTick)    (const struct _tagTimer *self); /* Call every frame */
	double (*GetElapsedTime)(const struct _tagTimer *self); /* Duration between frame in sec */
	double (*GetTotalTime)  (const struct _tagTimer *self); /* in Sec */
	float  (*GetRWAFPS)     (const struct _tagTimer *self); /* Get FPS over a Recency weighted Average */
} Timer;

int  CreateTimer(Timer **pOutTimer);
void ReleaseTimer(Timer *pTimer);
#endif