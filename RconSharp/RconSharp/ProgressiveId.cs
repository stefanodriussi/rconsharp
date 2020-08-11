namespace RconSharp
{
    /// <summary>
    /// Progressive id counter starting from 0
    /// </summary>
    public static class ProgressiveId
	{
		private static object padlock = new object();
		private static int counter = 1;
		/// <summary>
		/// Get next Id
		/// </summary>
		/// <returns>Next progressive Id</returns>
		public static int Next()
		{
			lock (padlock)
			{
				return counter++;
			}
		}
		/// <summary>
		/// Change the starting value for internal counter
		/// </summary>
		/// <param name="seed"></param>
		public static void Seed(int seed = 0)
		{
			lock (padlock)
			{
				counter = seed;
			}
		}
	}
}
