public partial class Restart : IYatCommand
{
	public string Name => "restart";

	public string Description => "Restarts the level.";

	public string Usage => "restart";

	public string[] Aliases => new string[] { "reboot" };

	public void Execute(YAT yat, params string[] args)
	{
		yat.Terminal.Println($"Restarting {yat.GetTree().CurrentScene.Name}...");
		yat.GetTree().ReloadCurrentScene();
	}
}
