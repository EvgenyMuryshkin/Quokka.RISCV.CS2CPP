#include "LanguageConstructsTest.h"
#include <stdint.h>
#include <stdbool.h>
#include "soc.h"
namespace LanguageConstructsTestSource
{
	void Firmware::IfStatement(int counter)
	{
		if ((counter < 5))
		{
			LanguageConstructsTestSource_SOC_Value = 0;
		}
		else
		{
			if ((counter < 8))
			{
				LanguageConstructsTestSource_SOC_Value = 1;
			}
			else
			{
				LanguageConstructsTestSource_SOC_Value = (counter + 10);
			}
		}
	}
	void Firmware::SwitchStatement(int counter)
	{
		switch (counter)
		{
			case 0:
			{
				LanguageConstructsTestSource_SOC_Value = 0;
				break;
			}
			case 1:
			{
				LanguageConstructsTestSource_SOC_Value = 0;
				break;
			}
			case 2:
			case 3:
			{
				LanguageConstructsTestSource_SOC_Value = 2;
				break;
			}
			default:
			{
				LanguageConstructsTestSource_SOC_Value = (counter * 3);
				break;
			}
		}
	}
	void Firmware::LoopStatements(int counter)
	{
		for(int i = 0; (i < counter); (++i))
		{
			LanguageConstructsTestSource_SOC_Value *= i;
		}
		for(int i = counter; (i >= 0); (i--))
		{
			LanguageConstructsTestSource_SOC_Value /= i;
		}
		while(((counter--) >= 0))
		{
			LanguageConstructsTestSource_SOC_Value += counter;
		}
		do
		{
			LanguageConstructsTestSource_SOC_Value += counter;
		}
		while(((counter++) < 10));
	}
	void Firmware::EntryPoint()
	{
		int counter = 0;
		while((counter < 10))
		{
			IfStatement(counter);
			SwitchStatement(counter);
			LoopStatements(counter);
		}
	}
}
