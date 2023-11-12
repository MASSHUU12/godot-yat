using System;
using Godot;
using YAT.Overlay.Components.Terminal;

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
		"- wireframe: Renders the viewport in wireframe mode.\n\n" +
		"You can also use the integer value of the type " +
		"[url=https://docs.godotengine.org/en/stable/classes/class_renderingserver.html#class-renderingserver-method-set-debug-generate-wireframes]ViewportDebugDraw[/url]."
	)]
	public partial class View : ICommand
	{
		public YAT Yat { get; set; }

		public View(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(params string[] args)
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
					var iMode = int.TryParse(mode, out var i) ? i : -1;
					var max = Enum.GetValues(typeof(RenderingServer.ViewportDebugDraw)).Length - 1;

					if (iMode < 0 || iMode > max)
					{
						Yat.Terminal.Print($"Invalid mode: {mode}.", Terminal.PrintType.Error);
						return CommandResult.InvalidArguments;
					}

					debugDraw = (RenderingServer.ViewportDebugDraw)iMode;
					break;
			}

			RenderingServer.ViewportSetDebugDraw(
				Yat.GetViewport().GetViewportRid(),
				debugDraw
			);

			Yat.Terminal.Print($"Set viewport debug draw to {debugDraw} ({(uint)debugDraw}).");

			return CommandResult.Success;
		}
	}
}
