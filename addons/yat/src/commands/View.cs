using System;
using System.Collections.Generic;
using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;
using static Godot.RenderingServer;

namespace YAT.Commands;

[Command(
	"view",
	"Changes the rendering mode of the viewport.\n"
	+ "You can use either the name of the mode or the integer value.\n"
	+ "See: [url=https://docs.godotengine.org/en/stable/classes/class_renderingserver.html#class-renderingserver-method-set-debug-generate-wireframes]ViewportDebugDraw[/url].",
	"[b]Usage[/b]: view [i]type[/i]\n\n"
)]
[Argument("mode", "string|int(0:)", "The type of debug draw to use.")]
public sealed class View : ICommand
{
	private static readonly int MAX_DRAW_MODE;
	private static readonly List<StringName> Modes = new();

	static View()
	{
		var values = Enum.GetValues(typeof(ViewportDebugDraw));
		MAX_DRAW_MODE = values.Length - 1;

		foreach (var mode in values) Modes.Add(mode.ToString().ToLower());
	}

	private YAT _yat;

	public CommandResult Execute(CommandData data)
	{
		var mode = (string)data.Arguments["mode"];

		_yat = data.Yat;

		if (Modes.Contains(mode)) return SetDebugDraw(
			(ViewportDebugDraw)Enum.Parse(typeof(ViewportDebugDraw), mode, true)
		);

		if (!int.TryParse(mode, out var iMode))
			return ICommand.InvalidArguments($"Invalid mode: {mode}.");

		if (!IsValidMode(iMode))
			return ICommand.InvalidArguments($"Invalid mode: {mode}. Valid range: 0 to {MAX_DRAW_MODE}.");

		return SetDebugDraw((ViewportDebugDraw)iMode);
	}

	private CommandResult SetDebugDraw(ViewportDebugDraw debugDraw)
	{
		ViewportSetDebugDraw(_yat.GetViewport().GetViewportRid(), debugDraw);

		return ICommand.Success($"Set viewport debug draw to {debugDraw} ({(int)debugDraw}).");
	}

	private static bool IsValidMode(int mode) => mode >= 0 && mode <= MAX_DRAW_MODE;
}
