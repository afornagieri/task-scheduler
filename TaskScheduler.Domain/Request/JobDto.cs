using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskScheduler.Domain.Request;

public class JobDto
{
    [Required]
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("payload")]
    public Dictionary<string, object>? Payload { get; set; }
}
