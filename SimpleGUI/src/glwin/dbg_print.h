#ifdef _DEBUG_PRINT
	#include <stdio.h>
	#define DBGP_ERR(...) fprintf(stderr, __VA_ARGS__)
	#define DBGP(...)     printf(__VA_ARGS__)
#else
	#define DBGP_ERR(...) 
	#define DBGP(...)
#endif