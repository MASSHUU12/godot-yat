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
        var cache = new LRUCache<string, int>((ushort)capacity);

        for (var i = 0; i < additions; i++)
        {
            cache.Add(i.ToString(), i);
            cache.Size.ConfirmLessThanOrEqual(capacity);
        }

        cache.Size.ConfirmLessThanOrEqual(capacity);
    }

    [TestCase]
    public static void Add_WhenAtCapacity()
    {
        var cache = new LRUCache<string, int>(1);
        cache.Add("test", 1);
        cache.Size.ConfirmEqual(1);
    }

    [TestCase]
    public static void Add_WhenOverCapacity()
    {
        var cache = new LRUCache<string, int>(1);
        cache.Add("test", 1);
        cache.Add("test2", 2);
        cache.Size.ConfirmEqual(1);
    }
    #endregion

    #region Get
    [TestCase]
    public static void Get_WhenExists()
    {
        var cache = new LRUCache<string, int>(1);
        cache.Add("test", 1);
        cache.Get("test").ConfirmEqual(1);
    }

    [TestCase]
    public static void Get_WhenNonExistent()
    {
        var cache = new LRUCache<string, int>(1);
        cache.Get("test").ConfirmEqual(0);
    }

    [TestCase]
    public static void Get_WhenLaterAccessed()
    {
        var cache = new LRUCache<string, int>(1);
        cache.Add("test", 1);
        cache.Add("test2", 2);
        cache.Get("test").ConfirmEqual(0);
        cache.Get("test2").ConfirmEqual(2);
    }

    [TestCase]
    public static void Get_WhenAccessedTwice()
    {
        var cache = new LRUCache<string, int>(1);
        cache.Add("test", 1);
        cache.Get("test").ConfirmEqual(1);
        cache.Get("test").ConfirmEqual(1);
    }
    #endregion
}
