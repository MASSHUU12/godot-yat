using Godot;

namespace YAT.Scenes;

public partial class ContextMenu : PopupMenu
{
	[Export] public ushort WallMargin { get; set; } = 16;
	[Export] public bool ShrinkToFit { get; set; } = true;

	private Viewport? _viewport = null;
	private Rect2 _viewportRect = new();

	public override void _Ready()
	{
		_viewport = GetTree().Root.GetViewport();
		_viewportRect = _viewport.GetVisibleRect();

		Hide();

		if (ShrinkToFit) MenuChanged += Shrink;
	}

	private void Shrink()
	{
		var itemsSize = GetItemsSize();

		Size = new()
		{
			X = 1,
			Y = 1
		};
		Size = (Vector2I)itemsSize;
	}

	private Vector2 GetItemsSize()
	{
		var items = GetChildren();
		var size = new Vector2();

		foreach (var item in items)
		{
			if (item is Control control)
				size += control.Size;
			if (item is PopupMenu popupMenu)
				size += popupMenu.Size;
		}

		return size;
	}

	public void ShowNextToMouse()
	{
		if (_viewport is null) return;

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
