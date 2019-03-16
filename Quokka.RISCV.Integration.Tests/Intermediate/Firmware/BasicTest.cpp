#include <stdint.h>
#include <stdbool.h>
#include "dma.h"
namespace UnitTests
{
	class BasicTest
	{
		public: static int increment(int value)
		{
			return (value + 1);
		}
		public: static void incrementByRef(int &value)
		{
			(value++);
		}
		public: static void _main()
		{
			int counter = 0;
			double tmp = 0.5;
			while((counter < 1024))
			{
				counter = increment(counter);
				incrementByRef(counter);
				tmp = (tmp * 2);
			}
		}
	};
}
