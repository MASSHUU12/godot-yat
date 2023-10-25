public partial class Echo : IYatCommand
{
	public string Name => "echo";

	public string Description => "Displays the given text.";

	public string Usage => "echo <text>";

	public string[] Aliases => System.Array.Empty<string>();

	public void Execute(string[] args, YAT yat)
	{
		if (args.Length < 2)
		{
			yat.Cli.Println("Invalid input.");
			return;
		}

		var text = string.Join(" ", args[1..^0]);
		yat.Cli.Println(text);
	}
}
