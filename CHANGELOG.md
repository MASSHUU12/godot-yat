# Change Log

All notable changes to this project will be documented in this file.

## [Unreleased]

### Added

-   NotImplemented command result.
-   Automatic argument validation.
-   Arguments & options attributes for automatic validation.
-   NoValidate attribute indicating that no automatic validation should be performed.
-   New methods to LogHelper: Error, InvalidArgumentType.
-   Execute method overload accepting converted arguments as an additional parameter.

### Changed

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
