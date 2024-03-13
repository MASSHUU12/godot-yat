<div align="center">
	<h3>BaseTerminal</h1>
	<p>A base class containing all the components needed to run commands.</p>
</div>

### Description

**Inherits**: Control

### Properties

| Type                   | Name              | Default          |
| ---------------------- | ----------------- | ---------------- |
| bool                   | Locked            | -                |
| bool                   | Current           | true             |
| Input                  | Input             | -                |
| Output                 | Output            | -                |
| SelectedNode           | SelectedNode      | -                |
| CommandManager         | CommandManager    | -                |
| CommandValidator       | CommandValidator  | -                |
| FullWindowDisplay      | FullWindowDisplay | -                |
| ECommandResult         | LastCommandResult | Unavailable (-1) |
| LinkedList<string>     | History           | new              |
| LinkedListNode<string> | HistoryNode       | null             |

### Methods

| Type | Definition                                             |
| ---- | ------------------------------------------------------ |
| void | Print (string text, PrintType type = PrintType.Normal) |
| void | Print<T> (T text, PrintType type = PrintType.Normal)   |
| void | Clear ()                                               |

### Signals

**CloseRequested** ()

**TitleChanged** (string title)

**PositionResetRequested** ()

**SizeResetRequested** ()

**MethodCalled** (StringName method, Variant returnValue, EMethodStatus status)

### Enumerations

**-**
