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
	/// <summary>
	/// Name of the Yat command.
	/// </summary>
	public string Name { get; private set; }
	/// <summary>
	/// Short description of the Yat command.
	///
	/// Note: Supports BBCode.
	/// </summary>
	public string Description { get; private set; }
	/// <summary>
	/// Manual for this command. <br />
	///
	/// Note: Supports BBCode.
	/// </summary>
	public string Manual { get; private set; }
	/// <summary>
	/// Aliases for this command.
	/// </summary>
	public string[] Aliases { get; private set; }

	public CommandAttribute(string name, string description = "", string manual = "", params string[] aliases)
	{
		Name = name;
		Description = description;
		Manual = manual;
		Aliases = aliases;
	}
}
