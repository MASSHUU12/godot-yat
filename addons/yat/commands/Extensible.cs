using System;
using System.Collections.Generic;
using Godot;

namespace YAT.Commands
{
	public interface IExtensible
	{
		public static Dictionary<string, IExtension> Extensions { get; set; }

		public virtual void Register(IExtension extension)
		{
			if (Attribute.GetCustomAttribute(extension.GetType(), typeof(ExtensionAttribute))
				is not ExtensionAttribute attribute)
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
