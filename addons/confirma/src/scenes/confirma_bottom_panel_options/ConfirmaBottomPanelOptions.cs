#if TOOLS

using Confirma.Enums;
using Confirma.Types;
using Godot;
using static Confirma.Enums.ELogOutputType;
using static Confirma.Enums.ERunTargetType;

namespace Confirma.Scenes;

[Tool]
public partial class ConfirmaBottomPanelOptions : Window
{
#nullable disable
    private TreeContent _tree;
    private TreeItem
        _verbose,
        _parallelize,
        _disableOrphansMonitor,
        _category,
        _outputLog,
        _outputJson,
        _outputPath;
    private ConfirmaAutoload _autoload;
#nullable restore

    public override void _Ready()
    {
        CloseRequested += CloseRequest;

        InitializeTree();
        PopulateTree();

        _ = CallDeferred("LateInit");
    }

    private void LateInit()
    {
        _autoload = GetNode<ConfirmaAutoload>("/root/Confirma");

        InitializePanelOptions();
    }

    private void InitializeTree()
    {
        _tree = GetNode<TreeContent>("%TreeContent");
        _ = _tree.AddRoot();
        _tree.ItemEdited += () => UpdateSettings(ref _autoload.Props);
    }

    private void PopulateTree()
    {
        _category = _tree.AddTextInput("Category");
        _verbose = _tree.AddCheckBox("Verbose");
        _parallelize = _tree.AddCheckBox("Disable parallelization");
        _disableOrphansMonitor = _tree.AddCheckBox("Disable orphans monitor");

        TreeItem output = _tree.AddLabel("Output");
        output.Collapsed = true;
        _outputPath = _tree.AddTextInput("Output path", output);

        TreeItem outputType = _tree.AddLabel("Output type", output);
        outputType.Collapsed = true;
        _outputLog = _tree.AddCheckBox("Log", outputType);
        _outputJson = _tree.AddCheckBox("JSON", outputType);
    }

    private void InitializePanelOptions()
    {
        _verbose.SetChecked(1, _autoload.Props.IsVerbose);
        _parallelize.SetChecked(1, _autoload.Props.DisableParallelization);
        _disableOrphansMonitor.SetChecked(1, !_autoload.Props.MonitorOrphans);

        RunTarget target = _autoload.Props.Target;
        _category.SetText(1,
            target.Target == Category ? target.Name : string.Empty
        );

        _outputPath.SetText(1, _autoload.Props.OutputPath);
        _outputLog.SetChecked(1, (_autoload.Props.OutputType & Log) == Log);
        _outputJson.SetChecked(1,
            (_autoload.Props.OutputType & ELogOutputType.Json)
            == ELogOutputType.Json
        );
    }

    // TODO: Find a way to detect only items that have changed
    // without having to refresh all of them.
    private void UpdateSettings(ref TestsProps props)
    {
        props.IsVerbose = _verbose.IsChecked(1);
        props.DisableParallelization = _parallelize.IsChecked(1);
        props.MonitorOrphans = !_disableOrphansMonitor.IsChecked(1);

        // Handling method name (--confirma-method) will be problematic
        // with this approach, as the category name will be mutually exclusive.
        // TODO: Find a better approach.
        string categoryName = _category.GetText(1);
        props.Target = props.Target with
        {
            Target = string.IsNullOrEmpty(categoryName) ? All : Category,
            Name = categoryName
        };

        string path = _outputPath.GetText(1);
        props.OutputPath = string.IsNullOrEmpty(path)
            ? ProjectSettings
                .GetSetting("confirma/config/output_path")
                .AsString()
            : path;

        props.OutputType = _outputLog.IsChecked(1)
            ? props.OutputType | Log
            : props.OutputType & ~Log;

        props.OutputType = _outputJson.IsChecked(1)
            ? props.OutputType | ELogOutputType.Json
            : props.OutputType & ~ELogOutputType.Json;
    }

    private void CloseRequest()
    {
        Hide();
    }
}
#endif
