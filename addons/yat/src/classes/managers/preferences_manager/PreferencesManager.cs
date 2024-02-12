using Godot;
using YAT.Resources;

namespace YAT.Classes.Managers;

public partial class PreferencesManager : Node
{
	[Signal] public delegate void PreferencesUpdatedEventHandler(YatPreferences preferences);

	[Export] public YatPreferences Preferences { get; set; } = new();

	private const string PREFERENCES_PATH = "user://yat_preferences.tres";

	public override void _Ready()
	{
		GD.Print(Load() ? "Preferences loaded." : "Preferences not loaded.");
	}

	public bool Save()
	{
		return ResourceSaver.Save(Preferences, PREFERENCES_PATH) == Error.Ok;
	}

	public bool Load()
	{
		if (!ResourceLoader.Exists(PREFERENCES_PATH)) return false;

		Preferences = ResourceLoader.Load<YatPreferences>(PREFERENCES_PATH);
		EmitSignal(nameof(PreferencesUpdated), Preferences);

		return true;
	}
}
