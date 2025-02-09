using System.IO;
using System.Runtime.InteropServices;
using static YAT.Helpers.OS;

namespace YAT.Classes;

public static class Clipboard
{
    public static EExecutionResult SetText(string text)
    {
        EExecutionResult result = EExecutionResult.Success;

#if GODOT_WINDOWS
            _ = RunCommand($"echo {text} | clip", out result);
#else
        _ = RunCommand(
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                ? $"echo {text} | xclip -selection clipboard"
                : $"echo {text} | pbcopy",
            out result
        );
#endif

        return result;
    }

    public static EExecutionResult SetImageData(byte[] data)
    {
        EExecutionResult result = EExecutionResult.Success;

        string tempFile = Path.GetTempFileName();
        File.WriteAllBytes(tempFile, data);

#if GODOT_WINDOWS
            // ProjectSettings.GlobalizePath works only in the editor.
            // To get access to the PowerShell script,
            // we need to copy it to a temporary file where PowerShell can access it.
            string path = CreateScriptTempFile(
                Updater.GetPluginPath() + "src/classes/clipboard/CopyImageToClipboard.ps1"
            );

            _ = RunCommand(
                $"{path} -imagePath \"{tempFile}\"",
                out result,
                "powershell.exe"
            );

            File.Delete(path);
#else
        _ = RunCommand(
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                ? $"xclip -selection clipboard -t image/png < {tempFile}"
                : $"cat {tempFile} | pbcopy",
            out result
        );
#endif

        File.Delete(tempFile);

        return result;
    }

    private static string CreateScriptTempFile(string path)
    {
        string script = Godot.FileAccess.GetFileAsString(path);
        string extension = Path.GetExtension(path);
        string tempFile = Path.GetTempFileName();

        File.WriteAllText(tempFile, script);
        File.Move(tempFile, tempFile + extension);

        return tempFile;
    }
}
