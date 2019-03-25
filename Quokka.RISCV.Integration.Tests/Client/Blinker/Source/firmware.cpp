#include <stdint.h>
#include <stdbool.h>

// a pointer to this is a null pointer, but the compiler does not
// know that because "sram" is a linker symbol from sections.lds.
extern uint32_t sram;

#include "plumbing.h"
#include "dma.h"

extern uint32_t _sidata, _sdata, _edata, _sbss, _ebss,_heap_start;

void main() {
    set_irq_mask(0xff);

    // zero out .bss section
    for (uint32_t *dest = &_sbss; dest < &_ebss;) {
        *dest++ = 0;
    }

	
    // blink the user LED
    uint32_t led_timer = 0;
       
    while (1) {
        LED1 = led_timer >> 16;
        led_timer = led_timer + 1;
    } 

}
