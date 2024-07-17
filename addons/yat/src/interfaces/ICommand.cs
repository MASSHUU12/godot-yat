using System;
using System.Collections.Generic;
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

    public static CommandResult Success(string? message = null)
    {
        return new(ECommandResult.Success, message ?? string.Empty);
    }

    public static CommandResult Failure(string? message = null)
    {
        return new(ECommandResult.Failure, message ?? string.Empty);
    }

    public static CommandResult InvalidArguments(string? message = null)
    {
        return new(ECommandResult.InvalidArguments, message ?? string.Empty);
    }

    public static CommandResult InvalidCommand(string? message = null)
    {
        return new(ECommandResult.InvalidCommand, message ?? string.Empty);
    }

    public static CommandResult InvalidPermissions(string? message = null)
    {
        return new(ECommandResult.InvalidPermissions, message ?? string.Empty);
    }

    public static CommandResult InvalidState(string? message = null)
    {
        return new(ECommandResult.InvalidState, message ?? string.Empty);
    }

    public static CommandResult NotImplemented(string? message = null)
    {
        return new(ECommandResult.NotImplemented, message ?? string.Empty);
    }

    public static CommandResult UnknownCommand(string? message = null)
    {
        return new(ECommandResult.UnknownCommand, message ?? string.Empty);
    }

    public static CommandResult UnknownError(string? message = null)
    {
        return new(ECommandResult.UnknownError, message ?? string.Empty);
    }

    public static CommandResult Ok(string? message = null)
    {
        return new(ECommandResult.Ok, message ?? string.Empty);
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
                arguments is not null && arguments.Any()
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

    public virtual StringBuilder GenerateManual<T>(string @for) where T : Attribute
    {
        StringBuilder sb = new(
            $"[p align=center][font_size=22]{@for}[/font_size][/p]\n"
        );
        IEnumerable<T>? attributes = Reflection.GetAttributes<T>(this);

        if (attributes is null || !attributes.Any())
        {
            _ = sb.AppendLine("\nThis command does not have any.");
            return sb;
        }

        foreach (T attribute in attributes)
        {
            _ = sb.AppendLine(attribute.ToString());
        }

        return sb;
    }

    public virtual StringBuilder GenerateArgumentsManual()
    {
        return GenerateManual<ArgumentAttribute>("Arguments");
    }

    public virtual StringBuilder GenerateOptionsManual()
    {
        return GenerateManual<OptionAttribute>("Options");
    }

    public virtual StringBuilder GenerateSignalsManual()
    {
        IEnumerable<EventInfo> signals = Reflection.GetEvents(
            this,
            BindingFlags.DeclaredOnly
            | BindingFlags.Instance
            | BindingFlags.Public
        );

        if (!signals.Any())
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
