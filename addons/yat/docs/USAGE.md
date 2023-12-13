<div align="center">
	<h3>Usage</h1>
	<p>Here you can find information about using this extension and its basic configuration.</p>
</div>

## Keybindings

YAT automatically adds the default actions and key bindings needed for the plugin to work properly.

All actions used have the prefix `yat`, so there should be no conflicts with actions specific to your project.

You can find all the used actions below.

### YAT

-   `yat_toggle` - Toggles the state of the overlay.
-   `yat_context_menu` - Allows to call the context menu.
-   `yat_terminal_history_next` - Displays the next command from history.
-   `yat_terminal_history_previous` - Displays the previous command from history.
-   `yat_terminal_interrupt` - Used to stop command working on separate thread.
-   `yat_terminal_autocompletion` - Used to scroll through suggestions from autocompletion.

### Example

-   `yat_example_player_move_left`
-   `yat_example_player_move_right`
-   `yat_example_player_move_forward`
-   `yat_example_player_move_backward`

### Default keybindings

-   yat_toggle: `~`
-   yat_terminal_interrupt: `Ctrl + C`
-   yat_context_menu: `Right Mouse Button`
-   yat_terminal_history_next: `Arrow Down`
-   yat_terminal_history_previous: `Arrow Up`
-   yat_example_player_move_left: `A`
-   yat_example_player_move_right: `D`
-   yat_example_player_move_forward: `W`
-   yat_example_player_move_backward: `S`

## Options

### YAT enable file

You can make YAT accessible only when a specific file (default .yatenable)
is present in either the `user://` or `res://` directory (default user://).

This makes it easy to restrict terminal access to players without removing the extension.
