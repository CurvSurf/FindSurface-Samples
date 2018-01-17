#include "floatMatrix.h"
#include <math.h>

static const FLOAT_MATRIX _IMAT = { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 };

FLOAT_MATRIX *MatrixIdentity( FLOAT_MATRIX *pOut )
{
	*pOut = _IMAT;
	return pOut;
}

FLOAT_MATRIX *MatrixTranspose( FLOAT_MATRIX *pOut, const FLOAT_MATRIX *pM1 )
{
	FLOAT_MATRIX tmp = {
		pM1->arr[0], pM1->arr[4], pM1->arr[8],  pM1->arr[12], 
		pM1->arr[1], pM1->arr[5], pM1->arr[9],  pM1->arr[13],
		pM1->arr[2], pM1->arr[6], pM1->arr[10], pM1->arr[14],
		pM1->arr[3], pM1->arr[7], pM1->arr[11], pM1->arr[15]
	};

	*pOut = tmp;
	return pOut;
}

FLOAT_MATRIX *MatrixAdd( FLOAT_MATRIX *pOut, const FLOAT_MATRIX *pM1, const FLOAT_MATRIX *pM2)
{
	pOut->arr[0]  = pM1->arr[0]  + pM2->arr[0];
	pOut->arr[1]  = pM1->arr[1]  + pM2->arr[1];
	pOut->arr[2]  = pM1->arr[2]  + pM2->arr[2];
	pOut->arr[3]  = pM1->arr[3]  + pM2->arr[3];
	pOut->arr[4]  = pM1->arr[4]  + pM2->arr[4];
	pOut->arr[5]  = pM1->arr[5]  + pM2->arr[5];
	pOut->arr[6]  = pM1->arr[6]  + pM2->arr[6];
	pOut->arr[7]  = pM1->arr[7]  + pM2->arr[7];
	pOut->arr[8]  = pM1->arr[8]  + pM2->arr[8];
	pOut->arr[9]  = pM1->arr[9]  + pM2->arr[9];
	pOut->arr[10] = pM1->arr[10] + pM2->arr[10];
	pOut->arr[11] = pM1->arr[11] + pM2->arr[11];
	pOut->arr[12] = pM1->arr[12] + pM2->arr[12];
	pOut->arr[13] = pM1->arr[13] + pM2->arr[13];
	pOut->arr[14] = pM1->arr[14] + pM2->arr[14];
	pOut->arr[15] = pM1->arr[15] + pM2->arr[15];

	return pOut;
}

FLOAT_MATRIX *MatrixSub( FLOAT_MATRIX *pOut, const FLOAT_MATRIX *pM1, const FLOAT_MATRIX *pM2)
{
	pOut->arr[0]  = pM1->arr[0]  - pM2->arr[0];
	pOut->arr[1]  = pM1->arr[1]  - pM2->arr[1];
	pOut->arr[2]  = pM1->arr[2]  - pM2->arr[2];
	pOut->arr[3]  = pM1->arr[3]  - pM2->arr[3];
	pOut->arr[4]  = pM1->arr[4]  - pM2->arr[4];
	pOut->arr[5]  = pM1->arr[5]  - pM2->arr[5];
	pOut->arr[6]  = pM1->arr[6]  - pM2->arr[6];
	pOut->arr[7]  = pM1->arr[7]  - pM2->arr[7];
	pOut->arr[8]  = pM1->arr[8]  - pM2->arr[8];
	pOut->arr[9]  = pM1->arr[9]  - pM2->arr[9];
	pOut->arr[10] = pM1->arr[10] - pM2->arr[10];
	pOut->arr[11] = pM1->arr[11] - pM2->arr[11];
	pOut->arr[12] = pM1->arr[12] - pM2->arr[12];
	pOut->arr[13] = pM1->arr[13] - pM2->arr[13];
	pOut->arr[14] = pM1->arr[14] - pM2->arr[14];
	pOut->arr[15] = pM1->arr[15] - pM2->arr[15];

	return pOut;
}

