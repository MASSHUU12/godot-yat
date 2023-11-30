<div align="center">
	<h3>Automatic input validation</h1>
	<p>Here you will find information on how YAT can perform validation of arguments, and options for your commands automatically.</p>
</div>

## Automatic input Validation

> If you want documentation to be created based on attributes,
> but do not want validation you can use the `NoValidate` attribute.

Custom input validation gives you more options, but can be cumbersome and unnecessarily lengthy commands. For this reason, YAT supports two ways of validating data: required (arguments), and optional (options).

After the validation is complete, YAT returns the data converted to the appropriate type, or cancels the command execution if a validation error occurs.

For information on how to get data that has passed validation, see [CREATING_COMMANDS.md](./CREATING_COMMANDS.md).

Supported data types:

-   string
-   int
-   float
-   double
-   bool

### Arguments

> The order of arguments above the class matters.

Arguments are required, if an argument is missing or data that does not meet the requirements is passed,
validation will fail and the command will not run.

You can specify what arguments, under what rules and in what order the command expects using the `Argument` attribute.

Arguments are defined as follows:

-   `argument` - the user must use the name of this argument.
-   `argument:data_type` - the user must use the given data type in place of this argument.
-   `argument:[option1, option2]` - the user must use any of the given options in place of the argument.
-   `argument:[option1, data_type, option3]` - the user must specify any of the given options or use the listed data type in place of the given argument

Numeric types can accept ranges of values. For example:

```csharp
[Argument("step", "double(0, 69.420)", "Number of steps.")]
```

Example of use:

```csharp
[Argument("action", "[move, jump]", "Action to perform.")]
[Argument("direction", "[left, right, int]", "Direction to move/jump.")]
```

In the above example, the command takes two arguments.
The first argument can take one of two values: `move` or `jump`.
The second argument has three possibilities, it can take `"left"`, `"right"` or a `number`.

### Options

> Options are not required, however, if an option is passed,
> but the data does not match, validation will fail and the command will not run.

Options are defined as follows:

-   `-opt` - passed option must match the name of the option. If option is present it returns `true` if not `false`.
-   `-opt=data_type` - value of passed option must be of the specified data type.
-   `-opt=option1|data_type|option3` - value of passed option must be of the specified data type or one of the specified options.
-   `-opt=data_type...` - value of passed option must be an array of values of the specified data type.

Numeric types can accept ranges of values. For example:

```csharp
[Option("-step", "double(0:69.420)", "Number of steps to move.")]
```

Example of use:

```csharp
[Option("-action", "move|jump", "Action to perform.")]
[Option("-direction", "left|right|int(-1:1)", "Direction to move.")]
```

In the above example, command can take two options.
The first option can take one of two values: `move` or `jump`.
The second option has three possibilities, it can take `"left"`, `"right"` or a `number` limited to a range from -1 to 1.
