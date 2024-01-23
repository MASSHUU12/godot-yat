using System;
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
			CommandManager.AddCommand(new Type[] {
				typeof(Ls),
				typeof(Ip),
				typeof(Cn),
				typeof(Cs),
				typeof(Cls),
				typeof(Man),
				typeof(Set),
				typeof(Cat),
				typeof(Sys),
				typeof(Quit),
				typeof(Echo),
				typeof(List),
				typeof(View),
				typeof(Ping),
				typeof(Wenv),
				typeof(Pause),
				typeof(Watch),
				typeof(Stats),
				typeof(Reset),
				typeof(Cowsay),
				// typeof(Options),
				typeof(Restart),
				typeof(History),
				typeof(Whereami),
				typeof(Timescale),
				typeof(ToggleAudio),
				typeof(Commands.QuickCommands)
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