FLOAT_MATRIX *MatrixMulFloat( FLOAT_MATRIX *pOut, const FLOAT_MATRIX *pM1, float fVal )
{
	pOut->arr[0]  = pM1->arr[0]  * fVal;
	pOut->arr[1]  = pM1->arr[1]  * fVal;
	pOut->arr[2]  = pM1->arr[2]  * fVal;
	pOut->arr[3]  = pM1->arr[3]  * fVal;
	pOut->arr[4]  = pM1->arr[4]  * fVal;
	pOut->arr[5]  = pM1->arr[5]  * fVal;
	pOut->arr[6]  = pM1->arr[6]  * fVal;
	pOut->arr[7]  = pM1->arr[7]  * fVal;
	pOut->arr[8]  = pM1->arr[8]  * fVal;
	pOut->arr[9]  = pM1->arr[9]  * fVal;
	pOut->arr[10] = pM1->arr[10] * fVal;
	pOut->arr[11] = pM1->arr[11] * fVal;
	pOut->arr[12] = pM1->arr[12] * fVal;
	pOut->arr[13] = pM1->arr[13] * fVal;
	pOut->arr[14] = pM1->arr[14] * fVal;
	pOut->arr[15] = pM1->arr[15] * fVal;

	return pOut;
}

FLOAT_MATRIX *MatrixDivFloat( FLOAT_MATRIX *pOut, const FLOAT_MATRIX *pM1, float fVal )
{
	pOut->arr[0]  = pM1->arr[0]  / fVal;
	pOut->arr[1]  = pM1->arr[1]  / fVal;
	pOut->arr[2]  = pM1->arr[2]  / fVal;
	pOut->arr[3]  = pM1->arr[3]  / fVal;
	pOut->arr[4]  = pM1->arr[4]  / fVal;
	pOut->arr[5]  = pM1->arr[5]  / fVal;
	pOut->arr[6]  = pM1->arr[6]  / fVal;
	pOut->arr[7]  = pM1->arr[7]  / fVal;
	pOut->arr[8]  = pM1->arr[8]  / fVal;
	pOut->arr[9]  = pM1->arr[9]  / fVal;
	pOut->arr[10] = pM1->arr[10] / fVal;
	pOut->arr[11] = pM1->arr[11] / fVal;
	pOut->arr[12] = pM1->arr[12] / fVal;
	pOut->arr[13] = pM1->arr[13] / fVal;
	pOut->arr[14] = pM1->arr[14] / fVal;
	pOut->arr[15] = pM1->arr[15] / fVal;

	return pOut;
}

FLOAT_MATRIX *MatrixMulGL( FLOAT_MATRIX *pOut, const FLOAT_MATRIX *pM1, const FLOAT_MATRIX *pM2 )
{
	FLOAT_MATRIX tmp;

	tmp.glm._11 = pM1->glm._11 * pM2->glm._11 + pM1->glm._12 * pM2->glm._21 + pM1->glm._13 * pM2->glm._31 + pM1->glm._14 * pM2->glm._41;
	tmp.glm._12 = pM1->glm._11 * pM2->glm._12 + pM1->glm._12 * pM2->glm._22 + pM1->glm._13 * pM2->glm._32 + pM1->glm._14 * pM2->glm._42;
	tmp.glm._13 = pM1->glm._11 * pM2->glm._13 + pM1->glm._12 * pM2->glm._23 + pM1->glm._13 * pM2->glm._33 + pM1->glm._14 * pM2->glm._43;
	tmp.glm._14 = pM1->glm._11 * pM2->glm._14 + pM1->glm._12 * pM2->glm._24 + pM1->glm._13 * pM2->glm._34 + pM1->glm._14 * pM2->glm._44;

	tmp.glm._21 = pM1->glm._21 * pM2->glm._11 + pM1->glm._22 * pM2->glm._21 + pM1->glm._23 * pM2->glm._31 + pM1->glm._24 * pM2->glm._41;
	tmp.glm._22 = pM1->glm._21 * pM2->glm._12 + pM1->glm._22 * pM2->glm._22 + pM1->glm._23 * pM2->glm._32 + pM1->glm._24 * pM2->glm._42;
	tmp.glm._23 = pM1->glm._21 * pM2->glm._13 + pM1->glm._22 * pM2->glm._23 + pM1->glm._23 * pM2->glm._33 + pM1->glm._24 * pM2->glm._43;
	tmp.glm._24 = pM1->glm._21 * pM2->glm._14 + pM1->glm._22 * pM2->glm._24 + pM1->glm._23 * pM2->glm._34 + pM1->glm._24 * pM2->glm._44;

	tmp.glm._31 = pM1->glm._31 * pM2->glm._11 + pM1->glm._32 * pM2->glm._21 + pM1->glm._33 * pM2->glm._31 + pM1->glm._34 * pM2->glm._41;
	tmp.glm._32 = pM1->glm._31 * pM2->glm._12 + pM1->glm._32 * pM2->glm._22 + pM1->glm._33 * pM2->glm._32 + pM1->glm._34 * pM2->glm._42;
	tmp.glm._33 = pM1->glm._31 * pM2->glm._13 + pM1->glm._32 * pM2->glm._23 + pM1->glm._33 * pM2->glm._33 + pM1->glm._34 * pM2->glm._43;
	tmp.glm._34 = pM1->glm._31 * pM2->glm._14 + pM1->glm._32 * pM2->glm._24 + pM1->glm._33 * pM2->glm._34 + pM1->glm._34 * pM2->glm._44;

	tmp.glm._41 = pM1->glm._41 * pM2->glm._11 + pM1->glm._42 * pM2->glm._21 + pM1->glm._43 * pM2->glm._31 + pM1->glm._44 * pM2->glm._41;
	tmp.glm._42 = pM1->glm._41 * pM2->glm._12 + pM1->glm._42 * pM2->glm._22 + pM1->glm._43 * pM2->glm._32 + pM1->glm._44 * pM2->glm._42;
	tmp.glm._43 = pM1->glm._41 * pM2->glm._13 + pM1->glm._42 * pM2->glm._23 + pM1->glm._43 * pM2->glm._33 + pM1->glm._44 * pM2->glm._43;
	tmp.glm._44 = pM1->glm._41 * pM2->glm._14 + pM1->glm._42 * pM2->glm._24 + pM1->glm._43 * pM2->glm._34 + pM1->glm._44 * pM2->glm._44;

	*pOut = tmp;

	return pOut;
}

