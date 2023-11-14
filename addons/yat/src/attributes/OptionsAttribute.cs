using System.Collections.Generic;

namespace YAT.Attributes
{
	[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
	public sealed class OptionsAttribute : System.Attribute
	{
		// TODO
		// Options are defined as follows and are optional:
		// -arg or --arg - passed argument must match the name of the argument
		// -arg:data_type - passed argument must be of the specified data type
		//					and look like this: -arg=value
		// -arg:[option1, data_type, option3] - passed argument must be
		//										of the specified data types or
		//										one of the specified options
		//										many arguments can be passed
		//										like this: -arg=value1,value2
		// Numeric data types can have a range specified as follows:
		// data_type(min, max)
		public Dictionary<string, object> Options { get; private set; }

		public OptionsAttribute(params string[] options)
		{
			// TODO: Parse options
			// Options = options;
		}
	}
}
