#if TOOLS
using System.Collections.Generic;
using System.IO;
using Confirma.Classes;
using Confirma.Enums;
using Confirma.Helpers;
using Confirma.Terminal;
using Confirma.Types;
using Godot;

namespace Confirma.Scenes;

[Tool]
public partial class ConfirmaAutoload : Node
{
    public TestsProps Props = new();

    private bool _usedConfirmaApi;
    private readonly Cli _cli = new("--confirma-");

    public override void _Ready()
    {
        Initialize();

        if (!ParseArguments())
        {
            Props.RunTests = false;
            GetTree().Quit(1);
            return;
        }

        Props.Autoload = this;
        SetupGlobals();

        if (!Engine.IsEditorHint())
        {
            ChangeScene();
        }
    }

    private void SetupGlobals()
    {
        Log.IsHeadless = Props.IsHeadless;
        Global.Root = GetTree().Root;
    }

    private void Initialize()
    {
        if (DisplayServer.GetName() == "headless")
        {
            Props.IsHeadless = true;
        }

        _ = _cli.RegisterArgument(
            new(
                "run",
                allowEmpty: true,
                action: (value) =>
                {
                    string name = (string)value;

                    Props.RunTests = true;
                    Props.Target = Props.Target with
                    {
                        Target = string.IsNullOrEmpty(name)
                            ? ERunTargetType.All
                            : ERunTargetType.Class,
                        Name = name
                    };
                }
            ),
            new(
                "help",
                allowEmpty: true,
                action: (value) =>
                {
                    string name = (string)value;

                    Props.ShowHelp = true;
                    Props.SelectedHelpPage = string.IsNullOrEmpty(name)
                        ? "default"
                        : name;
                }
            ),
            new(
                "method",
                action: (value) =>
                {
                    if (string.IsNullOrEmpty(Props.Target.Name))
                    {
                        Log.PrintError(
                            "Invalid value: argument '--confirma-run' cannot be empty"
                            + " when using argument '--confirma-method'.\n"
                        );
                        Props.RunTests = false;
                        return;
                    }

                    Props.Target = Props.Target with
                    {
                        Target = ERunTargetType.Method,
                        DetailedName = (string)value
                    };
                }
            ),
            new(
                "category",
                action: (value) =>
                {
                    Props.Target = Props.Target with
                    {
                        Target = ERunTargetType.Category,
                        Name = (string)value
                    };
                }
            ),
            new(
                "exit-on-failure",
                isFlag: true,
                action: (_) => Props.ExitOnFail = true
            ),
            new(
                "verbose",
                isFlag: true,
                action: (_) => Props.IsVerbose = true
            ),
            new(
                "sequential",
                isFlag: true,
                action: (_) => Props.DisableParallelization = true
            ),
            new(
                "disable-orphans-monitor",
                isFlag: true,
                action: (_) => Props.MonitorOrphans = false
            ),
            new(
                "disable-cs",
                isFlag: true,
                action: (_) => Props.DisableCsharp = true
            ),
            new(
                "disable-gd",
                isFlag: true,
                action: (_) => Props.DisableGdScript = true
            ),
            new(
                "gd-path",
                allowEmpty: false,
                action: (value) => Props.GdTestPath = (string)value
            ),
            new(
                "output",
                action: (value) =>
                {
                    if (!EnumHelper.TryParseFlagsEnum(
                        (string)value,
                        out ELogOutputType type
                    ))
                    {
                        Log.PrintError(
                            $"Invalid value '{value}' for '--confirma-output' argument.\n"
                        );
                        Props.RunTests = false;
                        return;
                    }

                    Props.OutputType = type;
                }
            ),
            new(
                "output-path",
                action: (value) =>
                {
                    string path = (string)value;

                    if (!Path.Exists(Path.GetDirectoryName(path))
                        || Path.GetExtension(path) != ".json"
                    )
                    {
                        Log.PrintError($"Invalid output path: {path}.\n");
                        Props.RunTests = false;
                        return;
                    }

                    Props.OutputPath = path;
                }
            )
        );
    }

    private bool ParseArguments()
    {
        List<string> errors = _cli.Parse(OS.GetCmdlineUserArgs(), true);

        _usedConfirmaApi = _cli.GetValuesCount() != 0;

        if (errors.Count == 0)
        {
            return true;
        }

        foreach (string error in errors)
        {
            Log.PrintError(error + "\n");
        }

        return false;
    }

    private void ChangeScene()
    {
        if (Props.ShowHelp)
        {
            _ = GetTree().CallDeferred("change_scene_to_file",
            $"{Plugin.GetPluginLocation()}src/scenes/help_panel/help_panel.tscn");
            return;
        }

        if (!Props.RunTests)
        {
            if (_usedConfirmaApi)
            {
                Log.PrintWarning(
                    "You're trying to use Confirma without '--confirma-run' argument."
                    + " The game continues normally.\n"
                );
            }
            return;
        }

        _ = GetTree().CallDeferred("change_scene_to_file", "uid://cq76c14wl2ti3");

        if (!Engine.IsEditorHint())
        {
            GetTree().Quit();
        }
    }
}
#endif
