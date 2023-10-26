namespace YAT
{
    [Command("echo", "Displays the given text.", "[b]Usage[/b]: echo [i]text[/i]")]
    public partial class Echo : ICommand
    {
        public void Execute(YAT yat, params string[] args)
        {
            if (args.Length < 2)
            {
                yat.Terminal.Println("Invalid input.");
                return;
            }

            var text = string.Join(" ", args[1..^0]);
            yat.Terminal.Println(text);
        }
    }

}
