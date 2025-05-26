using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Driver;
using TaskScheduler.Domain.Jobs;
using TaskScheduler.Domain.Settings;
using TaskScheduler.Infra.Repositories.Interfaces;

namespace TaskScheduler.Infra.Repositories;

public class JobRepository : IJobRepository
{
    private const string COLLECTION_NAME = "Jobs";
    private const string FIELD_ID = "Id";
    private const string FIELD_TYPE = "Type";
    private const string FIELD_PAYLOAD = "Payload";
    private const string FIELD_STATUS = "JobStatus";
    private const string FIELD_ATTEMPTS = "Attempts";
    private const string FIELD_CREATED_AT = "CreatedAt";

    private readonly IMongoCollection<BsonDocument> collection;

    public JobRepository(IMongoClient mongoClient, AppSettings appSettings)
    {
        var db = mongoClient.GetDatabase(appSettings.DatabaseSettings?.DbName);
        collection = db.GetCollection<BsonDocument>(COLLECTION_NAME);
    }

    public async Task AddAsync(Job job)
    {
        if (job.Payload is null) job.Payload = new Dictionary<string, object>();

        var payloadJson = JsonSerializer.Serialize(job.Payload);
        var payloadBson = BsonDocument.Parse(payloadJson);

        var document = new BsonDocument
        {
            { FIELD_ID, job.Id.ToString() },
            { FIELD_TYPE, job.Type },
            { FIELD_PAYLOAD, payloadBson.ToString() },
            { FIELD_STATUS, job.JobStatus.ToString() },
            { FIELD_ATTEMPTS, job.Attempts },
            { FIELD_CREATED_AT, job.CreatedAt }
        };

        await collection.InsertOneAsync(document);
    }

    public async Task<Job?> GetByIdAsync(Guid id)
    {
        var filter = Builders<BsonDocument>.Filter.Eq(FIELD_ID, id.ToString());
        var document = await collection.Find(filter).FirstOrDefaultAsync();

        if (document is null) return null;

        var payloadString = document[FIELD_PAYLOAD].AsString;

        var payload = JsonSerializer.Deserialize<Dictionary<string, object>>(payloadString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return new Job
        {
            Id = Guid.Parse(document[FIELD_ID].AsString),
            Type = document[FIELD_TYPE].AsString,
            Payload = payload!,
            JobStatus = Enum.Parse<JobStatus>(document[FIELD_STATUS].AsString),
            Attempts = document[FIELD_ATTEMPTS].AsInt32,
            CreatedAt = document[FIELD_CREATED_AT].ToUniversalTime()
        };
    }
}
