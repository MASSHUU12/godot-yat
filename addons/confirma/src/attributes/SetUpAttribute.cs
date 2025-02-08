using System;

namespace Confirma.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class SetUpAttribute(string methodName = "SetUp")
: LifecycleAttribute(methodName)
{ }
