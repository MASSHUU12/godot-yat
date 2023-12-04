using System;

#nullable enable
namespace YAT.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class OptionAttribute : Attribute
	{
		public string Name { get; private set; }
		public string Description { get; private set; }
		public object? Type { get; set; }
		public object? DefaultValue { get; private set; }

		public OptionAttribute(
			string name,
			string type,
			string description = "",
			object? defaultValue = null
		)
		{
			Name = name;
			Type = ParseDataType(type);
			Description = description;
			DefaultValue = defaultValue;
		}

		/// <summary>
		/// Parses the given data type string and returns an object representing the parsed data.
		/// </summary>
		/// <param name="dataType">The data type string to parse.</param>
		/// <returns>An object representing the parsed data.</returns>
		private static object? ParseDataType(string dataType)
		{
			var data = dataType?.Trim();

			if (string.IsNullOrEmpty(data)) return null;

			var splitted = data.Split(',');

			return splitted.Length > 1 ? splitted : splitted[0];
		}
	}
}
