using System.Collections.Generic;
using Confirma.Types;
using Godot;
using Godot.Collections;

namespace Confirma.Classes;

public class ScriptInfo(Script script, LinkedList<ScriptMethodInfo> methods)
{
    public Script Script { get; init; } = script;
    public LinkedList<ScriptMethodInfo> Methods { get; init; } = methods;

    public static ScriptInfo Parse(in Script script)
    {
        LinkedList<ScriptMethodInfo> list = new();

        foreach (Dictionary method in script.GetScriptMethodList())
        {
            Dictionary returnInfo = (Dictionary)method["return"];
            LinkedList<ScriptMethodArgumentInfo> arg = new();

            foreach (Dictionary argInfo in method["args"].AsGodotArray<Dictionary>())
            {
                _ = arg.AddLast(ParseArgumentInfo(argInfo));
            }

            _ = list.AddLast(ParseMethodInfo(method, returnInfo, arg));
        }

        return new(script, list);
    }

    private static ScriptMethodInfo ParseMethodInfo(
        Dictionary methodInfo,
        Dictionary returnInfo,
        LinkedList<ScriptMethodArgumentInfo> arg
    )
    {
        return new(
            methodInfo["name"].AsString(),
            [.. arg],
            methodInfo["default_args"].AsStringArray(),
            methodInfo["flags"].AsInt32(),
            methodInfo["id"].AsInt32(),
            ParseReturnInfo(returnInfo)
        );
    }

    private static ScriptMethodReturnInfo ParseReturnInfo(Dictionary info)
    {
        return new(
            info["name"].AsString(),
            info["class_name"].AsString(),
            info["type"].AsInt32(),
            info["hint"].AsInt32(),
            info["hint_string"].AsString(),
            info["usage"].AsInt32()
        );
    }

    private static ScriptMethodArgumentInfo ParseArgumentInfo(Dictionary info)
    {
        return new(
            info["name"].AsString(),
            info["class_name"].AsString(),
            info["type"].AsInt32(),
            info["hint"].AsInt32(),
            info["hint_string"].AsString(),
            info["usage"].AsInt32()
        );
    }
}
