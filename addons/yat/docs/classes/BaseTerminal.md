<div align="center">
	<h3>BaseTerminal</h1>
	<p>A base class containing all the components needed to run commands.</p>
</div>

### Description

**Inherits**: Control

### Properties

| Type                   | Name             | Default |
| ---------------------- | ---------------- | ------- |
| bool                   | Locked           | -       |
| Input                  | Input            | -       |
| Output                 | Output           | -       |
| TerminalContext        | Context          | -       |
| SelectedNode           | SelectedNode     | -       |
| CommandValidator       | CommandValidator | -       |
| CommandManager         | CommandManager   | -       |
| LinkedList<string>     | History          | new     |
| LinkedListNode<string> | HistoryNode      | null    |

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

### Enumerations

**-**
