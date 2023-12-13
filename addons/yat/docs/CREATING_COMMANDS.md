<div align="center">
	<h3>Creating commands</h1>
	<p>Here you can find information on creating new commands, extending existing commands, and adding them to YAT</p>
</div>

## Creating commands

Instructions on how to create a `regular command` can be found [here](./REGULAR_COMMANDS.md), while one that runs on a `separate thread` can be found [here](./THREADED_COMMANDS.md).

Information about automatic input validation can be found [here](./AUTOMATIC_INPUT_VALIDATION.md).

### Overridable methods

-   GenerateCommandManual
-   GenerateArgumentsManual
-   GenerateOptionsManual
-   GenerateSignalsManual

## Adding commands

To add a command to the YAT all you have to do is call `AddCommand` method on YAT instance:

```csharp
var yat = GetNode<YAT>("/root/YAT");
yat.AddCommand(new Cls(yat));
```

## Making commands extendable

It is possible to extend existing commands under several conditions:

1. an existing command must inherit from the class `Extensible`.
2. The command must call the `Execute` method on a specific extension. Example below.

```csharp
var variable = args[1];

if (Extensions.ContainsKey(variable))
{
   var extension = Extensions[variable];
   return extension.Execute(yat, this, args[1..]);
}
```

### Overridable methods

#### GenerateExtensionsManual

This method creates documentation for all extensions of a command. It does this by running the method `GenerateExtensionManual` on each extension.

## Extending commands

To be able to extend a command, you first need to create an extension,
which is a class that implements the `IExtension` interface,
and contains the `Extension` attribute, the rest works just like a regular command.

You can find an example of such a class in [example](./example) folder.

### Overridable methods

#### GenerateExtensionManual

This method generates extension documentation based on the metadata added via the `Extension` attribute.

## Signals

| Name            | Arguments             | Description                                          |
| --------------- | --------------------- | ---------------------------------------------------- |
| OptionsChanged  | YatOptions            | Sent when YAT options have changed                   |
| YatReady        | N/A                   | Sent when YAT is ready                               |
| CommandStarted  | command, args         | Signal emitted when a command execution has started. |
| CommandFinished | command, args, result | Signal emitted when a command has been executed      |
| TerminalOpened  | N/A                   | Emitted when terminal is opened.                     |
| TerminalClosed  | N/A                   | Emitted when terminal is closed.                     |
