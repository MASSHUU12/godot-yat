using System;
using System.Collections.Generic;
using System.Text;
using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Classes;

public partial class Extensible : Node
{
	protected static Dictionary<StringName, Dictionary<StringName, Type>> Extensions { get; set; } = new();

	public static bool RegisterExtension(StringName commandName, Type extension)
	{
		if (string.IsNullOrEmpty(commandName) ||
			!Reflection.HasInterface(extension, nameof(IExtension))
		) return false;

		var instance = Activator.CreateInstance(extension) as IExtension;

		if (Reflection.GetAttribute<ExtensionAttribute>(instance)
			is not ExtensionAttribute attribute
		) return false;

		// Check if dictionary have entry for the command
		// if entry exists, check if the entry contains the extension
		if (!Extensions.TryGetValue(commandName, out Dictionary<StringName, Type> extensions))
			Extensions.Add(commandName, new Dictionary<StringName, Type>());
		else if (extensions.ContainsKey(commandName)) return false;

		if (Extensions[commandName].ContainsKey(attribute.Name)) return false;

		Extensions[commandName].Add(attribute.Name, extension);

		foreach (StringName alias in attribute.Aliases)
		{
			if (Extensions[commandName].ContainsKey(alias)) return false;
			Extensions[commandName].Add(alias, extension);
		}

		return true;
	}

	public static bool UnregisterExtension(StringName commandName, Type extension)
	{
		if (string.IsNullOrEmpty(commandName) ||
			!Reflection.HasInterface(extension, nameof(IExtension))
		) return false;

		if (Reflection.GetAttribute<ExtensionAttribute>(extension)
			is not ExtensionAttribute attribute
		) return false;

		if (!Extensions.TryGetValue(commandName, out Dictionary<StringName, Type> extensions))
			return false;

		if (!extensions.ContainsKey(attribute.Name)) return false;

		extensions.Remove(attribute.Name);

		foreach (StringName alias in attribute.Aliases)
		{
			if (!extensions.ContainsKey(alias)) return false;

			extensions.Remove(alias);
		}

		return true;
	}

	public virtual CommandResult ExecuteExtension(Type extension, CommandData args)
	{
		if (!Reflection.HasInterface(extension, nameof(IExtension)))
			return ICommand.InvalidCommand();

		return (Activator.CreateInstance(extension) as IExtension).Execute(args);
	}

	public virtual StringBuilder GenerateExtensionsManual()
	{
		StringBuilder sb = new();
		var commandName = Reflection.GetAttribute<CommandAttribute>(this)?.Name;

		if (!Extensions.TryGetValue(commandName, out Dictionary<StringName, Type> value))
		{
			sb.AppendLine("\nThis command does not have any extensions.");
			return sb;
		}

		sb.AppendLine("[p align=center][font_size=22]Extensions[/font_size][/p]");

		if (Extensions.Count == 0)
		{
			sb.AppendLine("\nThis command does not have any extensions.");
			return sb;
		}

		foreach (var extension in value)
		{
			var extensionInstance = Activator.CreateInstance(extension.Value) as IExtension;
			sb.Append(extensionInstance.GenerateExtensionManual());
		}

		return sb;
	}

	public static Dictionary<StringName, Type> GetCommandExtensions(StringName commandName)
	{
		if (string.IsNullOrEmpty(commandName)) return null;

		if (!Extensions.TryGetValue(commandName, out Dictionary<StringName, Type> extensions))
			return null;

		return extensions;
	}
}
