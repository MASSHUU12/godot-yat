<div align="center">
	<h3>RegisteredCommands</h1>
	<p>Class responsible for storing and registering commands.</p>
</div>

### Description

**Inherits**: Node

This class gives access to registered commands, adding new ones, and is in charge of registering built-in commands.

### Properties

| Type                            | Name       | Default |
| ------------------------------- | ---------- | ------- |
| static Dictionary<string, Type> | Registered | new     |

### Methods

| Type | Definition                                 |
| ---- | ------------------------------------------ |
| void | static AddCommand (Type commandType)       |
| void | static AddCommand (params Type[] commands) |

### Signals

### -

### Enumerations

enum **AddingResult**

-   **Success**
-   **UnknownCommand**
-   **MissingAttribute**