FLOAT_MATRIX *MatrixMulDX( FLOAT_MATRIX *pOut, const FLOAT_MATRIX *pM1, const FLOAT_MATRIX *pM2 )
{
	FLOAT_MATRIX tmp;

	tmp.dxm._11 = pM1->dxm._11 * pM2->dxm._11 + pM1->dxm._12 * pM2->dxm._21 + pM1->dxm._13 * pM2->dxm._31 + pM1->dxm._14 * pM2->dxm._41;
	tmp.dxm._12 = pM1->dxm._11 * pM2->dxm._12 + pM1->dxm._12 * pM2->dxm._22 + pM1->dxm._13 * pM2->dxm._32 + pM1->dxm._14 * pM2->dxm._42;
	tmp.dxm._13 = pM1->dxm._11 * pM2->dxm._13 + pM1->dxm._12 * pM2->dxm._23 + pM1->dxm._13 * pM2->dxm._33 + pM1->dxm._14 * pM2->dxm._43;
	tmp.dxm._14 = pM1->dxm._11 * pM2->dxm._14 + pM1->dxm._12 * pM2->dxm._24 + pM1->dxm._13 * pM2->dxm._34 + pM1->dxm._14 * pM2->dxm._44;

	tmp.dxm._21 = pM1->dxm._21 * pM2->dxm._11 + pM1->dxm._22 * pM2->dxm._21 + pM1->dxm._23 * pM2->dxm._31 + pM1->dxm._24 * pM2->dxm._41;
	tmp.dxm._22 = pM1->dxm._21 * pM2->dxm._12 + pM1->dxm._22 * pM2->dxm._22 + pM1->dxm._23 * pM2->dxm._32 + pM1->dxm._24 * pM2->dxm._42;
	tmp.dxm._23 = pM1->dxm._21 * pM2->dxm._13 + pM1->dxm._22 * pM2->dxm._23 + pM1->dxm._23 * pM2->dxm._33 + pM1->dxm._24 * pM2->dxm._43;
	tmp.dxm._24 = pM1->dxm._21 * pM2->dxm._14 + pM1->dxm._22 * pM2->dxm._24 + pM1->dxm._23 * pM2->dxm._34 + pM1->dxm._24 * pM2->dxm._44;

	tmp.dxm._31 = pM1->dxm._31 * pM2->dxm._11 + pM1->dxm._32 * pM2->dxm._21 + pM1->dxm._33 * pM2->dxm._31 + pM1->dxm._34 * pM2->dxm._41;
	tmp.dxm._32 = pM1->dxm._31 * pM2->dxm._12 + pM1->dxm._32 * pM2->dxm._22 + pM1->dxm._33 * pM2->dxm._32 + pM1->dxm._34 * pM2->dxm._42;
	tmp.dxm._33 = pM1->dxm._31 * pM2->dxm._13 + pM1->dxm._32 * pM2->dxm._23 + pM1->dxm._33 * pM2->dxm._33 + pM1->dxm._34 * pM2->dxm._43;
	tmp.dxm._34 = pM1->dxm._31 * pM2->dxm._14 + pM1->dxm._32 * pM2->dxm._24 + pM1->dxm._33 * pM2->dxm._34 + pM1->dxm._34 * pM2->dxm._44;

	tmp.dxm._41 = pM1->dxm._41 * pM2->dxm._11 + pM1->dxm._42 * pM2->dxm._21 + pM1->dxm._43 * pM2->dxm._31 + pM1->dxm._44 * pM2->dxm._41;
	tmp.dxm._42 = pM1->dxm._41 * pM2->dxm._12 + pM1->dxm._42 * pM2->dxm._22 + pM1->dxm._43 * pM2->dxm._32 + pM1->dxm._44 * pM2->dxm._42;
	tmp.dxm._43 = pM1->dxm._41 * pM2->dxm._13 + pM1->dxm._42 * pM2->dxm._23 + pM1->dxm._43 * pM2->dxm._33 + pM1->dxm._44 * pM2->dxm._43;
	tmp.dxm._44 = pM1->dxm._41 * pM2->dxm._14 + pM1->dxm._42 * pM2->dxm._24 + pM1->dxm._43 * pM2->dxm._34 + pM1->dxm._44 * pM2->dxm._44;

	*pOut = tmp;

	return pOut;
}

