using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;

namespace Confirma.Helpers;

public static class Json
{
    private static JsonSerializerOptions options = new()
    {
        WriteIndented = true
    };

    public static async Task<bool> DumpToFileAsync<T>(
        string fileName,
        T data,
        bool pretty
    )
    {
        if (options.WriteIndented != pretty)
        {
            options = new()
            {
                WriteIndented = pretty
            };
        }

        try
        {
            await using FileStream stream = new(fileName, FileMode.Create);
            await JsonSerializer.SerializeAsync(stream, data, options);
        }
        catch (Exception e) when (
            e is FileNotFoundException
            or UnauthorizedAccessException
            or IOException
            or DirectoryNotFoundException
            or PathTooLongException
        )
        {
            return false;
        }

        return true;
    }

    public static async Task<T?> LoadFromFile<T>(string fileName)
    {
        fileName = ProjectSettings.GlobalizePath(fileName);

        try
        {
            await using FileStream stream = new(fileName, FileMode.Open);
            return await JsonSerializer.DeserializeAsync<T>(stream);
        }
        catch (Exception e) when (
            e is FileNotFoundException or
            DirectoryNotFoundException or
            UnauthorizedAccessException or
            IOException or
            JsonException
        )
        {
            return default;
        }
    }
}
