#ifndef LanguageConstructsTest_H
#define LanguageConstructsTest_H
#include <stdint.h>
#include <stdbool.h>
#include "soc.h"
namespace LanguageConstructsTestSource
{
	class Firmware
	{
		private: static void IfStatement(int counter);
		private: static void SwitchStatement(int counter);
		private: static void LoopStatements(int counter);
		public: static void EntryPoint();
	};
}
#endif
