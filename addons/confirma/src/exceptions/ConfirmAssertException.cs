using System;

namespace Confirma.Exceptions;

public class ConfirmAssertException : Exception
{
    public ConfirmAssertException(string message) : base(message) { }

    public ConfirmAssertException(string message, Exception innerException)
    : base(message, innerException) { }

    public ConfirmAssertException() { }
}
