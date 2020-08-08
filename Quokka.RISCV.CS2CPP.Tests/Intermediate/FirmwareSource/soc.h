#define firmware ((volatile unsigned int*)0x00000000)
#define firmware_size 512
#define ArrayDeclarationTestSource_SOC_U8Result (*(volatile unsigned char*)0x80000000)
#define ArrayDeclarationTestSource_SOC_S8Result (*(volatile char*)0x80100000)
#define ArrayDeclarationTestSource_SOC_C16Result (*(volatile wchar_t*)0x80200000)
#define ArrayDeclarationTestSource_SOC_U16Result (*(volatile unsigned short*)0x80300000)
#define ArrayDeclarationTestSource_SOC_S16Result (*(volatile short*)0x80400000)
#define ArrayDeclarationTestSource_SOC_U32Result (*(volatile unsigned int*)0x80500000)
#define ArrayDeclarationTestSource_SOC_S32Result (*(volatile int*)0x80600000)
