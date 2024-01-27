using Godot;
using YAT.Helpers;
using YAT.Scenes.BaseTerminal;
using YAT.Scenes.OptionsManager;
using YAT.Scenes.TerminalManager;
using YAT.Scenes;

namespace YAT;

public partial class YAT : Node
{
	[Signal] public delegate void YatReadyEventHandler();

	public bool YatEnabled = true;
	public Node Windows { get; private set; }
	public BaseTerminal CurrentTerminal { get; set; }

	public RegisteredCommands Commands { get; private set; }
	public OptionsManager OptionsManager { get; private set; }
	public TerminalManager TerminalManager { get; private set; }

	public override void _Ready()
	{
		Windows = GetNode<Node>("./Windows");
		Commands = GetNode<RegisteredCommands>("./RegisteredCommands");

		// TODO: Clean up this mess.
		OptionsManager = GetNode<OptionsManager>("./OptionsManager");
		OptionsManager.OptionsChanged += (YatOptions options) =>
		{
			if (options.UseYatEnableFile) CheckYatEnableSettings();
		};

		TerminalManager = GetNode<TerminalManager>("./TerminalManager");
		TerminalManager.GameTerminal.Ready += () =>
		{
			TerminalManager.GameTerminal.TerminalSwitcher.TerminalSwitcherInitialized += () =>
			{
				CurrentTerminal = TerminalManager.GameTerminal.TerminalSwitcher.CurrentTerminal;
				OptionsManager.CallDeferred(nameof(OptionsManager.Load));
				CallDeferred(nameof(CheckYatEnableSettings));

			};
			TerminalManager.GameTerminal.TerminalSwitcher.CurrentTerminalChanged +=
			(BaseTerminal terminal) =>
			{
				CurrentTerminal = terminal;
			};

			EmitSignal(SignalName.YatReady);
		};

		Keybindings.LoadDefaultActions();
	}

	private void CheckYatEnableSettings()
	{
		if (!OptionsManager.Options.UseYatEnableFile) return;

		var path = OptionsManager.Options.YatEnableLocation switch
		{
			YatOptions.YatEnableFileLocation.UserData => "user://",
			YatOptions.YatEnableFileLocation.CurrentDirectory => "res://",
			_ => "user://"
		};

		YatEnabled = FileAccess.FileExists(path + OptionsManager.Options.YatEnableFile);
	}
}
