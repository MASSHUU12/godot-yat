using System.IO;
using System.Runtime.InteropServices;
using static YAT.Helpers.OS;

namespace YAT.Classes;

public static class Clipboard
{
    public static bool SetText(string text)
    {
#if GODOT_WINDOWS
            _ = RunCommand($"echo {text} | clip", out EExecutionResult result);
#else
        EExecutionResult result;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            _ = RunCommand(
                UsingWayland
                    ? $"echo {text} | wl-copy"
                    : $"echo {text} | xclip -selection clipboard",
                out result
            );
        }
        else
        {
            _ = RunCommand($"echo {text} | pbcopy", out result);
        }
#endif

        return result == EExecutionResult.Success;
    }

    public static bool SetImageData(byte[] data)
    {
        string tempFile = Path.GetTempFileName();
        File.WriteAllBytes(tempFile, data);

#if GODOT_WINDOWS
        // ProjectSettings.GlobalizePath works only in the editor.
        // To get access to the PowerShell script,
        // we need to copy it to a temporary file where PowerShell can access it.
        string path = CreateScriptTempFile(
            Updater.GetPluginPath()
            + "src/classes/clipboard/CopyImageToClipboard.ps1"
        );

        _ = RunCommand(
            $"{path} -imagePath \"{tempFile}\"",
            out EExecutionResult result,
            "powershell.exe"
        );

        File.Delete(path);
#else
        EExecutionResult result;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            _ = RunCommand(
                UsingWayland
                    ? $"wl-copy -t image/png < {tempFile}"
                    : $"xclip -selection clipboard -t image/png < {tempFile}",
                out result
            );
        }
        else
        {
            _ = RunCommand($"cat {tempFile} | pbcopy", out result);
        }
#endif

        File.Delete(tempFile);

        return result == EExecutionResult.Success;
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
