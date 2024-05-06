<div align="center">
 <h3>Testing</h1>
</div>

Confirma will detect all tests, regardless of where in the project you place them.

Each class that contains tests must be labelled with the `TestClass` attribute.

Each method, however, must be tagged with the `TestCase` attribute.

Example test class:

```cs
using Confirma.Attributes;
using Confirma.Exceptions;
using Confirma.Extensions;

namespace Confirma.Tests;

[TestClass]
[Parallelizable]
public static class ConfirmBooleanTest
{
 [TestCase]
 public static void ConfirmTrue_WhenTrue()
 {
  true.ConfirmTrue();
 }
}
```

## Accessing scene tree

Access to the scene tree is enabled by the static `Global` class. It provides the variable `Root` where the tree's root Window is located.

## Attributes

### TestClass

The `TestClass` attribute is used to identify the classes in which the tests are located.
It is required, Confirma ignores all classes that do not have this attribute.

### TestCase

The `TestCase` attribute is used to mark the methods that perform the tests.
It also accepts arguments, which allows parameterized tests.

### AfterAll

Runs after all test methods in the class.

### BeforeAll

Runs before all test methods in the class.

### Category

Not implemented.

### Ignore

Ignore method during testing.

### SetUp

Runs before every test method in the class.

### TearDown

Runs after every test method in the class.

### Timeout

Not implemented.

### TestName

Used to display a different name for the test than the method name.

### Parallelizable

Allows to run all the tests included in the class on separate CPU cores.
