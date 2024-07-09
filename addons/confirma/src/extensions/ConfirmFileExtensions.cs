using System.IO;
using Confirma.Exceptions;
using Godot;

namespace Confirma.Extensions;

public static class ConfirmFileExtensions
{
    public static StringName ConfirmIsFile(this StringName path, string? message = null)
    {
        if (File.Exists(path)) return path;

        throw new ConfirmAssertException(
            message ??
            $"File '{path}' does not exist."
        );
    }

    public static StringName ConfirmIsNotFile(this StringName path, string? message = null)
    {
        if (!File.Exists(path)) return path;

        throw new ConfirmAssertException(
            message ??
            $"File '{path}' exists, but was not expected to."
        );
    }

    public static StringName ConfirmIsDirectory(this StringName path, string? message = null)
    {
        if (Directory.Exists(path)) return path;

        throw new ConfirmAssertException(
            message ??
            $"Directory '{path}' does not exist."
        );
    }

    public static StringName ConfirmIsNotDirectory(this StringName path, string? message = null)
    {
        if (!Directory.Exists(path)) return path;

        throw new ConfirmAssertException(
            message ??
            $"Directory '{path}' exists, but was not expected to."
        );
    }

    public static StringName ConfirmFileContains(this StringName path, string content, string? message = null)
    {
        if (File.ReadAllText(path).Contains(content)) return path;

        throw new ConfirmAssertException(
            message ??
            $"File '{path}' does not contain '{content}'."
        );
    }

    public static StringName ConfirmFileDoesNotContain(this StringName path, string content, string? message = null)
    {
        if (!File.ReadAllText(path).Contains(content)) return path;

        throw new ConfirmAssertException(
            message ??
            $"File '{path}' contains '{content}', but was not expected to."
        );
    }

    public static StringName ConfirmFileHasLength(this StringName path, long length, string? message = null)
    {
        if (new FileInfo(path).Length == length) return path;

        throw new ConfirmAssertException(
            message ??
            $"File '{path}' has length {new FileInfo(path).Length}, but expected {length}."
        );
    }

    public static StringName ConfirmFileDoesNotHaveLength(this StringName path, long length, string? message = null)
    {
        if (new FileInfo(path).Length != length) return path;

        throw new ConfirmAssertException(
            message ??
            $"File '{path}' has length {length}, but was not expected to."
        );
    }

    public static StringName ConfirmFileHasAttributes(this StringName path, FileAttributes attributes, string? message = null)
    {
        if ((new FileInfo(path).Attributes & attributes) == attributes) return path;

        throw new ConfirmAssertException(
            message ??
            $"File '{path}' does not have attributes '{attributes}'."
        );
    }

    public static StringName ConfirmFileDoesNotHaveAttributes(this StringName path, FileAttributes attributes, string? message = null)
    {
        if ((new FileInfo(path).Attributes & attributes) != attributes) return path;

        throw new ConfirmAssertException(
            message ??
            $"File '{path}' has attributes '{attributes}', but was not expected to."
        );
    }
}
