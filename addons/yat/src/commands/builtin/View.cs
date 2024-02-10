using System;
using System.Collections.Generic;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Scenes.BaseTerminal;
using YAT.Types;
using static Godot.RenderingServer;

namespace YAT.Commands;

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
[Argument("type", "[normal, unshaded, lightning, overdraw, wireframe, int]", "The type of debug draw to use.")]
public sealed class View : ICommand
{
	private readonly int MAX_DRAW_MODE = Enum.GetValues(typeof(ViewportDebugDraw)).Length - 1;

	private readonly Dictionary<string, ViewportDebugDraw> modeMappings = new()
		{
			{"normal", ViewportDebugDraw.Disabled},
			{"unshaded", ViewportDebugDraw.Unshaded},
			{"lightning", ViewportDebugDraw.Lighting},
			{"overdraw", ViewportDebugDraw.Overdraw},
			{"wireframe", ViewportDebugDraw.Wireframe}
		};

	private YAT _yat;
	private BaseTerminal _terminal;

	public CommandResult Execute(CommandData data)
	{
		var mode = data.RawData[1];

		_yat = data.Yat;
		_terminal = data.Terminal;

		if (modeMappings.TryGetValue(mode, out ViewportDebugDraw debugDraw))
			return SetDebugDraw(debugDraw);

		if (!int.TryParse(mode, out var iMode))
		{
			data.Terminal.Print($"Invalid mode: {mode}.", EPrintType.Error);
			return CommandResult.InvalidArguments;
		}

		if (!IsValidMode((ushort)iMode))
		{
			data.Terminal.Print($"Invalid mode: {mode}. Valid range: 0 to {MAX_DRAW_MODE}.", EPrintType.Error);
			return CommandResult.InvalidArguments;
		}

		return SetDebugDraw((ViewportDebugDraw)iMode);
	}

	private CommandResult SetDebugDraw(ViewportDebugDraw debugDraw)
	{
		ViewportSetDebugDraw(_yat.GetViewport().GetViewportRid(), debugDraw);

		_terminal.Print($"Set viewport debug draw to {debugDraw} ({(ushort)debugDraw}).");

		return CommandResult.Success;
	}

	private bool IsValidMode(ushort mode) => mode >= 0 && mode <= MAX_DRAW_MODE;
}
