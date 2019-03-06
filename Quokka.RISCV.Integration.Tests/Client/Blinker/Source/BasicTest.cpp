include <stdint>
include <stdbool>
namespace UnitTests
{
	static class BasicTest
	{
		public: static void _main()
		{
			int counter = 0;
			double tmp = 0.5;
			while((counter < 1024))
			{
				counter = (counter + 1);
				tmp = (tmp * 2);
			}
		}
	}
}
