using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Types;

namespace YAT.Interfaces;

public interface ICommand
{
    CommandResult Execute(CommandData data);

    static CommandResult Success(string[]? OutData = null, string? message = null)
    {
        return new(ECommandResult.Success, OutData, message ?? string.Empty);
    }

    static CommandResult Failure(string? message = null)
    {
        return new(ECommandResult.Failure, null, message ?? string.Empty);
    }

    static CommandResult InvalidArguments(string? message = null)
    {
        return new(ECommandResult.InvalidArguments, null, message ?? string.Empty);
    }

    static CommandResult InvalidCommand(string? message = null)
    {
        return new(ECommandResult.InvalidCommand, null, message ?? string.Empty);
    }

    static CommandResult InvalidPermissions(string? message = null)
    {
        return new(ECommandResult.InvalidPermissions, null, message ?? string.Empty);
    }

    static CommandResult InvalidState(string? message = null)
    {
        return new(ECommandResult.InvalidState, null, message ?? string.Empty);
    }

    static CommandResult NotImplemented(string? message = null)
    {
        return new(ECommandResult.NotImplemented, null, message ?? string.Empty);
    }

    static CommandResult UnknownCommand(string? message = null)
    {
        return new(ECommandResult.UnknownCommand, null, message ?? string.Empty);
    }

    static CommandResult UnknownError(string? message = null)
    {
        return new(ECommandResult.UnknownError, null, message ?? string.Empty);
    }

    static CommandResult Ok(string[]? OutData = null, string? message = null)
    {
        return new(ECommandResult.Ok, OutData, message ?? string.Empty);
    }

    virtual StringBuilder GenerateUsageInformation()
    {
        UsageAttribute? usage = this.GetAttribute<UsageAttribute>()!;
        CommandAttribute command = this.GetAttribute<CommandAttribute>()!;
        IEnumerable<ArgumentAttribute>? arguments = this.GetAttributes<ArgumentAttribute>();

        string GetUsage()
        {
            if (!string.IsNullOrEmpty(command.Manual))
            {
                return command.Manual;
            }

            if (usage is not null)
            {
                return $"[b]Usage[/b]: {usage.Usage}";
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                "[b]Usage[/b]: {0} {1}",
                command.Name,
                arguments?.Any() == true
                    ? string.Join(' ', arguments.Select(arg => $"[i]{arg.Name}[/i]"))
                    : string.Empty
            );
        }

        return new(GetUsage());
    }

    virtual StringBuilder GenerateCommandManual()
    {
        CommandAttribute command = Reflection.GetAttribute<CommandAttribute>(this)!;
        UsageAttribute usage = Reflection.GetAttribute<UsageAttribute>(this)!;
        DescriptionAttribute description = Reflection.GetAttribute<DescriptionAttribute>(this)!;
        bool isThreaded = Reflection.HasAttribute<ThreadedAttribute>(this);

        StringBuilder sb = new();

        _ = sb.AppendFormat(
                CultureInfo.InvariantCulture,
                "[p align=center][font_size=22]{0}[/font_size] [font_size=14]{1}[/font_size][/p]",
                command.Name,
                isThreaded ? "[threaded]" : string.Empty
            )
            .AppendLine();
        _ = sb.AppendLine(
            CultureInfo.InvariantCulture,
            $"[p align=center]{description?.Description ?? command.Description}[/p]"
        )
            .Append(GenerateUsageInformation())
            .AppendLine("\n[b]Aliases[/b]:")
            .AppendLine(command.Aliases.Length > 0
                ? string.Join("\n", command.Aliases.Select(
                    static alias => $"[ul]\t{alias}[/ul]")
                )
                : "[ul]\tNone[/ul]");

        return sb;
    }

    virtual StringBuilder GenerateManual<T>(string name) where T : Attribute
    {
        StringBuilder sb = new(
            $"[p align=center][font_size=22]{name}[/font_size][/p]\n"
        );
        IEnumerable<T>? attributes = Reflection.GetAttributes<T>(this);

        if (attributes?.Any() != true)
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

    virtual StringBuilder GenerateArgumentsManual()
    {
        return GenerateManual<ArgumentAttribute>("Arguments");
    }

    virtual StringBuilder GenerateOptionsManual()
    {
        return GenerateManual<OptionAttribute>("Options");
    }

    virtual StringBuilder GenerateSignalsManual()
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
