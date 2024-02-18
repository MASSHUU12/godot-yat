using System;
using Godot;
using YAT.Helpers;
using YAT.Types;

namespace YAT.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class CommandInputAttribute : Attribute
{
	public string Name { get; private set; }
	public CommandInputType Type { get; private set; }
	public string Description { get; private set; }

	public CommandInputAttribute(string name, string type, string description = "")
	{
		Name = name;
		Description = description;

		if (Text.TryParseCommandInputType(type, out var t))
			Type = t;
		else
			GD.PrintErr($"Invalid command input type: {type}");
	}
}
