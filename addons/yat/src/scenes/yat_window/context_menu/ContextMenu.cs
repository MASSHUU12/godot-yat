using Godot;

namespace YAT.Scenes.YatWindow;

public partial class ContextMenu : PopupMenu
{
	[Export] public ushort WallMargin { get; set; } = 16;

	private Viewport _viewport = null;
	private Rect2 _viewportRect = new();

	public override void _Ready()
	{
		_viewport = GetTree().Root.GetViewport();
		_viewportRect = _viewport.GetVisibleRect();

		Hide();
	}

	/// <summary>
	/// Shows the context menu next to the mouse cursor.
	/// </summary>
	public void ShowNextToMouse()
	{
		var mousePos = _viewport.GetMousePosition();
		var (limitX, limitY) = CalculateLimits(_viewportRect);

		Show();

		Position = new()
		{
			X = (int)Mathf.Clamp(mousePos.X, WallMargin, limitX),
			Y = (int)Mathf.Clamp(mousePos.Y, WallMargin, limitY)
		};
	}

	public (float, float) CalculateLimits(Rect2 rect)
	{
		var limitX = rect.Size.X - Size.X - WallMargin;
		var limitY = rect.Size.Y - Size.Y - WallMargin;

		return ((int)limitX, (int)limitY);
	}
}
