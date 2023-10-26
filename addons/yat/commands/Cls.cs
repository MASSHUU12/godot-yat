public partial class Cls : IYatCommand
{
	public string Name => "cls";

	public string Description => "Clears the console.";

	public string Usage => "cls";

	public string[] Aliases => new string[] { "clear" };

	public void Execute(YAT yat, params string[] args)
	{
		yat.Terminal.Clear();
	}
}
