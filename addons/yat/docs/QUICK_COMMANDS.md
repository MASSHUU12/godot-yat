<div align="center">
	<h3>Quick Commands</h1>
	<p>Quick Commands are user-defined command prompts.</p>
</div>

> Quick Commands are saved in user://yat_qc.tres

### Managing Quick Commands

You can manage them via the `qc` command.

Example of adding a command to Quick Commands:

```bash
$ qc add -name="Red Hello" -command="echo [color=red]Hello[/color]"
```

### Running Quick Commands

The terminal's context menu allows you to run Quick Commands, moreover, you can run them in the same way as existing commands, just enter its full name (spaces are supported).
