using System;

namespace Confirma.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class BeforeAllAttribute(string methodName = "BeforeAll")
: LifecycleAttribute(methodName)
{ }
