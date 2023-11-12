using Godot;
using YAT.Attributes;

namespace YAT.Commands
{
	[Command("options", "Creates a window with the available options.", "[b]Usage[/b]: options", "opts")]
	public partial class Options : ICommand
	{
		public YAT Yat { get; set; }

		private static Node _optionsWindowInstance;
		private static readonly PackedScene _optionsWindow = GD.Load<PackedScene>(
			"res://addons/yat/src/overlay/components/options_window/OptionsWindow.tscn"
		);

		public Options(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(params string[] args)
		{
			var instanceValid = GodotObject.IsInstanceValid(_optionsWindowInstance);

			if (instanceValid)
			{
				_optionsWindowInstance.QueueFree();
				_optionsWindowInstance = null;
				return CommandResult.Success;
			}

			_optionsWindowInstance = instanceValid ? _optionsWindowInstance : _optionsWindow.Instantiate();
			Yat.Overlay.AddChild(_optionsWindowInstance);

			return CommandResult.Success;
		}
	}
}
