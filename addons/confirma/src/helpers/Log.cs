using Godot;

namespace Confirma.Helpers;

public class Log
{
#nullable disable
	private readonly RichTextLabel _richOutput;
#nullable restore

	private readonly bool _headless = false;
	private readonly Colors _colors = new();

	public Log(RichTextLabel? richOutput)
	{
		_richOutput = richOutput;
		_headless = _richOutput is null;
		_colors.Terminal = _headless;
	}

	public Log()
	{
		_richOutput = null;
		_headless = true;
		_colors.Terminal = _headless;
	}

	public void Print(string message)
	{
		if (_headless) GD.PrintRaw(message);
		else _richOutput.AppendText(message);
	}

	public void PrintLine(string message)
	{
		Print($"{message}\n");
	}

	public void PrintLine()
	{
		Print("\n");
	}

	public void PrintError(string message)
	{
		Print(ColorText(message, Colors.Error));
	}

	public void PrintSuccess(string message)
	{
		Print(ColorText(message, Colors.Success));
	}

	public void PrintWarning(string message)
	{
		Print(ColorText(message, Colors.Warning));
	}

	private string ColorText(string text, string color)
	{
		_colors.Color = new(color);

		return _headless ? _colors.ToTerminal(text) : _colors.ToGodot(text);
	}
}
