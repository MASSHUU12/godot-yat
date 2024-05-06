using Confirma.Attributes;
using Confirma.Extensions;
using YAT.Classes;

namespace YAT.Test;

[TestClass]
[Parallelizable]
public static class LruCacheTest
{
	[TestCase(0, 1)]
	[TestCase(1, 1)]
	[TestCase(128, 32)]
	[TestCase(32, 128)]
	[TestCase(128, 128)]
	public static void TestAdd(int additions, int capacity)
	{
		var cache = new LRUCache<string, int>((ushort)capacity);

		for (var i = 0; i < additions; i++)
		{
			cache.Add(i.ToString(), i);
			cache.Size.ConfirmLessThanOrEqual(capacity);
		}

		cache.Size.ConfirmLessThanOrEqual(capacity);
	}

	[TestCase]
	public static void TestGet()
	{
		var cache = new LRUCache<string, int>(1);
		cache.Add("test", 1);
		cache.Get("test").ConfirmEqual(1);
	}

	[TestCase]
	public static void TestGetNonExistent()
	{
		var cache = new LRUCache<string, int>(1);
		cache.Get("test").ConfirmEqual(0);
	}

	[TestCase]
	public static void TestGetAfterAdd()
	{
		var cache = new LRUCache<string, int>(1);
		cache.Add("test", 1);
		cache.Add("test2", 2);
		cache.Get("test").ConfirmEqual(0);
		cache.Get("test2").ConfirmEqual(2);
	}
}
