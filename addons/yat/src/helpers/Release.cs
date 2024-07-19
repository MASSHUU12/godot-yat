using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace YAT.Helpers;

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

        HttpResponseMessage httpResponse = await client.SendAsync(request);

        if (!httpResponse.IsSuccessStatusCode)
        {
            return (false, httpResponse.ReasonPhrase ?? string.Empty);
        }

        return (true, await httpResponse.Content.ReadAsStringAsync());
    }

    public static bool ExtractLatestVersion(string content, out string version)
    {
        version = string.Empty;

        try
        {
            JsonDocument json = JsonDocument.Parse(content);
            JsonElement root = json.RootElement;

            if (root.GetArrayLength() == 0)
            {
                return false;
            }

            if (!root[0].TryGetProperty("name", out JsonElement element))
            {
                return false;
            }

            version = element.GetString() ?? string.Empty;

            if (string.IsNullOrEmpty(version))
            {
                return false;
            }
        }
        catch (Exception e) when (e is JsonException or ArgumentException)
        {
            return false;
        }

        return true;
    }

    public static (bool, string) CheckLatestVersion()
    {
        Task<(bool, string)> task = Task.Run(GetTagsAsync);
        task.Wait();

        (bool isSuccess, string content) = task.Result;

        if (!isSuccess)
        {
            return (false, content);
        }

        return (ExtractLatestVersion(content, out string version), version);
    }
}
