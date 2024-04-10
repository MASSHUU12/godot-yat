using System;

namespace Confirma;

public class ConfirmAssertException : Exception
{
	public ConfirmAssertException(string message) : base(message) { }

	public ConfirmAssertException(string message, Exception innerException)
	: base(message, innerException) { }
}
