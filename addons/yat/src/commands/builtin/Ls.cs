using System;
using System.IO;
using System.Linq;
using System.Text;
using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Scenes.BaseTerminal;
using static YAT.Scenes.BaseTerminal.BaseTerminal;

namespace YAT.Commands
{
	[Command("ls", "Lists the contents of the current directory.", "[b]Usage[/b]: ls")]
	[Argument("path", "string", "The path to list the contents of.")]
	[Option("-n", null, "Displays the children of the current node.", false)]
	[Option("-m", null, "Lists the methods of the current node.", false)]
	public sealed class Ls : ICommand
	{
		private BaseTerminal _terminal;

		public CommandResult Execute(CommandData data)
		{
			string path = (string)data.Arguments["path"];
			bool n = (bool)data.Options["-n"];
			bool m = (bool)data.Options["-m"];

			_terminal = data.Terminal;

			if (n) return Scene.PrintChildren(_terminal, path)
				? CommandResult.Success
				: CommandResult.Failure;
			if (m) return PrintNodeMethods(path);
			return PrintDirectoryContents(ProjectSettings.GlobalizePath(path));
		}

		private CommandResult PrintNodeMethods(string path)
		{
			Node node = Scene.GetFromPathOrDefault(path, _terminal.SelectedNode.Current, out path);

			if (node is null)
			{
				_terminal.Print($"Node '{path}' does not exist.", PrintType.Error);
				return CommandResult.Failure;
			}

			var methods = node.GetMethodList().GetEnumerator();

			StringBuilder sb = new();

			while (methods.MoveNext())
			{
				string name = methods.Current.TryGetValue("name", out var value)
					? (string)value
					: string.Empty;

				string[] arguments = methods.Current.TryGetValue("args", out value)
					? value.AsGodotArray<Godot.Collections.Dictionary<string, Variant>>()
						.Select(arg => $"[u]{arg["name"]}[/u]: {((Variant.Type)(int)arg["type"]).ToString()}").ToArray()
					: Array.Empty<string>();

				int returns = methods.Current.TryGetValue("return", out value)
					? value.AsGodotDictionary<string, int>()["type"]
					: 0;

				sb.Append($"[b]{name}[/b]");
				sb.Append($"({string.Join(", ", arguments)}) -> ");
				sb.Append(((Variant.Type)returns).ToString());
				sb.AppendLine();
			}

			_terminal.Print(sb.ToString());

			return CommandResult.Success;
		}

		private CommandResult PrintDirectoryContents(string path)
		{
			if (!Directory.Exists(path))
			{
				_terminal.Print($"Directory '{path}' does not exist.", PrintType.Error);
				return CommandResult.Failure;
			}

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
				_terminal.Print($"Access to '{path}' is denied.", PrintType.Error);
				return CommandResult.Failure;
			}
			catch (PathTooLongException)
			{
				_terminal.Print($"Path '{path}' is too long.", PrintType.Error);
				return CommandResult.Failure;
			}
			catch (Exception ex)
			{
				_terminal.Print($"Error accessing directory '{path}': {ex.Message}", PrintType.Error);
				return CommandResult.Failure;
			}

			return CommandResult.Success;
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
									? NumericHelper.SizeToString(file.Length).Length
									: 0
				);
			}

			maxFileSizeLength += 4;

			// Append the details of each FileSystemInfo object to the StringBuilder.
			foreach (FileSystemInfo info in infos)
			{
				var fileSizeString = NumericHelper.SizeToString(
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
}
