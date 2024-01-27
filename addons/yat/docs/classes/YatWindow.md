<div align="center">
	<h3>YatWindow</h1>
	<p>Base class to create custom windows.</p>
</div>

### Description

**Inherits**: Window

A class used to create custom windows with a consistent appearance, for example, used in the GameTerminal class.

### Properties

| Type                   | Name                  | Default                |
| ---------------------- | --------------------- | ---------------------- |
| Export ushort          | ViewportEdgeOffset    | -                      |
| Export EWindowPosition | DefaultWindowPosition | EWindowPosition.Center |
| bool                   | AllowToGoOffScreen    | true                   |
| ContextMenu            | ContextMenu           | -                      |
| Vector2I               | InitialSize           | -                      |
| bool                   | IsWindowMoving        | false                  |

### Methods

| Type | Definition                                       |
| ---- | ------------------------------------------------ |
| void | ResetPosition ()                                 |
| void | Move (EWindowPosition position, uint offset = 0) |
| void | protected MoveTopLeft (uint offset)              |
| void | protected MoveTopRight (uint offset)             |
| void | protected MoveBottomRight (uint offset)          |
| void | protected MoveBottomLeft (uint offset)           |
| void | protected MoveToTheCenter (uint offset)          |

### Signals

**WindowMoved** (Vector2 position)

### Enumerations

enum **EWindowPosition**

- **TopLeft**
- **TopRight**
- **BottomLeft**
- **BottomRight**
- **Center**
