<div align="center">
 <h3>Calling methods on nodes</h1>
 <p>Here you will find information on how to call methods on selected nodes,
 how to select them and many more.</p>
</div>

### Calling methods on nodes

> [!NOTE]
> Please keep in mind that this feature is still in development. Many things may not work as expected.
>
> Method chaining is not yet working as it should. At the moment, the chained methods will be run one at a time on the selected node, not on the returned data.

The terminal in YAT can operate in two modes: by default, the terminal treats any input as **commands** and tries to parse them.

The second mode allows you to run **methods** on the currently selected **node**, this mode is triggered by giving a `$` character at the beginning of the input data.

To run a method, you just need to specify the whole name, in the case of methods built in Godot, the names are written in `snake_case`.

### Changing selected node

To change the selected node, use the `cn` command.

### Checking available methods

To see what methods are available for a particular node, use the `ls` command with the `-m` option. For the currently selected node, use `ls . -m`.
