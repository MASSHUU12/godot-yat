using Godot;
using YAT.Helpers;
using YAT.Resources;

namespace YAT.Scenes;

public partial class YatWindow : Window
{
    [Signal] public delegate void WindowMovedEventHandler(Vector2 position);

    [Export(PropertyHint.Range, "0, 128, 1")]
    public ushort ViewportEdgeOffset { get; set; }

    [Export] public EWindowPosition DefaultWindowPosition = EWindowPosition.Center;
    [Export] public bool AllowToGoOffScreen = true;

#nullable disable
    public ContextMenu ContextMenu { get; private set; }
    public Vector2I InitialSize { get; private set; }

    public bool IsWindowMoving { get; private set; } = false;

    protected YAT _yat;
    protected PanelContainer _content;

    private Viewport _viewport;
#nullable restore

    private Vector2 _previousPosition;
    private float _windowMoveTimer = 0f;
    private const float WINDOW_MOVE_REFRESH_RATE = 0.0128f;

    public enum EWindowPosition
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Center
    }

    public override void _Ready()
    {
        _yat = GetNode<YAT>("/root/YAT");
        _yat.PreferencesManager.PreferencesUpdated += UpdateOptions;

        _content = GetNode<PanelContainer>("Content");

        _viewport = _yat.GetTree().Root.GetViewport();
        _viewport.SizeChanged += OnViewportSizeChanged;

        ContextMenu = GetNode<ContextMenu>("ContextMenu");
        InitialSize = Size;

        WindowInput += OnWindowInput;
        WindowMoved += OnWindowMoved;

        Move(DefaultWindowPosition, ViewportEdgeOffset);
        OnViewportSizeChanged();
        UpdateOptions(_yat.PreferencesManager.Preferences);
    }

    public void ResetPosition()
    {
        Move(DefaultWindowPosition, ViewportEdgeOffset);
    }

    public override void _Process(double delta)
    {
        _windowMoveTimer += (float)delta;

        if (_windowMoveTimer >= WINDOW_MOVE_REFRESH_RATE && _previousPosition != Position)
        {
            IsWindowMoving = true;
            _windowMoveTimer = 0f;
            _previousPosition = Position;
            _ = EmitSignal(SignalName.WindowMoved, Position);
        }
        else
        {
            IsWindowMoving = false;
        }
    }

    private void OnWindowInput(InputEvent @event)
    {
        if (@event.IsActionPressed(Keybindings.ContextMenu) && ContextMenu.ItemCount > 0)
        {
            ContextMenu.ShowNextToMouse();
        }
        else
        {
            ContextMenu.Hide();
        }
    }

    private void OnWindowMoved(Vector2 position)
    {
        if (!AllowToGoOffScreen)
        {
            var (limitX, limitY) = CalculateLimits(_viewport.GetVisibleRect());

            Position = new(
                (int)Mathf.Clamp(Position.X, ViewportEdgeOffset, limitX),
                (int)Mathf.Clamp(Position.Y, ViewportEdgeOffset, limitY)
            );
        }
    }

    private (float, float) CalculateLimits(Rect2 rect)
    {
        float limitX = rect.Size.X - Size.X - ViewportEdgeOffset;
        float limitY = rect.Size.Y - Size.Y - ViewportEdgeOffset;

        return ((int)limitX, (int)limitY);
    }

    private void OnViewportSizeChanged()
    {
        Vector2I viewportSize = (Vector2I)_viewport.GetVisibleRect().Size;

        MaxSize = MaxSize with
        {
            X = viewportSize.X - ViewportEdgeOffset,
            Y = viewportSize.Y - ViewportEdgeOffset
        };
    }

    public void Move(EWindowPosition position, uint offset = 0)
    {
        switch (position)
        {
            case EWindowPosition.TopLeft:
                MoveTopLeft(offset);
                break;
            case EWindowPosition.TopRight:
                MoveTopRight(offset);
                break;
            case EWindowPosition.BottomRight:
                MoveBottomRight(offset);
                break;
            case EWindowPosition.BottomLeft:
                MoveBottomLeft(offset);
                break;
            case EWindowPosition.Center:
                MoveToTheCenter();
                break;
            default:
                break;
        }
    }

    protected void MoveTopLeft(uint offset)
    {
        Position = new((int)offset, (int)offset);
    }

    protected void MoveTopRight(uint offset)
    {
        Rect2 viewportRect = GetTree().Root.GetViewport().GetVisibleRect();
        Vector2 bottomLeft = viewportRect.Position + viewportRect.Size;
        Rect2 rect = GetVisibleRect();

        Position = new(
            (int)(bottomLeft.X - rect.Size.X - offset),
            (int)offset
        );
    }

    protected void MoveBottomRight(uint offset)
    {
        Rect2 viewportRect = GetTree().Root.GetViewport().GetVisibleRect();
        Vector2 topRight = viewportRect.Position + viewportRect.Size;
        Rect2 rect = GetVisibleRect();

        Position = new(
            (int)(topRight.X - rect.Size.X - offset),
            (int)(topRight.Y - rect.Size.Y - offset)
        );
    }

    protected void MoveBottomLeft(uint offset)
    {
        Rect2 viewportRect = GetTree().Root.GetViewport().GetVisibleRect();
        Vector2 bottomLeft = viewportRect.Position + viewportRect.Size;
        Rect2 rect = GetVisibleRect();

        Position = new(
            (int)offset,
            (int)(bottomLeft.Y - rect.Size.Y - offset)
        );
    }

    protected void MoveToTheCenter()
    {
        MoveToCenter();
    }

    protected void UpdateOptions(YatPreferences prefs)
    {
        AddThemeFontSizeOverride("title_font_size", prefs.BaseFontSize);
        AddThemeFontOverride("title_font", prefs.BaseFont);

        Theme theme = _content.Theme;
        theme.DefaultFont = prefs.BaseFont;
        theme.DefaultFontSize = prefs.BaseFontSize;
        _content.Theme = theme;
    }
}
