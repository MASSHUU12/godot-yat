using System;
using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;
using static Godot.GodotObject;

namespace YAT.Commands;

[Command("sc", "Makes a screenshot.")]
[Usage("sc")]
[Option("-cp", "bool", "Copy the screenshot to the clipboard.")]
[Option(
	"-path",
	"string",
	"Save the screenshot to the specified path. If not specified, the screenshot will be saved to the current directory."
)]
[Option("-name", "string", "Name of the screenshot file.")]
[Option("-keepOpen", "bool", "Do not close the terminal window.")]
[Option("-e", "exr|jpg|png|webp", "The extension of the screenshot file.", "png")]
public sealed class Sc : ICommand
{
	private YAT _yat;
	private BaseTerminal _terminal;

	public CommandResult Execute(CommandData data)
	{
		var cp = (bool)data.Options["-cp"];
		var path = (string)data.Options["-path"] ?? OS.GetExecutablePath().GetBaseDir() + "/";
		var name = (string)data.Options["-name"] ?? $"{((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds()}";
		var keepOpen = (bool)data.Options["-keepOpen"];
		var extension = (string)data.Options["-e"];

		_yat = data.Yat;
		_terminal = data.Terminal;

		// if (cp)
		// {
		// 	OS.Clipboard = image.SavePngToBuffer();
		// }

		if (!keepOpen) data.Yat.TerminalManager.CloseTerminal();

		RenderingServer.Singleton.Connect("frame_post_draw", Callable.From(() =>
		{
			CreateScreenshot(data.Yat.GetViewport(), path, name, extension);
			data.Yat.TerminalManager.OpenTerminal();
		}), (uint)ConnectFlags.OneShot);

		return ICommand.Success();
	}

	private void CreateScreenshot(Viewport viewport, string path, string name, string extension)
	{
		var image = viewport.GetTexture().GetImage();
		var fileName = path + name + extension;

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
		}

		if (err != Error.Ok) _terminal.Output.Error($"Error saving the {fileName}");
	}
}
