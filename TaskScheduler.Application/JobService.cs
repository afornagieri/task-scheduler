using TaskScheduler.Application.Interfaces;
using TaskScheduler.Domain.Jobs;
using TaskScheduler.Domain.Request;
using TaskScheduler.Infra.MessageBroker.Interfaces;
using TaskScheduler.Infra.Repositories.Interfaces;

namespace TaskScheduler.Application;

public class JobService : IJobService
{
    private readonly IJobRepository _repository;
    private readonly IRabbitPublisher _publisher;

    public JobService(IJobRepository repository, IRabbitPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<Guid> CreateJobAsync(JobDto request)
    {
        var job = new Job
        {
            Id = Guid.NewGuid(),
            Type = request.Type,
            Payload = request.Payload,
            JobStatus = JobStatus.Awaiting,
            Attempts = 0,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(job);
#pragma warning disable CS8604 // Possible null reference argument.
        await _publisher.PublishAsync(job?.ToString());
#pragma warning restore CS8604 // Possible null reference argument.

        return job.Id;
    }

    public Task<Job?> GetJobAsync(Guid id)
    {
        return _repository.GetByIdAsync(id);
    }
}

