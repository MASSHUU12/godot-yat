<div align="center">
	<h3>Threaded commands</h1>
	<p>Here you can find information about creating threaded commands.</p>
</div>

> [!NOTE]
> General information on creating commands can be found [here](./CREATING_COMMANDS.md).

### Creating commands

> [!IMPORTANT]
> Using engine features via a command running on a separate thread can be problematic.
>
> Fortunately, rather most errors can be circumvented by using methods:
> CallDeferred, CallThreadSafe or CallDeferredThreadGroup.

Creating a command that runs on a separate thread looks very similar to creating a [regular command](./REGULAR_COMMANDS.md).

Therefore, first create a `regular command` and then add a `Threaded` attribute to it, which allows it to run on a separate thread.

### Command execution

The execution of the command begins in the `Execute` method.

YAT supports two types of `Execute` method for threaded commands:

```csharp
public CommandResult Execute(CancellationToken ct, params string[] args)
```

Basic version, passes raw arguments, works with and without automatic validation.
Accepts the `CancellationToken`, which indicates when the command was asked to terminate prematurely

```csharp
public CommandResult Execute(Dictionary<string, object> cArgs, params string[] args)
```

It works similarly to the basic version with the difference that one of the arguments is a dictionary with the arguments converted to the appropriate types.
