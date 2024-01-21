using System;
using System.Collections.Generic;
using Godot;
using YAT.Attributes;
using YAT.Commands;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Scenes.BaseTerminal;
using YAT.Scenes.CommandManager;
using YAT.Scenes.GameTerminal;

namespace YAT
{
	/// <summary>
	/// YAT (Yet Another Terminal) is an addon that provides a customizable, in-game terminal for your project.
	/// This class is the main entry point for the addon, and provides access to the terminal, options, and commands.
	/// </summary>
	public partial class YAT : Node
	{
		[Signal]
		public delegate void OptionsChangedEventHandler(YatOptions options);
		[Signal] public delegate void YatReadyEventHandler();
		[Signal] public delegate void TerminalOpenedEventHandler();
		[Signal] public delegate void TerminalClosedEventHandler();

		[Export] public YatOptions Options { get; set; } = new();

		[Obsolete]
		public BaseTerminal Terminal { get; private set; } // TODO: Remove this
		public Node Windows { get; private set; }
		public Dictionary<string, ICommand> Commands { get; private set; } = new();

		public OptionsManager OptionsManager { get; private set; }
		public CommandManager CommandManager { get; private set; }

		private bool _yatEnabled = true;
		private GameTerminal _gameTerminal;

		public override void _Ready()
		{
			CheckYatEnableSettings();

			_gameTerminal = GD.Load<PackedScene>("uid://dsyqv187j7w76").Instantiate<GameTerminal>();
			_gameTerminal.Ready += () =>
			{
				Terminal = _gameTerminal.BaseTerminal;
				Log.Terminal = Terminal;
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
			AddCommand(new Commands.QuickCommands());

			Keybindings.LoadDefaultActions();
		}

		public override void _UnhandledInput(InputEvent @event)
		{
			if (@event.IsActionPressed(Keybindings.TerminalToggle)) ToggleTerminal();
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

		public void ToggleTerminal()
		{
			if (!_yatEnabled) return;

			if (_gameTerminal.IsInsideTree()) CloseTerminal();
			else OpenTerminal();
		}

		public void OpenTerminal()
		{
			if (_gameTerminal.IsInsideTree()) return;

			Windows.AddChild(_gameTerminal);

			EmitSignal(SignalName.TerminalOpened);
		}

		public void CloseTerminal()
		{
			if (!_gameTerminal.IsInsideTree()) return;

			Windows.RemoveChild(_gameTerminal);

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
