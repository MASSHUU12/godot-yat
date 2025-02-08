using System;
using System.Reflection;
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
            "Expected event {1} to be raised.",
            nameof(ConfirmRaisesEvent),
            null,
            GetEventName(eventHandler),
            null,
            message
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
            "Expected event {1} not to be raised.",
            nameof(ConfirmRaisesEvent),
            null,
            GetEventName(eventHandler),
            null,
            message
        );

        void Handler(object? sender, TEventArgs e)
        {
            eventRaised = true;
        }
    }

    // TODO: Test this method.
    private static string GetEventName<TEventArgs>(
        EventHandler<TEventArgs>? eventHandler
    )
    where TEventArgs : EventArgs
    {
        if (eventHandler is null) return "null";

        MethodInfo methodInfo = eventHandler.Method;
        if (methodInfo.DeclaringType is null) return "unknown";

        FieldInfo? fieldInfo = methodInfo.DeclaringType.GetField(
            methodInfo.Name,
            BindingFlags.Instance | BindingFlags.NonPublic
        );
        if (fieldInfo is null) return "unknown";

        EventInfo? eventInfo = fieldInfo.DeclaringType?.GetEvent(fieldInfo.Name);
        if (eventInfo is null) return "unknown";

        return eventInfo.Name;
    }
}
