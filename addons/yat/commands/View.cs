using Godot;

namespace YAT.Commands
{
	[Command(
		"view",
		"Changes the rendering mode of the viewport.",
		"[b]Usage[/b]: view [i]type[/i]\n\n" +
		"Types:\n- normal: Disables debug draw.\n" +
		"- unshaded: Renders the viewport without shading.\n" +
		"- lightning: Enables lighting debug draw.\n" +
		"- overdraw: Enables overdraw debug draw.\n" +
		"- wireframe: Renders the viewport in wireframe mode."
	)]
	public partial class View : ICommand
	{
		public CommandResult Execute(YAT yat, params string[] args)
		{
			if (args.Length < 2) return CommandResult.InvalidArguments;

			var mode = args[1].ToLower().Trim();
			RenderingServer.ViewportDebugDraw debugDraw = RenderingServer.ViewportDebugDraw.Disabled;

			switch (mode)
			{
				case "normal":
					debugDraw = RenderingServer.ViewportDebugDraw.Disabled;
					break;
				case "unshaded":
					debugDraw = RenderingServer.ViewportDebugDraw.Unshaded;
					break;
				case "lightning":
					debugDraw = RenderingServer.ViewportDebugDraw.Lighting;
					break;
				case "overdraw":
					debugDraw = RenderingServer.ViewportDebugDraw.Overdraw;
					break;
				case "wireframe":
					debugDraw = RenderingServer.ViewportDebugDraw.Wireframe;
					break;
				default:
					break;
			}

			RenderingServer.ViewportSetDebugDraw(
				yat.GetViewport().GetViewportRid(), debugDraw);

			yat.Terminal.Println($"Set viewport debug draw to {mode}.");

			return CommandResult.Success;
		}
	}
}
