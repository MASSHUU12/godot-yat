<div align="center">
 <h3>Automatic input validation</h1>
 <p>Here you will find information on how YAT can perform validation of arguments, and options for your commands automatically.</p>
</div>

> If you want documentation to be created based on attributes,
> but do not want validation you can use the `NoValidate` attribute.

Custom input validation is more flexible, but at the same time cumbersome, it can negatively affect the readability of the code as well as the time it takes to create a command. For this reason, YAT allows automatic input validation in two ways: required arguments and optional options.

During validation, YAT checks whether the submitted data meets the relevant requirements and additionally performs conversion to the appropriate type. If the data does not meet the requirements, YAT displays an appropriate message.

> For information on how to get data that has passed validation, see [CREATING_COMMANDS.md](./CREATING_COMMANDS.md).

### Defining data type requirements

Both arguments and options allow flexibility in creating requirements for the type of data to be submitted.

The created requirement can consist of a single type, e.g., **int**, **float**. It can also take several options like **int|float**, or **yes|no**. Multiple options are separated by a pipe **|**.

Supported data types:

- [string](#string)
- [int](#int)
- [float](#float)
- [bool](#bool)
- [enum](#enum)
- [constants](#constants)

Example of a requirement with one type and a range of values:

```c
int(5:15) -> The value can be only an integer in the range 5-15.
```

An example of a requirement with many possibilities:

```txt
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
float(:15) -> range is -3.4028235E+38 - 15
```

Example of a range of values with only minimum limit given:

```cs
float(5:) -> range is 5 - 3.4028235E+38
```

#### String

The **string** type allows the command to accept strings of characters.

String supports setting a minimum and/or maximum string length,
see [Creating a range](#creating-a-range).

String can be written in two ways:

- one word (no spaces)
- multiple words (in quotation marks)

#### Int

The **int** type allows the command to accept integers (the range of numbers corresponds to that of the float type).

Int supports setting a minimum and/or maximum value,
see [Creating a range](#creating-a-range).

#### Float

The **float** type allows the command to accept floats.

Float supports setting a minimum and/or maximum value,
see [Creating a range](#creating-a-range).

#### Bool

The **bool** type allows logical values to be passed to commands (**true**/**false**).

For **arguments**, the value true/false must be passed.
For an **option**, passing it is treated as true, the absence of an option is treated as false.

#### Enum

The **enum** type allows to map constant values to numbers.

It is defined as follows:

```cs
[Argument("name", "enum(A:1, B:2, C:3)")]
```

In the above example, the number **2** is assigned for the value **B**. When the user passes **B** to the command, it will be replaced by the number **2**.

#### Constants

**Constants** are anything that is not a defined type. For example, "left|middle|right" is three different constants, the user must specify the name of any of these constants.

### Arguments

Arguments are **required**, if an argument is missing or data that does not meet the requirements is passed,
validation will fail and the command will not run.

When calling a command, the arguments must be given in the order in which they are defined.

You can specify arguments using the `Argument` attribute:

```cs
[Argument("action", "move|jump", "Action to perform.")]
[Argument("direction", "left|right|int", "Direction to move/jump.")]
```

In the above example, the command takes two arguments.
The first argument can take one of two values: **move** or **jump**.
The second argument has three possibilities, it can take **"left"**, **"right"** or a **number**.

### Options

Options are **not** required, however, if an option is passed, but the data does not match requirements, validation will fail and the command will not run.

The order of the passed options does not matter as long as they are passed after all the arguments.

You can specify options using the `Option` attribute:

```cs
[Option("--action", "move|jump", "Action to perform.")]
[Option("-direction", "left|right|int(-1:1)", "Direction to move.")]
```

In the above example, command can take two options.
The first option can take one of two values: **move** or **jump**.
The second option has three possibilities, it can take **"left"**, **"right"** or a **number** limited to a range from -1 to 1.

#### Arrays

Options, unlike arguments, can also take an array of data of a given type, you just need to add **...** at the end of the type definition.

Example:

```cs
[Option("-p", "int(1:99)...", "Lorem ipsum.")]
```

In this case, the transfer of data to the option is as follows:

```sh
some_command -p=5,2,32,47
```

#### Default values

Options also support **default values**, which will be assigned to them when the user does not pass an option when running the command. If the default value is not set, and the user does not use the option, then its value is set to **null** (**false** if type is bool):

```cs
[Option("--action", "move|jump", "Action to perform.", "move")]
```

In the example above, if the user does not use the **--action** option then it will default to **move**.
