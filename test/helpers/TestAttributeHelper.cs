using YAT.Attributes;
using YAT.Helpers;

namespace GdUnit4
{
	using static Assertions;


	[TestSuite]
	public partial class TestAttributeHelper
	{
		[Command("test")]
		[Argument("arg1", "string")]
		[Argument("arg3", "string")]
		[Argument("arg3", "string")]
		private class TestClass { }

		[TestCase]
		public void TestGetAttribute_AttributePresent()
		{
			var obj = new TestClass();
			var attribute = Reflection.GetAttribute<CommandAttribute>(obj);

			AssertObject(attribute).IsNotNull();
			AssertString(attribute.Name).IsEqual("test");
		}

		[TestCase]
		public void TestGetAttribute_AttributeNotPresent()
		{
			var obj = new TestClass();
			var attribute = Reflection.GetAttribute<OptionAttribute>(obj);

			AssertObject(attribute).IsNull();
		}

		[TestCase]
		public void TestGetAttribute_AttributesPresent()
		{
			var obj = new TestClass();
			var attributes = Reflection.GetAttributes<ArgumentAttribute>(obj);

			AssertObject(attributes).IsNotNull();
			AssertInt(attributes.Length).IsEqual(3);
			AssertString(attributes[0].Name).IsEqual("arg1");
			AssertString(attributes[1].Name).IsEqual("arg3");
			AssertString(attributes[2].Name).IsEqual("arg3");
		}

		[TestCase]
		public void TestGetAttribute_AttributesNotPresent()
		{
			var obj = new TestClass();
			var attributes = Reflection.GetAttributes<OptionAttribute>(obj);

			AssertArray(attributes).IsEmpty();
		}

		[TestCase]
		public void TestHasAttribute_AttributePresent()
		{
			var obj = new TestClass();
			var hasAttribute = Reflection.HasAttribute<CommandAttribute>(obj);

			AssertBool(hasAttribute).IsEqual(true);
		}

		[TestCase]
		public void TestHasAttribute_AttributeNotPresent()
		{
			var obj = new TestClass();
			var hasAttribute = Reflection.HasAttribute<OptionAttribute>(obj);

			AssertBool(hasAttribute).IsEqual(false);
		}
	}
}
