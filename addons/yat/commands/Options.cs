using Godot;

public partial class Options : IYatCommand
{
	public string Name => "options";

	public string Description => "Creates a window with the available options.";

	public string Usage => "options";

	public string[] Aliases => new[] { "opts" };

	private static Node _optionsWindowInstance;
	private static readonly PackedScene _optionsWindow = GD.Load<PackedScene>(
		"res://globals/yat/yat_overlay/components/yat_options_window/YatOptionsWindow.tscn"
	);

	public void Execute(string[] args, YAT yat)
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
