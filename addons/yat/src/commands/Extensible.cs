using System;
using System.Collections.Generic;
using System.Text;
using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

public partial class Extensible : Node
{
	protected static Dictionary<string, Type> Extensions { get; set; } = new();

	/// <summary>
	/// Registers an extension type to be used by the application.
	/// </summary>
	/// <param name="extension">The type of the extension to register.</param>
	/// <returns><c>true</c> if the extension was successfully registered, <c>false</c> otherwise.</returns>
	public static bool RegisterExtension(Type extension)
	{
		var extensionInstance = Activator.CreateInstance(extension);

		if (extensionInstance is not IExtension) return false;

		if (Reflection.GetAttribute<ExtensionAttribute>(extensionInstance)
			is not ExtensionAttribute attribute
		) return false;

		if (Extensions.ContainsKey(attribute.Name)) return false;

		Extensions[attribute.Name] = extension;
		foreach (string alias in attribute.Aliases)
		{
			if (Extensions.ContainsKey(alias)) return false;

			Extensions[alias] = extension;
		}

		return true;
	}

	public static bool UnregisterExtension(Type extension)
	{
		if (extension is null || extension is not IExtension) return false;

		if (Reflection.GetAttribute<ExtensionAttribute>(extension)
			is not ExtensionAttribute attribute
		) return false;

		if (!Extensions.ContainsKey(attribute.Name)) return false;

		Extensions.Remove(attribute.Name);
		foreach (string alias in attribute.Aliases)
		{
			if (!Extensions.ContainsKey(alias)) return false;

			Extensions.Remove(alias);
		}

		return true;
	}

	public virtual CommandResult ExecuteExtension(Type extension, CommandData args)
	{
		var extensionInstance = Activator.CreateInstance(extension) as IExtension;

		return extensionInstance.Execute(args);
	}

	public virtual StringBuilder GenerateExtensionsManual()
	{
		StringBuilder sb = new();

		sb.AppendLine("[p align=center][font_size=22]Extensions[/font_size][/p]");

		if (Extensions.Count == 0)
		{
			sb.AppendLine("[p align=center]No extensions are currently loaded.[/p]");
			return sb;
		}

		foreach (var extension in Extensions)
		{
			var extensionInstance = Activator.CreateInstance(extension.Value) as IExtension;
			sb.Append(extensionInstance.GenerateExtensionManual());
		}

		return sb;
	}
}
