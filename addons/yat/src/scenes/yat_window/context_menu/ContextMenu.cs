using Godot;
using YAT.Helpers;

namespace YAT.Scenes.YatWindow
{
	public partial class ContextMenu : PopupMenu
	{
		private const ushort _wallMargin = 16;
		private Viewport _viewport = null;
		private Rect2 _viewportRect = new();

		public override void _Ready()
		{
			_viewport = GetTree().Root.GetViewport();
			_viewportRect = _viewport.GetVisibleRect();

			Hide();
		}

		public override void _Input(InputEvent @event)
		{
			if (@event.IsActionPressed(Keybindings.ContextMenu)) ShowNextToMouse();
		}

		/// <summary>
		/// Shows the context menu next to the mouse cursor.
		/// </summary>
		public void ShowNextToMouse()
		{
			var mousePos = _viewport.GetMousePosition();
			var limitX = _viewportRect.Size.X - Size.X - _wallMargin;
			var limitY = _viewportRect.Size.Y - Size.Y - _wallMargin;

			Show();

			Position = new()
			{
				X = (int)Mathf.Clamp(mousePos.X, _wallMargin, limitX),
				Y = (int)Mathf.Clamp(mousePos.Y, _wallMargin, limitY)
			};
		}
	}
}
