<div align="center">
 <img src="./addons/yat/docs/assets/yat_icon_baner.png" />
 <h3>YAT</h1>
 <p>YAT is an addon that provides a customizable, in-game terminal for your project.</p>
</div>

> [!NOTE]
> This project is in the early stages of development. Many things may be added,
> removed or changed.

YAT stands for Yet Another Terminal. The goal of this project is to create a
real terminal integrated with Godot that allows you to perform actions whether
in the game, editor, or user system. This is intended to facilitate game
development, debugging, and prototyping.

## Prerequisites

- [.NET SDK 8^](https://dotnet.microsoft.com/en-us/download)
- [.NET enabled Godot 4.2^](https://godotengine.org/download)
- C# 12^

Make sure to update your [.csproj](./addons/yat/docs/USAGE.md).

## Showcase

<video src="https://github.com/MASSHUU12/godot-yat/assets/61974579/fff0af36-ef62-4e1d-b3c7-ff680f30c100" controls title="YAT showcase video"></video>

### Features

- Over 35 built-in commands
- Small size footprint (< 512 KB)
- Custom commands (regular & ~~threaded~~), extensions and windows
- Automatic input validation (arguments, options)
- Customizable debug screen (FPS, CPU, GPU, etc.)
- Access to the node tree (experimental)
- Plugin customization
- Quick Commands
- Autocompletion
- Script templates
- Ability to restrict access to the plugin
- Automatic update

## Documentation

Instructions on how to get started can be found in the
[USAGE.md](./addons/yat/docs/USAGE.md) file.

You can find the documentation in the [docs](./addons/yat/docs/) folder
and example in the [example](./example/) folder.

The project also has templates in the
[script_templates](./script_templates/) folder, which can make it easier to
create commands, etc.

## License

Licensed under [MIT license](./LICENSE).
