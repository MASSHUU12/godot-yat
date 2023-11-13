using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;

namespace YAT.Interfaces
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
		/// <param name="cArgs">The converted arguments to pass to the command.</param>
		/// <param name="args">The arguments to pass to the command.</param>
		/// <returns>The result of the command execution.</returns>
		public virtual CommandResult Execute(Dictionary<string, object> cArgs, params string[] args)
		{
			return CommandResult.InvalidCommand;
		}

		/// <summary>
		/// Executes the YAT command with the given arguments.
		/// </summary>
		/// <param name="cArgs">The converted arguments to pass to the command.</param>
		/// <param name="ct">The cancellation token.</param>
		/// <param name="args">The arguments to pass to the command.</param>
		/// <returns>The result of the command execution.</returns>
		public virtual CommandResult Execute(Dictionary<string, object> cArgs, CancellationToken ct, params string[] args)
		{
			return CommandResult.InvalidCommand;
		}

		/// <summary>
		/// Generates the manual for the command, including its name,
		/// description, manual, and aliases.
		/// </summary>
		/// <param name="args">The arguments for the command.</param>
		/// <returns>The manual for the command.</returns>
		public virtual string GenerateCommandManual(params string[] args)
		{
			// TODO: Use ArgumentAttribute to generate manual for command arguments.

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
}
