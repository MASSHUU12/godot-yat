using Confirma.Extensions;
using Godot;
using Godot.Collections;

namespace Confirma.Wrappers;

public partial class ConfirmDictionaryWrapper : WrapperBase
{
    public static Dictionary ConfirmContainsKey(
        Dictionary dict,
        Variant key,
        string? message = null
    )
    {
        CallAssertion(
            () => dict.ConfirmContainsKey(key, ParseMessage(message))
        );

        return dict;
    }

    public static Dictionary ConfirmNotContainsKey(
        Dictionary dict,
        Variant key,
        string? message = null
    )
    {
        CallAssertion(
            () => dict.ConfirmNotContainsKey(key, ParseMessage(message))
        );

        return dict;
    }

    public static Dictionary ConfirmContainsValue(
        Dictionary dict,
        Variant value,
        string? message = null
    )
    {
        CallAssertion(
            () => dict.ConfirmContainsValue(value, ParseMessage(message))
        );

        return dict;
    }

    public static Dictionary ConfirmNotContainsValue(
        Dictionary dict,
        Variant value,
        string? message = null
    )
    {
        CallAssertion(
            () => dict.ConfirmNotContainsValue(value, ParseMessage(message))
        );

        return dict;
    }

    public static Dictionary ConfirmContainsKeyValuePair(
        Dictionary dict,
        Variant key,
        Variant value,
        string? message = null
    )
    {
        CallAssertion(
            () => dict.ConfirmContainsKeyValuePair(
                key,
                value,
                ParseMessage(message)
            )
        );

        return dict;
    }

    public static Dictionary ConfirmNotContainsKeyValuePair(
        Dictionary dict,
        Variant key,
        Variant value,
        string? message = null
    )
    {
        CallAssertion(
            () => dict.ConfirmNotContainsKeyValuePair(
                key,
                value,
                ParseMessage(message)
            )
        );

        return dict;
    }
}
