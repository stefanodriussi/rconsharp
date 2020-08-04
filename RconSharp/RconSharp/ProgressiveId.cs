using System;
using System.Collections.Generic;
using System.Text;

namespace RconSharp
{
	public static class ProgressiveId
	{
		private static object padlock = new object();
		private static int counter = 0;
		public static int Next()
		{
			lock (padlock)
			{
				return counter++;
			}
		}

		public static void Seed(int seed = 0) => counter = seed;
	}
}
