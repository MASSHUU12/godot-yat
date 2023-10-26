using System.Collections.Generic;
using System;
using Godot;

namespace YAT
{
	public partial class YAT : Control
	{
		[Signal]
		public delegate void OptionsChangedEventHandler(YatOptions options);

		[Export] public YatOptions Options { get; set; } = new();

		public Overlay Overlay { get; private set; }
		public Terminal Terminal;
		public OptionsManager OptionsManager;
		public LinkedListNode<string> HistoryNode = null;
		public readonly LinkedList<string> History = new();
		public Dictionary<string, ICommand> Commands = new();

		private Window _root;

		public override void _Ready()
		{
			_root = GetTree().Root;

			Overlay = GetNode<Overlay>("Overlay");
			Terminal = Overlay.Terminal;
			OptionsManager = new(this);

			// Set options at startup.
			// OptionsManager.CallDeferred(nameof(OptionsManager.Load));
			EmitSignal(SignalName.OptionsChanged, Options);

			AddCommand(new Cls());
			AddCommand(new Man());
			AddCommand(new Quit());
			AddCommand(new Echo());
			AddCommand(new Options());
			AddCommand(new Restart());
		}

		/// <summary>
		/// Adds a CLICommand to the list of available commands.
		/// </summary>
		/// <param name="command">The CLICommand to add.</param>
		public void AddCommand(ICommand command)
		{
			if (Attribute.GetCustomAttribute(command.GetType(), typeof(CommandAttribute))
				is not CommandAttribute attribute)
			{
				var message = string.Format(
					"The command {0} does not have a CommandAttribute, and will not be added to the list of available commands.",
					command.GetType().Name
				);

				GD.PrintErr(message);
				// Terminal.Print(message, YatTerminal.PrintType.Warning);
				return;
			}

			Commands[attribute.Name] = command;
			foreach (string alias in attribute.Aliases)
			{
				Commands[alias] = command;
			}
		}
	}

}
