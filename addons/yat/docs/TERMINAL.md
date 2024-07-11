<div align="center">
 <h3>Terminal</h1>
 <p>Here you will find information on the operation of the terminal.</p>
</div>

### Aborting the command

> Note that sending an interrupt signal does not necessarily mean immediate termination of the command.
>
> Whether and when the command terminates depends on the command and whether it respects the request.

Interrupting a command is possible only if it runs on a **separate thread** (and if its work does not end in a fraction of a second).

To terminate the command you must use the keybinding **yat_terminal_interrupt**.

### Context menu

YAT uses the keybinding **yat_context_menu** to launch the context menu.

### Signals

| Name                   | Arguments      | Description                                        |
| ---------------------- | -------------- | -------------------------------------------------- |
| CloseRequested         | N/A            | Sent when terminal is requested to close.          |
| TitleChangeRequested   | title (string) | Sent when change to terminal's title is requested. |
| PositionResetRequested | N/A            | Sent when terminal position reset is requested.    |
| SizeResetRequested     | N/A            | Sent when terminal size reset is requested.        |
