# Change Log

All notable changes to this project will be documented in this file.

## [1.11.0-beta 2023-11-30]

### Added

-   Ping command.
-   Ip command.
-   Ls command.
-   FileSizeToString numeric helper method.
-   GetAttributes method for AttributeHelper.
-   Argument attribute.
-   Option attribute.

### Changed

-   Godot.NET.Sdk version to 4.2.0.
-   All builtin commands use new Argument attribute.
-   All builtin commands use new Option attribute.
-   List command alias renamed to 'lc'.

### Removed

-   Arguments attribute.
-   Options attribute.

## [1.10.0-beta 2023-11-28]

### Added

-   Cat command.
-   Cowsay command.
-   QuickCommands command.
-   TextHelper.ConcatenateSentence method.
-   yat_context_menu input event keybinding.
-   Generic context menu.
-   Generic context submenu.
-   Context menu for terminal.
-   Quick Commands for terminal.
-   StartsWith & EndsWith text helper method.
-   Support for sentences in options.
-   Terminal documentation.
-   StorageHelper class.

### Changed

-   Godot.NET.Sdk version to 4.2.0-rc.2.
-   The terminal now distinguishes between sentences wrapped in " or ' and treats them as a whole.
-   Renamed keybindings for example scenes.
-   New showcase video.

### Fixed

-   The CommandManager variable in YAT was empty, instead of storing a reference to Command Manager.
-   Providing an '=' character in an option resulted in incorrect parsing of the option.

## [1.9.0-beta 2023-11-23]

### Added

-   Autocompletion scene.
-   On-the-fly argument validation.
-   MakeBold method for TextHelper.

### Changed

-   Separated logic from ValidateCommandArguments to ValidateCommandArgument method.
-   Improved input sanitization.
-   Existing autocompletion have been moved from Input.cs to Autocompletion.cs.

### Fixed

-   Background color difference between prompt and input.
-   Terminal is not centered by default.

## [1.8.0-beta 2023-11-21]

### Added

-   CommandManager scene.
-   CommandStarted signal.
-   The name of the currently running command is displayed in the title of the terminal window.

### Changed

-   Terminal is under using YAT.Scenes.Overlay.Components.Terminal namespace.
-   YatWindow is under YAT.Scenes.Overlay.Components.YatWindow namespace.
-   Overlay is under YAT.Scenes.Overlay namespace.
-   Overlay folder have been moved to the scenes folder.
-   YAT inherits from Node.
-   CommandExecuted signal have been renamed to CommandFinished.
-   Moved command execution logic from terminal to command manager.
-   CommandExecuted signal, Locked and cancellation token have been moved to command manager.

## [1.7.0-beta 2023-11-20]

### Added

-   Input actions for example.
-   Documentation for custom windows.

### Changed

-   Bumped Godot.NET.Sdk version to 4.2.0-rc.1.
-   OptionWindow renamed to SettingsWindow.
-   SettingsWindow inherits YatWindow.
-   The appearance of the terminal differs slightly.
-   Renamed custom window to YatWindow.
-   Completely redesigned custom window for YAT.
-   Created from scratch an example of basic plugin usage.
-   Include script templates in exported ZIP file.

### Removed

-   CloseRequested signal (replaced by native signal).

## [1.6.0-beta 2023-11-16]

### Added

-   Automatic validation of options.
-   MissingValue method to LogHelper.
-   Documentation (man command) displays information about options.

### Changed

-   Update Godot.NET.Sdk version to 4.2.0-beta.6.
-   Dictionary passed to Execute method can contain both arguments and options.
-   Command whereami displays abbreviated form by default, adding -l option restores previous behavior.
-   Methods that generate documentation take no arguments.
-   Moved documentation on automatic input validation to a separate file.

## [1.5.0-beta 2023-11-14]

### Added

-   NumericHelper class.
-   NotImplemented command result.
-   Automatic argument validation.
-   Arguments & options attributes for automatic validation.
-   NoValidate attribute indicating that no automatic validation should be performed.
-   New methods to LogHelper: Error, InvalidArgumentType.
-   Execute method overload accepting converted arguments as an additional parameter.

### Changed

-   Man command displays command arguments.
-   Built-in commands use automatic argument validation.
-   Attributes, interfaces and enums have been moved to their respective folders and namespaces.
-   Attributes are no longer partial, but are sealed.
-   Templates have been updated.

## [1.4.0-beta 2023-11-12]

### Added

-   History command.
-   yat_terminal_autocompletion keybinding.
-   Autocompletion system.
-   Size property to LRU cache.

### Changed

-   Improved encapsulation.
-   Moved overlay under YAT.Overlay namespace.
-   Moved terminal under YAT.Overlay.Components.Terminal namespace.

### Fixed

-   Caret sticks to the beginning of the string when scrolling through the command history &
    when scrolling through suggestions from autocompletion.

## [1.3.0-beta - 2023-11-10]

### Added

-   InvalidArgument method to LogHelper.
-   watch command.
-   Terminal lock feature to prevent calling a new command during execution of a command.
-   Threaded attribute for running commands on a separate thread.
-   yat_terminal_interrupt keybinding.
-   Overload for Execute method that takes a CancellationToken.

### Changed

-   Terminal's Print method uses CallDeferred to display messages in the output.
-   Execute method is now virtual.
-   Renamed keybindings:
    -   yat_history_next to yat_terminal_history_next
    -   yat_history_previous to yat_terminal_history_previous

### Removed

-   InvalidArguments method from LogHelper.

## [1.2.0-beta - 2023-11-09]

### Added

-   Overridable GenerateCommandManual method for commands.
-   Overridable GenerateExtensionManual method for extensions.
-   Overridable GenerateExtensionsManual method for Extensible.

### Changed

-   Man uses LRU cache for manuals.
-   Commands must take YAT as a property in the constructor.

### Removed

-   YAT reference in command Execute method.
-   YAT reference in extension Execute method.

## [1.1.0-beta - 2023-11-08]

### Added

-   YAT enable file.
-   Support for Godot 4 (SDK 4.2.0-beta.4).

### Fixed

-   Path to the options window.
-   Text formatting.

### Changed

-   Terminal fetches settings on Ready.
-   The print method always displays the message on a new line of the terminal.
-   Only add-ons folder will be included in the exported ZIP.
-   The documentation has been separated from the README.md file and is located in the docs folder.

### Removed

-   Println method for terminal output.

## [1.0.0-beta - 2023-11-02]

Initial release.
