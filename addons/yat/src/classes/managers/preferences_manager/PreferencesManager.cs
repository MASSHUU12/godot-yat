using Godot;
using YAT.Resources;

namespace YAT.Classes.Managers;

public partial class PreferencesManager : Node
{
	[Signal] public delegate void PreferencesUpdatedEventHandler(YatPreferences preferences);

	[Export] public YatPreferences Preferences { get; set; } = new();

	private const string PREFERENCES_PATH = "user://yat_preferences.tres";
#nullable disable
	private YatPreferences _defaultPreferences;
#nullable restore

	public override void _Ready()
	{
		_defaultPreferences = Preferences.Duplicate() as YatPreferences;

		Load();
	}

	public void RestoreDefaults()
	{
		Preferences = (_defaultPreferences.Duplicate() as YatPreferences)!;
		EmitSignal(SignalName.PreferencesUpdated, Preferences!);
	}

	public bool Save()
	{
		return ResourceSaver.Save(Preferences, PREFERENCES_PATH) == Error.Ok;
	}

	public bool Load()
	{
		if (!ResourceLoader.Exists(PREFERENCES_PATH)) return false;

		Preferences = ResourceLoader.Load<YatPreferences>(
			PREFERENCES_PATH,
			cacheMode: ResourceLoader.CacheMode.Replace
		);
		EmitSignal(SignalName.PreferencesUpdated, Preferences);

		return true;
	}
}
