#include "DataDeclarationTest.h"
#include <stdint.h>
#include <stdbool.h>
#include "soc.h"
namespace DataDeclarationTestSource
{
	void Firmware::EntryPoint()
	{
		unsigned char b1 = 10, b2 = 20;
		int i1 = 0, i2 = 100;
		unsigned int ui1 = 10000;
		DataDeclarationTestSource_SOC_Result = (int)(((((b1 + b2)) * ((i1 - i2))) + ui1));
	}
}
