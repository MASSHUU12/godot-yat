namespace YAT.Attributes
{
	[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
	public sealed class OptionsAttribute : System.Attribute
	{
		// TODO: Add support for optional arguments after =.

		public string[] Options { get; private set; }

		public OptionsAttribute(params string[] options)
		{
			Options = options;
		}
	}
}
