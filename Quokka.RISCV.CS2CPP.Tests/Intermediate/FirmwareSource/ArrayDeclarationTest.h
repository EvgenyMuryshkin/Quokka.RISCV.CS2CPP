#ifndef ArrayDeclarationTest_H
#define ArrayDeclarationTest_H
#include <stdint.h>
#include <stdbool.h>
#include "soc.h"
namespace ArrayDeclarationTestSource
{
	class Firmware
	{
		private: static unsigned char U8Result;
		private: static unsigned char U8Buff[];
		private: static char S8Result;
		private: static char S8Buff[];
		private: static wchar_t C16Result;
		private: static wchar_t C16Buff[];
		private: static unsigned short U16Result;
		private: static unsigned short U16Buff[];
		private: static short S16Result;
		private: static short S16Buff[];
		private: static unsigned int U32Result;
		private: static unsigned int U32Buff[];
		private: static int S32Result;
		private: static int S32Buff[];
		private: static void InitData();
		private: static void CalculateResult();
		private: static void SetResult();
		public: static void EntryPoint();
	};
}
#endif
