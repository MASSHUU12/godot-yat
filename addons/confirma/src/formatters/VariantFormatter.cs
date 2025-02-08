using Godot;

namespace Confirma.Formatters;

public class VariantFormatter : Formatter
{
    public override string Format(object? value)
    {
        return new AutomaticFormatter().Format(
            value is Variant v
                ? v.Obj
                : value
        );
    }
}
