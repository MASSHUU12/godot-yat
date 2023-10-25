using System.Collections.Generic;
using Godot;

public partial class YAT : Control
{
	[Export] public YatOptions Options { get; set; } = new();

	public Control Overlay;
	public YatTerminal Cli;
	public LinkedListNode<string> HistoryNode = null;
	public readonly LinkedList<string> History = new();
	public Dictionary<string, IYatCommand> Commands = new();

	private Window _root;
	private const ushort HISTORY_LIMIT = 25;

	public override void _Ready()
	{
		_root = GetTree().Root;

		Overlay = GD.Load<PackedScene>("res://addons/yat/yat_overlay/YatOverlay.tscn").Instantiate() as Control;
		Cli = Overlay.GetNode<YatTerminal>("YatTerminal");

		// Set options at startup.
		Options.EmitSignal(nameof(Options.OptionsChanged), Options);

		AddCommand(new Cls());
		AddCommand(new Man());
		AddCommand(new Quit());
		AddCommand(new Echo());
		AddCommand(new Options());
		AddCommand(new Restart());
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("yat_toggle"))
		{
			// Toggle the CLI window.
			if (Overlay.IsInsideTree())
			{
				Cli.Input.ReleaseFocus();
				_root.RemoveChild(Overlay);
			}
			else
			{
				_root.AddChild(Overlay);

				// Grabbing focus this way prevents writing to the input field
				// from the previous frame.
				Cli.Input.CallDeferred("grab_focus");
			}
		}

		// Handle history navigation if the CLI window is open.
		if (Overlay.IsInsideTree())
		{
			if (@event.IsActionPressed("yat_history_previous"))
			{
				if (HistoryNode == null && History.Count > 0)
				{
					HistoryNode = History.Last;
					Cli.Input.Text = HistoryNode.Value;
				}
				else if (HistoryNode?.Previous != null)
				{
					HistoryNode = HistoryNode.Previous;
					Cli.Input.Text = HistoryNode.Value;
				}
			}

			if (@event.IsActionPressed("yat_history_next"))
			{
				if (HistoryNode != null && HistoryNode.Next != null)
				{
					HistoryNode = HistoryNode.Next;
					Cli.Input.Text = HistoryNode.Value;
				} else {
					HistoryNode = null;
					Cli.Input.Text = string.Empty;
				}
			}
		}
	}

	/// <summary>
	/// Adds a CLICommand to the list of available commands.
	/// </summary>
	/// <param name="command">The CLICommand to add.</param>
	public void AddCommand(IYatCommand command)
	{
		Commands[command.Name] = command;
		foreach (string alias in command.Aliases)
		{
			Commands[alias] = command;
		}
	}
}
