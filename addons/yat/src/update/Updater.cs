using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using YAT.Classes;
using YAT.Types;

namespace YAT.Update;

public static class Updater
{
    private static SemanticVersion? _currentVersion;

    public static async Task<string?> DownloadZipAsync(string url)
    {
        string tempFilePath = Path.Combine(Path.GetTempPath(), "yat.zip");

        using (HttpClient client = new())
        {
            HttpRequestMessage request = new(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/vnd.github+json");
            request.Headers.Add("User-Agent", "YAT");

            HttpResponseMessage response;

            try
            {
                response = await client.SendAsync(request);
            }
            catch (Exception e) when (e is HttpRequestException)
            {
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            await using FileStream fileStream = new(tempFilePath, FileMode.Create);
            await response.Content.CopyToAsync(fileStream);
        }

        return tempFilePath;
    }

    public static bool DeleteCurrentVersion()
    {
        string path = Godot.ProjectSettings.GlobalizePath(GetPluginPath()[6..]);

        try
        {
            Directory.Delete(path, true);
        }
        catch (Exception e) when (
            e is IOException
            or UnauthorizedAccessException
            or PathTooLongException
            or DirectoryNotFoundException
        )
        {
            return false;
        }

        return true;
    }

    public static string GetPluginPath()
    {
        // Read the information from the project settings
        // because addons can be installed in various locations,
        // so it cannot be assumed that YAT will always be in the default location.
        string yatPath = Godot.ProjectSettings.GetSetting("autoload/YAT").AsString();
        return yatPath[1..].Remove(yatPath.Length - 13);
    }

    public static SemanticVersion? GetCurrentVersion()
    {
        if (_currentVersion is not null)
        {
            return _currentVersion;
        }

        string pluginConfigPath = GetPluginPath() + "plugin.cfg";

        Godot.ConfigFile config = new();
        if (config.Load(pluginConfigPath) != Godot.Error.Ok)
        {
            return null;
        }

        string version = config.GetValue("plugin", "version").AsString();

        _currentVersion = SemanticVersion.TryParse(version, out SemanticVersion? v) ? v : null;
        return _currentVersion;
    }

    public static bool IsUpdateAvailable()
    {
        (bool isSuccess, ReleaseTagInfo? info) = Release.GetLatestVersion();
        SemanticVersion? currentVersion = GetCurrentVersion();

        return isSuccess
            && info is not null
            && currentVersion is not null
            && info.Version > currentVersion;
    }
}
