using System;
using System.Collections.Generic;
using System.Linq;

namespace YAT.Attributes
{
	[Obsolete("Use ArgumentAttribute instead.")]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class ArgumentsAttribute : Attribute
	{
		// Arguments are defined as follows:
		// arg - passed argument must match the name of the argument
		// arg:data_type - passed argument must be of the specified data type
		// arg:[option1, data_type, option3] - passed argument must be
		//										of the specified data types or
		//										one of the specified options
		// arg:[option1, option2, option3] - passed argument must be one of
		//									the specified options
		// Numeric data types can have a range specified as follows:
		// data_type(min, max)
		public Dictionary<string, object> Args { get; private set; }

		public ArgumentsAttribute(params string[] args)
		{
			Args = GetArguments(args);
		}

		/// <summary>
		/// Gets the arguments for the specified command.
		/// </summary>
		/// <param name="args">The arguments for the command.</param>
		/// <returns>A dictionary containing the arguments for the command.</returns>
		private static Dictionary<string, object> GetArguments(string[] args)
		{
			Dictionary<string, object> arguments = new();

			if (args is null) return arguments;

			foreach (string arg in args)
			{
				string[] parts = arg.Split(':');
				string argName = parts[0];
				string dataType = parts.Length > 1 ? parts[1] : null;

				arguments.Add(argName, ParseDataType(dataType));
			}

			return arguments;
		}

		/// <summary>
		/// Parses a string representation of a data type and returns the corresponding object or type.
		/// </summary>
		/// <param name="dataType">The string representation of the data type to parse.</param>
		/// <returns>The parsed object or type, or null if the data type could not be parsed.</returns>
		private static object ParseDataType(string dataType)
		{
			var data = dataType?.Trim();

			if (string.IsNullOrEmpty(data)) return null;

			if (data.StartsWith("[") && data.EndsWith("]"))
			{
				string[] values = dataType.Trim('[', ']').Split(',').Select(v => v.Trim()).ToArray();
				return values;
			}

			return data;
		}
	}
}
