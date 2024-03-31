using Godot;
using YAT.Classes.Managers;

namespace YAT.Scenes;

public partial class YatEnable : Node
{
	[Export(PropertyHint.Flags, "UserData,CurrentDirectory,CmdArgument")]
	public short YatEnableAction { get; set; } = 0;

#nullable disable
	[Export] public PreferencesManager PreferencesManager { get; set; }
#nullable restore

	public bool YatEnabled { get; set; } = true;

	public override void _Ready()
	{
		if (!PreferencesManager.Preferences.UseYatEnableFile) return;

		var path = PreferencesManager.Preferences.YatEnableLocation switch
		{
			Enums.EYatEnableLocation.UserData => "user://",
			Enums.EYatEnableLocation.CurrentDirectory => "res://",
			_ => "user://"
		};

		YatEnabled = FileAccess.FileExists(path + PreferencesManager.Preferences.YatEnableFile);
	}
}
