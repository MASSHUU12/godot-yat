using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("preferences", aliases: "prefs")]
[Usage("prefs")]
[Description("Creates a window with the available preferences.")]
public sealed class Preferences : ICommand
{
	private static Scenes.Preferences _windowInstance;
	private static readonly PackedScene _prefsWindow = GD.Load<PackedScene>(
		"uid://ca2i4r24ny7y3"
	);

	public CommandResult Execute(CommandData data)
	{
		var instanceValid = GodotObject.IsInstanceValid(_windowInstance);

		if (instanceValid)
		{
			CloseWindow();
			return ICommand.Success();
		}

		_windowInstance = _prefsWindow.Instantiate<Scenes.Preferences>();
		data.Yat.Windows.AddChild(_windowInstance);

		return ICommand.Success();
	}

	private static void CloseWindow()
	{
		_windowInstance.QueueFree();
		_windowInstance = null;
	}
}
