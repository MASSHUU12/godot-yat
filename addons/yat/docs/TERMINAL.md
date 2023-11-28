<div align="center">
	<h3>Terminal</h1>
	<p>Here you will find information on the operation of the terminal.</p>
</div>

## Terminal

### Aborting the command

Interrupting a command is possible only if it runs on a separate thread (and if its work does not end in a fraction of a second).

To terminate the command you must use the keybinding `yat_terminal_interrupt`.

> Remember that sending an interrupt signal does not necessarily mean that the operation of the command will immediately end.

### Context menu

YAT uses the keybinding `yat_context_menu` to launch the context menu, so if you haven't created one, you need to do so to use the context menu.

#### Quick Commands

The terminal's context menu allows you to run Quick Commands.
These are user-defined command prompts, and you can manage them via the `qc` command.

Example of adding a command to Quick Commands:

```bash
$ qc add -name="Red Hello" -command="echo [color=red]Hello[/color]"
```
