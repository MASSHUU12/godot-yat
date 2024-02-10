using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Scenes.YatWindow;
using YAT.Types;

namespace YAT.Commands;

[Command("options", "Creates a window with the available options.", "[b]Usage[/b]: options", "opts")]
public sealed class Options : ICommand
{
	private static YatWindow _optionsWindowInstance;
	private static readonly PackedScene _optionsWindow = GD.Load<PackedScene>(
		"uid://liha0ppubuti"
	);

	public CommandResult Execute(CommandData data)
	{
		var instanceValid = GodotObject.IsInstanceValid(_optionsWindowInstance);

		if (instanceValid)
		{
			CloseWindow();
			_optionsWindowInstance = null;
			return CommandResult.Success;
		}

		data.Terminal.Print("Options will be improved in the future.", EPrintType.Warning);

		_optionsWindowInstance = instanceValid ? _optionsWindowInstance : _optionsWindow.Instantiate<YatWindow>();
		data.Yat.Windows.AddChild(_optionsWindowInstance);

		return CommandResult.Success;
	}

	private static void CloseWindow()
	{
		_optionsWindowInstance.QueueFree();
		_optionsWindowInstance = null;
	}
}
