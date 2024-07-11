using Confirma.Attributes;
using Confirma.Extensions;
using YAT.Classes;

namespace YAT.Test;

[TestClass]
[Parallelizable]
public static class LruCacheTest
{
    #region Add
    [TestCase(0, 1)]
    [TestCase(1, 1)]
    [TestCase(32, 64)]
    public static void Add_WhenWithinCapacity(int additions, int capacity)
    {
        LRUCache<string, int> cache = new((ushort)capacity);

        for (int i = 0; i < additions; i++)
        {
            cache.Add(i.ToString(), i);
            _ = cache.Size.ConfirmLessThanOrEqual(capacity);
        }

        _ = cache.Size.ConfirmLessThanOrEqual(capacity);
    }

    [TestCase]
    public static void Add_WhenAtCapacity()
    {
        LRUCache<string, int> cache = new(1);
        cache.Add("test", 1);
        _ = cache.Size.ConfirmEqual(1);
    }

    [TestCase]
    public static void Add_WhenOverCapacity()
    {
        LRUCache<string, int> cache = new(1);
        cache.Add("test", 1);
        cache.Add("test2", 2);
        _ = cache.Size.ConfirmEqual(1);
    }
    #endregion Add

    #region Get
    [TestCase]
    public static void Get_WhenExists()
    {
        LRUCache<string, int> cache = new(1);
        cache.Add("test", 1);
        _ = cache.Get("test").ConfirmEqual(1);
    }

    [TestCase]
    public static void Get_WhenNonExistent()
    {
        LRUCache<string, int> cache = new(1);
        _ = cache.Get("test").ConfirmEqual(0);
    }

    [TestCase]
    public static void Get_WhenLaterAccessed()
    {
        LRUCache<string, int> cache = new(1);
        cache.Add("test", 1);
        cache.Add("test2", 2);

        _ = cache.Get("test").ConfirmEqual(0);
        _ = cache.Get("test2").ConfirmEqual(2);
    }

    [TestCase]
    public static void Get_WhenAccessedTwice()
    {
        LRUCache<string, int> cache = new(1);
        cache.Add("test", 1);

        _ = cache.Get("test").ConfirmEqual(1);
        _ = cache.Get("test").ConfirmEqual(1);
    }
    #endregion Get
}
