namespace YAT.Attributes
{
	[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
	public sealed class ArgumentsAttribute : System.Attribute
	{
		// Arguments are defined as follows:
		// arg - passed argument must match the name of the argument
		// arg:data_type - passed argument must be of the specified data type
		// arg:[option1, data_type, option3] - passed argument must be
		//										of the specified data types or
		//										one of the specified options
		// arg:[option1, option2, option3] - passed argument must be one of
		//									the specified options
		public string[] Args { get; private set; }

		public ArgumentsAttribute(params string[] args)
		{
			Args = args;
		}
	}
}
