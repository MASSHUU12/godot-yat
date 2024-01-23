using System;
using System.Linq;

namespace YAT.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class ArgumentAttribute : Attribute
	{
		public string Name { get; private set; }
		public object Type { get; set; }
		public string Description { get; private set; }

		public ArgumentAttribute(string name, string type, string Description = "")
		{
			Name = name;
			Type = ParseDataType(type);
			this.Description = Description;
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
				string[] values = data.Trim('[', ']').Split(',').Select(v => v.Trim()).ToArray();
				return values;
			}

			return data;
		}
	}
}
