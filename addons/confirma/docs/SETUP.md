# Setup

## Configuring project

### 1. Create C# solution

To create a C# solution in your project,
you must select `Project > Tools > C# > Create C# solution`
in the upper left corner of the window.
This is a **required** action for **any** project created in C#.

### 2. Configure .csproj

Unfortunately, the official template in the generated .csproj file is inadequate
and cannot cope with more advanced projects,
and as a result, several improvements need to be made to it.

- Before Godot 4.4:

```xml
<Project Sdk="Godot.NET.Sdk/4.3.0">
  <PropertyGroup>
    <TargetFramework>net7.0</TargetFramework>
    <TargetFramework Condition=" '$(GodotTargetPlatform)' == 'android' ">net7.0</TargetFramework>
    <TargetFramework Condition=" '$(GodotTargetPlatform)' == 'ios' ">net8.0</TargetFramework>
    <EnableDynamicLoading>true</EnableDynamicLoading>
    <Nullable>enable</Nullable>
    <LangVersion>11.0</LangVersion>
  </PropertyGroup>
  <ItemGroup>
    <Compile Remove="script_templates/**/*.cs" />
  </ItemGroup>
</Project>
```

- After Godot 4.4:

```xml
<Project Sdk="Godot.NET.Sdk/4.4.0-dev.7">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <EnableDynamicLoading>true</EnableDynamicLoading>
    <Nullable>enable</Nullable>
    <LangVersion>12.0</LangVersion>
  </PropertyGroup>
  <ItemGroup>
    <Compile Remove="script_templates/**/*.cs" />
  </ItemGroup>
</Project>
```

After performing these two actions,
you can now **compile** the project and **activate** Confirma.

## Running tests

### via the editor

After activating the plugin, a "Confirma" button should appear
in the bottom panel of the editor.
Through this button, you can activate tests, and view their results.

## via scripts

There are several scripts available in the [scripts](../../scripts/) folder
that can help with testing.

## via Visual Studio Code

You can run/debug tests via VSCode.

To do this, you **must** create an environment variable `GODOT`
that points to the Godot executable file.
This variable will make it easier to create the configuration.

In the root of your project, create a folder named **.vscode**
and place the following **tasks.json** and **launch.json** inside of it.

This will allow you to run/debug tests from within VSCode, just press `F5`.

### tasks.json

```json
{
 "version": "2.0.0",
 "tasks": [
  {
   "label": "build",
   "command": "dotnet",
   "type": "process",
   "args": ["build", "--no-restore"],
   "problemMatcher": "$msCompile",
   "presentation": {
    "echo": true,
    "reveal": "silent",
    "focus": false,
    "panel": "shared",
    "showReuseMessage": false,
    "clear": false
   }
  }
 ]
}
```

### launch.json

```json
{
 "version": "0.2.0",
 "configurations": [
  {
   "name": "🧪 Run Tests",
   "type": "coreclr",
   "request": "launch",
   "preLaunchTask": "build",
   "program": "${env:GODOT}",
   "args": ["--", "--confirma-run"],
   "cwd": "${workspaceFolder}",
   "stopAtEntry": false
  },
  {
   "name": "🧪 Run Tests (headless)",
   "type": "coreclr",
   "request": "launch",
   "preLaunchTask": "build",
   "program": "${env:GODOT}",
   "args": ["--headless", "--", "--confirma-run"],
   "cwd": "${workspaceFolder}",
   "stopAtEntry": false
  },
  {
   "name": "🧪 Run Single Test (headless)",
   "type": "coreclr",
   "request": "launch",
   "preLaunchTask": "build",
   "program": "${env:GODOT}",
   "args": ["--headless", "--", "--confirma-run=${fileBasenameNoExtension}"],
   "cwd": "${workspaceFolder}",
   "stopAtEntry": false
  }
 ]
}
```
