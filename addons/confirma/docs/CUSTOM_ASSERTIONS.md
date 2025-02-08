# Custom Assertions

Creating custom assertions for Confirma is fairly straightforward,
but the process differs between C# and GDScript.

## C#

In the case of C#, all you need to do is create a method
(preferred extension of a given type)
that throws an `ConfirmAssertException` exception when an assertion fails.

A `ConfirmAssertException` can be created in several ways:

1. Passing a message to be displayed.

```cs
throw new ConfirmAssertException("Your message");
```

2. Generating a message to be displayed.

```cs
throw new ConfirmAssertException(
    "Expected {1} to be {2}."
    nameof(YourMethod),
    new TypeFormatter(),
    expected,
    actual,
    customMessage
);
```

Expected, actual and custom message can be omitted, as can the formatter,
but the default formatter will be used in its case.

## GDScript

Creating an assertion for GDScript is a bit more complicated than for C#
and consists of several steps:

1 First, you need to create an assertion for **C#** e.q.:

```cs
public static class ConfirmBooleanExtensions
{
    public static bool ConfirmTrue(this bool actual, string? message = null)
    {
        if (actual)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            Formatter.DefaultFormat,
            nameof(ConfirmTrue),
            null,
            true,
            actual,
            message
        );
    }
    // ...
}
```

2 Next, you need to create a wrapper for this assertion
to communicate with GDScript e.q.:

```cs
public partial class ConfirmBooleanWrapper : WrapperBase
{
    public static bool ConfirmTrue(bool actual, string? message = null)
    {
        CallAssertion(
            () => actual.ConfirmTrue(ParseMessage(message))
        );

        return actual;
    }
    // ...
}
```

3. Finally, you can create a version of the assertion in GDScript e.q.:

```gd
@tool
class_name ConfirmBoolean

static var exts: CSharpScript = load(
	GdHelper.get_plugin_path() + "wrappers/ConfirmBooleanWrapper.cs"
).new()


static func is_true(actual: bool, message: String = "") -> bool:
	exts.ConfirmTrue(actual, message)

	return actual
# ...
```
