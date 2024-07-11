using Godot;
using YAT.Enums;

namespace YAT.Scenes;

public partial class InputContainer : PanelContainer
{
    [Export] public StringName Text { get; set; } = string.Empty;
    [Export] public EInputType InputType { get; set; } = EInputType.String;
    [Export] public float MinValue { get; set; } = 0;
    [Export] public float MaxValue { get; set; } = 1;

#nullable disable
    private Label _label;
    private SpinBox _spinBox;
    private CheckBox _checkBox;
    private LineEdit _lineEdit;
    private ColorPickerButton _colorPickerButton;
#nullable restore

    public override void _Ready()
    {
        _label = GetNode<Label>("%Label");
        _spinBox = GetNode<SpinBox>("%SpinBox");
        _checkBox = GetNode<CheckBox>("%CheckBox");
        _lineEdit = GetNode<LineEdit>("%LineEdit");
        _colorPickerButton = GetNode<ColorPickerButton>("%ColorPickerButton");

        // Hide when displaying checkbox
        _label.Visible = InputType != EInputType.Bool;
        _label.Text = Text;

        _spinBox.Visible = InputType is EInputType.Float or EInputType.Int;
        _spinBox.MinValue = MinValue;
        _spinBox.MaxValue = MaxValue;

        _checkBox.Visible = InputType == EInputType.Bool;
        _checkBox.Text = Text;

        _lineEdit.Visible = InputType == EInputType.String;

        _colorPickerButton.Visible = InputType == EInputType.Color;
    }

    public Variant GetValue()
    {
        return InputType switch
        {
            EInputType.Float => _spinBox.Value,
            EInputType.Int => (int)_spinBox.Value,
            EInputType.Bool => _checkBox.ButtonPressed,
            EInputType.String => _lineEdit.Text,
            EInputType.Color => _colorPickerButton.Color,
            _ => new()
        };
    }

    public void SetValue(Variant value)
    {
        switch (InputType)
        {
            case EInputType.Float:
                _spinBox.Value = (float)value;
                break;
            case EInputType.Int:
                _spinBox.Value = (int)value;
                break;
            case EInputType.Bool:
                _checkBox.ButtonPressed = (bool)value;
                break;
            case EInputType.String:
                _lineEdit.Text = (string)value;
                break;
            case EInputType.Color:
                _colorPickerButton.Color = (Color)value;
                break;
            default:
                break;
        }
    }
}
