using System;

namespace Confirma.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class IgnoreAttribute : Attribute
{
	public string? Reason { get; }

	public IgnoreAttribute(string? reason = null)
	{
		Reason = reason;
	}
}
