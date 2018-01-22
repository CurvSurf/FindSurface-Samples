#ifndef _KEY_CODE_H_
#define _KEY_CODE_H_

/* Common Key Code */

#define _KEY_SPACE_ 0x20

#define _KEY_0_ 0x30
#define _KEY_1_ 0x31
#define _KEY_2_ 0x32
#define _KEY_3_ 0x33
#define _KEY_4_ 0x34
#define _KEY_5_ 0x35
#define _KEY_6_ 0x36
#define _KEY_7_ 0x37
#define _KEY_8_ 0x38
#define _KEY_9_ 0x39

#if defined(_WIN32) || defined(_WIN64)
/* 
 * Win32 Virtaul Key Code 
 * https://msdn.microsoft.com/ko-kr/library/windows/desktop/dd375731(v=vs.85).aspx
 */

#define _KEY_A_ 0x41
#define _KEY_B_ 0x42
#define _KEY_C_ 0x43
#define _KEY_D_ 0x44
#define _KEY_E_ 0x45
#define _KEY_F_ 0x46
#define _KEY_G_ 0x47
#define _KEY_H_ 0x48
#define _KEY_I_ 0x49
#define _KEY_J_ 0x4A
#define _KEY_K_ 0x4B
#define _KEY_L_ 0x4C
#define _KEY_M_ 0x4D
#define _KEY_N_ 0x4E
#define _KEY_O_ 0x4F
#define _KEY_P_ 0x50
#define _KEY_Q_ 0x51
#define _KEY_R_ 0x52
#define _KEY_S_ 0x53
#define _KEY_T_ 0x54
#define _KEY_U_ 0x55
#define _KEY_V_ 0x56
#define _KEY_W_ 0x57
#define _KEY_X_ 0x58
#define _KEY_Y_ 0x59
#define _KEY_Z_ 0x5A

#define _KEY_BACK_         0x08
#define _KEY_TAB_          0x09
#define _KEY_ENTER_        0x0D
#define _KEY_ESC_          0x1B

#define _KEY_LEFT_         0x25
#define _KEY_UP_           0x26
#define _KEY_RIGHT_        0x27
#define _KEY_DOWN_         0x28

#define _KEY_PGUP_         0x21
#define _KEY_PGDOWN_       0x22
#define _KEY_END_          0x23
#define _KEY_HOME_         0x24
#define _KEY_INS_          0x2D
#define _KEY_DEL_          0x2E

#define _KEY_F1_           0x70
#define _KEY_F2_           0x71
#define _KEY_F3_	       0x72
#define _KEY_F4_	       0x73
#define _KEY_F5_	       0x74
#define _KEY_F6_	       0x75
#define _KEY_F7_	       0x76
#define _KEY_F8_	       0x77
#define _KEY_F9_	       0x78
#define _KEY_F10_	       0x79
#define _KEY_F11_	       0x7A
#define _KEY_F12_	       0x7B

#define _KEY_GRAVE_        0xC0  /* For the US standard keyboard, the '`~' key */
#define _KEY_MINUS_        0xBD
#define _KEY_EQUAL_	       0xBB
#define _KEY_PLUS_	       0xBB
#define _KEY_SEMICOLON_	   0xBA
#define _KEY_QUOTE_	       0xDE
#define _KEY_BARKETLEFT_   0xDB
#define _KEY_BARKETRIGHT_  0xDD
#define _KEY_BACKSLASH_	   0xDC
#define _KEY_COMMA_	       0xBC
#define _KEY_PERIOD_       0xBE
#define _KEY_SLASH_        0xBF

/* Win32 Mouse Key */
#define _KEY_LBTN_         0x01
#define _KEY_RBTN_         0x02
#define _KEY_MBTN_         0x04

/* Win32 Key State */
#define _KS_CTRL_          0x08
#define _KS_SHIFT_         0x04
#define _KS_LBTN_          0x01
#define _KS_RBTN_          0x02
#define _KS_MBTN_          0x10

#else
/*
 * X Windows Key Code
 * #include <X11/keysymdef.h>
 */
#define _KEY_A_ 0x61
#define _KEY_B_ 0x62
#define _KEY_C_ 0x63
#define _KEY_D_ 0x64
#define _KEY_E_ 0x65
#define _KEY_F_ 0x66
#define _KEY_G_ 0x67
#define _KEY_H_ 0x68
#define _KEY_I_ 0x69
#define _KEY_J_ 0x6A
#define _KEY_K_ 0x6B
#define _KEY_L_ 0x6C
#define _KEY_M_ 0x6D
#define _KEY_N_ 0x6E
#define _KEY_O_ 0x6F
#define _KEY_P_ 0x70
#define _KEY_Q_ 0x71
#define _KEY_R_ 0x72
#define _KEY_S_ 0x73
#define _KEY_T_ 0x74
#define _KEY_U_ 0x75
#define _KEY_V_ 0x76
#define _KEY_W_ 0x77
#define _KEY_X_ 0x78
#define _KEY_Y_ 0x79
#define _KEY_Z_ 0x7A

#define _KEY_BACK_         0xFF08
#define _KEY_TAB_          0xFF09
#define _KEY_ENTER_        0xFF0D
#define _KEY_ESC_          0xFF1B

#define _KEY_LEFT_         0xFF51
#define _KEY_UP_           0xFF52
#define _KEY_RIGHT_        0xFF53
#define _KEY_DOWN_         0xFF54

#define _KEY_PGUP_         0xFF55
#define _KEY_PGDOWN_       0xFF56
#define _KEY_END_          0xFF57
#define _KEY_HOME_         0xFF50
#define _KEY_INS_          0xFF63
#define _KEY_DEL_          0xFFFF

#define _KEY_F1_           0xFFBE
#define _KEY_F2_           0xFFBF
#define _KEY_F3_	       0xFFC0
#define _KEY_F4_	       0xFFC1
#define _KEY_F5_	       0xFFC2
#define _KEY_F6_	       0xFFC3
#define _KEY_F7_	       0xFFC4
#define _KEY_F8_	       0xFFC5
#define _KEY_F9_	       0xFFC6
#define _KEY_F10_	       0xFFC7
#define _KEY_F11_	       0xFFC8
#define _KEY_F12_	       0xFFC9

#define _KEY_GRAVE_        0x0060 /* For the US standard keyboard, the '`~' key */
#define _KEY_MINUS_        0x002D
#define _KEY_EQUAL_        0x003D
#define _KEY_PLUS_         0x003D
#define _KEY_SEMICOLON_	   0x002F
#define _KEY_QUOTE_	       0x0027
#define _KEY_BARKETLEFT_   0x005B
#define _KEY_BARKETRIGHT_  0x005D
#define _KEY_BACKSLASH_	   0x005C
#define _KEY_COMMA_	       0x002C
#define _KEY_PERIOD_       0x002E
#define _KEY_SLASH_        0x002F

 /* X Window Mouse Key */
#define _KEY_LBTN_         0x0001
#define _KEY_RBTN_         0x0003
#define _KEY_MBTN_         0x0002

 /* X Window Key State */
#define _KS_CTRL_          0x0004
#define _KS_SHIFT_         0x0001
#define _KS_LBTN_          0x0100
#define _KS_RBTN_          0x0300
#define _KS_MBTN_          0x0200

#endif

#endif
