<div align="center">
	<h3>YatWindow</h1>
	<p>Base class to create custom windows.</p>
</div>

### Description

**Inherits**: Window

A class used to create custom windows with a consistent appearance, for example, used in the GameTerminal class.

### Properties

| Type                     | Name                  | Default                |
| ------------------------ | --------------------- | ---------------------- |
| Export ushort            | ViewportEdgeOffset    | -                      |
| Export EWindowPosition   | DefaultWindowPosition | EWindowPosition.Center |
| bool                     | AllowToGoOffScreen    | true                   |
| ContextMenu              | ContextMenu           | -                      |
| Vector2I                 | InitialSize           | -                      |
| bool                     | IsWindowMoving        | false                  |
| Protected YAT            | _yat                  | -                      |
| Protected PanelContainer | _content              | -                      |

### Methods

| Type           | Definition                                |
| -------------- | ----------------------------------------- |
| void           | ResetPosition ()                          |
| void           | Move (EWindowPosition position, uint = 0) |
| protected void | MoveTopLeft (uint)                        |
| protected void | MoveTopRight (uint)                       |
| protected void | MoveBottomRight (uint)                    |
| protected void | MoveBottomLeft (uint)                     |
| protected void | MoveToTheCenter (uint)                    |
| Protected void | UpdateOptions (YatPreferences)            |

### Signals

**WindowMoved** (Vector2 position)

### Enumerations

enum **EWindowPosition**

- **TopLeft**
- **TopRight**
- **BottomLeft**
- **BottomRight**
- **Center**
