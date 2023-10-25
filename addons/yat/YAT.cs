using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class YAT : Control
{
	[Export] public YatOptions Options { get; set; } = new();

	public Control Overlay;
	public YatTerminal Cli;
	public Dictionary<string, IYatCommand> Commands = new();

	private Node _root;

	private const ushort HISTORY_LIMIT = 25;
	private LinkedListNode<string> _historyNode = null;
	private readonly LinkedList<string> _history = new();

	public override void _Ready()
	{
		_root = GetTree().Get("root").As<Node>();

		Overlay = GD.Load<PackedScene>("res://addons/yat/yat_overlay/YatOverlay.tscn").Instantiate() as Control;
		Overlay.Ready += OnOverlayReady;

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
				if (_historyNode == null && _history.Count > 0)
				{
					_historyNode = _history.Last;
					Cli.Input.Text = _historyNode.Value;
				}
				else if (_historyNode?.Previous != null)
				{
					_historyNode = _historyNode.Previous;
					Cli.Input.Text = _historyNode.Value;
				}
			}

			if (@event.IsActionPressed("yat_history_next"))
			{
				if (_historyNode != null && _historyNode.Next != null)
				{
					_historyNode = _historyNode.Next;
					Cli.Input.Text = _historyNode.Value;
				} else {
					_historyNode = null;
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

	private void OnOverlayReady()
	{
		Cli.Input.TextSubmitted += OnCommandSubmitted;
	}

	/// <summary>
	/// Executes the given CLI command.
	/// </summary>
	/// <param name="input">The input arguments for the command.</param>
	private void ExecuteCommand(string[] input)
	{
		if (input.Length == 0)
		{
			Cli.Println("Invalid input.");
			return;
		}

		string commandName = input[0];
		if (!Commands.ContainsKey(commandName))
		{
			Cli.Println($"Unknown command: {commandName}");
			return;
		}

		IYatCommand command = Commands[commandName];
		command.Execute(input, this);
	}

	/// <summary>
	/// Handles the submission of a command by sanitizing the input,
	/// executing the command, and clearing the input buffer.
	/// </summary>
	/// <param name="command">The command to be submitted.</param>
	private void OnCommandSubmitted(string command)
	{
		var input = SanitizeInput(command);

		if (input.Length == 0) return;

		_historyNode = null;
		_history.AddLast(command);
		if (_history.Count > HISTORY_LIMIT) _history.RemoveFirst();

		ExecuteCommand(input);
		Cli.Input.Clear();
	}

	/// <summary>
	/// Sanitizes the input command by removing leading/trailing white space
	/// and extra spaces between words.
	/// </summary>
	/// <param name="command">The command to sanitize.</param>
	/// <returns>The sanitized command.</returns>
	private static string[] SanitizeInput(string command)
	{
		command = command.Trim();
		return command.Split(' ').Where(
			s => !string.IsNullOrWhiteSpace(s)
		).ToArray();
	}
}
