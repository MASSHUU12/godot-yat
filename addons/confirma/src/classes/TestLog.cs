using System.Globalization;
using Confirma.Enums;
using Confirma.Extensions;
using Confirma.Formatters;
using Confirma.Helpers;

namespace Confirma.Classes;

public class TestLog
{
    public string? Message { get; }
    public string? Name { get; }
    public ETestCaseState State { get; } = ETestCaseState.Ignored;
    public ELogType Type { get; }
    public ELangType Lang { get; set; } = ELangType.None;
    public long ExecutionTime { get; }

    private readonly NumericFormatter _numericFormatter = new(3);

    public TestLog(ELogType type)
    {
        Type = type;
    }

    public TestLog(ELogType type, string message)
    {
        Type = type;
        Message = message;
    }

    public TestLog(ELogType type, ELangType testLang, string message)
    {
        Type = type;
        Message = message;
        Lang = testLang;
    }

    public TestLog(
        ELogType type,
        string name,
        ETestCaseState state,
        long executionTime,
        string parameters = "",
        string? message = null,
        ELangType lang = ELangType.None
    )
    {
        Type = type;
        Message = message;
        Name = name + (
            parameters?.Length > 0
            ? $"({parameters.EscapeInvisibleCharacters()})"
            : string.Empty
        );
        State = state;
        Lang = lang;
        ExecutionTime = executionTime;
    }

    public static string GetTestCaseStateString(ETestCaseState state)
    {
        return state.ToString().ToLowerInvariant();
    }

    public static string GetTestCaseStateColor(ETestCaseState state)
    {
        return state switch
        {
            ETestCaseState.Passed => Colors.Success,
            ETestCaseState.Failed => Colors.Error,
            ETestCaseState.Ignored => Colors.Warning,
            _ => "unknown"
        };
    }

    public void PrintOutput(bool verbose = false)
    {
        switch (Type)
        {
            case ELogType.Method:
                switch (verbose)
                {
                    case false when State is ETestCaseState.Passed:
                        return;
                    case true:
                        PrintMethodVerbose();
                        break;
                    default:
                        PrintMethodDefault();
                        break;
                }
                break;
            case ELogType.Class:
                Log.Print($"{GetLangHeader()} {Colors.ColorText(Message!, Colors.Class)}...");
                break;
            case ELogType.Info:
            case ELogType.Warning:
                if (Message is not null)
                {
                    Log.PrintLine(Message);
                }
                break;
            case ELogType.Error:
                if (Message is not null)
                {
                    Log.PrintError(Message);
                }
                break;
            case ELogType.Newline:
                Log.PrintLine();
                break;
        }
    }

    private void PrintMethodDefault()
    {
        string pipe = Colors.ColorText("|", GetLangColor());

        Log.PrintLine($"{pipe} {Name}...");

        if (State is not ETestCaseState.Passed)
        {
            Log.PrintLine($"\\_ {pipe} {GetMethodStringDefinition()}");
        }

        PrintMessageIfNotNull();
    }

    private void PrintMethodVerbose()
    {
        string pipe = Colors.ColorText("|", GetLangColor());

        Log.PrintLine($"{pipe} {GetMethodStringDefinition()}");

        PrintMessageIfNotNull();
    }

    private void PrintMessageIfNotNull()
    {
        string color = GetTestCaseStateColor(State);
        string pipe = Colors.ColorText("|", GetLangColor());

        if (Message is not null)
        {
            Log.PrintLine($"{pipe}- {Colors.ColorText(Message, color)}");
        }
    }

    private string GetMethodStringDefinition()
    {
        string color = GetTestCaseStateColor(State);
        string sState = GetTestCaseStateString(State);

        return string.Format(
            CultureInfo.InvariantCulture,
            "{0}... {1} ({2}s).",
            Name,
            Colors.ColorText(sState, color),
            _numericFormatter.Format(ExecutionTime * 0.001)
        );
    }

    private string GetLangHeader()
    {
        string langText = "N/A";

        switch (Lang)
        {
            case ELangType.CSharp:
                langText = "C#";
                break;
            case ELangType.GDScript:
                langText = "GDScript";
                break;
            case ELangType.None:
                break;
        }

        return Colors.ColorText($"[{langText}]", GetLangColor());
    }

    private string GetLangColor()
    {
        switch (Lang)
        {
            case ELangType.CSharp:
                return Colors.CSharp;
            case ELangType.GDScript:
                return Colors.Gdscript;
            case ELangType.None:
                break;
        }
        return Colors.Error;
    }
}
