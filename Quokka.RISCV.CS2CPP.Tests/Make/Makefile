elf: start.S main.cpp SOCBlinker.cpp
	riscv32-unknown-elf-gcc -march=rv32i -nostartfiles -Wl,-Bstatic,-T,sections.lds,--strip-debug,-Map=firmware.map,--cref  -ffreestanding -nostdlib -o firmware.elf start.S main.cpp SOCBlinker.cpp

bin: elf
	riscv32-unknown-elf-objcopy -O binary firmware.elf /dev/stdout > firmware.bin




