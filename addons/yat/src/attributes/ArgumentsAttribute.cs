namespace YAT.Attributes
{
	[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
	public sealed class ArgumentsAttribute : System.Attribute
	{
		// TODO: Add optional type validation.
		// For example: "min:int"

		public string[] Args { get; private set; }

		public ArgumentsAttribute(params string[] args)
		{
			Args = args;
		}
	}
}
