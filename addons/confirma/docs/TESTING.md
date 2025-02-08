# Testing

> Confirma provides built-in script templates that can be used
to quickly create tests.

## C#

### Writing tests

Confirma will detect all tests,
regardless of where in the project they're placed.

Each class that contains tests must be labelled with the `TestClass` attribute.
Each test method must be tagged with the `TestCase` attribute.

Chaining assertions is allowed, so something like this is possible:

> Confirma supports static and non-static test classes.

```cs
using Confirma.Attributes;
using Confirma.Extensions;

[TestClass]
[Parallelizable]
public static class TestSomething
{
    [TestCase]
    public static void Something()
    {
        5.ConfirmInRange(0, 15)
            .ConfirmNotEqual(7);
    }
}
```

### Mocking

Confirma includes a simple library for
[mocking](https://stackoverflow.com/questions/2665812/what-is-mocking#2666006),
which is located in the `Confirma.Classes.Mock` namespace.

### Flaky tests

Confirma includes basic support for handling
[flaky tests](https://www.lambdatest.com/learning-hub/flaky-test).

Flaky tests use the [Repeat](#repeat) attribute.

### Fuzz testing

Confirma includes basic support for
[fuzz testing](https://en.wikipedia.org/wiki/Fuzzing).

This function allows you to pass random values to test method arguments.
The supported data types are: `int`, `double`, `float`, `string` and `bool`.

Each attribute corresponds to one method parameter.
Their order is important, they are taken from top to bottom,
so the first argument corresponds to the first attribute of the method.

The `Repeat` attribute is allowed,
it must be assigned to only one `Fuzz` argument.
The `Repeat` argument will have the same effect as when it is assigned to the
`TestCase` argument.

### Accessing scene tree

Access to the scene tree is enabled by the static `Global` class.
It provides the variable `Root` where the tree's root Window is located.

### Attributes

#### TestClass (required)

The TestClass attribute is used to identify the classes
in which the tests are located.
It is required, Confirma ignores all classes that do not have this attribute.

#### TestCase (required)

It is used to create test methods.
Each method can take multiple of these attributes,
which is especially useful for parameterized tests.

Example:

```cs
[TestCase("abc")]
[TestCase("123")]
[TestCase("A$B2c_")]
public static void NextChar_AllowedChars_ReturnsCharFromAllowedChars(string allowedChars)
{
    _ = allowedChars.ConfirmContains(_rg.NextChar(allowedChars));
}
```

Confirma supports `params` modifier:

```cs
[TestCase(2, 40, 37)]
public void Test(int num, params int[] nums)
{
    _ = nums.ConfirmCount(num);
}
```

#### Category

Allows to assign a category to a test class.
The category can be used to run only tests from the category,
or to exclude them from running.

#### Ignore

Ignore class/method during testing.

Can ignore always, only when run in editor/headless mode
or when tests are not run from the specified category.

The third option can be a bit confusing,
but allows creating tests that will not be always run,
but only under certain circumstances.

#### TestName

Used to display a different name for the test than the method name.

#### Parallelizable

Allows to run all the tests included in the class on separate CPU cores.

#### Repeat

The Repeat attribute allows to run a particular TestCase several times.
Repeat refers to the next TestCase,
so the order in which the attributes are defined matters.

The attribute optionally takes a flag as a second argument
indicating whether to stop running the test after the first error encountered.

This attribute also allows the test to be marked as [flaky](#flaky-tests).
Meaning that if it fails, it will be restarted up to a certain number of times
before it is considered a failure.
The `Backoff` variable allows to set the interval in milliseconds between
successive attempts to execute the test.

### Lifecycle attributes

All lifecycle attributes are assignable to the test class
and **not** the method.

They take the name of the method to run, by default the attribute name.

#### AfterAll

Runs after all test methods in the class.

#### BeforeAll

Runs before all test methods in the class.

#### SetUp

Runs before every test method in the class.

#### TearDown

Runs after every test method in the class.

## GDScript

### Writing tests

Testing of GDScript code is possible via exposed wrappers for C# assertions.
Confirma's current architecture does not allow native assertions
to be created in GDScript, as the language does not support exceptions.

Confirma searches the selected folder for tests and runs them one by one.
Each class that contains tests must extends `TestClass` class.

"Chaining" assertions is allowed, so something like this is possible:

```gd
class_name TestSomething extends TestClass

func something() -> void:
	ConfirmRange.in_range_int(ConfirmEqual.not_equal(5, 7), 0, 15)
```

### Overrideable methods

#### after_all

Runs after all test methods in the class.

#### before_all

Runs before all test methods in the class.

#### set_up

Runs before every test method in the class.

#### tear_down

Runs after every test method in the class.

#### category

Allows to assign a category to a test class.
The category can be used to run only tests from the category.

#### ignore

Ignore class during testing.

Can ignore always, only when run from the editor
or when tests are not run from the specified category.
The third option can be a bit confusing,
but allows creating tests that will not be always run,
but only under certain circumstances.

To ignore a class, it is required to override the `ignore` method
and return an `Ignore` object.
This object accepts `mode`, `reason`, `hide_from_results` and `category`.

The following example shows a method that tells Confirma
that a class is to be run only when category "SomeCategory" is tested:

```gd
func ignore() -> Ignore:
	return Ignore.new(
		# Or use numeric value, 2 in this case.
		Ignore.IgnoreMode.WHEN_NOT_RUNNING_CATEGORY,
		"Interesting reason",
		false,
		"SomeCategory"
	)
```
