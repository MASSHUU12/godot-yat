using Godot;
using YAT.Attributes;
using YAT.Commands;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Scenes.CommandManager;
using YAT.Scenes.TerminalManager;

namespace YAT
{
	public partial class YAT : Node
	{
		[Signal]
		public delegate void OptionsChangedEventHandler(YatOptions options);
		[Signal] public delegate void YatReadyEventHandler();

		[Export] public YatOptions Options { get; set; } = new();

		public bool YatEnabled = true;

		public Node Windows { get; private set; }

		public OptionsManager OptionsManager { get; private set; }
		public CommandManager CommandManager { get; private set; }
		public TerminalManager TerminalManager { get; private set; }

		public override void _Ready()
		{
			CheckYatEnableSettings();

			TerminalManager = GetNode<TerminalManager>("./TerminalManager");
			TerminalManager.GameTerminal.Ready += () =>
			{
				Log.Terminal = TerminalManager.GameTerminal.BaseTerminal;
				OptionsManager.Load();

				EmitSignal(SignalName.YatReady);
			};

			Windows = GetNode<Node>("./Windows");
			OptionsManager = new(this, Options);
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
				new Options(),
				new Restart(),
				new History(),
				new Whereami(),
				new Timescale(),
				new ToggleAudio(),
				new QuickCommands()
			});

			Keybindings.LoadDefaultActions();
		}

		private void CheckYatEnableSettings()
		{
			if (!Options.UseYatEnableFile) return;

			var path = Options.YatEnableLocation switch
			{
				YatOptions.YatEnableFileLocation.UserData => "user://",
				YatOptions.YatEnableFileLocation.CurrentDirectory => "res://",
				_ => "user://"
			};

			YatEnabled = FileAccess.FileExists(path + Options.YatEnableFile);
		}
	}
}
