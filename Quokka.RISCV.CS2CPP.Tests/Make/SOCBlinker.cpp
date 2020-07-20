#include "SOCBlinker.h"
#include <stdint.h>
#include <stdbool.h>
#include "soc.h"
namespace SOCBlinker
{
	void Firmware::EntryPoint()
	{
		while(true)
		{
			unsigned int counter = SOCBlinker_SOC_Counter;
			(counter++);
			SOCBlinker_SOC_Counter = counter;
		}
	}
}
