using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Godot;
using Godot.Collections;
using YAT.Enums;
using YAT.Types;

namespace YAT.Helpers;

public static class Scene
{
    public static string PrintChildren(Node node)
    {
        Array<Node> children = node.GetChildren();

        if (children.Count == 0)
        {
            return $"Node '{node.GetPath()}' has no children.";
        }

        StringBuilder sb = new();
        _ = sb.AppendLine(
            CultureInfo.InvariantCulture,
            $"Found {children.Count} children of '{node.Name}' node:"
        );

        foreach (Node child in children)
        {
            _ = sb.Append(
                CultureInfo.InvariantCulture,
                $"[{child.Name}] ({child.GetType().Name}) - {child.GetPath()}"
            );
            _ = sb.AppendLine();
        }

        return sb.ToString();
    }

    /// <summary>
    /// Retrieves a Node from a given path or returns a default Node if the path is "." or "./".
    /// </summary>
    /// <param name="path">The path to the Node.</param>
    /// <param name="defaultNode">The default Node to return if the path is "." or "./".</param>
    /// <param name="newPath">The updated path of the retrieved Node.</param>
    /// <returns>The retrieved Node if it is a valid instance; otherwise, null.</returns>
    public static Node? GetFromPathOrDefault(string path, Node defaultNode, out string newPath)
    {
        Node node = (path is "." or "./")
            ? defaultNode
            : defaultNode.GetNodeOrNull(path);

        NodePath? temp = node?.GetPath();
        newPath = (string?)temp ?? path;

        return GodotObject.IsInstanceValid(node) ? node : null;
    }

    public static IEnumerator<Dictionary>? GetNodeMethods(Node node)
    {
        return !GodotObject.IsInstanceValid(node)
            ? (IEnumerator<Dictionary>?)null
            : node.GetMethodList().GetEnumerator();
    }

    public static bool TryFindNodeMethodInfo(Node node, string methodName, out NodeMethodInfo info)
    {
        info = default;

        if (!GodotObject.IsInstanceValid(node))
        {
            return false;
        }

        IEnumerator<Dictionary>? method = GetNodeMethods(node);

        if (method is null)
        {
            return false;
        }

        while (method.MoveNext())
        {
            if (!method.Current.TryGetValue("name", out var value) || (string)value != methodName)
            {
                continue;
            }

            info = GetNodeMethodInfo(method.Current);

            return true;
        }

        return true;
    }

    public static NodeMethodInfo GetNodeMethodInfo(Dictionary method)
    {
        StringName name = method.TryGetValue("name", out var value)
            ? value.AsStringName()
            : (StringName)string.Empty;
        var arguments = method.TryGetValue("args", out var args)
            ? args.AsGodotArray<Godot.Collections.Dictionary<string, Variant>>()
            : new Array<Godot.Collections.Dictionary<string, Variant>>();
        Variant defaultArguments = method.TryGetValue("default_args", out Variant defaultArgs)
            ? defaultArgs
            : new Array<Variant>();
        MethodFlags flags = method.TryGetValue("flags", out var f)
            ? (MethodFlags)(int)f
            : MethodFlags.Default;
        int id = method.TryGetValue("id", out var i) ? (int)i : 0;
        var @return = method.TryGetValue("return", out var r)
            ? r.AsGodotDictionary<string, Variant>()
            : new Godot.Collections.Dictionary<string, Variant>();

        return new(
            name,
            arguments,
            defaultArguments.AsGodotArray<Variant>(),
            flags,
            id,
            @return
        );
    }

    public static MethodValidationResult ValidateMethod(this Node node, StringName method)
    {
        if (!GodotObject.IsInstanceValid(node))
        {
            return MethodValidationResult.InvalidInstance;
        }

        return node.HasMethod(method)
            ? MethodValidationResult.Success
            : MethodValidationResult.InvalidMethod;
    }

    public static Variant CallMethod(this Node node, StringName method, params Variant[] args)
    {
        return args.Length == 0 ? node.Call(method) : node.Call(method, args);
    }

    public static (float, float, float) GetRangeFromHint(string hint)
    {
        string[] range = hint.Split(',');

        if (range.Length <= 1)
        {
            return (0, 0, 0);
        }

        if (range.Length == 2)
        {
            return (range[0].ToFloat(), range[1].ToFloat(), 0);
        }

        float min = range[0].ToFloat();
        float max = range[1].ToFloat();
        float step = range[2].ToFloat();

        return (min, max, step);
    }
}
