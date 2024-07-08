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
        if (!verbose && state == ETestCaseState.Passed) return;

        if (verbose) PrintVerbose(name, parameters, state, message);
        else PrintDefault(name, parameters, state, message);
    }

    private static void PrintDefault(string name, string parameters, ETestCaseState state, string? message = null)
    {
        var color = GetTestCaseStateColor(state);
        var sState = GetTestCaseStateString(state);

        Log.PrintLine($"| {name}... ");

        if (state != ETestCaseState.Passed)
        {
            Log.Print($"\\_ {name}{(parameters.Length > 0 ? $"({parameters})" : string.Empty)}... ");
            Log.PrintLine($"{Colors.ColorText(sState, color)}.");
        }

        if (message is not null) Log.PrintLine($"  |- {Colors.ColorText(message, color)}");
    }

    private static void PrintVerbose(string name, string parameters, ETestCaseState state, string? message = null)
    {
        var color = GetTestCaseStateColor(state);
        var sState = GetTestCaseStateString(state);

        Log.PrintLine($"| {name}{(
            parameters.Length > 0
            ? $"({parameters})"
            : string.Empty)}... {Colors.ColorText(sState, color)}.");

        if (message is not null) Log.PrintLine($"- {Colors.ColorText(message, color)}");
    }
}
