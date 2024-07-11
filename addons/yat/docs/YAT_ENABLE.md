<div align="center">
 <h3>YatEnable</h1>
 <p>Here you can find information on the YatEnable functionality that allows you to restrict access to the terminal.</p>
</div>

<br />

If you don't want users to access the terminal, you can easily **block** access to YAT in several different ways. By default YAT is **accessible**, you can change this in the `YatEnable` scene, which is a child of the `YAT` scene.

Checking whether the requirements to allow access to YAT have been met is done only once during the startup of the plugin.

Requirements can be combined, when **one** of them is met YAT will become available.

### File

You can set the terminal to become accessible only if the specified file (default ".yatenable") is in the **user://** and/or **res://** directory.

### Terminal argument

The access restriction can also be lifted when the specified argument (default "--yat") is passed to the executable file.

The argument can be passed this way:

```sh
my_awesome_game -- --yat
```

Note the two hyphens preceding the argument, without them, it won't work because Godot will consume the input.
