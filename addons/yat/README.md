<div align="center">
 <img src="./docs/assets/yat_icon_baner.png" />
 <h3>YAT</h1>
 <p>YAT is an addon that provides a customizable, in-game terminal for your project.</p>
</div>

> [!NOTE]
> This project is in the early stages of development. Many things may be added, removed or changed.

YAT stands for Yet Another Terminal. The goal of this project is to create a real terminal integrated with Godot that allows you to perform actions whether in the game, editor, or user system. This is intended to facilitate game development, debugging, and prototyping.

The second goal, which is particularly important to me, is to make YAT as open as possible to change, personalization, and expansion, so that everyone can customize it as much as possible for their own project.

## Prerequisites

- [.NET SDK 7^](https://dotnet.microsoft.com/en-us/download)
- [.NET enabled Godot 4.2^](https://godotengine.org/download)
- C# 11^

Make sure to update your [.csproj](./docs/USAGE.md).

## Showcase

<video src="https://github.com/MASSHUU12/godot-yat/assets/61974579/fff0af36-ef62-4e1d-b3c7-ff680f30c100" controls title="YAT showcase video"></video>

### Features

- Over 35 built-in commands
- Small size footprint (< 256 KB)
- Custom commands (regular & threaded), extensions and windows
- Automatic input validation (arguments, options)
- Debug screen (FPS, CPU, GPU, etc.)
- Access to the node tree (experimental)
- Plugin customization
- Quick Commands
- Autocompletion
- Script templates
- Ability to restrict access to the plugin

## Documentation

Instructions on how to get started can be found in the [USAGE.md](./docs/USAGE.md) file.

You can find the documentation in the [docs](./docs/) folder
and example in the [example](../../example/) folder.

The project also has templates in the [script_templates](../../script_templates/) folder, which can make it easier to create commands, etc.

## License

Licensed under [MIT license](./LICENSE).
