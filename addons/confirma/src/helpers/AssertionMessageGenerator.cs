using System.Globalization;
using Confirma.Formatters;

namespace Confirma.Helpers;

public class AssertionMessageGenerator(
    string format,
    string assertion,
    Formatter formatter,
    object? expected,
    object? actual,
    byte formatNulls = 0
    )
{
    private readonly string _format = format;
    private readonly string _assertion = assertion;
    private readonly object? _expected = expected;
    private readonly object? _actual = actual;
    private readonly Formatter _formatter = formatter;
    private readonly byte _formatNulls = formatNulls;

    public string GenerateMessage()
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            "Assertion {0} failed: " + _format,
            _assertion,
            _expected is not null || (_formatNulls & 1) == 1
                ? _formatter.Format(_expected)
                : null,
            _actual is not null || (_formatNulls & 2) == 2
                ? _formatter.Format(_actual)
                : null
        );
    }
}
