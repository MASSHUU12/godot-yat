using System.Collections.Generic;
using Godot;
using YAT.Commands;
using YAT.Helpers;
using YAT.Overlay.Components.Terminal;

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

		public Overlay.Overlay Overlay { get; private set; }
		public Terminal Terminal { get; private set; }
		public readonly LinkedList<string> History = new();
		public LinkedListNode<string> HistoryNode = null;
		public OptionsManager OptionsManager { get; private set; }
		public Dictionary<string, ICommand> Commands { get; private set; } = new();

		private Godot.Window _root;
		private bool _yatEnabled = true;

		public override void _Ready()
		{
			CheckYatEnableSettings();

			_root = GetTree().Root;

			Overlay = GD.Load<PackedScene>("res://addons/yat/src/overlay/Overlay.tscn").Instantiate<Overlay.Overlay>();
			Overlay.Ready += OnOverlayReady;
			OptionsManager = new(this, Options);

			AddCommand(new Cls(this));
			AddCommand(new Man(this));
			AddCommand(new Set(this));
			AddCommand(new Quit(this));
			AddCommand(new Echo(this));
			AddCommand(new List(this));
			AddCommand(new View(this));
			AddCommand(new Pause(this));
			AddCommand(new Watch(this));
			AddCommand(new Options(this));
			AddCommand(new Restart(this));
			AddCommand(new History(this));
			AddCommand(new Whereami(this));
		}

		public override void _Input(InputEvent @event)
		{
			if (@event.IsActionPressed("yat_toggle") && _yatEnabled)
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

			EmitSignal(SignalName.YatReady);
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
