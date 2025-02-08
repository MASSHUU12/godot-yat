namespace Confirma.Formatters;

public class DefaultFormatter : Formatter
{
    public override string Format(object? value)
    {
        return value?.ToString() ?? "null";
    }
}
