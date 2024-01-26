<div align="center">
	<h3>RegisteredCommands</h1>
	<p>Class responsible for storing and registering commands.</p>
</div>

### Description

**Inherits**: Node

This class gives access to registered and quick commands, adding new ones, and is in charge of registering built-in commands.

### Properties

| Type                            | Name          | Default |
| ------------------------------- | ------------- | ------- |
| static Dictionary<string, Type> | Registered    | new     |
| Export QuickCommands            | QuickCommands | new     |

### Methods

| Type | Definition                                    |
| ---- | --------------------------------------------- |
| void | static AddCommand (Type commandType)          |
| void | static AddCommand (params Type[] commands)    |
| bool | AddQuickCommand (string name, string command) |
| bool | RemoveQuickCommand (string name)              |
| bool | GetQuickCommands ()                           |

### Signals

**QuickCommandsChanged** ()

### Enumerations

enum **AddingResult**

-   **Success**
-   **UnknownCommand**
-   **MissingAttribute**
