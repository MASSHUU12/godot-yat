using System;

namespace Confirma.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class AfterAllAttribute(string methodName = "AfterAll")
: LifecycleAttribute(methodName)
{ }