FLOAT_MATRIX *MatrixLookAtRH( FLOAT_MATRIX *pOut,
                              float pos_x, float pos_y, float pos_z,
                              float at_x,  float at_y,  float at_z,
                              float up_x,  float up_y,  float up_z )
{
	float _x[3] = { 0, };
	float _y[3] = { 0, };
	float _z[3] = { pos_x - at_x, pos_y - at_y, pos_z - at_z };

	float _len_z = 0, _len_x = 0;

	/* Normalize Z */
	_len_z = sqrtf( _z[0] * _z[0] + _z[1] * _z[1] + _z[2] * _z[2] );
	_z[0] /= _len_z;
	_z[1] /= _len_z;
	_z[2] /= _len_z;

	/* Cross ( up , z ) */
	_x[0] = up_y * _z[2] - up_z * _z[1];
	_x[1] = up_z * _z[0] - up_x * _z[2];
	_x[2] = up_x * _z[1] - up_y * _z[0];

	/* Normalize X */
	_len_x = sqrtf( _x[0] * _x[0] + _x[1] * _x[1] + _x[2] * _x[2] );
	_x[0] /= _len_x;
	_x[1] /= _len_x;
	_x[2] /= _len_x;

	/* Cross ( z, x ) */
	_y[0] = _z[1] * _x[2] - _z[2] * _x[1];
	_y[1] = _z[2] * _x[0] - _z[0] * _x[2];
	_y[2] = _z[0] * _x[1] - _z[1] * _x[0];

	pOut->arr[0]  = _x[0];
	pOut->arr[1]  = _y[0];
	pOut->arr[2]  = _z[0];
	pOut->arr[3]  = 0.0f;

	pOut->arr[4]  = _x[1];
	pOut->arr[5]  = _y[1];
	pOut->arr[6]  = _z[1];
	pOut->arr[7]  = 0.0f;

	pOut->arr[8]  = _x[2];
	pOut->arr[9]  = _y[2];
	pOut->arr[10] = _z[2];
	pOut->arr[11] = 0.0f;

	pOut->arr[12] = -(_x[0] * pos_x + _x[1] * pos_y + _x[2] * pos_z); /* Dot (x, pos) */
	pOut->arr[13] = -(_y[0] * pos_x + _y[1] * pos_y + _y[2] * pos_z); /* Dot (y, pos) */
	pOut->arr[14] = -(_z[0] * pos_x + _z[1] * pos_y + _z[2] * pos_z); /* Dot (z, pos) */
	pOut->arr[15] = 1.0f;

	return pOut;
}

