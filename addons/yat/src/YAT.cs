using Godot;
using YAT.Commands;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Scenes.BaseTerminal;
using YAT.Scenes.CommandManager;
using YAT.Scenes.OptionsManager;
using YAT.Scenes.TerminalManager;

namespace YAT
{
	public partial class YAT : Node
	{
		[Signal] public delegate void YatReadyEventHandler();

		public bool YatEnabled = true;
		public Node Windows { get; private set; }
		public BaseTerminal CurrentTerminal { get; set; }

		public OptionsManager OptionsManager { get; private set; }
		public CommandManager CommandManager { get; private set; }
		public TerminalManager TerminalManager { get; private set; }

		public override void _Ready()
		{
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

			Windows = GetNode<Node>("./Windows");

			CommandManager = GetNode<CommandManager>("./CommandManager");
			CommandManager.AddCommand(new ICommand[] {
				new Ls(),
				new Ip(),
				new Cn(),
				new Cs(),
				new Cls(),
				new Man(),
				new Set(),
				new Cat(),
				new Sys(),
				new Quit(),
				new Echo(),
				new List(),
				new View(),
				new Ping(),
				new Wenv(),
				new Pause(),
				new Watch(),
				new Stats(),
				new Reset(),
				new Cowsay(),
				// new Options(),
				new Restart(),
				new History(),
				new Whereami(),
				new Timescale(),
				new ToggleAudio(),
				new Commands.QuickCommands()
			});

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
}
