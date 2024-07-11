<div align="center">
 <h3>Builtin commands</h1>
 <p>Here you can find information about builtin commands.</p>
</div>

> [!NOTE]
> To disable the built-in command, just comment it out in the [RegisteredCommands](../src/scenes/registered_commands/RegisteredCommands.cs) file in the `RegisterBuiltinCommands` method.
>
> More information about each command can be found in their manuals.

| Command       | Alias  | Description                                                                      |
| ------------- | ------ | -------------------------------------------------------------------------------- |
| cls           | clear  | Clears the console.                                                              |
| man           | N/A    | Displays the manual for a command.                                               |
| quit          | N/A    | Quits the game.                                                                  |
| echo          | N/A    | Displays the given text.                                                         |
| restart       | reboot | Restarts the level.                                                              |
| pause         | N/A    | Toggles the game pause state.                                                    |
| whereami      | wai    | Prints the current scene name and path.                                          |
| list          | lc     | List all available commands.                                                     |
| view          | N/A    | Changes the rendering mode of the viewport.                                      |
| set           | N/A    | Sets a variable to a value. Does nothing by default, requires adding extensions. |
| history       | hist   | Manages the command history of the current session.                              |
| cat           | N/A    | Prints content of a file.                                                        |
| cowsay        | N/A    | Make a cow say something.                                                        |
| quickcommands | qc     | Manages Quick Commands.                                                          |
| ping          | N/A    | Sends ICMP (Internet Control Message Protocol) echo request to the server.       |
| ip            | N/A    | Displays your private IP addresses.                                              |
| ls            | N/A    | Lists the contents of the current directory.                                     |
| cn            | N/A    | Changes the selected node to the specified node path.                            |
| cs            | N/A    | Changes the scene.                                                               |
| wenv          | N/A    | Manages the world environment.                                                   |
| reset         | N/A    | Resets the terminal to its default position and/or size.                         |
| sys           | N/A    | Runs a system command.                                                           |
| timescale     | N/A    | Sets the timescale.                                                              |
| toggleaudio   | N/A    | Toggles audio on or off.                                                         |
| preferences   | prefs  | Creates the preferences window.                                                  |
| traceroute    | N/A    | Displays the route that packets take to reach the specified host.                |
| load          | N/A    | Loads specified object into the scene.                                           |
| ds            | N/A    | Displays items in the debug screen.                                              |
| ss            | N/A    | Makes a screenshot.                                                              |
| fov           | N/A    | Sets the field of view for the camera.                                           |
| inspect       | ins    | Inspect selected object.                                                         |
| crash         | N/A    | Crashes the game.                                                                |
| forcegc       | fgc    | Forces the garbage collector to run.                                             |
| version       | N/A    | Displays the current game version.                                               |
| $             | N/A    | Executes a method on the selected node.                                          |
| tfs           | N/A    | Toggles the full screen mode.                                                    |
| sr            | N/A    | Set the screen resolution.                                                       |
| lcr           | N/A    | Shows the result of the last command.                                            |

<!-- | watch         | N/A    | Runs user-defined (not threaded) commands at regular intervals.                  | -->
