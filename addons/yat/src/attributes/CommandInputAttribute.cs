using System;
using System.Collections.Generic;
using Godot;
using YAT.Classes;
using YAT.Helpers;
using YAT.Types;

namespace YAT.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public class CommandInputAttribute : Attribute
{
	public StringName Name { get; private set; }
	public List<CommandInputType> Types { get; private set; } = new();
	public string Description { get; private set; }

	public CommandInputAttribute(StringName name, string type, string description = "")
	{
		Name = name;
		Description = description;

		if (string.IsNullOrEmpty(type))
			GD.PushError(Messages.InvalidCommandInputType(type, name));

		foreach (var t in type.Split('|'))
		{
			if (Parser.TryParseCommandInputType(t, out var commandInputType))
				Types.Add(commandInputType);
			else
				GD.PushError(Messages.InvalidCommandInputType(t, name));
		}
	}
}
