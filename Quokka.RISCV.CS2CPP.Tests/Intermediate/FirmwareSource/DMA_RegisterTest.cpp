#include "DMA_RegisterTest.h"
#include <stdint.h>
#include <stdbool.h>
#include "dma.h"
namespace DMA_RegisterTestSource
{
	void Firmware::EntryPoint()
	{
		while(true)
		{
			(DMA_RegisterTestSource_DMA_Counter++);
		}
	}
}
