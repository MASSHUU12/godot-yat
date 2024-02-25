using Godot;
using YAT.Helpers;
using YAT.Scenes;
using YAT.Classes.Managers;

namespace YAT;

public partial class YAT : Node
{
	[Signal] public delegate void YatReadyEventHandler();

	public bool YatEnabled = true;
	public Node Windows { get; private set; }
	public BaseTerminal CurrentTerminal { get; set; }

	public DebugScreen DebugScreen { get; private set; }
	public RegisteredCommands Commands { get; private set; }
	public TerminalManager TerminalManager { get; private set; }
	public PreferencesManager PreferencesManager { get; private set; }

	public override void _Ready()
	{
		Windows = GetNode<Node>("./Windows");
		DebugScreen = GetNode<DebugScreen>("./Windows/DebugScreen");
		Commands = GetNode<RegisteredCommands>("./RegisteredCommands");
		PreferencesManager = GetNode<PreferencesManager>("%PreferencesManager");

		TerminalManager = GetNode<TerminalManager>("./TerminalManager");
		TerminalManager.GameTerminal.Ready += () =>
		{
			TerminalManager.GameTerminal.TerminalSwitcher.CurrentTerminalChanged +=
			(BaseTerminal terminal) =>
			{
				CurrentTerminal = terminal;
			};

			EmitSignal(SignalName.YatReady);
		};

		Keybindings.LoadDefaultActions();

		CheckYatEnableSettings();
	}

	private void CheckYatEnableSettings()
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
