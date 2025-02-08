# Settings

Confirma allows you to customize the plugin to meet the needs of your project
not only through [arguments](./ARGUMENTS.md),
but also through the settings provided.

## Project settings

Project settings allow you to change some aspects of the plugin
specific to your project. They are saved in the **project.godot** file.

You can find the settings in the editor
by clicking `Project > Project Settings` in the **Confirma** section.

### GDScript Tests Folder

**Requires restart of the editor**.

Specifies the path where GDScript tests are located,
the default path is **./gdtests**.

Argument **--confirma-gd-path** overrides this setting.

### Output Path

**Requires restart of the editor**.

Specifies the path in which to create
a report of the tests performed (directory must exist).
The default path is **./test_results.json**.

Argument **--confirma-output-path** overrides this setting.

## Session settings

Session settings are settings that can be customized in the bottom panel
of the editor, they also allow you to override global settings.
When you close the editor, the settings are reset.

The available settings are:

- Category
- Verbose
- Disable parallelization
- Disable orphans monitor
- Output path
- Output type
