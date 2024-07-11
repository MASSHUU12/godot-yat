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
    public CommandResult Execute(CommandData data);

    public static CommandResult Success(string message = "")
    {
        return new(ECommandResult.Success, message);
    }

    public static CommandResult Failure(string message = "")
    {
        return new(ECommandResult.Failure, message);
    }

    public static CommandResult InvalidArguments(string message = "")
    {
        return new(ECommandResult.InvalidArguments, message);
    }

    public static CommandResult InvalidCommand(string message = "")
    {
        return new(ECommandResult.InvalidCommand, message);
    }

    public static CommandResult InvalidPermissions(string message = "")
    {
        return new(ECommandResult.InvalidPermissions, message);
    }

    public static CommandResult InvalidState(string message = "")
    {
        return new(ECommandResult.InvalidState, message);
    }

    public static CommandResult NotImplemented(string message = "")
    {
        return new(ECommandResult.NotImplemented, message);
    }

    public static CommandResult UnknownCommand(string message = "")
    {
        return new(ECommandResult.UnknownCommand, message);
    }

    public static CommandResult UnknownError(string message = "")
    {
        return new(ECommandResult.UnknownError, message);
    }

    public static CommandResult Ok(string message = "")
    {
        return new(ECommandResult.Ok, message);
    }

    public virtual StringBuilder GenerateUsageInformation()
    {
        var usage = this.GetAttribute<UsageAttribute>()!;
        var command = this.GetAttribute<CommandAttribute>()!;
        var arguments = this.GetAttributes<ArgumentAttribute>();

        string GetUsage()
        {
            if (command.Manual != string.Empty)
            {
                return command.Manual;
            }

            if (usage is not null)
            {
                return $"[b]Usage[/b]: {usage.Usage}";
            }

            return string.Format(
                "[b]Usage[/b]: {0} {1}",
                command.Name,
                arguments?.Length > 0
                    ? string.Join(' ', arguments.Select(arg => $"[i]{arg.Name}[/i]"))
                    : string.Empty
            );
        }

        return new(GetUsage());
    }

    public virtual StringBuilder GenerateCommandManual()
    {
        CommandAttribute command = Reflection.GetAttribute<CommandAttribute>(this)!;
        UsageAttribute usage = Reflection.GetAttribute<UsageAttribute>(this)!;
        DescriptionAttribute description = Reflection.GetAttribute<DescriptionAttribute>(this)!;
        bool isThreaded = Reflection.HasAttribute<ThreadedAttribute>(this);

        StringBuilder sb = new();

        _ = sb.AppendFormat(
                "[p align=center][font_size=22]{0}[/font_size] [font_size=14]{1}[/font_size][/p]",
                command.Name,
                isThreaded ? "[threaded]" : string.Empty
            )
            .AppendLine();
        _ = sb.AppendLine($"[p align=center]{description?.Description ?? command.Description}[/p]")
            .Append(GenerateUsageInformation())
            .AppendLine("\n[b]Aliases[/b]:")
            .AppendLine(command.Aliases.Length > 0
                ? string.Join("\n", command.Aliases.Select(alias => $"[ul]\t{alias}[/ul]"))
                : "[ul]\tNone[/ul]");

        return sb;
    }

    public virtual StringBuilder GenerateArgumentsManual()
    {
        var arguments = Reflection.GetAttributes<ArgumentAttribute>(this);

        if (arguments is null || arguments.Length == 0)
        {
            return new("\nThis command does not have any arguments.");
        }

        StringBuilder sb = new();

        _ = sb.AppendLine("[p align=center][font_size=18]Arguments[/font_size][/p]");

        foreach (ArgumentAttribute arg in arguments)
        {
            string types = string.Join(" | ", arg.Types.Select(t => t.Type));

            _ = sb.AppendLine($"[b]{arg.Name}[/b]: {types} - {arg.Description}");
        }

        return sb;
    }

    public virtual StringBuilder GenerateOptionsManual()
    {
        var options = Reflection.GetAttributes<OptionAttribute>(this);

        if (options is null || options.Length == 0)
        {
            return new("\nThis command does not have any options.");
        }

        StringBuilder sb = new();

        _ = sb.AppendLine("[p align=center][font_size=18]Options[/font_size][/p]");

        foreach (OptionAttribute opt in options)
        {
            string types = string.Join(" | ", opt.Types.Select(t => t.Type));

            _ = sb.AppendLine($"[b]{opt.Name}[/b]: {types} - {opt.Description}");
        }

        return sb;
    }

    public virtual StringBuilder GenerateSignalsManual()
    {
        EventInfo[] signals = Reflection.GetEvents(
            this,
            BindingFlags.DeclaredOnly
            | BindingFlags.Instance
            | BindingFlags.Public
        );

        if (signals.Length == 0)
        {
            return new("\nThis command does not have any signals.");
        }

        StringBuilder sb = new();

        _ = sb.AppendLine("[p align=center][font_size=18]Signals[/font_size][/p]");

        foreach (EventInfo signal in signals)
        {
            _ = sb.AppendLine(signal.Name);
        }

        return sb;
    }
}
