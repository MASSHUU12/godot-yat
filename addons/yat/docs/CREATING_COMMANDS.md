<div align="center">
 <h3>Creating commands</h1>
 <p>Here you can find information on creating new commands, extending existing commands, and adding them to YAT.</p>
</div>

<br />

> Information about automatic input validation can be found [here](./AUTOMATIC_INPUT_VALIDATION.md).

To create a command, you need to create C# file and implement `ICommand` interface with `Command` attribute.

The `Command` attribute accepts the command **name**, and optionally its **description**, **manual** and **aliases**.
For description and manual, you can also use `Description` and `Usage` attributes.
The description and manual/usage have BBCode support.

If you do not specify **manual** or **Usage** it'll be automatically generated based on arguments.

The execution of the command begins in the **Execute** method.
The **Execute** method accepts **CommandData**, which contains probably all the things your command could ever need, these are things like: reference to YAT and BaseTerminal, raw arguments & options, converted arguments & options, cancellation token and more.

Each created command, at the end of its operation, must return a status and an optional message with which it finished. You can create it yourself, or use one of the static methods of the `ICommand` interface, such as **Success()**, or **Failure()**.

> [!NOTE]
> Each time a command is called, a new instance of it is created.
>
> If you want to maintain state between instances, you can use static variables.

As an example, let's look at Cls command:

```csharp
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("cls", "Clears the console.", aliases: "clear")]
public sealed class Cls : ICommand
{
 public CommandResult Execute(CommandData data)
 {
  data.Terminal.Clear();

  return ICommand.Success();
 }
}
```

### Threaded commands

> [!IMPORTANT]
> Using engine features via a command running on a separate thread can be problematic.
>
> Fortunately, rather most errors can be circumvented by using:
> **CallDeferred**, **CallThreadSafe** or **CallDeferredThreadGroup**.

Creating a command that runs on a separate thread looks very similar to creating a regular command.

Therefore, first create a regular command and then add a `Threaded` attribute to it, which allows it to run on a separate thread.

In the passed **CommandData** you can find the **CancellationToken** which indicates when the command was asked to terminate prematurely.

### Overridable methods

- GenerateUsageInformation
- GenerateCommandManual
- GenerateArgumentsManual
- GenerateOptionsManual
- GenerateSignalsManual

## Adding commands

To add a command to the YAT all you have to do is call `AddCommand` method on `RegisteredCommands` class:

```cs
RegisteredCommands.AddCommand(typeof(Cls));
```

## Making commands extendable

It is possible to extend existing commands under several conditions:

1. An existing command must inherit from the class `Extensible`.
2. The command must call the `ExecuteExtension` with a specific extension as an argument. Example below.

```cs
public CommandResult Execute(CommandData data)
{
   var extensions = GetCommandExtensions("command_name");

   if (extensions.TryGetValue((string)data.Arguments["variable"], out Type extension))
      return ExecuteExtension(extension, data with { RawData = data.RawData[1..] });

   return ICommand.Failure("Variable not found.");
}
```

### Overridable methods

#### GenerateExtensionsManual

This method creates documentation for all extensions of a command. It does this by running the method `GenerateExtensionManual` on each extension.

## Extending commands

To be able to extend a command, you first need to create an extension,
which is a class that implements the `IExtension` interface,
and contains the `Extension` attribute, the rest works just like a regular command.

Remember to register every extension inside a command:

```cs
Extensible.RegisterExtension("target_command", typeof(YourExtension));
```

You can find an example of such a class in [example](./example) folder.

### Overridable methods

#### GenerateExtensionManual

This method generates extension documentation based on the metadata added via the `Extension` attribute.
