using System;
using static System.AttributeTargets;

namespace Confirma.Attributes;

[AttributeUsage(Class | Method)]
public class IgnoreAttribute : Attribute
{
	public string? Reason { get; }

	public IgnoreAttribute(string? reason = null)
	{
		Reason = string.IsNullOrEmpty(reason) ? null : reason;
	}
}
