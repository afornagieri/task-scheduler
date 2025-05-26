using MongoDB.Driver;
using TaskScheduler.Domain.Jobs;
using TaskScheduler.Domain.Settings;
using TaskScheduler.Infra.Repositories.Interfaces;

namespace TaskScheduler.Infra.Repositories;
public class JobRepository : IJobRepository
{
    private const string COLLECTION_NAME = "jobs";
    private readonly IMongoCollection<Job> collection;
    public JobRepository(IMongoClient mongoClient, AppSettings appSettings)
    {
        var db = mongoClient.GetDatabase(appSettings.DatabaseSettings?.DbName);
        collection = db.GetCollection<Job>(COLLECTION_NAME);
    }

    public async Task AddAsync(Job job)
    {
        try
        {
            await collection.InsertOneAsync(job);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Job?> GetByIdAsync(Guid id)
    {
        try
        {
            var filter = Builders<Job>.Filter.Eq("id", id);
            var document = await collection.FindAsync(filter).Result.FirstOrDefaultAsync();
            return document;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
