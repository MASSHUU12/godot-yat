namespace YAT.Commands
{
	public interface IExtension
	{
		public CommandResult Execute(YAT yat, params string[] args);
	}

	[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
	public partial class ExtensionAttribute : System.Attribute
	{
		public string Name { get; private set; }
		public string Description { get; private set; }
		public string[] Aliases { get; private set; }

		public ExtensionAttribute(string name, string description = "", params string[] aliases)
		{
			Name = name;
			Description = description;
			Aliases = aliases;
		}
	}
}
