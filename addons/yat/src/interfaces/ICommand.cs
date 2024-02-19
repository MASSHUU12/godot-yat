using System.Linq;
using System.Reflection;
using System.Text;
using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Types;

namespace YAT.Interfaces;

public partial interface ICommand
{
	/// <summary>
	/// Represents the result of executing a command.
	/// </summary>
	/// <param name="data">The data passed to the command.</param>
	/// <returns>The result of the command execution.</returns>
	public CommandResult Execute(CommandData data);

	public static CommandResult Success(string message = null) =>
		new(ECommandResult.Success, message);

	public static CommandResult Failure(string message = null) =>
		new(ECommandResult.Failure, message);

	public static CommandResult InvalidArguments(string message = null) =>
		new(ECommandResult.InvalidArguments, message);

	public static CommandResult InvalidCommand(string message = null) =>
		new(ECommandResult.InvalidCommand, message);

	public static CommandResult InvalidPermissions(string message = null) =>
		new(ECommandResult.InvalidPermissions, message);

	public static CommandResult InvalidState(string message = null) =>
		new(ECommandResult.InvalidState, message);

	public static CommandResult NotImplemented(string message = null) =>
		new(ECommandResult.NotImplemented, message);

	public static CommandResult UnknownCommand(string message = null) =>
		new(ECommandResult.UnknownCommand, message);

	public static CommandResult UnknownError(string message = null) =>
		new(ECommandResult.UnknownError, message);

	public virtual string GenerateCommandManual()
	{
		CommandAttribute command = Reflection.GetAttribute<CommandAttribute>(this);
		UsageAttribute usage = Reflection.GetAttribute<UsageAttribute>(this);
		DescriptionAttribute description = Reflection.GetAttribute<DescriptionAttribute>(this);
		bool isThreaded = Reflection.HasAttribute<ThreadedAttribute>(this);

		StringBuilder sb = new();

		sb.AppendLine(
			string.Format(
				"[p align=center][font_size=22]{0}[/font_size] [font_size=14]{1}[/font_size][/p]",
				command.Name,
				isThreaded ? "[threaded]" : string.Empty
			)
		);
		sb.AppendLine($"[p align=center]{description?.Description ?? command.Description}[/p]");
		sb.AppendLine(usage is null ? command.Manual : $"\n[b]Usage[/b]: {usage?.Usage}");
		sb.AppendLine("\n[b]Aliases[/b]:");
		sb.AppendLine(command.Aliases.Length > 0
				? string.Join("\n", command.Aliases.Select(alias => $"[ul]\t{alias}[/ul]"))
				: "[ul]\tNone[/ul]");

		return sb.ToString();
	}

	public virtual string GenerateArgumentsManual()
	{
		var arguments = Reflection.GetAttributes<ArgumentAttribute>(this);

		if (arguments is null) return "\nThis command does not have any arguments.";

		StringBuilder sb = new();

		if (arguments.Length == 0)
		{
			sb.AppendLine("\nThis command does not have any arguments.");
			return sb.ToString();
		}

		sb.AppendLine("[p align=center][font_size=18]Arguments[/font_size][/p]");

		foreach (var arg in arguments)
			sb.AppendLine($"[b]{arg.Name}[/b]: {string.Join(" | ", arg.Types)}");

		return sb.ToString();
	}

	public virtual string GenerateOptionsManual()
	{
		var options = Reflection.GetAttributes<OptionAttribute>(this);

		if (options is null) return "\nThis command does not have any options.";

		StringBuilder sb = new();

		if (options.Length == 0)
		{
			sb.AppendLine("\nThis command does not have any options.");
			return sb.ToString();
		}

		sb.AppendLine("[p align=center][font_size=18]Options[/font_size][/p]");

		foreach (var opt in options)
			sb.AppendLine($"[b]{opt.Name}[/b]: {string.Join(" | ", opt.Types)}");

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
