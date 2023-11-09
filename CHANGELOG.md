# Change Log

All notable changes to this project will be documented in this file.

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
