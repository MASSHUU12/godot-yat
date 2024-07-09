using Godot;
using YAT.Helpers;

namespace Example;

public partial class Player : CharacterBody3D
{
    [ExportSubgroup("Mouse")]
    [Export(PropertyHint.Range, "0, 100, 0.1")] public float MouseSensitivity { get; set; } = 0.005f;

    [ExportSubgroup("Camera")]
    [Export] public float CameraLimitAngle { get; set; } = 1.047198f; // 60 deg in radians

    [ExportSubgroup("Walk")]
    [Export(PropertyHint.Range, "0, 100, 0.1")] public float MovementSpeed { get; set; } = 3.0f;

    [ExportSubgroup("Jump")]
    [Export(PropertyHint.Range, "0, 100, 0.1")] public float JumpVelocity { get; set; } = 4.5f;

#nullable disable
    private YAT.YAT _yat;
    private Node3D _head;
    private Camera3D _camera;
    private Vector2 _mouseMovement;
#nullable restore
    private bool _playerHaveControl = true;

    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;

        _head = GetNode<Node3D>("Head");
        _camera = GetNode<Camera3D>("Head/Camera3D");

        _yat = GetNode<YAT.YAT>("/root/YAT");
        _yat.TerminalManager.TerminalOpened += () =>
        {
            _playerHaveControl = false;
            Input.MouseMode = Input.MouseModeEnum.Visible;
        };
        _yat.TerminalManager.TerminalClosed += () =>
        {
            _playerHaveControl = true;
            Input.MouseMode = Input.MouseModeEnum.Captured;
        };
    }

    public override void _PhysicsProcess(double delta)
    {
        HandleMovement(delta);
    }

    public override void _Input(InputEvent @event)
    {
        HandleCamera(@event);
    }

    /// <summary>
    /// Handles the player's movement.
    /// </summary>
    /// <param name="delta">The time elapsed since the last frame.</param>
    private void HandleMovement(double delta)
    {
        if (!IsOnFloor())
            Velocity = Velocity with { Y = Velocity.Y - gravity * (float)delta };

        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor() && _playerHaveControl)
            Velocity = Velocity with { Y = JumpVelocity };

        // Get the input direction and handle the movement/deceleration.
        Vector2 inputDir = _playerHaveControl ? GetInputVector() : Vector2.Zero;
        Vector3 direction = (
            _head.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)
        ).Normalized();

        bool isMoving = direction.LengthSquared() > 0;

        // If the player is moving, apply the movement speed.
        Velocity = Velocity with
        {
            X = isMoving ? direction.X * MovementSpeed : 0,
            Z = isMoving ? direction.Z * MovementSpeed : 0
        };

        MoveAndSlide();
    }

    /// <summary>
    /// Handles camera movement based on mouse input,
    /// rotating the head and camera if player rotation is enabled.
    /// </summary>
    /// <param name="@event">The input event to handle.</param>
    private void HandleCamera(InputEvent @event)
    {
        if (Input.MouseMode == Input.MouseModeEnum.Captured && _playerHaveControl)
        {
            // Rotate the head and camera.
            if (@event is InputEventMouseMotion mouseMotion)
            {
                _head.RotateY(-mouseMotion.Relative.X * MouseSensitivity);
                _camera.RotateX(-mouseMotion.Relative.Y * MouseSensitivity);
                _camera.Rotation = _camera.Rotation with
                {
                    X = Mathf.Clamp(_camera.Rotation.X, -CameraLimitAngle, CameraLimitAngle)
                };
            }
        }
    }

    /// <summary>
    /// Returns a 2D vector representing the player's input for movement.
    /// </summary>
    private static Vector2 GetInputVector()
    {
        return Input.GetVector(
            Keybindings.ExamplePlayerMoveLeft, Keybindings.ExamplePlayerMoveRight,
            Keybindings.ExamplePlayerMoveForward, Keybindings.ExamplePlayerMoveBackward
        );
    }
}
