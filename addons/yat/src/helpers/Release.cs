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

        string content = await httpResponse.Content.ReadAsStringAsync();
        return (true, content);
    }

    public static async Task<(bool, string)> CheckLatestVersionAsync()
    {
        (bool isSuccess, string response) = await GetTagsAsync();

        if (!isSuccess) return (false, response);

        JsonDocument json = JsonDocument.Parse(response);

        return (true, "");
    }

    public static (bool, string) CheckLatestVersion()
    {
        Task<(bool, string)> task = Task.Run(CheckLatestVersionAsync);
        task.Wait();

        return task.Result;
    }
}
