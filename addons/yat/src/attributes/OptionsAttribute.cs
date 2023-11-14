using System.Collections.Generic;

namespace YAT.Attributes
{
	[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
	public sealed class OptionsAttribute : System.Attribute
	{
		// Options are defined as follows and are optional:
		// -arg or --arg - passed argument must match the name of the argument
		// -arg=data_type - passed argument must be of the specified data type
		//					and look like this: -arg=value
		// -arg=option1,data_type,option3 - passed argument must be
		//										of the specified data types or
		//										one of the specified options
		//										many arguments can be passed
		//										like this: -arg=value1,value2
		// Numeric data types can have a range specified as follows:
		// data_type(min, max)
		public Dictionary<string, object> Options { get; private set; }

		public OptionsAttribute(params string[] options)
		{
			Options = GetArguments(options);
		}

		/// <summary>
		/// Parses the given command line arguments and returns a dictionary
		/// of argument names and their corresponding values.
		/// </summary>
		/// <param name="args">The command line arguments to parse.</param>
		/// <returns>A dictionary of argument names and their corresponding values.</returns>
		private static Dictionary<string, object> GetArguments(string[] args)
		{
			Dictionary<string, object> arguments = new();

			if (args is null) return arguments;

			foreach (string arg in args)
			{
				string[] parts = arg.Split('=');
				string argName = parts[0];
				string dataType = parts.Length > 1 ? parts[1] : null;

				arguments.Add(argName, ParseDataType(dataType));
			}

			return arguments;
		}

		/// <summary>
		/// Parses the given data type string and returns an object representing the parsed data.
		/// </summary>
		/// <param name="dataType">The data type string to parse.</param>
		/// <returns>An object representing the parsed data.</returns>
		private static object ParseDataType(string dataType)
		{
			var data = dataType?.Trim();

			if (string.IsNullOrEmpty(data)) return null;

			var splitted = data.Split(',');

			return splitted.Length > 1 ? splitted : splitted[0];
		}
	}
}
