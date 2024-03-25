using System.Collections.Generic;
using Godot;
using YAT.Classes.Managers;
using YAT.Enums;
using YAT.Helpers;
using YAT.Resources;

namespace YAT.Scenes;

public partial class BaseTerminal : Control
{
	[Signal] public delegate void CloseRequestedEventHandler();
	[Signal] public delegate void TitleChangeRequestedEventHandler(string title);
	[Signal] public delegate void PositionResetRequestedEventHandler();
	[Signal] public delegate void SizeResetRequestedEventHandler();
	[Signal]
	public delegate void MethodCalledEventHandler(
		StringName method, Variant returnValue, EMethodStatus status
	);

#nullable disable
	public bool Locked { get; set; }
	public bool Current { get; set; } = true;
	public Input Input { get; private set; }
	public Output Output { get; private set; }
	public SelectedNode SelectedNode { get; private set; }
	public CommandManager CommandManager { get; private set; }
	public CommandValidator CommandValidator { get; private set; }
	public FullWindowDisplay FullWindowDisplay { get; private set; }
	public ECommandResult LastCommandResult { get; set; } = ECommandResult.Unavailable;

	public readonly LinkedList<string> History = new();
	public LinkedListNode<string> HistoryNode = null;

	private YAT _yat;
	private Label _promptLabel;
	private Label _selectedNodeLabel;
	private PanelContainer _content;
#nullable restore

	private string _prompt = "> ";

	public override void _Ready()
	{
		_yat = GetNode<YAT>("/root/YAT");
		_yat.YatReady += () =>
		{
			_yat.PreferencesManager.PreferencesUpdated += UpdateOptions;
			UpdateOptions(_yat.PreferencesManager.Preferences);
		};

		CommandManager = GetNode<CommandManager>("Components/CommandManager");
		CommandManager.CommandStarted += (command, args) =>
			EmitSignal(SignalName.TitleChangeRequested, "YAT - " + command);
		CommandManager.CommandFinished += (command, args, result) =>
			EmitSignal(SignalName.TitleChangeRequested, "YAT");

		SelectedNode = GetNode<SelectedNode>("SelectedNode");
		SelectedNode.CurrentNodeChanged += OnCurrentNodeChanged;

		FullWindowDisplay = GetNode<FullWindowDisplay>("FullWindowDisplay");
		FullWindowDisplay.Opened += () => { Input.ReleaseFocus(); };
		FullWindowDisplay.Closed += () => { Input.CallDeferred("grab_focus"); };

		Input = GetNode<Input>("%Input");
		CommandValidator = GetNode<CommandValidator>("Components/CommandValidator");

		_promptLabel = GetNode<Label>("%PromptLabel");
		_content = GetNode<PanelContainer>("PanelContainer");
		_selectedNodeLabel = GetNode<Label>("%SelectedNodePath");

		Output = GetNode<Output>("%Output");
		Output.MetaClicked += (link) => Godot.OS.ShellOpen((string)link);

		OnCurrentNodeChanged(SelectedNode.Current);
	}

	public override void _Input(InputEvent @event)
	{
		// Handle history navigation if the Terminal window is open.
		if (IsInsideTree())
		{
			if (@event.IsActionPressed(Keybindings.TerminalHistoryPrevious))
			{
				if (HistoryNode is null && History.Count > 0)
				{
					HistoryNode = History.Last;
					Input.Text = HistoryNode!.Value;
				}
				else if (HistoryNode?.Previous is not null)
				{
					HistoryNode = HistoryNode.Previous;
					Input.Text = HistoryNode.Value;
				}

				Input.CallDeferred(nameof(Input.MoveCaretToEnd));
			}

			if (@event.IsActionPressed(Keybindings.TerminalHistoryNext))
			{
				if (HistoryNode is not null && HistoryNode.Next is not null)
				{
					HistoryNode = HistoryNode.Next;
					Input.Text = HistoryNode.Value;
				}
				else
				{
					HistoryNode = null;
					Input.Text = string.Empty;
				}

				Input.CallDeferred(nameof(Input.MoveCaretToEnd));
			}

			if (@event.IsActionPressed(Keybindings.TerminalInterrupt) && Locked)
			{
				Print("Command cancellation requested.", EPrintType.Warning);

				CommandManager.Cts.Cancel();
				CommandManager.Cts.Dispose();
				// CommandManager.Cts = null;
			}
		}
	}

	private void UpdateOptions(YatPreferences prefs)
	{
		OnCurrentNodeChanged(SelectedNode.Current);
		_promptLabel.Visible = prefs.ShowPrompt;
		_promptLabel.Text = prefs.Prompt;

		Output.ScrollFollowing = prefs.AutoScroll;
		Input.AddThemeColorOverride("font_color", prefs.InputColor);

		var theme = _content.Theme;
		theme.DefaultFont = prefs.BaseFont;
		theme.DefaultFontSize = prefs.FontSize;
		_content.Theme = theme;
	}

	private void OnCurrentNodeChanged(Node node)
	{
		var path = node?.GetPath() ?? string.Empty;

		_selectedNodeLabel.TooltipText = path;
		_selectedNodeLabel.Text = Text.ShortenPath(path, 32);
	}

	/// <summary>
	/// Prints the specified text to the terminal with the specified print type.
	/// </summary>
	/// <param name="text">The text to print.</param>
	/// <param name="type">The type of print to use (e.g. error, warning, success, normal).</param>
	public void Print(string text, EPrintType type = EPrintType.Normal)
	{
		var color = type switch
		{
			EPrintType.Error => _yat.PreferencesManager.Preferences.ErrorColor,
			EPrintType.Warning => _yat.PreferencesManager.Preferences.WarningColor,
			EPrintType.Success => _yat.PreferencesManager.Preferences.SuccessColor,
			EPrintType.Normal => _yat.PreferencesManager.Preferences.OutputColor,
			_ => _yat.PreferencesManager.Preferences.OutputColor,
		};

		// Using CallDeferred to avoid issues when running this method in a separate thread.
		Output.CallDeferred("push_color", color);
		// Paragraph is needed because newline characters are breaking formatting.
		Output.CallDeferred("push_paragraph", (ushort)HorizontalAlignment.Left);
		Output.CallDeferred("append_text", text);
		Output.CallDeferred("pop_all");
	}

	public void Print<T>(T text, EPrintType type = EPrintType.Normal)
	{
		if (text is null)
		{
			GD.PushError("Text is null.");
			return;
		}

		Print(text.ToString() ?? string.Empty, type);
	}

	public void Clear() => Output.Clear();
}
