using System.Collections.Generic;
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
		public Dictionary<string, ICommand> Commands { get; private set; } = new();

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

			AddCommand(new Ls());
			AddCommand(new Ip());
			AddCommand(new Cn());
			AddCommand(new Cs());
			AddCommand(new Cls());
			AddCommand(new Man());
			AddCommand(new Set());
			AddCommand(new Cat());
			AddCommand(new Sys());
			AddCommand(new Quit());
			AddCommand(new Echo());
			AddCommand(new List());
			AddCommand(new View());
			AddCommand(new Ping());
			AddCommand(new Wenv());
			AddCommand(new Pause());
			AddCommand(new Watch());
			AddCommand(new Stats());
			AddCommand(new Reset());
			AddCommand(new Cowsay());
			AddCommand(new Options());
			AddCommand(new Restart());
			AddCommand(new History());
			AddCommand(new Whereami());
			AddCommand(new Timescale());
			AddCommand(new ToggleAudio());
			AddCommand(new QuickCommands());

			Keybindings.LoadDefaultActions();
		}

		/// <summary>
		/// Adds a CLICommand to the list of available commands.
		/// </summary>
		/// <param name="command">The CLICommand to add.</param>
		public void AddCommand(ICommand command)
		{
			if (AttributeHelper.GetAttribute<CommandAttribute>(command)
				is not CommandAttribute attribute)
			{
				Log.Error(Messages.MissingAttribute("CommandAttribute", command.GetType().Name));
				return;
			}

			Commands[attribute.Name] = command;
			foreach (string alias in attribute.Aliases)
			{
				Commands[alias] = command;
			}
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
