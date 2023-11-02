using System.Collections.Generic;
using System;
using Godot;
using YAT.Commands;
using YAT.Helpers;

namespace YAT
{
	/// <summary>
	/// YAT (Yet Another Terminal) is an addon that provides a customizable, in-game terminal for your project.
	/// This class is the main entry point for the addon, and provides access to the terminal, options, and commands.
	/// </summary>
	public partial class YAT : Control
	{
		#region Signals
		/// <summary>
		/// Signal emitted when the YatOptions have been changed.
		/// </summary>
		/// <param name="options">The new YatOptions.</param>
		[Signal]
		public delegate void OptionsChangedEventHandler(YatOptions options);
		/// <summary>
		/// A signal that is emitted when the overlay is opened.
		/// </summary>
		[Signal]
		public delegate void OverlayOpenedEventHandler();
		/// <summary>
		/// Signal that is emitted when the overlay is closed.
		/// </summary>
		[Signal]
		public delegate void OverlayClosedEventHandler();
		[Signal]
		/// <summary>
		/// A signal that is emitted when the YAT addon is ready.
		/// </summary>
		public delegate void YatReadyEventHandler();
		#endregion

		[Export] public YatOptions Options { get; set; } = new();

		public Overlay Overlay { get; private set; }
		public Terminal Terminal;
		public OptionsManager OptionsManager;
		public LinkedListNode<string> HistoryNode = null;
		public readonly LinkedList<string> History = new();
		public Dictionary<string, ICommand> Commands = new();

		private Godot.Window _root;

		public override void _Ready()
		{
			_root = GetTree().Root;

			Overlay = GD.Load<PackedScene>("res://addons/yat/overlay/Overlay.tscn").Instantiate<Overlay>();
			Overlay.Ready += OnOverlayReady;
			OptionsManager = new(this, Options);

			AddCommand(new Cls());
			AddCommand(new Man());
			AddCommand(new Set());
			AddCommand(new Quit());
			AddCommand(new Echo());
			AddCommand(new List());
			AddCommand(new View());
			AddCommand(new Pause());
			AddCommand(new Options());
			AddCommand(new Restart());
			AddCommand(new Whereami());

			EmitSignal(SignalName.YatReady);
		}

		public override void _Input(InputEvent @event)
		{
			if (@event.IsActionPressed("yat_toggle"))
			{
				// Toggle the Overlay.
				if (Overlay.IsInsideTree())
				{
					Terminal.Input.ReleaseFocus();
					_root.RemoveChild(Overlay);
					EmitSignal(SignalName.OverlayClosed);
				}
				else
				{
					_root.AddChild(Overlay);
					// Grabbing focus this way prevents writing to the input field
					// from the previous frame.
					Terminal.Input.CallDeferred("grab_focus");
					EmitSignal(SignalName.OverlayOpened);
				}
			}
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

		private void OnOverlayReady()
		{
			Terminal = Overlay.Terminal;
			LogHelper.Terminal = Terminal;
			OptionsManager.Load();
		}
	}

}
