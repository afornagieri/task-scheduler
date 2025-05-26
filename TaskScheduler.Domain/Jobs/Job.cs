using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskScheduler.Domain.Jobs;

public class Job
{
    [Required]
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [Required]
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [Required]
    [JsonPropertyName("status")]
    public JobStatus JobStatus { get; set; }

    [JsonPropertyName("payload")]
    public Dictionary<string, object>? Payload { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("attempts")]
    public int Attempts { get; set; }
}
