using YAT.Helpers;

namespace GdUnit4
{
	using static Assertions;

	[TestSuite]
	public partial class TestScene
	{
		[TestCase("", 0, 0, 0)]
		[TestCase("0, 32", 0, 32, 0)]
		[TestCase("1,255,1", 1, 255, 1)]
		[TestCase("0,1,0.1", 0, 1, 0.1f)]
		[TestCase("0.3,0.5,0.1", 0.3f, 0.5f, 0.1f)]
		public void TestGetRangeFromHint(string hint, float min, float max, float step)
		{
			var (actualMin, actualMax, actualStep) = Scene.GetRangeFromHint(hint);

			AssertFloat(min).IsEqual(actualMin);
			AssertFloat(max).IsEqual(actualMax);
			AssertFloat(step).IsEqual(actualStep);
		}
	}
}
