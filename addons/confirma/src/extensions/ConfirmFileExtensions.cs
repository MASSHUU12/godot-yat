using System.IO;
using Confirma.Exceptions;
using Godot;

namespace Confirma.Extensions;

public static class ConfirmFileExtensions
{
	public static void ConfirmIsFile(this StringName path, string? message = null)
	{
		if (File.Exists(path)) return;

		throw new ConfirmAssertException(message ?? "Expected file to exist but it does not.");
	}

	public static void ConfirmIsNotFile(this StringName path, string? message = null)
	{
		if (!File.Exists(path)) return;

		throw new ConfirmAssertException(message ?? "Expected file to not exist but it does.");
	}

	public static void ConfirmIsDirectory(this StringName path, string? message = null)
	{
		if (Directory.Exists(path)) return;

		throw new ConfirmAssertException(message ?? "Expected directory to exist but it does not.");
	}

	public static void ConfirmIsNotDirectory(this StringName path, string? message = null)
	{
		if (!Directory.Exists(path)) return;

		throw new ConfirmAssertException(message ?? "Expected directory to not exist but it does.");
	}

	public static void ConfirmFileContains(this StringName path, string content, string? message = null)
	{
		if (File.ReadAllText(path).Contains(content)) return;

		throw new ConfirmAssertException(message ?? $"Expected file to contain: {content}");
	}

	public static void ConfirmFileDoesNotContain(this StringName path, string content, string? message = null)
	{
		if (!File.ReadAllText(path).Contains(content)) return;

		throw new ConfirmAssertException(message ?? $"Expected file to not contain: {content}");
	}

	public static void ConfirmFileHasLength(this StringName path, long length, string? message = null)
	{
		if (new FileInfo(path).Length == length) return;

		throw new ConfirmAssertException(message ?? $"Expected file to have length: {length} but was {new FileInfo(path).Length}");
	}

	public static void ConfirmFileDoesNotHaveLength(this StringName path, long length, string? message = null)
	{
		if (new FileInfo(path).Length != length) return;

		throw new ConfirmAssertException(message ?? $"Expected file to not have length: {length}");
	}

	public static void ConfirmFileHasAttributes(this StringName path, FileAttributes attributes, string? message = null)
	{
		if ((new FileInfo(path).Attributes & attributes) == attributes) return;

		throw new ConfirmAssertException(message ?? $"Expected file to have attributes: {attributes}");
	}

	public static void ConfirmFileDoesNotHaveAttributes(this StringName path, FileAttributes attributes, string? message = null)
	{
		if ((new FileInfo(path).Attributes & attributes) != attributes) return;

		throw new ConfirmAssertException(message ?? $"Expected file to not have attributes: {attributes}");
	}
}
