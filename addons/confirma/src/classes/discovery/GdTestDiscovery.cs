using System;
using System.Collections.Generic;
using System.IO;
using Godot;

namespace Confirma.Classes.Discovery;

public static class GdTestDiscovery
{
    private static bool? _testScriptsDirectoryCached = null;

    public static IEnumerable<GdScriptInfo> GetTestScripts(
        string pathToTests,
        int maxDepth = 16
    )
    {
        string globalizedPath = ProjectSettings.GlobalizePath(pathToTests);

        _testScriptsDirectoryCached ??= Directory.Exists(globalizedPath);

        if (_testScriptsDirectoryCached == false)
        {
            yield break;
        }

        IEnumerable<string>? files;

        try
        {
            files = Directory.EnumerateFiles(globalizedPath);
        }
        catch (Exception e) when (
            e is ArgumentException
            or DirectoryNotFoundException
            or IOException
            or PathTooLongException
            or System.Security.SecurityException
            or UnauthorizedAccessException
        )
        {
            yield break;
        }

        foreach (string filePath in Directory.EnumerateFiles(globalizedPath))
        {
            if (!filePath.EndsWith(".gd", StringComparison.Ordinal))
            {
                continue;
            }

            GDScript script = GD.Load<GDScript>(filePath);

            if (script.GetBaseScript()?.GetGlobalName().ToString() == "TestClass")
            {
                yield return GdScriptInfo.Parse(script);
            }
        }

        if (maxDepth > 1)
        {
            foreach (string dirPath in Directory.EnumerateDirectories(globalizedPath))
            {
                foreach (GdScriptInfo scriptInfo in GetTestScripts(dirPath, maxDepth - 1))
                {
                    yield return scriptInfo;
                }
            }
        }
    }
}