FLOAT_MATRIX *MatrixPerspectiveFovRH( FLOAT_MATRIX *pOut, float fovY, float aspect, float near, float far )
{
	float h;
	if( fovY < 0.001f || near >= far ) { return (FLOAT_MATRIX *)0; }

	h = 1.0f / tanf( fovY / 2.0f );

	pOut->arr[0]  = h / aspect;
	pOut->arr[1]  = 0.0f;
	pOut->arr[2]  = 0.0f;
	pOut->arr[3]  = 0.0f;

	pOut->arr[4]  = 0.0f;
	pOut->arr[5]  = h;
	pOut->arr[6]  = 0.0f;
	pOut->arr[7]  = 0.0f;

	pOut->arr[8]  = 0.0f;
	pOut->arr[9]  = 0.0f;
	pOut->arr[10] = ( near + far ) / ( near - far ); /* D3D = far / ( near - far ) */
	pOut->arr[11] = -1.0f;

	pOut->arr[12] = 0.0f;
	pOut->arr[13] = 0.0f;
	pOut->arr[14] = ( 2.0f * near * far ) / ( near - far ); /* D3D = ( near * far ) / ( near - far ) */
	pOut->arr[15] = 0.0f;

	return pOut;
}

FLOAT_MATRIX *MatrixTranslation( FLOAT_MATRIX *pOut, float x, float y, float z )
{
	*pOut = _IMAT;
	pOut->arr[12] = x;
	pOut->arr[13] = y;
	pOut->arr[14] = z;

	return pOut;
}

FLOAT_MATRIX *MatrixRotationAxis( FLOAT_MATRIX *pOut, float axis_x, float axis_y, float axis_z, float angle )
{
	float s = sinf( angle );
	float c = cosf( angle );
	float d = 1.0f - c;

	/* Normalize Axis */
	float _len = sqrtf( axis_x * axis_x + axis_y * axis_y + axis_z * axis_z );
	if( _len < 0.001f ) { return (FLOAT_MATRIX *)0; } /* Divide by Zero */

	axis_x /= _len;
	axis_y /= _len;
	axis_z /= _len;

	pOut->arr[0]  = d * axis_x * axis_x + c;
	pOut->arr[1]  = d * axis_y * axis_x + axis_z * s;
	pOut->arr[2]  = d * axis_z * axis_x - axis_y * s;
	pOut->arr[3]  = 0.0f;

	pOut->arr[4]  = d * axis_x * axis_y - axis_z * s;
	pOut->arr[5]  = d * axis_y * axis_y + c;
	pOut->arr[6]  = d * axis_z * axis_y + axis_x *s;
	pOut->arr[7]  = 0.0f;

	pOut->arr[8]  = d * axis_x * axis_z + axis_y * s;
	pOut->arr[9]  = d * axis_y * axis_z - axis_x * s;
	pOut->arr[10] = d * axis_z * axis_z + c;
	pOut->arr[11] = 0.0f;

	pOut->arr[12] = 0.0f;
	pOut->arr[13] = 0.0f;
	pOut->arr[14] = 0.0f;
	pOut->arr[15] = 1.0f;

	return pOut;

}

FLOAT_MATRIX *MatrixRotationX( FLOAT_MATRIX *pOut, float angle )
{
	*pOut = _IMAT;
	pOut->arr[5] = pOut->arr[10] = cosf( angle );
	pOut->arr[6] = sinf( angle );
	pOut->arr[9] = -(pOut->arr[6]);

	return pOut;
}

FLOAT_MATRIX *MatrixRotationY( FLOAT_MATRIX *pOut, float angle )
{
	*pOut = _IMAT;
	pOut->arr[0] = pOut->arr[10] = cosf( angle );
	pOut->arr[2] = -sinf( angle );
	pOut->arr[8] = -(pOut->arr[2]);

	return pOut;
}

FLOAT_MATRIX *MatrixRotationZ( FLOAT_MATRIX *pOut, float angle )
{
	*pOut = _IMAT;
	pOut->arr[0] = pOut->arr[5] = cosf( angle );
	pOut->arr[1] = sinf( angle );
	pOut->arr[4] = -(pOut->arr[1]);

	return pOut;
}

FLOAT_MATRIX *MatrixScaling( FLOAT_MATRIX *pOut, float sx, float sy, float sz )
{
	*pOut = _IMAT;
	pOut->arr[0]  = sx;
	pOut->arr[5]  = sy;
	pOut->arr[10] = sz;

	return pOut;
}