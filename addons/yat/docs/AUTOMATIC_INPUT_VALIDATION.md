<div align="center">
	<h3>Automatic input validation</h1>
	<p>Here you will find information on how YAT can perform validation of arguments, and options for your commands automatically.</p>
</div>

<br />

> If you want documentation to be created based on attributes,
> but do not want validation you can use the `NoValidate` attribute.

Custom input validation is more flexible, but at the same time cumbersome, it can negatively affect the readability of the code as well as the time it takes to create a command. For this reason, YAT allows automatic input validation in two ways: required arguments and optional options.

During validation, YAT checks whether the submitted data meets the relevant requirements and additionally performs conversion to the appropriate type. If the data does not meet the requirements, YAT displays an appropriate message.

> For information on how to get data that has passed validation, see [CREATING_COMMANDS.md](./CREATING_COMMANDS.md).

### Defining data type requirements

Both arguments and options allow flexibility in creating requirements for the type of data to be submitted.

The created requirement can consist of a single type, e.g., `int`, `float`. It can also take several options like `int|float`, or `yes|no`. Multiple options are separated by a pipe `|`.

Supported data types:

-   string
-   int
-   float
-   bool
-   constant string (e.q., `yes`, `normal`, `wireframe` etc.)

Both `string`, `int` and `float` support defining the ranges they can take. In the case of `string` it is the minimum and maximum length of the string, in the case of `int` and `float` it is the minimum and maximum value.

The limit for ranges is `-3.4028235E+38` and `3.4028235E+38`.

Example of a requirement with one type and a range of values:

```c
int(5:15) -> The value passed can only be an integer in the range 5 -15.
```

An example of a requirement with many possibilities:

```
normal|unshaded|wireframe|int(0:30)
```

In this case, the transferred data can take either a numeric value from 0 to 30 or one of the specified strings.

#### Creating a range

The range is created in parentheses given after the type that supports the range. The minimum and maximum values must be separated by a colon `:`.

One of the values may be omitted (the colon must still be present), in which case:
- the minimum value is omitted: then it takes the value `-3.4028235E+38`,
- maximum value is omitted: then it takes the value `3.4028235E+38`.

Example of a range of values with both limits given:

```cs
int(5:15) -> range is 5 - 15
```

Example of a range of values with only maximum limit given:

```cs
int(:15) -> range is -3.4028235E+38 - 15
```

Example of a range of values with only minimum limit given:

```cs
int(5:) -> range is 5 - 3.4028235E+38
```

### Arguments

> The order of arguments matters.

Arguments are required, if an argument is missing or data that does not meet the requirements is passed,
validation will fail and the command will not run.

You can specify arguments using the `Argument` attribute:

```cs
[Argument("action", "move|jump", "Action to perform.")]
[Argument("direction", "left|right|int", "Direction to move/jump.")]
```

In the above example, the command takes two arguments.
The first argument can take one of two values: `move` or `jump`.
The second argument has three possibilities, it can take `"left"`, `"right"` or a `number`.

### Options

Options are **not** required, however, if an option is passed, but the data does not match requirements, validation will fail and the command will not run.

You can specify options using the `Option` attribute:

```cs
[Option("-action", "move|jump", "Action to perform.")]
[Option("-direction", "left|right|int(-1:1)", "Direction to move.")]
```

In the above example, command can take two options.
The first option can take one of two values: `move` or `jump`.
The second option has three possibilities, it can take `"left"`, `"right"` or a `number` limited to a range from -1 to 1.

#### Arrays

Options, unlike arguments, can also take an array of data of a given type, you just need to add `...` at the end of the type definition.

Example:

```cs
[Option("-p", "int(1:99)...", "Lorem ipsum.", new int[] { 1 })]
```

In this case, the transfer of data to the option is as follows:

```sh
some_command -p=5,2,32,47
```

#### Default values

Options also support `default values`, which will be assigned to them when the user does not pass an option when running the command. If the default value is not set, and the user does not use the option, then its value is set to `null` (`false` if type is bool):

```cs
[Option("-action", "move|jump", "Action to perform.", "move")]
```

In the example above, if the user does not use the `-action` option then it will default to `move`.
