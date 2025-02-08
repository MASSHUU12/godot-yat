namespace Confirma.Formatters;

public class StringFormatter(char quote = '"') : Formatter
{
    private readonly char _quote = quote;

    public override string Format(object? value)
    {
        return $"{_quote}{value?.ToString() ?? "null"}{_quote}";
    }
}
