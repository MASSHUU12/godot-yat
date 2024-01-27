using Godot;

namespace YAT.Scenes.OptionsManager;

public partial class OptionsManager : Node
{
	[Signal]
	public delegate void OptionsChangedEventHandler(YatOptions options);

	[Export] public YatOptions Options { get; set; } = new();

	private YAT _yat;
	private YatOptions _defaultOptions;

	public override void _Ready()
	{
		_yat = GetNode<YAT>("/root/YAT");
		_defaultOptions = Options.Duplicate() as YatOptions;
	}

	/// <summary>
	/// <list type="unordered">
	/// <item> The actual directory paths for user:// are: </item>
	/// <item> Windows: %APPDATA%\Godot\app_userdata\[project_name] </item>
	/// <item> Linux: ~/.local/share/godot/app_userdata/[project_name] </item>
	/// <item> macOS: ~/Library/Application Support/Godot/app_userdata/[project_name] </item>
	/// </list>
	/// </summary>
	private const string _optionsPath = "user://yat_options.tres";

	/// <summary>
	/// Saves the current options to the specified options path.
	/// </summary>
	public void Save()
	{
		switch (ResourceSaver.Save(Options, _optionsPath))
		{
			case Error.Ok:
				_yat.CurrentTerminal.Output.Print("Options saved successfully.");
				break;
			default:
				_yat.CurrentTerminal.Output.Error("Failed to save options.");
				break;
		}
	}

	/// <summary>
	/// Loads the YatOptions from the specified options file path.
	/// </summary>
	public void Load()
	{
		if (!ResourceLoader.Exists(_optionsPath))
		{
			_yat.CurrentTerminal.Output.Print("Options file does not exist, leaving options unchanged.");
			return;
		}

		Options = ResourceLoader.Load<YatOptions>(_optionsPath);
		EmitSignal(SignalName.OptionsChanged, Options);
	}

	/// <summary>
	/// Restores the options to their default values.
	/// </summary>
	public void RestoreDefaults()
	{
		Options = _defaultOptions;
		EmitSignal(SignalName.OptionsChanged, Options);

		_yat.CurrentTerminal.Output.Success("Restored default options.");
	}
}
