using System;
using System.IO;
using System.Linq;
using System.Text;
using Godot;
using YAT.Attributes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Commands;

[Command("ls", "Lists the contents of the current directory.", "[b]Usage[/b]: ls")]
[Argument("path", "string", "The path to list the contents of.")]
[Option("-n", "bool", "Displays the children of the current node.")]
[Option("-m", "bool", "Lists the methods of the current node.")]
public sealed class Ls : ICommand
{
#nullable disable
	private BaseTerminal _terminal;
#nullable restore

	public CommandResult Execute(CommandData data)
	{
		string path = (string)data.Arguments["path"];
		bool n = (bool)data.Options["-n"];
		bool m = (bool)data.Options["-m"];

		_terminal = data.Terminal;

		if (n) return Scene.PrintChildren(_terminal, path)
			? ICommand.Success()
			: ICommand.Failure();
		if (m) return PrintNodeMethods(path);
		return PrintDirectoryContents(ProjectSettings.GlobalizePath(path));
	}

	private CommandResult PrintNodeMethods(string path)
	{
		StringBuilder sb = new();
		Node? node = Scene.GetFromPathOrDefault(path, _terminal.SelectedNode.Current, out path);

		if (node is null) return ICommand.Failure($"Node '{path}' does not exist.");

		var methods = node.GetMethodList().GetEnumerator();
		while (methods.MoveNext())
		{
			var info = Scene.GetNodeMethodInfo(methods.Current);
			string[] arguments = info.Args.Select(
				arg => $"[u]{arg["name"]}[/u]: {((Variant.Type)(int)arg["type"]).ToString()}"
			).ToArray();
			int returns = info.Return["type"].AsInt16();

			sb.Append($"[b]{info.Name}[/b]");
			sb.Append($"({string.Join(", ", arguments)}) -> ");
			sb.Append(((Variant.Type)returns).ToString());
			sb.AppendLine();
		}

		return ICommand.Ok(sb.ToString());
	}

	private CommandResult PrintDirectoryContents(string path)
	{
		if (!Directory.Exists(path)) return ICommand.Failure($"Directory '{path}' does not exist.");

		try
		{
			string[] files = Directory.GetFiles(path);
			string[] directories = Directory.GetDirectories(path);

			DirectoryInfo directoryInfo = new(path);
			FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();

			StringBuilder sb = new();

			AppendDetails(sb, fileSystemInfos);

			_terminal.Print(sb.ToString());
		}
		catch (UnauthorizedAccessException)
		{
			return ICommand.Failure($"Access to '{path}' is denied.");
		}
		catch (PathTooLongException)
		{
			return ICommand.Failure($"Path '{path}' is too long.");
		}
		catch (Exception ex)
		{
			return ICommand.Failure($"Error accessing directory '{path}': {ex.Message}");
		}

		return ICommand.Success();
	}

	/// <summary>
	/// Appends details of the given FileSystemInfo array to the provided StringBuilder.
	/// </summary>
	/// <param name="sb">The StringBuilder to append the details to.</param>
	/// <param name="infos">The array of FileSystemInfo objects to retrieve details from.</param>
	private static void AppendDetails(StringBuilder sb, FileSystemInfo[] infos)
	{
		int maxFileSizeLength = 0;
		int maxLastWriteTimeLength = 0;

		// Get the maximum length of the file size and last write time strings.
		foreach (FileSystemInfo info in infos)
		{
			maxLastWriteTimeLength = Math.Max(
									maxLastWriteTimeLength,
									info.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss").Length
			);
			maxFileSizeLength = Math.Max(maxFileSizeLength, (info is FileInfo file)
								? Numeric.SizeToString(file.Length).Length
								: 0
			);
		}

		maxFileSizeLength += 4;

		// Append the details of each FileSystemInfo object to the StringBuilder.
		foreach (FileSystemInfo info in infos)
		{
			var fileSizeString = Numeric.SizeToString(
								info is FileInfo
								? ((FileInfo)info).Length
								: 0
			);

			var line = string.Format(
				"{0}\t\t{1}\t\t{2}{3}{4}",
				info.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss").PadRight(maxLastWriteTimeLength),
				info is FileInfo
					? fileSizeString.PadRight(Mathf.Clamp(maxFileSizeLength - fileSizeString.Length, 0, maxFileSizeLength))
					: string.Empty.PadRight(maxFileSizeLength),
				info.Name,
				info is DirectoryInfo ? '/' : string.Empty,
				(info.Attributes & FileAttributes.Hidden) != 0 ? "*" : string.Empty
			);

			sb.Append(line);
			sb.AppendLine();
		}
	}
}
