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
    private Button _update, _cancel;
#nullable restore

    public override void _Ready()
    {
        _cancel = GetNode<Button>("%Cancel");
        _cancel.Pressed += QueueFree;

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
        _cancel.Disabled = disabled;
    }

    private async Task Update()
    {
        _output.Text += $"Downloading ZIP from {UpdateInfo.ZipballUrl}...\n";

        string? path = await Updater.DownloadZipAsync(UpdateInfo.ZipballUrl);

        if (string.IsNullOrEmpty(path))
        {
            _output.Text += "Downloading ZIP failed.\n";
            return;
        }

        _output.Text += $"Downloaded ZIP to {path}\n";
    }
}
