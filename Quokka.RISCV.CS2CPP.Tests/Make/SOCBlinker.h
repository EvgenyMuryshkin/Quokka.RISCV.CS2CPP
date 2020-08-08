#ifndef SOCBlinker_H
#define SOCBlinker_H
#include <stdint.h>
#include <stdbool.h>
#include "soc.h"
namespace SOCBlinker
{
	class Firmware
	{
		public: static unsigned int result;
		public: static unsigned int buff[];
		public: static void EntryPoint();
	};
}
#endif
