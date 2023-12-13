using System.Collections.Generic;
using Godot;
using YAT.Attributes;
using YAT.Commands;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Scenes.CommandManager;
using YAT.Scenes.Terminal;

namespace YAT
{
	/// <summary>
	/// YAT (Yet Another Terminal) is an addon that provides a customizable, in-game terminal for your project.
	/// This class is the main entry point for the addon, and provides access to the terminal, options, and commands.
	/// </summary>
	public partial class YAT : Node
	{
		#region Signals
		/// <summary>
		/// Signal emitted when the YatOptions have been changed.
		/// </summary>
		/// <param name="options">The new YatOptions.</param>
		[Signal]
		public delegate void OptionsChangedEventHandler(YatOptions options);
		[Signal]
		/// <summary>
		/// A signal that is emitted when the YAT addon is ready.
		/// </summary>
		public delegate void YatReadyEventHandler();
		[Signal]
		public delegate void TerminalOpenedEventHandler();
		[Signal]
		public delegate void TerminalClosedEventHandler();
		#endregion

		[Export] public YatOptions Options { get; set; } = new();

		public Terminal Terminal { get; private set; }
		public Node Windows { get; private set; }
		public Dictionary<string, ICommand> Commands { get; private set; } = new();

		public readonly LinkedList<string> History = new();
		public LinkedListNode<string> HistoryNode = null;

		public OptionsManager OptionsManager { get; private set; }
		public CommandManager CommandManager { get; private set; }

		private bool _yatEnabled = true;

		public override void _Ready()
		{
			CheckYatEnableSettings();

			Terminal = GD.Load<PackedScene>("res://addons/yat/src/scenes/terminal/Terminal.tscn").Instantiate<Terminal>();
			Terminal.Ready += () =>
			{
				LogHelper.Terminal = Terminal;
				OptionsManager.Load();

				EmitSignal(SignalName.YatReady);
			};

			Windows = GetNode<Node>("./Windows");
			OptionsManager = new(this, Options);
			CommandManager = GetNode<CommandManager>("./CommandManager");

			AddCommand(new Ls(this));
			AddCommand(new Ip(this));
			AddCommand(new Cn(this));
			AddCommand(new Cls(this));
			AddCommand(new Man(this));
			AddCommand(new Set(this));
			AddCommand(new Cat(this));
			AddCommand(new Quit(this));
			AddCommand(new Echo(this));
			AddCommand(new List(this));
			AddCommand(new View(this));
			AddCommand(new Ping(this));
			AddCommand(new Pause(this));
			AddCommand(new Watch(this));
			AddCommand(new Cowsay(this));
			AddCommand(new Options(this));
			AddCommand(new Restart(this));
			AddCommand(new History(this));
			AddCommand(new Stats(this));
			AddCommand(new Whereami(this));
			AddCommand(new Commands.QuickCommands(this));

			Keybindings.LoadDefaultActions();
		}

		public override void _UnhandledInput(InputEvent @event)
		{
			if (@event.IsActionPressed("yat_toggle")) ToggleTerminal();
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
				LogHelper.MissingAttribute("CommandAttribute", command.GetType().Name);
				return;
			}

			Commands[attribute.Name] = command;
			foreach (string alias in attribute.Aliases)
			{
				Commands[alias] = command;
			}
		}

		public void ToggleTerminal()
		{
			if (!_yatEnabled) return;

			if (Terminal.IsInsideTree()) CloseTerminal();
			else OpenTerminal();
		}

		private void OpenTerminal()
		{
			if (Terminal.IsInsideTree()) return;

			Windows.AddChild(Terminal);

			EmitSignal(SignalName.TerminalOpened);
		}

		private void CloseTerminal()
		{
			if (!Terminal.IsInsideTree()) return;

			Windows.RemoveChild(Terminal);

			EmitSignal(SignalName.TerminalClosed);
		}

		/// <summary>
		/// Checks if YAT is enabled based on the user's settings.
		/// </summary>
		private void CheckYatEnableSettings()
		{
			if (!Options.UseYatEnableFile) return;

			var path = Options.YatEnableLocation switch
			{
				YatOptions.YatEnableFileLocation.UserData => "user://",
				YatOptions.YatEnableFileLocation.CurrentDirectory => "res://",
				_ => "user://"
			};

			_yatEnabled = FileAccess.FileExists(path + Options.YatEnableFile);
		}
	}
}
