<div align="center">
	<h3>Output</h1>
	<p>Displays text in the terminal window.</p>
</div>

### Description

**Inherits**: RichTextLabel

`Output` displays data returned by commands, or other classes. Supports `BBCode`.

### Properties

| Type | Name | Default |
| ---- | ---- | ------- |
| -    | -    | -       |

### Methods

| Type | Definition                                                                           |
| ---- | ------------------------------------------------------------------------------------ |
| void | Print (string message, LogOutput output = LogOutput.Terminal)                        |
| void | Error (string message, LogOutput output = LogOutput.Terminal)                        |
| void | Warning (string message, LogOutput output = LogOutput.Terminal)                      |
| void | Info (string message, LogOutput output = LogOutput.Terminal)                         |
| void | Success (string message, LogOutput output = LogOutput.Terminal)                      |
| void | Debug (string message, LogOutput output = LogOutput.Terminal)                        |
| void | Trace (string message, bool detailed = false, LogOutput output = LogOutput.Terminal) |

### Signals

#### -

### Enumerations

flags **LogOutput**

-   **None** = 0
-   **Terminal** = 1
-   **Editor** = 2
-   **EditorRich** = 4
