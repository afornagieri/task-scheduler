namespace TaskScheduler.Domain.Jobs;

public enum JobStatus
{
    Awaiting,
    Processing,
    Processed,
    Failed
}
