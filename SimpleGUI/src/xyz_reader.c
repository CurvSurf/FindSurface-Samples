#include "xyz_reader.h"
#include <stdio.h>
#include <stdlib.h>
#include <ctype.h>

#ifdef _WIN32
#define __sscanf sscanf_s
#else
#define __sscanf sscanf
#endif

int ReadXYZ(float **pOutPointCloudList, const char *szFileName)
{
	FILE *fp = NULL;
	char buf[1024] = { 0, };
	const int MAX_BUF_SIZE = sizeof(buf) - 1;
	float x, y, z;
	int numPC = 0;
	float *pOutBuf = NULL;
	float *pCurr = NULL;

	if (pOutPointCloudList == NULL) return 0;

#ifdef _WIN32
	fopen_s(&fp, szFileName, "r");
#else
	fp = fopen(szFileName, "r");
#endif

	if (fp) {
		numPC= 0;
		while (fgets(buf, MAX_BUF_SIZE, fp)) {
			if (__sscanf(buf, "%g %g %g", &x, &y, &z) == 3) {
				++numPC;
			}
		}
		if (numPC == 0) return 0;

		pOutBuf = (float *)malloc(sizeof(float) * 3 * numPC);
		if (pOutBuf == NULL) return -1; /* Memory Allocation Error */

		fseek(fp, 0, SEEK_SET);

		pCurr = pOutBuf;
		while (fgets(buf, MAX_BUF_SIZE, fp)) {
			if (__sscanf(buf, "%g %g %g", &x, &y, &z) == 3) {
				pCurr[0] = x;
				pCurr[1] = y;
				pCurr[2] = z;

				pCurr += 3;
			}
		}

		*pOutPointCloudList = pOutBuf;

		fclose(fp);

		return numPC;
	}

	return -2; /* File Open Error */
}