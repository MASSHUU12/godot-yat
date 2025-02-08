using System.Collections.Generic;
using System.Text.Json.Serialization;
using Confirma.Classes.HelpElements;

namespace Confirma.Deserialization.Json;

public class HelpFile
{
    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("data")]
    public List<FileElement> Data { get; set; } = [];
}
