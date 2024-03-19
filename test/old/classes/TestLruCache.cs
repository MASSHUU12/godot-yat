namespace GdUnit4
{
	using YAT.Classes;
	using static Assertions;

	[TestSuite]
	public partial class TestLruCache
	{
		// [TestCase(0, 0)]
		// [TestCase(1, 0)]
		[TestCase(0, 1)]
		[TestCase(1, 1)]
		[TestCase(128, 32)]
		[TestCase(32, 128)]
		[TestCase(128, 128)]
		public void TestAdd(int additions, int capacity)
		{
			var cache = new LRUCache<string, int>((ushort)capacity);

			for (var i = 0; i < additions; i++)
			{
				cache.Add(i.ToString(), i);
				AssertInt(cache.Size).IsLessEqual(capacity);
			}

			AssertInt(cache.Size).IsLessEqual(capacity);
		}

		public void TestGet()
		{
			var cache = new LRUCache<string, int>(1);
			cache.Add("test", 1);
			AssertInt(cache.Get("test")).IsEqual(1);
		}

		public void TestGetNonExistent()
		{
			var cache = new LRUCache<string, int>(1);
			AssertInt(cache.Get("test")).IsEqual(0);
		}

		public void TestGetAfterAdd()
		{
			var cache = new LRUCache<string, int>(1);
			cache.Add("test", 1);
			cache.Add("test2", 2);
			AssertInt(cache.Get("test")).IsEqual(0);
			AssertInt(cache.Get("test2")).IsEqual(2);
		}
	}
}
