using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("sr", "Set the screen resolution.")]
[Option("-w", "int(1:)", "The width of the screen resolution.", -1)]
[Option("-h", "int(1:)", "The height of the screen resolution.", -1)]
[Option("-s", "float(0:2)", "Resolution scale.", -1f)]
[Option("-fsr", "bool", "Enable AMD FSR 1.0")]
[Option("-fsr2", "bool", "Enable AMD FSR 2.2")]
public sealed class Sr : ICommand
{
    public CommandResult Execute(CommandData data)
    {
        var width = (int)data.Options["-w"];
        var height = (int)data.Options["-h"];
        var scale = (float)data.Options["-s"];
        var fsr = (bool)data.Options["-fsr"];
        var fsr2 = (bool)data.Options["-fsr2"];

        Viewport viewport = data.Yat.GetViewport();
        Window window = viewport.GetWindow();

        window.Size = new(
            width == -1 ? window.Size.X : width,
            height == -1 ? window.Size.Y : height
        );

        viewport.Scaling3DScale = scale == -1f
            ? viewport.Scaling3DScale
            : scale;

        window.MoveToCenter();

        viewport.Scaling3DMode = Viewport.Scaling3DModeEnum.Bilinear;

        if (fsr)
        {
            viewport.Scaling3DMode = Viewport.Scaling3DModeEnum.Fsr;
        }

        if (fsr2)
        {
            viewport.Scaling3DMode = Viewport.Scaling3DModeEnum.Fsr2;
        }

        data.Terminal.Output.Print(
            string.Format("Screen resolution: {0}x{1}, scale: {2} ({3}x{4}), mode: {5}.",
                window.Size.X, window.Size.Y,
                viewport.Scaling3DScale,
                (int)(window.Size.X * viewport.Scaling3DScale),
                (int)(window.Size.Y * viewport.Scaling3DScale),
                viewport.Scaling3DMode
            )
        );

        return ICommand.Success();
    }
}
