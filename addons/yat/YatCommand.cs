public partial interface IYatCommand
{
	/// <summary>
	/// Gets the name of the CLI.
	/// </summary>
	public string Name { get; }
	/// <summary>
	/// Gets the description of the CLI command.
	/// </summary>
	public string Description { get; }
	/// <summary>
	/// Gets the usage information for the command line interface.
	/// </summary>
	public string Usage { get; }
	/// <summary>
	/// Gets the aliases for this command.
	/// </summary>
	public string[] Aliases { get; }
	/// <summary>
	/// Executes the command with the given arguments and CLI instance.
	/// </summary>
	/// <param name="args">The arguments to pass to the command.</param>
	/// <param name="cli">The CLI instance to use for executing the command.</param>
	public void Execute(string[] args, YAT yat);
}
