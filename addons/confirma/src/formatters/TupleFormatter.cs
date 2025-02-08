using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Confirma.Helpers;

namespace Confirma.Formatters;

public class TupleFormatter : Formatter
{
    public override string Format(object? value)
    {
        return value?.GetType() switch
        {
            Type t when t.ImplementsAny(typeof(ITuple))
                => FormatTuple(value),
            _ => new DefaultFormatter().Format(value),
        };
    }

    private static string FormatTuple(object value)
    {
        Type type = value.GetType();
        FieldInfo[] fields = type.GetFields(
            BindingFlags.Instance | BindingFlags.Public
        );

        AutomaticFormatter auto = new();
        IEnumerable<string>? formattedFields = fields.Select(
            field => auto.Format(field.GetValue(value))
        );

        return $"({string.Join(", ", formattedFields)})";
    }
}
