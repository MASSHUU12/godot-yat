using System;
using System.IO;
using Godot;

namespace Confirma.Helpers;

public static class Log
{
    public static RichTextLabel? RichOutput { get; set; }
    public static bool IsHeadless { get; set; } = true;

    public static void Print<T>(T message, TextWriter? stream)
    where T : IConvertible
    {
        if (IsHeadless || RichOutput is null)
        {
            stream ??= Console.Out;
            stream.Write(message);
        }
        else
        {
            _ = RichOutput!.CallDeferred("append_text", message.ToString()!);
        }
    }

    public static void Print<T>(params T[] messages) where T : IConvertible
    {
        foreach (T message in messages)
        {
            Print(message, Console.Out);
        }
    }

    public static void PrintLine<T>(params T[] messages) where T : IConvertible
    {
        Print(messages);
        PrintLine();
    }

    public static void PrintLine()
    {
        Print("\n");
    }

    public static void PrintError<T>(T message) where T : IConvertible
    {
        Print(Colors.ColorText(message, Colors.Error), Console.Error);
    }

    public static void PrintSuccess<T>(T message) where T : IConvertible
    {
        Print(Colors.ColorText(message, Colors.Success));
    }

    public static void PrintWarning<T>(T message) where T : IConvertible
    {
        Print(Colors.ColorText(message, Colors.Warning));
    }
}
