using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
		/// Represents the result of executing a command.
		/// </summary>
		/// <param name="cArgs">The converted arguments for the command.</param>
		/// <param name="args">The arguments passed to the command.</param>
		/// <returns>The result of the command execution.</returns>
		public virtual CommandResult Execute(Dictionary<string, object> cArgs, params string[] args)
		{
			return CommandResult.NotImplemented;
		}

		/// <summary>
		/// Executes the YAT command with the given arguments.
		/// </summary>
		/// <param name="args">The arguments passed to the command.</param>
		/// <returns>The result of the command execution.</returns>
		public virtual CommandResult Execute(params string[] args)
		{
			return CommandResult.NotImplemented;
		}

		/// <summary>
		/// Executes the YAT command with the given arguments.
		/// </summary>
		/// <param name="cArgs">The converted arguments for the command.</param>
		/// <param name="ct">The cancellation token.</param>
		/// <param name="args">The arguments passed to the command.</param>
		/// <returns>The result of the command execution.</returns>
		public virtual CommandResult Execute(Dictionary<string, object> cArgs, CancellationToken ct, params string[] args)
		{
			return CommandResult.NotImplemented;
		}

		/// <summary>
		/// Executes the YAT command with the given arguments.
		/// </summary>
		/// <param name="ct">The cancellation token.</param>
		/// <param name="args">The arguments passed to the command.</param>
		/// <returns>The result of the command execution.</returns>
		public virtual CommandResult Execute(CancellationToken ct, params string[] args)
		{
			return CommandResult.NotImplemented;
		}

		/// <summary>
		/// Generates the manual for the command, including its name,
		/// description, manual, and aliases.
		/// </summary>
		/// <returns>The manual for the command.</returns>
#nullable enable
		public virtual string GenerateCommandManual()
		{
			CommandAttribute? attribute = AttributeHelper.GetAttribute<CommandAttribute>(this);

			if (string.IsNullOrEmpty(attribute?.Manual)) return "This command does not have a manual.";

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
#nullable disable

		/// <summary>
		/// Generates a manual for the arguments of the command.
		/// </summary>
		/// <returns>A string containing the manual for the arguments of the command.</returns>
		public virtual string GenerateArgumentsManual()
		{
			var attributes = AttributeHelper.GetAttributes<ArgumentAttribute>(this);

			if (attributes is null) return "\nThis command does not have any arguments.";

			StringBuilder sb = new();

			if (attributes.Length == 0)
			{
				sb.AppendLine("\nThis command does not have any arguments.");
				return sb.ToString();
			}

			sb.AppendLine("[p align=center][font_size=18]Arguments[/font_size][/p]");

			foreach (var attr in attributes)
			{
				sb.AppendLine($"[b]{attr.Name}[/b]: {(attr.Type is string[] type
						? string.Join(" | ", type)
						: attr.Type)} - {attr.Description}");
			}

			return sb.ToString();
		}

		/// <summary>
		/// Generates a manual for the command options
		/// based on the <see cref="OptionsAttribute"/> applied to the command.
		/// </summary>
		/// <returns>A string containing the manual for the command options.</returns>
		public virtual string GenerateOptionsManual()
		{
			var attributes = AttributeHelper.GetAttributes<OptionAttribute>(this);

			if (attributes is null) return "\nThis command does not have any options.";

			StringBuilder sb = new();

			if (attributes.Length == 0)
			{
				sb.AppendLine("\nThis command does not have any options.");
				return sb.ToString();
			}

			sb.AppendLine("[p align=center][font_size=18]Options[/font_size][/p]");

			foreach (var attr in attributes)
			{
				sb.AppendLine($"[b]{attr.Name}[/b]: {(attr.Type is string[] type
						? string.Join(" | ", type)
						: attr.Type)} - {attr.Description}");
			}

			return sb.ToString();
		}

		/// <summary>
		/// Generates a manual for the signals of the command.
		/// </summary>
		/// <returns>A string containing the generated manual.</returns>
		public virtual string GenerateSignalsManual()
		{
			var signals = Reflection.GetEvents(this, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

			if (signals.Length == 0) return "\nThis command does not have any signals.";

			StringBuilder sb = new();

			sb.AppendLine("[p align=center][font_size=18]Signals[/font_size][/p]");

			foreach (var signal in signals)
			{
				sb.AppendLine(signal.Name);
			}

			return sb.ToString();
		}
	}
}
