using System.Collections.Generic;
using Godot;
using YAT.Helpers;

namespace YAT.Commands
{
	public partial class Extensible
	{
		public Dictionary<string, IExtension> Extensions { get; } = new();

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
	}
}
