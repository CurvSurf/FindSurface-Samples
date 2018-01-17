#ifndef _FLOAT_MATRIX_H_
#define _FLOAT_MATRIX_H_

#define _PI_ 3.14159265359f
#define _TO_RADIAN(deg) (deg*_PI_/180.0f)
#define _TO_DEGREE(rad) (rad*180.0f/_PI_)

typedef struct
{
	union
	{
		struct {
			float _11, _21, _31, _41;
			float _12, _22, _32, _42;
			float _13, _23, _33, _43;
			float _14, _24, _34, _44;	
		} glm;
		struct {
			float _11, _12, _13, _14;
			float _21, _22, _23, _24;
			float _31, _32, _33, _34;
			float _41, _42, _43, _44;
		} dxm;
		float arr[16];
	};
} FLOAT_MATRIX;

/* Basic Operations */
FLOAT_MATRIX *MatrixIdentity( FLOAT_MATRIX *pOut );
FLOAT_MATRIX *MatrixTranspose( FLOAT_MATRIX *pOut, const FLOAT_MATRIX *pM1 );

FLOAT_MATRIX *MatrixAdd( FLOAT_MATRIX *pOut, const FLOAT_MATRIX *pM1, const FLOAT_MATRIX *pM2);
FLOAT_MATRIX *MatrixSub( FLOAT_MATRIX *pOut, const FLOAT_MATRIX *pM1, const FLOAT_MATRIX *pM2);
FLOAT_MATRIX *MatrixMulFloat( FLOAT_MATRIX *pOut, const FLOAT_MATRIX *pM1, float fVal );
FLOAT_MATRIX *MatrixDivFloat( FLOAT_MATRIX *pOut, const FLOAT_MATRIX *pM1, float fVal );

FLOAT_MATRIX *MatrixMulGL( FLOAT_MATRIX *pOut, const FLOAT_MATRIX *pM1, const FLOAT_MATRIX *pM2 );
FLOAT_MATRIX *MatrixMulDX( FLOAT_MATRIX *pOut, const FLOAT_MATRIX *pM1, const FLOAT_MATRIX *pM2 );

/* View & Projection Matrix */
FLOAT_MATRIX *MatrixLookAtRH( FLOAT_MATRIX *pOut,
                              float pos_x, float pos_y, float pos_z,
                              float at_x,  float at_y,  float at_z,
                              float up_x,  float up_y,  float up_z );

FLOAT_MATRIX *MatrixPerspectiveFovRH( FLOAT_MATRIX *pOut, float fovY, float aspect, float near, float far );

/* Transform Matrix */
FLOAT_MATRIX *MatrixTranslation( FLOAT_MATRIX *pOut, float x, float y, float z );
FLOAT_MATRIX *MatrixRotationAxis( FLOAT_MATRIX *pOut, float axis_x, float axis_y, float axis_z, float angle );
FLOAT_MATRIX *MatrixRotationX( FLOAT_MATRIX *pOut, float angle );
FLOAT_MATRIX *MatrixRotationY( FLOAT_MATRIX *pOut, float angle );
FLOAT_MATRIX *MatrixRotationZ( FLOAT_MATRIX *pOut, float angle );
FLOAT_MATRIX *MatrixScaling( FLOAT_MATRIX *pOut, float sx, float sy, float sz );

#endif