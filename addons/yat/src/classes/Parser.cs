using YAT.Helpers;

namespace YAT.Classes;

public static class Parser
{
	public static string[] ParseCommand(string command)
	{
		string[] input = Text.SanitizeText(command);

		return Text.ConcatenateSentence(input);
	}
}
