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

				StringBuilder sb = new();

				AppendNames(sb, directories, isDirectory: true);
				AppendNames(sb, files, isDirectory: false);

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

		private void AppendNames(StringBuilder sb, string[] names, bool isDirectory)
		{
			foreach (string name in names)
			{
				if (_ct.IsCancellationRequested) return;

				var attributes = File.GetAttributes(name);

				sb.Append(Path.GetFileName(name));

				if (isDirectory) sb.Append('/');

				sb.Append(' ');

				if (attributes.HasFlag(FileAttributes.Hidden)) sb.Append("[Hidden]");

				sb.AppendLine();
			}
		}
	}
}
