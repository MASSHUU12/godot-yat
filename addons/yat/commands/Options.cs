using Godot;

namespace YAT
{
	[Command("options", "Creates a window with the available options.", "[b]Usage[/b]: options", "opts")]
	public partial class Options : ICommand
	{
		private static Node _optionsWindowInstance;
		private static readonly PackedScene _optionsWindow = GD.Load<PackedScene>(
			"res://addons/yat/overlay/components/yat_options_window/YatOptionsWindow.tscn"
		);

		public void Execute(YAT yat, params string[] args)
		{
			var instanceValid = GodotObject.IsInstanceValid(_optionsWindowInstance);

			if (instanceValid)
			{
				_optionsWindowInstance.QueueFree();
				_optionsWindowInstance = null;
				return;
			}

			_optionsWindowInstance = instanceValid ? _optionsWindowInstance : _optionsWindow.Instantiate();
			yat.Overlay.AddChild(_optionsWindowInstance);
		}
	}
}
