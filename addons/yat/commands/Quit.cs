namespace YAT
{
    [Command("quit", "Quits the game.", "[b]Usage[/b]: quit", "exit")]
    public partial class Quit : ICommand
    {
        public void Execute(YAT yat, params string[] args)
        {
            yat.Terminal.Println("Quitting...");
            yat.GetTree().Quit();
        }
    }
}
