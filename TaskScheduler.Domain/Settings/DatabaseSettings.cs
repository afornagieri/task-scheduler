namespace TaskScheduler.Domain;

public class DatabaseSettings
{
    /// <summary>
    /// Connection string
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// Database name
    /// </summary>
    public string? DbName { get; set; }

    /// <summary>
    /// Collection name
    /// </summary>
    public string? Collection { get; set; }
}
