public partial interface IYatCommand
{
	/// <summary>
	/// Executes the YAT command with the given arguments.
	/// </summary>
	/// <param name="yat">The YAT instance to execute the command on.</param>
	/// <param name="args">The arguments to pass to the command.</param>
	public void Execute(YAT yat, params string[] args);
}


[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
public partial class CommandAttribute : System.Attribute
{
	public string Name { get; }
	public string Description { get; }
	public string Manual { get; }
	public string[] Aliases { get; }

	public CommandAttribute(string name, string description = "", string manual = "", params string[] aliases)
	{
		Name = name;
		Description = description;
		Manual = manual;
		Aliases = aliases;
	}
}
