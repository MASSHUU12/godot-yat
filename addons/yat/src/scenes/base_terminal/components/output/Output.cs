using System;
using System.Diagnostics;
using System.Text;
using Godot;
using YAT.Enums;

namespace YAT.Scenes;

public partial class Output : RichTextLabel
{
#nullable disable
    [Export] public BaseTerminal Terminal { get; set; }
#nullable restore

    [Flags]
    public enum LogOutput
    {
        None = 0b000,
        Terminal = 0b001,
        Editor = 0b010,
        EditorRich = 0b100
    }

    public void Print(string message, LogOutput output = LogOutput.Terminal)
    {
        if ((output & LogOutput.Terminal) == LogOutput.Terminal)
        {
            Terminal.Print(message);
        }

        if ((output & LogOutput.Editor) == LogOutput.Editor)
        {
            GD.Print(message);
        }

        if ((output & LogOutput.EditorRich) == LogOutput.EditorRich)
        {
            GD.PrintRich(message);
        }
    }

    public void Error(string message, LogOutput output = LogOutput.Terminal)
    {
        if ((output & LogOutput.Terminal) == LogOutput.Terminal)
        {
            Terminal.Print(message, EPrintType.Error);
        }

        if ((output & LogOutput.Editor) == LogOutput.Editor)
        {
            GD.PushError(message);
        }

        if ((output & LogOutput.EditorRich) == LogOutput.EditorRich)
        {
            GD.PushError(message);
        }
    }

    public void Warning(string message, LogOutput output = LogOutput.Terminal)
    {
        if ((output & LogOutput.Terminal) == LogOutput.Terminal)
        {
            Terminal.Print(message, EPrintType.Warning);
        }

        if ((output & LogOutput.Editor) == LogOutput.Editor)
        {
            GD.PushWarning(message);
        }

        if ((output & LogOutput.EditorRich) == LogOutput.EditorRich)
        {
            GD.PushWarning(message);
        }
    }

    public void Info(string message, LogOutput output = LogOutput.Terminal)
    {
        if ((output & LogOutput.Terminal) == LogOutput.Terminal)
        {
            Terminal.Print(message, EPrintType.Normal);
        }

        if ((output & LogOutput.Editor) == LogOutput.Editor)
        {
            GD.Print(message);
        }

        if ((output & LogOutput.EditorRich) == LogOutput.EditorRich)
        {
            GD.PrintRich(message);
        }
    }

    public void Success(string message, LogOutput output = LogOutput.Terminal)
    {
        if ((output & LogOutput.Terminal) == LogOutput.Terminal)
        {
            Terminal.Print(message, EPrintType.Success);
        }

        if ((output & LogOutput.Editor) == LogOutput.Editor)
        {
            GD.Print(message);
        }

        if ((output & LogOutput.EditorRich) == LogOutput.EditorRich)
        {
            GD.PrintRich(message);
        }
    }

    public void Debug(string message, LogOutput output = LogOutput.Terminal)
    {
#if DEBUG
        message = $"[DEBUG] {message}";

        if ((output & LogOutput.Terminal) == LogOutput.Terminal)
        {
            Terminal.Print(message);
        }

        if ((output & LogOutput.Editor) == LogOutput.Editor)
        {
            GD.Print(message);
        }

        if ((output & LogOutput.EditorRich) == LogOutput.EditorRich)
        {
            GD.PrintRich(message);
        }
#endif
    }

    public void Trace(string message, bool detailed = false, LogOutput output = LogOutput.Terminal)
    {
#if DEBUG
        if (!detailed)
        {
            message = $"[TRACE] {message}: {System.Environment.StackTrace}";
        }
        else
        {
            StackTrace st = new();
            StringBuilder sb = new();

            sb.Append($"[TRACE] {message}");

            for (int i = 0; i < st.FrameCount; i++)
            {
                StackFrame? sf = st.GetFrame(i);

                if (sf is null)
                {
                    continue;
                }

                sb.AppendFormat(
                    "\n{0}({1},{2}): {3}.{4}",
                    sf.GetFileName(),
                    sf.GetFileLineNumber(),
                    sf.GetFileColumnNumber(),
                    sf.GetMethod()?.DeclaringType?.FullName ?? string.Empty,
                    sf.GetMethod()
                );
            }

            message = sb.ToString();
        }

        if ((output & LogOutput.Terminal) == LogOutput.Terminal)
        {
            Terminal.Print(message);
        }

        if ((output & LogOutput.Editor) == LogOutput.Editor)
        {
            GD.Print(message);
        }

        if ((output & LogOutput.EditorRich) == LogOutput.EditorRich)
        {
            GD.PrintRich(message);
        }
#endif
    }
}
