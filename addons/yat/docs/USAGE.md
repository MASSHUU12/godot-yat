# Usage

Here you can find information about using this extension and its basic configuration.

## Getting Started

To get started with YAT, you must first do two things:

## 1. Create C# solution

To create a C# solution in your project,
you must select `Project > Tools > C# > Create C# solution`
in the upper left corner of the window.
This is a **required** action for **any** project created in C#.

## 2. Configure .csproj

Unfortunately, the official template in the generated .csproj file is inadequate
and cannot cope with more advanced projects,
and as a result, several improvements need to be made to it.

- Before Godot 4.4:

```xml
<Project Sdk="Godot.NET.Sdk/4.2.1">
  <PropertyGroup>
    <TargetFramework>net7.0</TargetFramework>
    <TargetFramework Condition=" '$(GodotTargetPlatform)' == 'android' ">net7.0</TargetFramework>
    <TargetFramework Condition=" '$(GodotTargetPlatform)' == 'ios' ">net8.0</TargetFramework>
    <EnableDynamicLoading>true</EnableDynamicLoading>
    <Nullable>enable</Nullable>
    <LangVersion>11.0</LangVersion>
    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
  </PropertyGroup>
  <ItemGroup>
    <Compile Remove="script_templates/**/*.cs" />
  </ItemGroup>
</Project>
```

- After Godot 4.4:

```xml
<Project Sdk="Godot.NET.Sdk/4.4.0-beta.3">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <EnableDynamicLoading>true</EnableDynamicLoading>
    <Nullable>enable</Nullable>
    <LangVersion>12.0</LangVersion>
    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
  </PropertyGroup>
  <ItemGroup>
    <Compile Remove="script_templates/**/*.cs" />
  </ItemGroup>
</Project>
```

After performing these two actions,
you can now **compile** the project and **activate** YAT.

### Keybindings

YAT automatically adds the default actions and key bindings needed for the
plugin to work properly.

All actions used have the prefix **yat**, so there should be no conflicts with
actions specific to your project.

You can find all the used actions below.

### YAT

- `yat_terminal_toggle` - Toggles the state of the overlay.
- `yat_context_menu` - Allows to call the context menu.
- `yat_terminal_history_next` - Displays the next command from history.
- `yat_terminal_history_previous` - Displays the previous command from history.
- `yat_terminal_interrupt` - Used to stop command working on separate thread.
- `yat_terminal_autocompletion_next` - Used to display next suggestions from
autocompletion.
- `yat_terminal_autocompletion_previous` - Used to display previous suggestions
from autocompletion.
- `yat_terminal_close_full_window_display` - Used to close full window display.

### Example

- `yat_example_player_move_left`
- `yat_example_player_move_right`
- `yat_example_player_move_forward`
- `yat_example_player_move_backward`

#### Default keybindings

- yat_terminal_toggle: `~`
- yat_terminal_interrupt: `Ctrl + C`
- yat_context_menu: `Right Mouse Button`
- yat_terminal_history_next: `Arrow Down`
- yat_terminal_history_previous: `Arrow Up`
- yat_terminal_autocompletion_next: `Tab`
- yat_terminal_autocompletion_previous: `Shift + Tab`
- yat_terminal_close_full_window_display: `Q`

##### Example

- yat_example_player_move_left: `A`
- yat_example_player_move_right: `D`
- yat_example_player_move_forward: `W`
- yat_example_player_move_backward: `S`

### Script Templates

YAT includes script templates that can be used as a base for creating new classes.
They are available from Godot in the `Create Script` window.
