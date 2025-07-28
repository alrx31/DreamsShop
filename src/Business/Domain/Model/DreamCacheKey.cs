using System.Text.Json.Serialization;
using Domain.Entity;

namespace Domain.Model;

public class DreamCacheKey
{
    [JsonIgnore]
    public int DafaultStartIndex { get; set; } = 0;
    [JsonIgnore]
    public int DefaultCount { get; set; } = 5;

    public string ModelName
    { get; set; } = nameof(Dream);
    public int StartIndex { get; set; } = 0;
    public int Count { get; set; } = 5;
}
