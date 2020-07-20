#include "SOCBlinker.h"
#include <stdint.h>
#include <stdbool.h>
#include "soc.h"
namespace SOCBlinker
{
	void Firmware::EntryPoint()
	{
		unsigned int counter = 0;
		while(true)
		{
			(counter++);
			SOCBlinker_SOC_Blinker = (unsigned char)((counter >> 24));
		}
	}
}
