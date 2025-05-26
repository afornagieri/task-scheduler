using TaskScheduler.Domain.Jobs;

namespace TaskScheduler.Infra.Repositories.Interfaces;

public interface IJobRepository
{
    /// <summary>
    /// Retrieve a document given an valid id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<Job?> GetByIdAsync(Guid id);

    /// <summary>
    /// Store a document
    /// </summary>
    /// <param name="job"></param>
    /// <returns></returns>
    public Task AddAsync(Job job);
}
