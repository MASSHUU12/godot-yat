using System.Linq;
using System.Reflection;
using System.Text;
using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;

namespace YAT.Interfaces;

public partial interface ICommand
{
	/// <summary>
	/// Represents the result of executing a command.
	/// </summary>
	/// <param name="data">The data passed to the command.</param>
	/// <returns>The result of the command execution.</returns>
	public CommandResult Execute(CommandData data);

	public virtual string GenerateCommandManual()
	{
		CommandAttribute command = AttributeHelper.GetAttribute<CommandAttribute>(this);
		bool isThreaded = AttributeHelper.HasAttribute<ThreadedAttribute>(this);

		if (string.IsNullOrEmpty(command?.Manual)) return "This command does not have a manual.";

		StringBuilder sb = new();

		sb.AppendLine(
			string.Format(
				"[p align=center][font_size=22]{0}[/font_size] [font_size=14]{1}[/font_size][/p]",
				command.Name,
				isThreaded ? "[threaded]" : string.Empty
			)
		);
		sb.AppendLine($"[p align=center]{command.Description}[/p]");
		sb.AppendLine(command.Manual);
		sb.AppendLine("\n[b]Aliases[/b]:");
		sb.AppendLine(command.Aliases.Length > 0
				? string.Join("\n", command.Aliases.Select(alias => $"[ul]\t{alias}[/ul]"))
				: "[ul]\tNone[/ul]");

		return sb.ToString();
	}

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
