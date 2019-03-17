#include <stdint.h>
#include <stdbool.h>

// a pointer to this is a null pointer, but the compiler does not
// know that because "sram" is a linker symbol from sections.lds.
extern uint32_t sram;

#include "plumbing.h"
#include "dma.h"
{FIRMWARE_INCLUDES}

extern uint32_t _sidata, _sdata, _edata, _sbss, _ebss,_heap_start;

void main() {
    set_irq_mask(0xff);

    // zero out .bss section
    for (uint32_t *dest = &_sbss; dest < &_ebss;) {
        *dest++ = 0;
    }

	{FIRMWARE_CODE}
}
