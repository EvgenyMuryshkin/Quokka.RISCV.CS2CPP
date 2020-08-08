#include "ArrayDeclarationTest.h"
#include <stdint.h>
#include <stdbool.h>
#include "soc.h"
namespace ArrayDeclarationTestSource
{
	unsigned char Firmware::U8Result = 255;
	unsigned char Firmware::U8Buff[16] = {0};
	char Firmware::S8Result = -128;
	char Firmware::S8Buff[16] = {0};
	wchar_t Firmware::C16Result = '￿';
	wchar_t Firmware::C16Buff[16] = {0};
	unsigned short Firmware::U16Result = 42;
	unsigned short Firmware::U16Buff[16] = {0};
	short Firmware::S16Result = (-42);
	short Firmware::S16Buff[16] = {0};
	unsigned int Firmware::U32Result = 0;
	unsigned int Firmware::U32Buff[16] = {0};
	int Firmware::S32Result = 0;
	int Firmware::S32Buff[16] = {0};
	void Firmware::InitData()
	{
		for(int i = 0; (i < 16); (i++))
		{
			int value = (((-40000) + (5000 * i)) + i);
			U8Buff[i] = (unsigned char)value;
			S8Buff[i] = (char)value;
			C16Buff[i] = (wchar_t)value;
			U16Buff[i] = (unsigned short)value;
			S16Buff[i] = (short)value;
			U32Buff[i] = (unsigned int)value;
			S32Buff[i] = value;
		}
	}
	void Firmware::CalculateResult()
	{
		for(int i = 0; (i < 16); (i++))
		{
			U8Result += U8Buff[i];
			S8Result += S8Buff[i];
			C16Result += C16Buff[i];
			U16Result += U16Buff[i];
			S16Result += S16Buff[i];
			U32Result += U32Buff[i];
			S32Result += S32Buff[i];
		}
	}
	void Firmware::SetResult()
	{
		ArrayDeclarationTestSource_SOC_U8Result = U8Result;
		ArrayDeclarationTestSource_SOC_S8Result = S8Result;
		ArrayDeclarationTestSource_SOC_C16Result = C16Result;
		ArrayDeclarationTestSource_SOC_U16Result = U16Result;
		ArrayDeclarationTestSource_SOC_S16Result = S16Result;
		ArrayDeclarationTestSource_SOC_U32Result = U32Result;
		ArrayDeclarationTestSource_SOC_S32Result = S32Result;
	}
	void Firmware::EntryPoint()
	{
		InitData();
		CalculateResult();
		SetResult();
	}
}
