<div align="center">
 <h3>Testing</h1>
</div>

Confirma will detect all tests, regardless of where in the project you place them.

Each class that contains tests must be labelled with the `TestClass` attribute.

Each method, however, must be tagged with the `TestCase` attribute.

Chaining assertions is allowed, so something like this is possible:

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

## Accessing scene tree

Access to the scene tree is enabled by the static `Global` class. It provides the variable `Root` where the tree's root Window is located.

## Attributes

### TestClass

The TestClass attribute is used to identify the classes in which the tests are located.
It is required, Confirma ignores all classes that do not have this attribute.

### TestCase

The TestCase attribute is used to mark the methods that perform the tests.
It also accepts arguments, which allows parameterized tests.

### AfterAll

Runs after all test methods in the class.

### BeforeAll

Runs before all test methods in the class.

### Category

Not implemented.

### Ignore

Ignore class/method during testing.

Can ignore always or only when ran from editor.

### SetUp

Runs before every test method in the class.

### TearDown

Runs after every test method in the class.

### TestName

Used to display a different name for the test than the method name.

### Parallelizable

Allows to run all the tests included in the class on separate CPU cores.

### Repeat

The Repeat attribute allows you to run a particular TestCase several times.
Repeat refers to the next TestCase, so the order in which the attributes are defined matters.
