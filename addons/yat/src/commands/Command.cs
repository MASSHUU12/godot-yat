using System.Linq;
using System.Text;
using YAT.Helpers;

namespace YAT.Commands
{
	public partial interface ICommand
	{
		/// <summary>
		/// Gets or sets the YAT instance associated with this command.
		/// </summary>
		public YAT Yat { get; set; }

		/// <summary>
		/// Executes the YAT command with the given arguments.
		/// </summary>
		/// <param name="args">The arguments to pass to the command.</param>
		public CommandResult Execute(params string[] args);

		/// <summary>
		/// Generates the manual for the command, including its name,
		/// description, manual, and aliases.
		/// </summary>
		/// <param name="args">The arguments for the command.</param>
		/// <returns>The manual for the command.</returns>
		public virtual string GenerateCommandManual(params string[] args)
		{
			CommandAttribute attribute = AttributeHelper.GetAttribute<CommandAttribute>(this);

			if (string.IsNullOrEmpty(attribute.Manual))
			{
				return $"Command {attribute.Name} does not have a manual.";
			}

			StringBuilder sb = new();

			sb.AppendLine($"[p align=center][font_size=22]{attribute.Name}[/font_size][/p]");
			sb.AppendLine($"[p align=center]{attribute.Description}[/p]");
			sb.AppendLine(attribute.Manual);
			sb.AppendLine("\n[b]Aliases[/b]:");
			sb.AppendLine(attribute.Aliases.Length > 0
					? string.Join("\n", attribute.Aliases.Select(alias => $"[ul]\t{alias}[/ul]"))
					: "[ul]\tNone[/ul]");

			return sb.ToString();
		}
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

	/// <summary>
	/// Represents the result of executing a command.
	/// </summary>
	public enum CommandResult
	{
		Success,
		Failure,
		InvalidArguments,
		InvalidCommand,
	}
}
