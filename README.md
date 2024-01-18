<div align="center">
	<img src="./addons/yat/docs/assets/yat_icon_baner.png" />
	<h3>YAT</h1>
	<p>YAT is an addon that provides a customizable, in-game terminal for your project.</p>
</div>

> [!NOTE]
> This project is in the early stages of development. Many things may be added, removed or changed.

YAT stands for Yet Another Terminal. The goal of this project is to create a real terminal integrated with Godot that allows you to perform actions whether in the game, editor, or user system. This is intended to facilitate game development, debugging, and prototyping.

The second goal, which is particularly important to me, is to make YAT as open as possible to change, personalization, and expansion, so that everyone can customize it as much as possible for their own project.

Of course, creating such a complex and ambitious project is extremely difficult (especially alone), so it will still be in beta for a long time to come. Nevertheless, all the basic functions are working mostly correctly, and major changes breaking compatibility are not expected to arrive anytime soon.

## Prerequisites

-   [.NET SDK 7](https://dotnet.microsoft.com/en-us/download)
-   [.NET enabled Godot ^4.0](https://godotengine.org/download/windows/)

## Features

-   Quick Commands
-   Autocompletion
-   Script templates
-   Performance monitor
-   Over 25 built-in commands
-   Small size footprint (< 175 KB)
-   Access to the node tree (experimental)
-   Ability to restrict access to the plugin
-   Plugin customization (limited at this point)
-   Automatic input validation (arguments, options)
-   Custom commands (regular & threaded), extensions and windows

## Showcase

<video src="https://github.com/MASSHUU12/godot-yat/assets/61974579/85ec5856-d9ea-4496-89e5-a9d6cbec20ce" controls title="YAT showcase video"></video>

## Documentation

Instructions on how to get started can be found in the [USAGE.md](./addons/yat/docs/USAGE.md) file.

You can find the documentation in the [docs](./addons/yat/docs/) folder
and example in the [example](./example/) folder.

The project also has templates in the [script_templates](./script_templates/) folder, which can make it easier to create commands, etc.

## License

Licensed under [MIT license](./LICENSE).
