using Godot;

namespace YAT
{
	public partial class OptionsManager : Node
	{
		private readonly YAT _yat;
		private YatOptions _defaultOptions;

		/// <summary>
		/// <list type="unordered">
		/// <item> The actual directory paths for user:// are: </item>
		/// <item> Windows: %APPDATA%\Godot\app_userdata\[project_name] </item>
		/// <item> Linux: ~/.local/share/godot/app_userdata/[project_name] </item>
		/// <item> macOS: ~/Library/Application Support/Godot/app_userdata/[project_name] </item>
		/// </list>
		/// </summary>
		private const string _optionsPath = "user://yat_options.tres";

		public OptionsManager(YAT yat, YatOptions defaultOptions)
		{
			_yat = yat;
			_defaultOptions = defaultOptions.Duplicate() as YatOptions;
		}

		/// <summary>
		/// Saves the current options to the specified options path.
		/// </summary>
		public void Save()
		{
			switch (ResourceSaver.Save(_yat.Options, _optionsPath))
			{
				case Error.Ok:
					_yat.Terminal.Print("Options saved successfully.");
					break;
				default:
					_yat.Terminal.Print("Failed to save options.", Terminal.PrintType.Error);
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
				_yat.Terminal.Print("Options file does not exist, leaving options unchanged.");
				return;
			}

			_yat.Options = ResourceLoader.Load<YatOptions>(_optionsPath);
			_yat.EmitSignal(nameof(_yat.OptionsChanged), _yat.Options);
			_yat.Terminal.Print("Options loaded successfully.");
		}

		/// <summary>
		/// Restores the options to their default values.
		/// </summary>
		public void RestoreDefaults()
		{
			_yat.Options = _defaultOptions;
			_yat.EmitSignal(nameof(_yat.OptionsChanged), _yat.Options);

			_yat.Terminal.Print("Restored default options.");
		}
	}
}
