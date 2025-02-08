using System;

namespace Confirma.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class TearDownAttribute(string methodName = "TearDown")
: LifecycleAttribute(methodName)
{ }
