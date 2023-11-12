using System.Collections.Generic;
using System.Text;
using YAT.Attributes;
using YAT.Helpers;
using YAT.Interfaces;

namespace YAT.Commands
{
	public partial class Extensible
	{
		public Dictionary<string, IExtension> Extensions { get; } = new();

		/// <summary>
		/// Registers an extension.
		/// </summary>
		/// <param name="extension">The extension to register.</param>
		public void Register(IExtension extension)
		{
			if (AttributeHelper.GetAttribute<ExtensionAttribute>(extension)
				is not ExtensionAttribute attribute
			)
			{
				LogHelper.MissingAttribute("ExtensionAttribute", extension.GetType().Name);
				return;
			}

			Extensions[attribute.Name] = extension;
			foreach (string alias in attribute.Aliases)
			{
				Extensions[alias] = extension;
			}
		}

		/// <summary>
		/// Generates a manual for all extensions.
		/// </summary>
		/// <param name="args">Optional arguments.</param>
		/// <returns>A string containing the generated manual.</returns>
		public virtual string GenerateExtensionsManual(params string[] args)
		{
			StringBuilder sb = new();

			sb.AppendLine("[p align=center][font_size=22]Extensions[/font_size][/p]");

			foreach (var extension in Extensions)
			{
				sb.Append(extension.Value.GenerateExtensionManual());
			}

			return sb.ToString();
		}
	}
}
