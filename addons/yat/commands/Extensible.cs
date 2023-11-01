using System.Collections.Generic;
using Godot;
using YAT.Helpers;

namespace YAT.Commands
{
	public partial interface IExtensible
	{
		public Extensible Extension { get; }
	}

	public partial class Extensible
	{
		public Dictionary<string, IExtension> Extensions { get; } = new();

		public void Register(IExtension extension)
		{
			if (AttributeHelper.GetAttribute<ExtensionAttribute>(extension)
				is not ExtensionAttribute attribute
			)
			{
				var message = string.Format(
					"The extension {0} does not have a ExtensionAttribute, and will not be added to the list of available extensions.",
					extension.GetType().Name
				);

				GD.PrintErr(message);
				// Terminal.Print(message, YatTerminal.PrintType.Warning);
				return;
			}

			Extensions[attribute.Name] = extension;
			foreach (string alias in attribute.Aliases)
			{
				Extensions[alias] = extension;
			}
		}
	}
}
