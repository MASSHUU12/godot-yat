using System;
using Confirma.Exceptions;

namespace Confirma.Extensions;

public static class ConfirmEventExtensions
{
    public static Action ConfirmRaisesEvent<TEventArgs>(
        this Action action,
        ref EventHandler<TEventArgs>? eventHandler,
        string? message = null
    )
    where TEventArgs : EventArgs
    {
        bool eventRaised = false;

        eventHandler += Handler;

        try
        {
            action();
        }
        finally
        {
            eventHandler -= Handler;
        }

        if (eventRaised)
        {
            return action;
        }

        throw new ConfirmAssertException(
                message ??
                $"{eventHandler?.GetType().Name ?? "event"} was not raised."
            );

        void Handler(object? sender, TEventArgs e)
        {
            eventRaised = true;
        }
    }

    public static Action ConfirmDoesNotRaiseEvent<TEventArgs>(
        this Action action,
        ref EventHandler<TEventArgs>? eventHandler,
        string? message = null
    )
    where TEventArgs : EventArgs
    {
        bool eventRaised = false;

        eventHandler += Handler;

        try
        {
            action();
        }
        finally
        {
            eventHandler -= Handler;
        }

        if (!eventRaised)
        {
            return action;
        }

        throw new ConfirmAssertException(
                message ??
                $"{eventHandler?.GetType().Name ?? "event"} was raised."
            );

        void Handler(object? sender, TEventArgs e)
        {
            eventRaised = true;
        }
    }
}
