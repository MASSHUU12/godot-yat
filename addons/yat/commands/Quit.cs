public partial class Quit : IYatCommand
{
    public string Name => "quit";

    public string Description => "Quits the game.";

    public string Usage => "quit";

    public string[] Aliases => new string[] { "exit" };

    public void Execute(string[] args, YAT yat)
    {
        yat.Terminal.Println("Quitting...");
        yat.GetTree().Quit();
    }
}
