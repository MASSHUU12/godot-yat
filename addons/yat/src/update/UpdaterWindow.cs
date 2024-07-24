using System.IO;
using System.Threading.Tasks;
using Godot;
using YAT.Classes;
using YAT.Helpers;
using YAT.Types;

namespace YAT.Update;

[Tool]
public partial class UpdaterWindow : Window
{
#nullable disable
    public SemanticVersion CurrentVersion { get; set; }
    public ReleaseTagInfo UpdateInfo { get; set; }

    private RichTextLabel _output;
    private Button _update, _close;
#nullable restore

    public override void _Ready()
    {
        if (CurrentVersion is null || UpdateInfo is null)
        {
            return;
        }

        _close = GetNode<Button>("%Close");
        _close.Pressed += QueueFree;

        _update = GetNode<Button>("%Update");
        _update.Pressed += async () =>
        {
            ToggleButtons(true);
            await Update();
            ToggleButtons(false);
        };

        _output = GetNode<RichTextLabel>("%Output");
        _output.MetaClicked += MetaClicked;

        CloseRequested += QueueFree;

        PrepareInitialState();
    }

    private void MetaClicked(Variant meta)
    {
        _ = Godot.OS.ShellOpen(meta.ToString());
    }

    private void PrepareInitialState()
    {
        Title = $"YAT Updater {CurrentVersion} to {UpdateInfo.Version}";

        _output.Text = $"YAT {UpdateInfo.Version} is available.\n";
        _output.Text += "\nErrors happen, many things can go wrong during an upgrade."
            + " It is recommended to use a version control system.\n";
        _output.Text += "If you don't use it, you can download the new version of YAT "
            + $"[url={UpdateInfo.ZipballUrl}]here[/url].\n";
    }

    private void ToggleButtons(bool disabled)
    {
        _update.Disabled = disabled;
        _close.Disabled = disabled;
    }

    private async Task Update()
    {
        _output.Text += $"\nDownloading a ZIP file from {UpdateInfo.ZipballUrl}...\n";

        string? zipPath = await Updater.DownloadZipAsync(UpdateInfo.ZipballUrl);

        if (string.IsNullOrEmpty(zipPath))
        {
            _output.Text += "The download of the ZIP file ended with a failure.\n";
            return;
        }

        _output.Text += $"Downloaded ZIP file to {zipPath}\n";
        _output.Text += "Attempting to remove the old version of YAT.\n";

        if (!Updater.DeleteCurrentVersion())
        {
            _output.Text += "An attempt to remove the old version of YAT was unsuccessful.\n";
            return;
        }

        _output.Text += "The old version of YAT was removed successfully.\n";

        string pluginPath = Path.GetFullPath(ProjectSettings.GlobalizePath(Updater.GetPluginPath()[6..]));

        _output.Text += $"Extracting the new version of YAT from the ZIP file to {pluginPath}.\n";

        if (!ZipExtractor.ExtractFolderFromZipFile(zipPath, pluginPath, "yat"))
        {
            _output.Text += "There was an error while exporting the ZIP file, try again or update YAT manually.\n";
            return;
        }

        _output.Text += "YAT has been updated successfully, in 5 seconds Godot will be restarted.\n";

        _ = await ToSignal(GetTree().CreateTimer(5f), SceneTreeTimer.SignalName.Timeout);

        _output.Text += "Restart";

        EditorInterface.Singleton.RestartEditor(true);
    }
}
