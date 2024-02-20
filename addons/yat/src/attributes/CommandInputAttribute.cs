using System;
using System.Collections.Generic;
using Godot;
using YAT.Helpers;
using YAT.Types;

namespace YAT.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public class CommandInputAttribute : Attribute
{
	public string Name { get; private set; }
	public LinkedList<CommandInputType> Types { get; private set; } = new();
	public string Description { get; private set; }

	public CommandInputAttribute(string name, string type, string description = "")
	{
		Name = name;
		Description = description;

		if (string.IsNullOrEmpty(type))
			GD.PushError($"Invalid command input type '{type}' for command '{name}'.");

		foreach (var t in type.Split('|'))
		{
			if (Text.TryParseCommandInputType(t, out var commandInputType))
				Types.AddLast(commandInputType);
			else
				GD.PushError($"Invalid command input type '{t}' for command '{name}'.");
		}
	}
}
