using TaskScheduler.Domain.Jobs;
using TaskScheduler.Domain.Request;

namespace TaskScheduler.Application.Interfaces;

public interface IJobService
{
    Task<Guid> CreateJobAsync(JobDto request);
    Task<Job?> GetJobAsync(Guid id);
}
