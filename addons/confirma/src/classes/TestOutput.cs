using Confirma.Enums;
using Confirma.Helpers;

namespace Confirma.Classes;

public static class TestOutput
{
    public static string GetTestCaseStateString(ETestCaseState state)
    {
        return state.ToString().ToLower();
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

    public static void PrintOutput(string name, string parameters, ETestCaseState state, bool verbose = false, string? message = null)
    {
        switch (verbose)
        {
            case false when state == ETestCaseState.Passed:
                return;
            case true:
                PrintVerbose(name, parameters, state, message);
                break;
            default:
                PrintDefault(name, parameters, state, message);
                break;
        }
    }

    private static void PrintDefault(string name, string parameters, ETestCaseState state, string? message = null)
    {
        string color = GetTestCaseStateColor(state);
        string sState = GetTestCaseStateString(state);

        Log.PrintLine($"| {name}... ");

        if (state != ETestCaseState.Passed)
        {
            Log.Print($"\\_ {name}{(parameters.Length > 0 ? $"({parameters})" : string.Empty)}... ");
            Log.PrintLine($"{Colors.ColorText(sState, color)}.");
        }

        if (message is not null)
        {
            Log.PrintLine($"  |- {Colors.ColorText(message, color)}");
        }
    }

    private static void PrintVerbose(string name, string parameters, ETestCaseState state, string? message = null)
    {
        string color = GetTestCaseStateColor(state);
        string sState = GetTestCaseStateString(state);

        Log.PrintLine($"| {name}{(
            parameters.Length > 0
            ? $"({parameters})"
            : string.Empty)}... {Colors.ColorText(sState, color)}.");

        if (message is not null)
        {
            Log.PrintLine($"- {Colors.ColorText(message, color)}");
        }
    }
}
