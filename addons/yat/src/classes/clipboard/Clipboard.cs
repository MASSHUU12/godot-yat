using System.IO;
using static YAT.Helpers.OS;

namespace YAT.Classes;

public static class Clipboard
{
	public static ExecutionResult SetText(string text)
	{
		ExecutionResult result = ExecutionResult.Success;

		if (Platform == OperatingSystem.Windows)
			RunCommand($"echo {text} | clip", out result);
		else if (Platform == OperatingSystem.Linux)
			RunCommand($"echo {text} | xclip -selection clipboard", out result);
		else if (Platform == OperatingSystem.OSX)
			RunCommand($"echo {text} | pbcopy", out result);

		return result;
	}

	public static ExecutionResult SetImageData(byte[] data)
	{
		ExecutionResult result = ExecutionResult.Success;

		string tempFile = Path.GetTempFileName();
		File.WriteAllBytes(tempFile, data);

		if (Platform == OperatingSystem.Windows)
		{
			// ProjectSettings.GlobalizePath works only in the editor.
			// To get access to the PowerShell script,
			// we need to copy it to a temporary file where PowerShell can access it.
			var path = CreateScriptTempFile("res://addons/yat/src/classes/clipboard/CopyImageToClipboard.ps1");

			RunCommand(
				$"{path} -imagePath \"{tempFile}\"",
				out result,
				"powershell.exe"
			);

			File.Delete(path);
		}
		else if (Platform == OperatingSystem.Linux)
		{
			RunCommand($"xclip -selection clipboard -t image/png -i {tempFile}", out result);
		}
		else if (Platform == OperatingSystem.OSX)
		{
			RunCommand($"cat {tempFile} | pbcopy", out result);
		}

		File.Delete(tempFile);

		return result;
	}

	private static string CreateScriptTempFile(string path)
	{
		var script = Godot.FileAccess.GetFileAsString(path);
		var extension = Path.GetExtension(path);

		string tempFile = Path.GetTempFileName();
		File.WriteAllText(tempFile, script);
		File.Move(tempFile, tempFile + extension);

		return tempFile;
	}
}
