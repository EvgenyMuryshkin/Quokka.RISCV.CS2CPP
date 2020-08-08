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

	unsigned int Firmware::result = 0;
	unsigned int Firmware::buff[10] = {0};
}
