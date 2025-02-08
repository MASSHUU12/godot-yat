using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Confirma.Mock;

public class Mock<T> where T : class
{
    private readonly ConcurrentBag<CallRecord> _callRecords = [];
    private readonly ConcurrentDictionary<string, object?> _defaultReturnValues = new();

    private static readonly ConcurrentDictionary<string, MethodInfo?> _methodInfoCache = new();

    public T Instance { get; }

    public Mock()
    {
        Instance = DispatchProxy.Create<T, MockProxy>();
        ((MockProxy)(object)Instance).SetMock(this);
    }

    public IReadOnlyCollection<CallRecord> GetCallRecords()
    {
        return _callRecords;
    }

    public void ClearCallRecords()
    {
        _callRecords.Clear();
    }

    public void SetDefaultReturnValue<TResult>(string methodName, TResult? value)
    {
        MethodInfo? method = Mock<T>.GetMethod(methodName);

        if (method?.ReturnType.IsAssignableFrom(typeof(TResult)) != true)
        {
            throw new ArgumentException(
                $"Method '{methodName}' does not exist or return type mismatch on '{typeof(T).Name}'."
            );
        }
        _defaultReturnValues[methodName] = value;
    }

    public bool VerifyCalled(string methodName, int expectedCallCount)
    {
        int actualCallCount = _callRecords.Count(
            cr => cr.MethodName == methodName
        );

        return actualCallCount == expectedCallCount;
    }

    public bool VerifyCalledWith(string methodName, params object?[] expectedArgs)
    {
        return _callRecords.Any(cr =>
            cr.MethodName == methodName &&
            cr.Arguments?.SequenceEqual(expectedArgs) == true
        );
    }

    private static MethodInfo? GetMethod(string methodName)
    {
        return _methodInfoCache.GetOrAdd(
            methodName,
            static nm => typeof(T).GetMethod(nm)
        );
    }

    private class MockProxy : DispatchProxy
    {
        private Mock<T>? _mock;

        public void SetMock(Mock<T> mock)
        {
            _mock = mock;
        }

        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            if (_mock is null || targetMethod is null)
            {
                throw new InvalidOperationException("Mock not initialized.");
            }

            string methodName = targetMethod.Name;
            _mock._callRecords.Add(new(methodName, args));

            if (_mock._defaultReturnValues.TryGetValue(
                    methodName, out object? returnValue
                )
            )
            {
                return returnValue;
            }

            if (targetMethod.ReturnType == typeof(void))
            {
                return null;
            }

            return targetMethod.ReturnType.IsValueType
                ? Activator.CreateInstance(targetMethod.ReturnType)
                : null;
        }
    }
}
