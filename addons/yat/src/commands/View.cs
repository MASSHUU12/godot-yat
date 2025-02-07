using System;
using System.Collections.Generic;
using System.Globalization;
using Godot;
using YAT.Attributes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Types;
using static Godot.RenderingServer;

namespace YAT.Commands;

[Command(
    "view",
    "Changes the rendering mode of the viewport.\n"
    + "You can use either the name of the mode or the integer value.\n"
    + "See: [url=https://docs.godotengine.org/en/stable/classes/"
    + "class_renderingserver.html#class-renderingserver-method-set-debug-"
    + "generate-wireframes]ViewportDebugDraw[/url]."
)]
[Argument("mode", "string|int(0:)", "The type of debug draw to use.")]
public sealed class View : ICommand
{
    private static readonly int MAX_DRAW_MODE;
    private static readonly List<StringName> Modes = [];

    static View()
    {
        Array values = Enum.GetValues<ViewportDebugDraw>();
        MAX_DRAW_MODE = values.Length - 1;

        foreach (object? mode in values)
        {
            Modes.Add(mode!.ToString()!.ToLowerInvariant());
        }
    }

#nullable disable
    private YAT _yat;
#nullable restore

    public CommandResult Execute(CommandData data)
    {
        string mode = (string)data.Arguments["mode"];

        _yat = data.Yat;

        if (Modes.Contains(mode))
        {
            return SetDebugDraw(
            Enum.Parse<ViewportDebugDraw>(mode, true)
        );
        }

        if (!int.TryParse(mode, out int iMode))
        {
            return ICommand.InvalidArguments($"Invalid mode: {mode}.");
        }

        return !IsValidMode(iMode)
            ? ICommand.InvalidArguments(
                $"Invalid mode: {mode}. Valid range: 0 to {MAX_DRAW_MODE}."
            )
            : SetDebugDraw((ViewportDebugDraw)iMode);
    }

    private CommandResult SetDebugDraw(ViewportDebugDraw debugDraw)
    {
        ViewportSetDebugDraw(_yat.GetViewport().GetViewportRid(), debugDraw);

        return ICommand.Success(
            [((int)debugDraw).ToStringInvariant()],
            $"Set viewport debug draw to {debugDraw} ({(int)debugDraw})."
        );
    }

    private static bool IsValidMode(int mode)
    {
        return mode >= 0 && mode <= MAX_DRAW_MODE;
    }
}
