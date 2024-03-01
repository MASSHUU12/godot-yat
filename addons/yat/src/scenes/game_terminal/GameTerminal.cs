using Godot;
using YAT.Helpers;
using YAT.Resources;

namespace YAT.Scenes;

public partial class GameTerminal : YatWindow
{
	public BaseTerminal CurrentTerminal { get; private set; }
	public TerminalSwitcher TerminalSwitcher { get; private set; }

	private YAT _yat;

	public override void _Ready()
	{
		base._Ready();

		_yat = GetNode<YAT>("/root/YAT");
		_yat.PreferencesManager.PreferencesUpdated += UpdateOptions;

		TerminalSwitcher = GetNode<TerminalSwitcher>("%TerminalSwitcher");

		CurrentTerminal = TerminalSwitcher.CurrentTerminal;
		CurrentTerminal.TitleChangeRequested += title => Title = title;
		CurrentTerminal.PositionResetRequested += ResetPosition;
		CurrentTerminal.SizeResetRequested += () => Size = InitialSize;

		CloseRequested += _yat.TerminalManager.CloseTerminal;

#if GODOT4_3_OR_GREATER
		ContextMenu.AddSubmenuNodeItem(
			"QuickCommands",
			GetNode<QuickCommandsContext>("ContextMenu/QuickCommandsContext")
		);
#else
		ContextMenu.AddSubmenuItem("QuickCommands", "QuickCommandsContext");
#endif
		ContextMenu.AddItem("Preferences", 1);
		ContextMenu.IdPressed += ContextMenuItemSelected;

		MoveToCenter();
	}

	private void ContextMenuItemSelected(long id)
	{
		if (id == 1)
		{
			new Commands.Preferences().Execute(new(
				_yat, CurrentTerminal, null, null, null, null, default
			));
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed(Keybindings.TerminalToggle))
			CallDeferred("emit_signal", SignalName.CloseRequested);
	}

	private void UpdateOptions(YatPreferences prefs)
	{
		Size = new(prefs.DefaultWidth, prefs.DefaultHeight);
	}
}
