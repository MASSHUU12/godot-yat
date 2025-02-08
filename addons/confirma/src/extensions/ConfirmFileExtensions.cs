using System.IO;
using Confirma.Exceptions;
using Confirma.Formatters;
using Godot;

namespace Confirma.Extensions;

public static class ConfirmFileExtensions
{
    public static StringName ConfirmIsFile(
        this StringName path,
        string? message = null
    )
    {
        if (File.Exists(path))
        {
            return path;
        }

        throw new ConfirmAssertException(
            "Expected file {1} to exist.",
            nameof(ConfirmIsFile),
            new StringFormatter(),
            path,
            null,
            message
        );
    }

    public static StringName ConfirmIsNotFile(
        this StringName path,
        string? message = null
    )
    {
        if (!File.Exists(path))
        {
            return path;
        }

        throw new ConfirmAssertException(
            "Expected file {1} to not exist.",
            nameof(ConfirmIsNotFile),
            new StringFormatter(),
            path,
            null,
            message
        );
    }

    public static StringName ConfirmIsDirectory(
        this StringName path,
        string? message = null
    )
    {
        if (Directory.Exists(path))
        {
            return path;
        }

        throw new ConfirmAssertException(
            "Expected directory {1} to exist.",
            nameof(ConfirmIsDirectory),
            new StringFormatter(),
            path,
            null,
            message
        );
    }

    public static StringName ConfirmIsNotDirectory(
        this StringName path,
        string? message = null
    )
    {
        if (!Directory.Exists(path))
        {
            return path;
        }

        throw new ConfirmAssertException(
            "Expected directory {1} to not exist.",
            nameof(ConfirmIsNotDirectory),
            new StringFormatter(),
            path,
            null,
            message
        );
    }

    public static StringName ConfirmFileContains(
        this StringName path,
        string content,
        string? message = null
    )
    {
        if (File.ReadAllText(path).Contains(content))
        {
            return path;
        }

        throw new ConfirmAssertException(
            "Expected file {1} to contain {2}.",
            nameof(ConfirmFileContains),
            new StringFormatter(),
            path,
            content,
            message
        );
    }

    public static StringName ConfirmFileDoesNotContain(
        this StringName path,
        string content,
        string? message = null
    )
    {
        if (!File.ReadAllText(path).Contains(content))
        {
            return path;
        }

        throw new ConfirmAssertException(
            "Expected file {1} to not contain {2}.",
            nameof(ConfirmFileDoesNotContain),
            new StringFormatter(),
            path,
            content,
            message
        );
    }

    public static StringName ConfirmFileHasLength(
        this StringName path,
        long length,
        string? message = null
    )
    {
        if (new FileInfo(path).Length == length)
        {
            return path;
        }

        throw new ConfirmAssertException(
            $"Expected file {new StringFormatter().Format(path)} to has length "
            + "{1}, but got {2}.",
            nameof(ConfirmFileHasLength),
            new NumericFormatter(),
            length,
            new FileInfo(path).Length,
            message
        );
    }

    public static StringName ConfirmFileDoesNotHaveLength(
        this StringName path,
        long length,
        string? message = null
    )
    {
        if (new FileInfo(path).Length != length)
        {
            return path;
        }

        throw new ConfirmAssertException(
            $"Expected file {new StringFormatter().Format(path)} to not have "
            + "length of {1}",
            nameof(ConfirmFileDoesNotHaveLength),
            new NumericFormatter(),
            length,
            null,
            message
        );
    }

    public static StringName ConfirmFileHasAttributes(
        this StringName path,
        FileAttributes attributes,
        string? message = null
    )
    {
        if ((new FileInfo(path).Attributes & attributes) == attributes)
        {
            return path;
        }

        throw new ConfirmAssertException(
            "Expected file {1} to have attributes {2}.",
            nameof(ConfirmFileDoesNotContain),
            new StringFormatter(),
            path,
            attributes,
            message
        );
    }

    public static StringName ConfirmFileDoesNotHaveAttributes(
        this StringName path,
        FileAttributes attributes,
        string? message = null
    )
    {
        if ((new FileInfo(path).Attributes & attributes) != attributes)
        {
            return path;
        }

        throw new ConfirmAssertException(
            "Expected file {1} to not have attributes {2}.",
            nameof(ConfirmFileDoesNotContain),
            new StringFormatter(),
            path,
            attributes,
            message
        );
    }
}
