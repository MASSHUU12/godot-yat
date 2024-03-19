using Chickensoft.GoDotTest;
using Godot;
using YAT.Helpers;
using Shouldly;

namespace Test;

public class TestScene : TestClass
{
	public TestScene(Node testScene) : base(testScene) { }

	[Test]
	public static void TestGetRangeFromHint()
	{
		GetRangeFromHint("", 0, 0, 0);
		GetRangeFromHint("0, 32", 0, 32, 0);
		GetRangeFromHint("1,255,1", 1, 255, 1);
		GetRangeFromHint("0,1,0.1", 0, 1, 0.1f);
		GetRangeFromHint("0.3,0.5,0.1", 0.3f, 0.5f, 0.1f);
	}

	private static void GetRangeFromHint(string hint, float min, float max, float step)
	{
		var (actualMin, actualMax, actualStep) = Scene.GetRangeFromHint(hint);

		min.ShouldBe(actualMin);
		max.ShouldBe(actualMax);
		step.ShouldBe(actualStep);
	}
}
