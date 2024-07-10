using System;
using System.Collections.Generic;
using YAT.Classes;
using YAT.Helpers;

namespace YAT.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public class CommandInputAttribute : Attribute
{
    public readonly string Name, Description;
    public readonly List<CommandType> Types = new();

    public CommandInputAttribute(string name, string type, string description = "")
    {
        Name = name;
        Description = description;

        if (string.IsNullOrEmpty(type))
            throw new ArgumentNullException(Messages.InvalidCommandInputType(type, name));

        foreach (var t in type.Split('|'))
        {
            if (Parser.TryParseCommandInputType(t, out var commandInputType))
            {
                Types.Add(commandInputType);
                continue;
            }

            throw new ArgumentException(Messages.InvalidCommandInputType(t, name));
        }
    }
}
