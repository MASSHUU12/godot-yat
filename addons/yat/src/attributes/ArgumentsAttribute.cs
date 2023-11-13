namespace YAT.Attributes
{
	[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
	public sealed class ArgumentsAttribute : System.Attribute
	{
		// Arguments are defined as follows:
		// arg_name - passed argument must match the name of the argument
		// arg_name:data_type - passed argument must be of the specified data type
		// arg_name:[option1, option2, option3] - passed argument must be one of the specified options
		public string[] Args { get; private set; }

		public ArgumentsAttribute(params string[] args)
		{
			Args = args;
		}
	}
}
