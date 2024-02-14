using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("preferences", "Creates a window with the available preferences.", "[b]Usage[/b]: options", "prefs")]
public sealed class Preferences : ICommand
{
	private static Scenes.Preferences _windowInstance;
	private static readonly PackedScene _prefsWindow = GD.Load<PackedScene>(
		"uid://ca2i4r24ny7y3"
	);

	public ECommandResult Execute(CommandData data)
	{
		var instanceValid = GodotObject.IsInstanceValid(_windowInstance);

		if (instanceValid)
		{
			CloseWindow();
			return ECommandResult.Success;
		}

		_windowInstance = _prefsWindow.Instantiate<Scenes.Preferences>();
		data.Yat.Windows.AddChild(_windowInstance);

		return ECommandResult.Success;
	}

	private static void CloseWindow()
	{
		_windowInstance.QueueFree();
		_windowInstance = null;
	}
}
