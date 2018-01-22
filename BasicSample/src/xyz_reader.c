#include "xyz_reader.h"
#include <stdio.h>
#include <stdlib.h>
#include <ctype.h>

int isEmpty(const char *buf) {
	while (*buf) {
		if (!isspace( (unsigned char)*buf)) return 0;
		buf++;
	}
	return 1;
}

int ReadXYZ(float **pOutPointCloudList, const char *szFileName)
{
	FILE *fp = NULL;
	char buf[1024] = { 0, };
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
		while (fgets(buf, 1023, fp)) {
			if (!isEmpty(buf)) {
				++numPC;
			}
		}
		if (numPC == 0) return 0;

		pOutBuf = (float *)malloc(sizeof(float) * 3 * numPC);
		if (pOutBuf == NULL) return -1;

		fseek(fp, 0, SEEK_SET);

		pCurr = pOutBuf;
		while (fgets(buf, 1023, fp)) {
			if (!isEmpty(buf)) {
#ifdef _WIN32
				sscanf_s(buf, "%f %f %f", &x, &y, &z);
#else
				sscanf(buf, "%f %f %f", &x, &y, &z);
#endif
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

	return -1;
}