using System.Collections.Generic;
using Godot;

public partial class YAT : Control
{
	[Export] public YatOptions Options { get; set; } = new();

	public YatOverlay Overlay { get; private set; }
	public YatTerminal Terminal;
	public LinkedListNode<string> HistoryNode = null;
	public readonly LinkedList<string> History = new();
	public Dictionary<string, IYatCommand> Commands = new();

	private Window _root;

	public override void _Ready()
	{
		_root = GetTree().Root;

		Overlay = GetNode<YatOverlay>("YatOverlay");
		Terminal = Overlay.Terminal;

		// Set options at startup.
		Options.EmitSignal(nameof(Options.OptionsChanged), Options);

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
	public void AddCommand(IYatCommand command)
	{
		Commands[command.Name] = command;
		foreach (string alias in command.Aliases)
		{
			Commands[alias] = command;
		}
	}
}
