namespace YAT.Overlay.Components
{
	public partial class Window : Godot.Window
	{
		/// <summary>
		/// Moves the terminal window to the default position at the center of the viewport.
		/// </summary>
		private void MoveToTheDefaultPosition()
		{
			var center = GetTree().Root.GetViewport().GetVisibleRect().GetCenter();

			Position = new(
				(int)(center.X - Size.X / 2),
				(int)(center.Y - Size.Y / 2)
			);
		}
	}
}
