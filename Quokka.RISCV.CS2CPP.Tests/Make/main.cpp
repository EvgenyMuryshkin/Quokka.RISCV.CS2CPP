#include <stdint.h>
#include <stdbool.h>

#include "soc.h"
#include "SOCBlinker.h"

void main() {
	SOCBlinker::Firmware::EntryPoint();
}
