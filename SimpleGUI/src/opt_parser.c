#include "opt_parser.h"
#include <stdio.h>
#include <stdlib.h>

int parseArguments(FS_OPT_PARAM *pOut, int argc, const char **argv)
{
	int i;
	const char *szTmp = 0;
	char *pEnd = 0;
	float *pTarget = 0;
	float tmp = 0.0f;

	if (!pOut) {
		fprintf(stderr, "Parse Argument Error: Invalid Function Arguments (FS_OPT_PARAM is NULL)\n");
		return -1;
	}

	/* Fill Default Value First */
	pOut->ma = 0.003f; /* 3mm  */
	pOut->md = 0.01f;  /* 10mm */
	pOut->trs = 0.01f; /* 10mm */
	pOut->szFileName = "sample.xyz";

	/* Parse Arguments */
	if (argc < 2) return 0; /* No Arguments */

	for (i = 1; i < argc; i++) {
		if (argv[i][0] == '-') {
			if (argv[i][2] != '\0') continue;
			pTarget = 0;
			switch (argv[i][1]) {
				case 'a': pTarget = &(pOut->ma); break;		// accuracy
				case 'd': pTarget = &(pOut->md); break;		// mean distance
				case 's': pTarget = &(pOut->trs); break;	// touch radius step
				default: fprintf(stderr, "Unknown Parameter: %s\n", argv[i]); return -1;
			}

			if (pTarget) {
				tmp = strtof(argv[i + 1], (char **)0);
				if (tmp <= 0.0f) {
					fprintf(stderr, "Invalid Parameter Value of [-%c] : %s\n(It must be positive floating point number)\n", argv[i][1], argv[i + 1]);
					return -1;
				}
				*pTarget = tmp;
				++i;
			}
		}
		else if (!szTmp) {
			szTmp = argv[i];
			pOut->szFileName = szTmp;
		}
		else {
			fprintf(stderr, "Unknown Parameter: %s\n", argv[i]);
			return -1;
		}
	}
	
	return 0;
}