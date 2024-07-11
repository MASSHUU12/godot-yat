<div align="center">
 <h3>Quick Commands</h1>
 <p>Quick Commands are user-defined command prompts.</p>
</div>

> Quick Commands are saved in **user://yat_qc.tres**

### Managing Quick Commands

You can manage them via the `qc` command.

Example of adding a command to Quick Commands:

```bash
qc add -name="Red Hello" -command="echo [color=red]Hello[/color]"
```

### Running Quick Commands

Quick Commands can be run in several different ways:

- via the terminal context menu
- using the command `qc run -name=command_name`
- by typing its full name in the terminal (spaces are supported)
