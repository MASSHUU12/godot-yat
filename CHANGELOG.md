# Change Log

All notable changes to this project will be documented in this file.

## [Unreleased]

### Added

-   Support for automatic usage info generation.

## [1.28.0-beta 2024-04-03]

### Added

-   YatEnable scene.
-   YAT can be enabled when a specific CMD argument is passed to executable.
-   '--interval' option to Ds command.
-   It's now possible to use Quick Commands w/o GUI.

### Changed

-   The display of the contents of the 'list' command has been improved (#271).
-   Moved Yat Enable preferences to YatEnable scene.
-   Reworked Yat Enable functionality.
-   Updated the documentation to reflect the latest changes.
-   DebugScreen updates WaitTime every time it resets.

### Removed

-   EYatEnableLocation.cs.

### Fixed

-   Closing and opening the terminal again deletes the displayed content.
-   Terminal state toggle button could write to additional terminals running.

## [1.27.1-beta 2024-03-25]

### Added

-   '-e' option for 'cat' command.
-   Ok to ECommandResult enum.
-   Unavailable to ECommandResult enum.
-   Ok method to ICommand interface.
-   LastCommandResult property to BaseTerminal.
-   'lcr' command.
-   Default constructor for CommandInputType record.

### Changed

-   'cat' command by default uses FullWindowDisplay to display content of the file.
-   'inspect' command displays node's UID.
-   'InputColor' preference is used by YAT from now on.
-   'BackgroundColor' preference is hidden.
-   Testing on Godot SDK 4.3.0-dev.5.
-   Improved type-safety via better handling null values.

### Fixed

-   Scrolling text in FullWindowDisplay did not work.
-   YatWindow did not update its settings when it was opened after changes were made.
-   'TryParseCommandInputType' should not accept '( : )', '(:)' and '()' as valid input type (#266).

## [1.27.0-beta 2024-03-11]

### Added

-   'sr' command.
-   JetBrainsMono font (OFL).
-   Title attribute for DebugScreenItems.
-   Value None to EDebugScreenItemPosition enum.
-   Setting for terminal font size & window font.
-   Ignore attribute.

### Changed

-   Terminal now uses JetBrainsMono font.
-   Timer in DebugScreen no longer works when DebugScreen does not display anything.
-   Reworked the way the items in DebugScreen are stored and used.
-   Reduced memory usage by around 25% when YAT is not in use,
    and by ~6% when using DebugScreen.
-   Default font size is now 16.
-   It is possible to customize:
    -   the font size in windows and separately in the terminal
    -   used font

## [1.26.0-beta 2024-03-06]

### Added

-   Commands:
    -   '$'
    -   'tfs'
-   FullWindowDisplay component (with docs) for BaseTerminal.
-   '-e' option for 'man' command.
-   'yat_terminal_close_full_window_display' keybinding.
-   'Current' property for BaseTerminal.

### Changed

-   Moved calling methods on selected nodes to '$' command.
-   Moved MethodCalled signal to BaseTerminal.
-   Renamed AddingResult enum to ECommandAdditionStatus
    and moved it to a separate file.
-   Updated docs:
    -   BaseTerminal
    -   Input
    -   Usage
-   The following methods now return StringBuilder:
    -   GenerateCommandManual
    -   GenerateArgumentsManual
    -   GenerateOptionsManual
    -   GenerateSignalsManual

### Removed

-   MethodManager component.

### Fixed

-   Description for arguments and options was missing in the manual.
-   Documentation was not included in the exported project.
-   Unnecessary files were included in the exported project.

## [1.25.0-beta 2024-03-01]

### Added

-   DebugScreen scene.
-   EDebugScreenItemPosition enum.
-   IDebugScreenItem interface.
-   Added access to DebugScreen from YAT.
-   GpuInfoItem for DebugScreen.
-   ShrinkToFit exported variable for ContextMenu.
-   Button to ContextMenu toggling preferences window.
-   Commands:
    -   'ds'
    -   'ss'
    -   'fov'
    -   'inspect'
    -   'crash'
    -   'forcegc'
    -   'version'
-   Clipboard class.
-   '-e' option for 'wai' command.

### Changed

-   Reworked components from Monitor for DebugScreen.
-   ShortenPath method shortens paths in a better way and takes more parameters.
-   Increased the maximum displayed path length in the terminal from 20 to 32 characters.
-   Testing on Godot SDK 4.3.0-dev.4.
-   Using 'AddSubmenuNodeItem' to display QuickCommands context menu on Godot 4.3,
    for Godot 4.2 'AddSubmenuItem' is still used.

### Removed

-   Monitor.
-   'Stats' command.

### Fixed

-   Not passing any value to an option that expects a value caused an exception to be thrown.
-   QuickCommands menu had a fixed size.

## [1.24.1-beta 2024-02-24]

### Added

-   InvalidCommandInputType message.
-   -f option to list command.
-   'trace' alias for traceroute command.

### Changed

-   Keybindings class now uses array of tuples instead of a list of tuples.
-   Moved TestTryParseCommandInputType to Parser class.
-   The list command by default displays the list of commands stored in the cache, updating the list requires using the -f option.
-   Removed left out log messages.
-   Improved view command.

### Fixed

-   The action argument for the QuickCommands command was not aligned with recent changes.
-   Entering text containing the '=' character was incorrectly handled.
-   Attempting to run the toggleaudio & traceroute command threw an exception.

## [1.24.0-beta 2024-02-21]

### Added

-   Options -id & -name to ToggleAudio command.
-   More results to ECommandResult.
-   CommandResult record.
-   Static methods to ICommand that corresponds to the ECommandResult results.
-   Networking helper class.
-   NetworkingOptions record.
-   -limit option to ping command.
-   CommandInputType record.
-   TryParseCommandInputType method for Text helper class.
-   String type in arguments and options can have defined min and max length.
-   Enums:
    -   ESceneChangeFailureReason
    -   EStringConversionResult
-   Commands:
    -   TraceRoute
    -   Load
-   Attributes:
    -   Description
    -   Usage
    -   CommandInput
-   Messages:
    -   InvalidOption,
    -   ValueOutOfRange

### Changed

-   Renamed CommandResult to ECommandResult.
-   Commands now return CommandResult record.
-   The SceneAboutToChange signal in the Cs command now also gives the old path, not just the new one.
-   Renamed ping command options.
-   Argument & Option attributes inherit from CommandInput attribute.
-   Options with null type are no longer supported.
-   The way of defining the type for arguments and options has changed.
-   CommandData now uses StringName instead of string for dictionaries.
-   Methods StartsWith & EndsWith from Text class are now extensions of string type.
-   Using default value for bool type options is no longer necessary.
-   Double is no longer a valid type for options & arguments.
-   Updated AUTOMATIC_INPUT_VALIDATION.md & script templates.

### Removed

-   FailureReason enum.

### Fixed

-   It is possible to call the history command via itself, which can cause an endless loop.
-   The set command throws an exception when there are no registered extensions.

## [1.23.0-beta 2024-02-13]

### Added

-   MethodManager class.
-   YatPreferences resource.
-   EInputType enum.
-   Scenes:
    -   Preferences
    -   PreferencesTab
    -   PreferencesSection
    -   PreferencesManager
    -   InputContainer
-   GetRangeFromHint method to Scene class.
-   PreferencesManager hold all the preferences & is a child of YAT.
-   Preferences command.
-   Documentation file: YAT_ENABLE.md.

### Changed

-   Moved method management logic from SelectedNode to MethodManager class.
-   Moved RejectionReason, MethodStatus, PrintType & YatEnableLocation enums to separate files.
-   Moved CommandManager to managers folder.
-   Updated documentation:
    -   CREATING_COMMANDS.md
    -   YAT.md
    -   Input.md
    -   BaseTerminal.md
-   Restored YatEnable functionality.
-   Changed namespaces to YAT.Scenes:
    -   TerminalManager
    -   EditorTerminal
    -   Monitor (& components)
    -   YatWindow (& components)
    -   GameTerminal (& components)
    -   BaseTerminal (& components)
-   Moved commands to commands folder.

### Fixed

-   CurrentTerminal was initialized with a null value instead of the initial terminal.

### Removed

-   Options, OptionsManager & SettingsWindow scene.
-   YatOptions resource.

## [1.22.0-beta 2024-02-10]

### Added

-   Tooltip for selected node.
-   Parser class.
-   Documentation for:
    -   Parser
    -   Storage
    -   Scene
-   NodeMethodInfo struct.
-   Methods to Scene class:
    -   GetNodeMethods
    -   TryFindNodeMethodInfo
    -   GetNodeMethodInfo
    -   ValidateMethod
    -   CallMethod
-   DisposedNode & InvalidMethod messages.
-   MethodValidationResult enum.
-   '-all' option to stats command.

### Changed

-   Renamed NumericHelper to Numeric & StorageHelper to Storage.
-   Improved exception handling in TryConvert method in Numeric class.
-   Refreshed example scenes.
-   Testing on Godot SDK 4.3.0-dev.3.
-   Slightly improved monitor theme.

### Fixed

-   Changing node when currently selected node is invalid throws ObjectDisposedException.
-   Shallow search of cn command throws NullReferenceException.
-   It is possible to call methods when the terminal is locked.
-   Calling methods on disposed nodes throws ObjectDisposedException.

## [1.21.0-beta 2024-02-01]

### Added

-   WindowMoved signal & IsWindowMoving property to YatWindow.
-   Implemented functionality for AllowToGoOffScreen property in YatWindow.
-   Documentation for:
    -   YatWindow
    -   Reflection
    -   Extensible
-   Script Templates & C# Project Configuration sections to USAGE.md file.
-   UnknownCommand result to CommandResult enum.
-   HasInterface method to Reflection class.

### Changed

-   Moved YatOptions to the YAT.Resources namespace.
-   Moved CommandData record to the YAT.Types namespace.
-   Updated script templates.
-   Combined AttributeHelper with Reflection class.
-   Reworked Extensible class.
-   Moved Extensible & LruCache class to the Classes folder and namespace.

### Fixed

-   Reset command does not reset terminal position to the center.
-   Pressing the termination key of a threaded command when no command is running makes an attempt to terminate the command.

## [1.20.0-beta 2024-01-26]

### Added

-   Documentation for classes:
    -   YAT
    -   BaseTerminal
    -   Input
    -   Output
    -   RegisteredCommands
-   RegisteredCommands class.
-   Reference to the RegisteredCommands class in YAT.
-   ContextMenu to YatWindow.

### Changed

-   Moved adding builtin commands to the RegisteredCommands class.
-   Moved CommandManager to YAT.Scenes namespace.
-   Updated folder structure.
-   Moved QuickCommands resource to the resources folder.
-   Moved QuickCommandsContext to the GameTerminal's components.
-   Moved ContextMenu & ContextSubmenu under YatWindow's folder.
-   RegisteredCommands class now manages QuickCommands.

### Fixed

-   When there are several simultaneously running threaded commands, only the last one can be terminated.

### Removed

-   TerminalContext.

## [1.19.0-beta 2024-01-23]

### Added

-   Generic Print method overload for BaseTerminal.
-   TerminalManager class.
-   Reference to the TerminalManager in YAT.
-   TerminalSwitcher class to the GameTerminal.
-   Reference to the TerminalSwitcher in GameTerminal class.
-   Support for multiple terminals.
-   Output class.
-   CommandValidator to the BaseTerminal.
-   Signals on TerminalSwitcher:
    -   TerminalAdded
    -   TerminalRemoved
    -   CurrentTerminalChanged
    -   TerminalSwitcherInitialized

### Changed

-   The terminal locking mechanism has been moved from CommandManager to BaseTerminal.
-   CancellationToken in CommandData is no longer nullable.
-   YatEnabled variable in YAT class is public.
-   Moved TerminalOpened & TerminalClosed signals from YAT to TerminalManager.
-   The ability to add commands has been moved from YAT to CommandManager.
-   AddCommand supports an array of commands.
-   Renamed BaseTerminal to CurrentTerminal in GameTerminal.
-   Moved logic from Log class to the Output class used in BaseTerminal.
-   Options manager is now component of YAT.
-   YatOptions resource is now in the 'resources' folder.
-   Yat options & OptionsChanged signal have been moved to the OptionsManager class.
-   Options are disabled until the entire system is rewritten from scratch.
-   YAT no longer stores instances of commands, only their type, and creates objects dynamically when needed.

### Fixed

-   Threaded commands could not be cancelled.
-   The 'sys' command treats the '-program' option as an argument.
-   The 'stats' command treats options as an arguments.

### Removed

-   Print method overload for BaseTerminal accepting StringBuilder.
-   Reference to the BaseTerminal in YAT.
-   Log class (use BaseTerminal.Output instead).
-   CommandHelper class.

## [1.18.0-beta 2024-01-19]

### Added

-   Ability for YatWindows to set default window position.
-   PositionResetRequested & SizeResetRequested signal for BaseTerminal.
-   OS helper.
-   HasAttribute method to AttributeHelper.
-   Audio to the example scene "Into the Maelstrom" by glaciære licensed under CC-BY.
-   Commands:
    -   Reset
    -   Sys
    -   Timescale
    -   Toggle audio

### Changed

-   The man command displays information when the command being checked is threaded.
-   Renamed CommandArguments to CommandData.
-   Separated converted arguments & options in CommandData.
-   Renamed Arguments to RawData in CommandData record.

### Removed

-   LogHelper class (use Log instead).
-   CommandName variable from CommandData.

## [1.17.0-beta 2024-01-12]

### Added

-   New logging mechanism.
-   Messages class.
-   yat_terminal_autocompletion_previous input action.
-   Ability to display previous suggestion from autocompletion.
-   InputInfo scene with CommandInfo component.
-   Overloads for StartsWith & EndsWith methods accepting string.
-   SearchNode method for World helper.
-   Cn command now can search node tree for a node using '?'/'??' prefix.
-   Wenv command.

### Changed

-   The content of some messages has been changed.
-   Renamed action yat_terminal_autocompletion to yat_terminal_autocompletion_next.
-   The creation of default keybindings has been rewritten.
-   Separated command info from autocompletion.
-   Renamed action yat_toggle to yat_terminal_toggle.
-   Action names are stored as static variables of the Keybindings class.
-   Quit command now have the ability to close the terminal via -t option.
-   Updated Godot .NET SDK to version 4.3.0-dev.2.
-   Updated script templates.

### Deprecated

-   LogHelper.

## [1.16.0-beta 2024-01-08]

### Added

-   Scene helper class with PrintChildren & GetFromPathOrDefault methods.
-   Print method overload to Terminal accepting StringBuilder.
-   BaseTerminal scene.
-   CloseRequested & TitleChangeRequested signals to the BaseTerminal.
-   CommandArguments record.

### Changed

-   Simplified logic of ls command.
-   Renamed TextHelper class to Text.
-   Terminal renamed to GameTerminal.
-   Terminal history is managed by BaseTerminal instead of YAT.
-   Documentation for terminal & command creation updated.
-   UIDs are used instead of paths to scenes.
-   Refactored view command.
-   Command and extension creation has been completely revamped.
-   Builtin commands have been adjusted to the new system.

### Removed

-   REGULAR_COMMANDS.md.
-   THREADED_COMMANDS.md.

## [1.15.2-beta 2024-01-04]

### Added

-   ShortenPath method to the TextHelper class.

### Changed

-   Godot .NET SDK version to 4.3.0-dev.1.
-   Logged messages from ls command are more descriptive.
-   Cn command logs error when path is invalid.
-   CurrentNode variable renamed to Current in SelectedNode scene.
-   Cn command now can switch current node to the one at which the camera is looking at.
-   Path to the currently selected node displayed in the terminal is limited to 20 characters.

### Fixed

-   "Options"/"Arguments" headers were displaying where a command had no options/arguments.
-   Ls command used path to the SelectedNode scene instead of path to the currently selected node.

## [1.15.1-beta 2023-12-15]

### Changed

-   EOL in .editorconfig to LF.

### Fixed

-   YatWindow do not scale with the main viewport.

## [1.15.0-beta 2023-12-13]

### Added

-   Keybindings helper class.
-   Automatic loading of default key bindings.
-   Cs command for changing scenes.
-   Reflection helper class.
-   Automatic generation of documentation for signals.

### Changed

-   Godot .NET SDK version to 4.2.1.
-   Included example folder in exported repo.

### Fixed

-   Arguments & options with multiple values were displayed incorrectly in manuals.

## [1.14.1-beta 2023-12-12]

### Added

-   TerminalOpened & TerminalClosed signals.

### Changed

-   The documentation has been revamped and expanded.
-   Log information when YAT enters/exits the tree.

### Removed

-   OverlayOpened & OverlayClosed signals.

### Fixed

-   The terminal toggle button no longer writes to the input
    (it's a workaround, the input is unfortunately cleared every time the terminal is opened).

## [1.14.0-beta 2023-12-08]

### Added

-   Windows node for YAT to store currently displayed windows.
-   Move method to YatWindow to move window (e.q. top left, bottom right).
-   Monitor scene.
-   IMonitorComponent interface.
-   FPS, OS, CPU, memory, Engine, LookingAt, Scene objects components for Monitor.
-   Stats command.

### Changed

-   Moved YatWindow under YAT.Scenes.YatWindow namespace.
-   Moved Terminal under YAT.Scenes.Terminal namespace.
-   Moved SettingsWindow under YAT.Scenes.SettingsWindow namespace.
-   Moved CommandManager under YAT.Scenes.CommandManager namespace.
-   Renamed method FileSizeToString to SizeToString.
-   SizeToString supports specifying precision.

### Removed

-   Overlay.

## [1.13.0-beta 2023-12-06]

### Added

-   SelectedNode scene.
-   UnknownMethod method for LogHelper.
-   CurrentNodeChanged signal.
-   CurrentNodeChangeFailed signal.
-   MethodCalled signal.
-   Cn command.
-   Support for scene tree for Ls command.
-   -s option for Whereami command for printing info about currently selected node.
-   -m option for Whereami command for printing the methods of the selected node.
-   SplitClean method to TextHelper.
-   Ability to run methods on a selected node (experimental).

### Changed

-   Ls command is no longer threaded.

## [1.12.0-beta 2023-12-04]

### Added

-   Ability to set a default value for the option.

### Changed

-   Not passed boolean options do not have the value set to false by default.
-   TryConvert method accepts fallback values.
-   Moved LruCache under Yat.Helpers namespace.
-   TKey & TValue in LruCache must not be null.
-   Reduced chance of null values in project.
-   Command information updates when autocompletion is used.
-   Some places in the code use #nullable enable.

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
