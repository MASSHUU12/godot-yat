using Godot;

namespace YAT
{
    public partial class OptionsManager : Node
    {
        private readonly YAT _yat;

        /// <summary>
        /// <list type="unordered">
        /// <item> The actual directory paths for user:// are: </item>
        /// <item> Windows: %APPDATA%\Godot\app_userdata\[project_name] </item>
        /// <item> Linux: ~/.local/share/godot/app_userdata/[project_name] </item>
        /// <item> macOS: ~/Library/Application Support/Godot/app_userdata/[project_name] </item>
        /// </list>
        /// </summary>
        private const string _optionsPath = "user://yat_options.tres";

        public OptionsManager(YAT yat)
        {
            _yat = yat;
        }

        /// <summary>
        /// Saves the current options to the specified options path.
        /// </summary>
        public void Save()
        {
            switch (ResourceSaver.Save(_yat.Options, _optionsPath))
            {
                case Error.Ok:
                    _yat.Terminal.Println("Options saved successfully.");
                    GD.Print("Options saved successfully.");
                    break;
                default:
                    _yat.Terminal.Println("Failed to save options.");
                    GD.PrintErr("Failed to save options.");
                    break;
            }
        }

        /// <summary>
        /// Loads the YatOptions from the specified options file path.
        /// </summary>
        public void Load()
        {
            if (ResourceLoader.Exists(_optionsPath))
            {
                _yat.Options = ResourceLoader.Load<YatOptions>(_optionsPath);
                _yat.EmitSignal(nameof(_yat.OptionsChanged), _yat.Options);

                _yat.Terminal.Println("Options loaded successfully.");
                GD.Print("Options loaded successfully.");

                return;
            }

            _yat.Terminal.Println("Options file does not exist, leaving options unchanged.");
            GD.Print("Options file does not exist, leaving options unchanged.");
        }
    }
}
