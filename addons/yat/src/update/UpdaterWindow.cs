using System.IO;
using System.Threading.Tasks;
using Godot;
using YAT.Classes;
using YAT.Types;

namespace YAT.Update;

[Tool]
public partial class UpdaterWindow : Window
{
#nullable disable
    public SemanticVersion CurrentVersion { get; set; }
    public ReleaseTagInfo UpdateInfo { get; set; }

    private TextEdit _output;
    private Button _update, _close;
#nullable restore

    public override void _Ready()
    {
        _close = GetNode<Button>("%Close");
        _close.Pressed += QueueFree;

        _update = GetNode<Button>("%Update");
        _update.Pressed += async () =>
        {
            ToggleButtons(true);
            await Update();
            ToggleButtons(false);
        };

        _output = GetNode<TextEdit>("%Output");

        CloseRequested += QueueFree;

        PrepareInitialState();
    }

    private void PrepareInitialState()
    {
        Title = $"YAT Updater {CurrentVersion} to {UpdateInfo.Version}";

        _output.Text = $"YAT {UpdateInfo.Version} is available.\n";
    }

    private void ToggleButtons(bool disabled)
    {
        _update.Disabled = disabled;
        _close.Disabled = disabled;
    }

    private async Task Update()
    {
        _output.Text += $"Downloading a ZIP file from {UpdateInfo.ZipballUrl}...\n";

        string? path = await Updater.DownloadZipAsync(UpdateInfo.ZipballUrl);

        if (string.IsNullOrEmpty(path))
        {
            _output.Text += "The download of the ZIP file ended with a failure.\n";
            return;
        }

        _output.Text += $"Downloaded ZIP file to {path}\n";
        _output.Text += "Attempting to remove the old version of YAT.\n";

        if (!Updater.DeleteCurrentVersion())
        {
            _output.Text += "An attempt to remove the old version of YAT was unsuccessful.\n";
            return;
        }

        _output.Text += "The old version of YAT was removed successfully.\n";
    }
}
