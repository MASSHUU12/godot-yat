<div align="center">
	<h3>Testing</h1>
</div>

Confirma will detect all tests, regardless of where in the project you place them.

Each class that contains tests must be labelled with the `TestClass` attribute.

Each method, however, must be tagged with the `TestCase` attribute.

Example test class:

```cs
using Confirma.Attributes;

namespace Confirma.Tests;

[TestClass]
public static class ConfirmBooleanTest
{
	[TestCase]
	public static void ConfirmTrue_WhenTrue()
	{
		true.ConfirmTrue();
	}
}
```

## TestClass

The `TestClass` attribute is used to identify the classes in which the tests are located.
It is required, Confirma ignores all classes that do not have this attribute.


## TestCase

The `TestCase` attribute is used to mark the methods that perform the tests.
It also accepts arguments, which allows parameterized tests.


## AfterAll

Not implemented.

## BeforeAll

Not implemented.

## Category

Not implemented.

## Ignore

Not implemented.

## SetUp

Not implemented.

## TerDown

Not implemented.

## Timeout

Not implemented.

## TestName

Used to display a different name for the test than the method name.
