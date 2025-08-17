using System.Text.Json.Serialization;
using Domain.Entity;

namespace Domain.Model;

public class DreamCacheKey
{
    [JsonIgnore]
    public static int DefaultStartIndex = 0;
    [JsonIgnore]
    public static int DefaultCount = 5;

    public string ModelName
    { get; set; } = nameof(Dream);
    public int StartIndex { get; set; } = DefaultStartIndex;
    public int Count { get; set; } = DefaultCount;
}
