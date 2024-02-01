<div align="center">
	<h3>Extensible</h1>
	<p>Base class, on the basis of which extensible commands are created.</p>
</div>

### Description

**Inherits**: Node

This class is used to create extensible command on top of it and allows registering and store all extensions.

### Properties

| Type                                                 | Name       | Default |
| ---------------------------------------------------- | ---------- | ------- |
| Dictionary<StringName, Dictionary<StringName, Type>> | Extensions | new     |

### Methods

| Type                                | Definition                                                   |
| ----------------------------------- | ------------------------------------------------------------ |
| static bool                         | RegisterExtension (StringName commandName, Type extension)   |
| static bool                         | UnregisterExtension (StringName commandName, Type extension) |
| virtual CommandResult               | ExecuteExtension (Type extension, CommandData args)          |
| virtual StringBuilder               | GenerateExtensionsManual ()                                  |
| static Dictionary<StringName, Type> | GetCommandExtensions (StringName commandName)                |

### Signals

**-**

### Enumerations

**-**
