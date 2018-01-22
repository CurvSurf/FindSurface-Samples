#ifndef _OPTION_PARSER_H_
#define _OPTION_PARSER_H_

typedef struct {
	float ma;  /* Point Measurement Accuracy              */
	float md;  /* Mean Distance of Neighboring Points     */
	float trs; /* Step size of in/decreasing Touch Radius */
	const char *szFileName;
}FS_OPT_PARAM;

/*
* Parse Command Line Options
*
* Options>
* -a : Point Measurement Accuracy (Default: 3 mm (0.003))
*    : Type => Float
* -d : Mean Distance of Neighboring Points (Default: 10 mm (0.01))
*    : Type => Float
* -s : Step size of in/decreasing Touch Radius (Default: 10 mm (0.01))
*    : Type => Float
*/
int parseArguments(FS_OPT_PARAM *pOut, int argc, const char **argv);

#endif