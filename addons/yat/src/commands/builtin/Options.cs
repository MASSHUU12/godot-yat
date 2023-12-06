using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Scenes.Terminal;
using YAT.Scenes.YatWindow;

namespace YAT.Commands
{
	[Command("options", "Creates a window with the available options.", "[b]Usage[/b]: options", "opts")]
	public partial class Options : ICommand
	{
		public YAT Yat { get; set; }

		private static YatWindow _optionsWindowInstance;
		private static readonly PackedScene _optionsWindow = GD.Load<PackedScene>(
			"res://addons/yat/src/scenes/settings_window/SettingsWindow.tscn"
		);

		public Options(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(params string[] args)
		{
			var instanceValid = GodotObject.IsInstanceValid(_optionsWindowInstance);

			if (instanceValid)
			{
				CloseWindow();
				_optionsWindowInstance = null;
				return CommandResult.Success;
			}

			Yat.Terminal.Print("Options will be improved in the future.", Terminal.PrintType.Warning);

			_optionsWindowInstance = instanceValid ? _optionsWindowInstance : _optionsWindow.Instantiate<YatWindow>();
			Yat.Windows.AddChild(_optionsWindowInstance);

			return CommandResult.Success;
		}

		private static void CloseWindow()
		{
			_optionsWindowInstance.QueueFree();
			_optionsWindowInstance = null;
		}
	}
}
