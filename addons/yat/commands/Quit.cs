public partial class Quit : IYatCommand
{
	public string Name => "quit";

	public string Description => "Quits the game.";

	public string Usage => "quit";

	public string[] Aliases => new string[] { "exit" };

	public void Execute(YAT yat, params string[] args)
	{
		yat.Terminal.Println("Quitting...");
		yat.GetTree().Quit();
	}
}
