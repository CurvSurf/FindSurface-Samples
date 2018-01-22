#include "glutil.h"
#include <stdlib.h>

#define _DEBUG_PRINT
#include "dbg_print.h"

void glCheckError(const char *operation)
{
	GLint err = glGetError();
	for(; err; err = glGetError()) {
		DBGP("after %s() glError( 0x%X )\n", operation, err);
	}
}

static GLuint glLoadShader(GLenum shader_type, const char *shader_source)
{
	GLuint shader = glCreateShader( shader_type );
	if( shader )
	{
		GLint compiled = 0;

		glShaderSource( shader, 1, &shader_source, NULL );
		glCompileShader( shader );
		glGetShaderiv( shader, GL_COMPILE_STATUS, &compiled );

		if( ! compiled )
		{
#ifdef _DEBUG_PRINT
			GLint info_len = 0;
			glGetShaderiv( shader, GL_INFO_LOG_LENGTH, &info_len );
			if( info_len )
			{
				char *buf = (char *)malloc(info_len);
				if( buf )
				{
					glGetShaderInfoLog( shader, info_len, NULL, buf );
					DBGP_ERR( "Could not compile shader %d:\n%s\n", shader_type, buf );
					free( buf );
				}
			}
#endif
			glDeleteShader( shader );
			shader = 0;
		}
	}
	return shader;
}

GLuint glCompileProgram(const char *szVtxSrc, const char *szFragSrc)
{
	GLuint vertex_shader   = 0;
	GLuint fragment_shader = 0;
	GLuint program         = 0;

	vertex_shader   = glLoadShader( GL_VERTEX_SHADER,   szVtxSrc  );
	fragment_shader = glLoadShader( GL_FRAGMENT_SHADER, szFragSrc );

	if( vertex_shader && fragment_shader )
	{
		program = glCreateProgram();
		if( program )
		{
			GLint link_status = GL_FALSE;

			glAttachShader( program, vertex_shader   );
			glAttachShader( program, fragment_shader );

			glLinkProgram( program );
			glGetProgramiv( program, GL_LINK_STATUS, &link_status );

			glDetachShader( program, vertex_shader   );
			glDetachShader( program, fragment_shader );

			if( link_status != GL_TRUE )
			{
#ifdef _DEBUG_PRINT
				GLint buf_length = 0;

				glGetProgramiv( program, GL_INFO_LOG_LENGTH, &buf_length );
				if( buf_length )
				{
					char *buf = (char *)malloc( buf_length );
					if( buf )
					{
						glGetProgramInfoLog( program, buf_length, NULL, buf );
						DBGP_ERR( "Could not link program:\n%s\n", buf );
						free( buf );
					}
				}
#endif
				glDeleteProgram( program );
				program = 0;
			}
		}
	}

	if( vertex_shader )   { glDeleteShader( vertex_shader );   }
	if( fragment_shader ) { glDeleteShader( fragment_shader ); }

	return program;
}

GLuint glGenVertexBuffer(GLsizeiptr size, const void *pVtxList)
{
	GLuint bid = 0;
	glGenBuffers(1, &bid);

	if (bid) {
		glBindBuffer(GL_ARRAY_BUFFER, bid);
		glBufferData(GL_ARRAY_BUFFER, size, pVtxList, GL_STATIC_DRAW);
		glBindBuffer(GL_ARRAY_BUFFER, 0);
	}

	return bid;
}

GLuint glGenIndexBuffer(GLsizeiptr size, const void *pIdxList)
{
	GLuint bid = 0;
	glGenBuffers(1, &bid);

	if (bid) {
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, bid);
		glBufferData(GL_ELEMENT_ARRAY_BUFFER, size, pIdxList, GL_STATIC_DRAW);
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
	}

	return bid;
}
