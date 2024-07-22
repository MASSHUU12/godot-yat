using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using YAT.Classes;
using YAT.Types;

namespace YAT.Update;

public static class Release
{
    public static async Task<(bool, string)> GetTagsAsync()
    {
        using HttpClient client = new();

        HttpRequestMessage request = new(
            HttpMethod.Get,
            "https://api.github.com/repos/MASSHUU12/godot-yat/tags"
        );
        request.Headers.Add("Accept", "application/vnd.github+json");
        request.Headers.Add("User-Agent", "YAT");

        HttpResponseMessage httpResponse;

        try
        {
            httpResponse = await client.SendAsync(request);
        }
        catch (Exception e) when (e is HttpRequestException)
        {
            return (false, "Unavailable");
        }

        if (!httpResponse.IsSuccessStatusCode)
        {
            return (false, httpResponse.ReasonPhrase ?? string.Empty);
        }

        return (true, await httpResponse.Content.ReadAsStringAsync());
    }

    public static bool TryExtractLatestReleaseTagInfo(JsonDocument document, out ReleaseTagInfo? tagInfo)
    {
        tagInfo = null;

        JsonElement root = document.RootElement;
        string versionString, zipballUrl, tarballUrl, commitSha, commitUrl, nodeId;

        if (root.GetArrayLength() == 0)
        {
            return false;
        }

        JsonElement e = root[0];

        if (!e.TryGetProperty("name", out JsonElement element))
        {
            return false;
        }

        versionString = element.GetString()!;
        versionString = versionString.StartsWith('v') ? versionString[1..] : versionString;

        if (!SemanticVersion.TryParse(versionString, out SemanticVersion? version))
        {
            return false;
        }

        if (!e.TryGetProperty("zipball_url", out element))
        {
            return false;
        }

        zipballUrl = element.GetString()!;

        if (!e.TryGetProperty("tarball_url", out element))
        {
            return false;
        }

        tarballUrl = element.GetString()!;

        if (!e.TryGetProperty("commit", out element))
        {
            return false;
        }

        commitSha = element.GetProperty("sha").GetString()!;
        commitUrl = element.GetProperty("url").GetString()!;

        if (!e.TryGetProperty("node_id", out element))
        {
            return false;
        }

        nodeId = element.GetString()!;

        tagInfo = new(version!, zipballUrl, tarballUrl, commitSha, commitUrl, nodeId);

        return true;
    }

    public static (bool, ReleaseTagInfo?) GetLatestVersion()
    {
        Task<(bool, string)> task = Task.Run(GetTagsAsync);
        task.Wait();

        (bool isSuccess, string content) = task.Result;

        if (!isSuccess)
        {
            return (false, null);
        }

        try
        {
            JsonDocument document = JsonDocument.Parse(content);

            return (
                TryExtractLatestReleaseTagInfo(document, out ReleaseTagInfo? info),
                info
            );
        }
        catch (Exception e) when (e is JsonException or ArgumentException)
        {
            return (false, null);
        }
    }
}
