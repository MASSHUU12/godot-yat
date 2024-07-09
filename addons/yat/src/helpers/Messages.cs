using Godot;

namespace YAT.Helpers;

public static class Messages
{
    // Unknown
    public static string UnknownCommand(StringName command)
        => $"{command} is not a valid command.";
    public static string UnknownMethod(string something, string method)
        => $"{something} does not have a method named {method}.";

    // Missing
    public static string MissingInterface(string something, StringName @interface)
        => $"{something} does not implement {@interface}.";
    public static string MissingArguments(StringName command, params string[] args)
        => $"{command} expected {string.Join(", ", args)} to be provided.";
    public static string MissingAttribute(string something, StringName attribute)
        => $"{something} is missing the {attribute} attribute.";
    public static string MissingValue(StringName command, StringName option)
        => $"{command} expected {option} to be provided.";

    // Invalid
    public static string InvalidNodePath(string path)
        => $"{path} is not a valid node path.";
    public static string InvalidArgument(StringName command, string argument, string expected)
        => $"{command} expected {Text.EscapeBBCode(argument)} to be an: {expected}.";
    public static string InvalidMethod(string method)
        => $"{method} is not a valid method.";
    public static string InvalidOption(StringName command, string opt)
        => $"{command} does not have an option named {opt}.";
    public static string InvalidCommandInputType(string type, StringName command)
        => $"Invalid command input type '{type}' for command '{command}'.";

    // Other
    public static string NodeHasNoChildren(string path)
        => $"{path} has no children.";
    public static string ValueOutOfRange(string value, float min, float max)
        => $"{value} is out of range. Expected a value between {min} and {max}.";
    public static string ArgumentValueOutOfRange(StringName command, StringName argument, float min, float max)
        => $"{command} expected {argument} to be between {min} and {max}.";

    public static string DisposedNode => "The node has been disposed.";
}
