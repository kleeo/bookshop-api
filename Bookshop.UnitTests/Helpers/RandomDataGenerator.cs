using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshop.UnitTests.Helpers
{
	public static class RandomDataGenerator
	{
		public static int GetRandomId()
		{
			var random = new Random();
			var number = random.Next(10000, 100000);
			return number;
		}
	}
}
