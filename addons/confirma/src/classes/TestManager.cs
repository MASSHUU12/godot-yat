using System;
using System.Globalization;
using System.Threading.Tasks;
using Confirma.Classes.Executors;
using Confirma.Helpers;
using Confirma.Types;
using Confirma.Enums;

namespace Confirma.Classes;

public static class TestManager
{
    public static TestsProps Props
    {
        get => _props;
        set
        {
            _props.ExitOnFailure -= static () => { };

            _props = value;

            _props.ExitOnFailure += static () =>
            {
                // GetTree().Quit() doesn't close the program immediately
                // and allows all the remaining tests to run.
                // This is a workaround to close the program immediately,
                // at the cost of Godot displaying a lot of errors.
                Environment.Exit(1);
            };
        }
    }

    private static TestsProps _props;

    public static void Run()
    {
        bool isCs = false;
        int totalClasses = 0;
        int startOrphansCount = GetOrphans();
        DateTime startTimeStamp = DateTime.Now;

        _props.ResetStats();

        if (!_props.DisableCsharp)
        {
            CsTestExecutor csExecutor = new(_props);
            int csClasses = csExecutor.Execute(out TestResult? res);

            if (csClasses == -1)
            {
                return;
            }
            else if (!string.IsNullOrEmpty(_props.Target.Name))
            {
                isCs = true;
            }

            totalClasses += csClasses;
            _props.Result += res!;
        }

        if (!_props.DisableGdScript && !isCs)
        {
            GdTestExecutor gdExecutor = new(_props);
            int gdClasses = gdExecutor.Execute(out TestResult? res);

            if (gdClasses == -1)
            {
                return;
            }

            totalClasses += gdClasses;
            _props.Result += res!;
        }

        _props.Result.TotalTime = (DateTime.Now - startTimeStamp).TotalSeconds;
        _props.Result.TotalClasses = (uint)totalClasses;

        if (_props.MonitorOrphans)
        {
            // TODO: Handle this better.
            // There is a chance that the number of orphans
            // at the end of testing will be lower than before testing began.
            // This must be handled accordingly,
            // as the number of orphans cannot be negative.
            _props.Result.TotalOrphans += (uint)Math.Max(
                GetOrphans() - startOrphansCount,
                0
            );
        }

        if ((_props.OutputType & ELogOutputType.Log) == ELogOutputType.Log)
        {
            PrintTestLogs();
            PrintSummary();
        }

        if ((_props.OutputType & ELogOutputType.Json) == ELogOutputType.Json)
        {
            DumpJson();
        }
    }

    private static int GetOrphans()
    {
        return (int)Godot.Performance.GetMonitor(
            Godot.Performance.Monitor.ObjectOrphanNodeCount
        );
    }

    private static void PrintTestLogs()
    {
        foreach (TestLog log in _props.Result.TestLogs)
        {
            log.PrintOutput(_props.IsVerbose);
        }
    }

    private static void PrintSummary()
    {
        Log.PrintLine(
            string.Format(
                CultureInfo.InvariantCulture,
                "\nConfirma ran {0} tests in {1} test classes. Tests took {2}s.\n{3}, {4}, {5}, {6}{7}.",
                _props.Result.TotalTests,
                _props.Result.TotalClasses,
                _props.Result.TotalTime,
                Colors.ColorText($"{_props.Result.TestsPassed} passed", Colors.Success),
                Colors.ColorText($"{_props.Result.TestsFailed} failed", Colors.Error),
                Colors.ColorText($"{_props.Result.TestsIgnored} ignored", Colors.Warning),
                _props.MonitorOrphans
                    ? Colors.ColorText($"{_props.Result.TotalOrphans} orphans, ", Colors.Warning)
                    : string.Empty,
                Colors.ColorText($"{_props.Result.Warnings} warnings", Colors.Warning)
            )
        );
    }

    public static void DumpJson()
    {
        bool success = true;

        // This weird wrapper is needed because of this error:
        // https://github.com/godotengine/godot/issues/94510.
        Task task = Task.Run(
            async () => success = await Json.DumpToFileAsync(
                _props.OutputPath,
                _props.Result,
                true
            )
        );
        task.Wait();

        if (!success)
        {
            Log.PrintError(
                $"Dumping results to JSON file in '{_props.OutputPath}' failed.\n"
            );
            _props.Autoload!.GetTree().Quit(1);
        }
    }
}
