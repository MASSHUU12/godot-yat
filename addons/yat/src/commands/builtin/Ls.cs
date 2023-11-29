using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using static YAT.Scenes.Overlay.Components.Terminal.Terminal;

namespace YAT.Commands
{
	[Command("ls", "Lists the contents of the current directory.", "[b]Usage[/b]: ls")]
	[Threaded]
	[Arguments("path:string")]
	public sealed class Ls : ICommand
	{
		public YAT Yat { get; set; }
		public Ls(YAT Yat) => this.Yat = Yat;

		private CancellationToken _ct;

		public CommandResult Execute(Dictionary<string, object> cArgs, CancellationToken ct, params string[] args)
		{
			string path = (string)cArgs["path"];
			path = ProjectSettings.GlobalizePath(path);

			_ct = ct;

			return PrintDirectoryContents(path);
		}

		private CommandResult PrintDirectoryContents(string path)
		{
			if (!Directory.Exists(path))
			{
				Yat.Terminal.Print($"Directory '{path}' does not exist.", PrintType.Error);
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

				Yat.Terminal.Print(sb.ToString());
			}
			catch (UnauthorizedAccessException)
			{
				Yat.Terminal.Print($"Access to '{path}' is denied.", PrintType.Error);
				return CommandResult.Failure;
			}
			catch (PathTooLongException)
			{
				Yat.Terminal.Print($"Path '{path}' is too long.", PrintType.Error);
				return CommandResult.Failure;
			}
			catch (Exception ex)
			{
				Yat.Terminal.Print($"Error accessing directory '{path}': {ex.Message}", PrintType.Error);
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
			foreach (FileSystemInfo info in infos)
			{
				var line = string.Format(
					"{0}\t{1}\t{2}{3}{4}",
					info.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"),
					info is FileInfo file ? GetFileSizeString(file.Length) : string.Empty,
					info.Name,
					info is DirectoryInfo ? '/' : string.Empty,
					(info.Attributes & FileAttributes.Hidden) != 0 ? '*' : string.Empty
				);

				sb.Append(line);
				sb.AppendLine();
			}
		}

		/// <summary>
		/// Converts a file size in bytes to a human-readable string representation.
		/// </summary>
		/// <param name="fileSize">The file size in bytes.</param>
		/// <returns>A string representing the file size in a human-readable format.</returns>
		private static string GetFileSizeString(long fileSize)
		{
			const int byteConversion = 1024;
			double bytes = fileSize;

			if (bytes < byteConversion) return $"{bytes} B";

			double kilobytes = bytes / byteConversion;
			if (kilobytes < byteConversion) return $"{kilobytes:0.##} KB";

			double megabytes = kilobytes / byteConversion;
			if (megabytes < byteConversion) return $"{megabytes:0.##} MB";

			double gigabytes = megabytes / byteConversion;

			return $"{gigabytes:0.##} GB";
		}
	}
}
