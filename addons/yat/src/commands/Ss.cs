using System;
using Godot;
using YAT.Attributes;
using YAT.Classes;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;
using static Godot.GodotObject;
using static YAT.Helpers.OS;

namespace YAT.Commands;

[Command("ss", "Makes a screenshot.")]
[Option("-cp", "bool", "Copy the screenshot to the clipboard.")]
[Option(
    "-path",
    "string",
    "Save the screenshot to the specified path. If not specified, the screenshot will be saved to the current directory."
)]
[Option("-name", "string", "Name of the screenshot file.")]
[Option("-keepOpen", "bool", "Do not close the terminal window.")]
[Option("-e", "exr|jpg|png|webp", "The extension of the screenshot file.", "png")]
public sealed class Ss : ICommand
{
#nullable disable
    private YAT _yat;
    private BaseTerminal _terminal;
#nullable restore

    public CommandResult Execute(CommandData data)
    {
        var cp = (bool)data.Options["-cp"];
        var path = (string)data.Options["-path"] ?? OS.GetExecutablePath().GetBaseDir() + "/";
        var name = (string)data.Options["-name"] ?? $"{((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds()}";
        var keepOpen = (bool)data.Options["-keepOpen"];
        var extension = (string)data.Options["-e"];

        _yat = data.Yat;
        _terminal = data.Terminal;

        if (!keepOpen)
        {
            data.Yat.TerminalManager.CloseTerminal();
        }

        _ = RenderingServer.Singleton.Connect("frame_post_draw", Callable.From(() =>
        {
            if (cp)
            {
                SaveScreenshotToClipboard(data.Yat.GetViewport(), extension);
            }
            else
            {
                SaveScreenshot(data.Yat.GetViewport(), path, name, extension);
            }

            data.Yat.TerminalManager.OpenTerminal();
        }), (uint)ConnectFlags.OneShot);

        return ICommand.Success();
    }

    private void SaveScreenshot(Viewport viewport, string path, string name, string extension)
    {
        Image image = viewport.GetTexture().GetImage();
        string fileName = path + name + extension;

        Error err = Error.Ok;
        switch (extension)
        {
            case "exr":
                err = image.SaveExr(fileName);
                break;
            case "jpg":
                err = image.SaveJpg(fileName);
                break;
            case "png":
                err = image.SavePng(fileName);
                break;
            case "webp":
                err = image.SaveWebp(fileName);
                break;
            default:
                break;
        }

        if (err != Error.Ok)
        {
            _terminal.Output.Error($"Error saving the {fileName}");
        }
        else
        {
            _terminal.Output.Success($"Screenshot saved to {fileName}");
        }
    }

    private void SaveScreenshotToClipboard(Viewport viewport, string extension)
    {
        Image image = viewport.GetTexture().GetImage();

        byte[] buffer = Array.Empty<byte>();
        switch (extension)
        {
            case "exr":
                buffer = image.SaveExrToBuffer();
                break;
            case "jpg":
                buffer = image.SaveJpgToBuffer();
                break;
            case "png":
                buffer = image.SavePngToBuffer();
                break;
            case "webp":
                buffer = image.SaveWebpToBuffer();
                break;
            default:
                break;
        }

        ExecutionResult result = Clipboard.SetImageData(buffer);

        if (result != ExecutionResult.Success)
        {
            _terminal.Output.Error("Error saving the screenshot to the clipboard.");
        }
        else
        {
            _terminal.Output.Success("Screenshot saved to the clipboard.");
        }
    }
}
