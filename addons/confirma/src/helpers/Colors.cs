using Godot;

namespace Confirma.Helpers;

public class Colors
{
	public Color Color { get; set; }
	public bool Terminal { get; set; } = false;

	public static readonly string Success = "#8eef97";
	public static readonly string Warning = "#ffdd65";
	public static readonly string Error = "#ff786b";

	public Colors()
	{
		Color = new();
	}

	public Colors(Color color)
	{
		Color = color;
	}

	public Colors(string colors)
	{
		Color = new(colors);
	}

	public Colors(bool terminal)
	{
		Terminal = terminal;
	}

	public string ToTerminal()
	{
		return $"\x1b[38;2;{Color.R * 0xFF};{Color.G * 0xFF};{Color.B * 0xFF}m";
	}

	public static string TerminalReset()
	{
		return "\x1b[0m";
	}

	public string ToTerminal(string text)
	{
		return $"{ToTerminal()}{text}{TerminalReset()}";
	}

	public string Auto(string text, string color)
	{
		Color = new Color(color);

		return Terminal ? ToTerminal(text) : ToGodot(text);
	}

	public string ToGodot(string text)
	{
		return $"[color=#{Color.ToHtml()}]{text}[/color]";
	}
}
