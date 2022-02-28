using Microsoft.Extensions.Caching.Memory;

namespace SuggestionsBlog.Library.Services;

public class MongoStatusService : IStatusService
{
    private readonly IMongoCollection<Status> _statuses;
    private readonly IMemoryCache _cache;
    private const string _cacheKey = "Cache_GetAllStatuses";

    public MongoStatusService(IDbConnection db, IMemoryCache cache)
    {
        _cache = cache;
        _statuses = db.StatusCollection;
    }

    public async Task<List<Status>> GetAllStatuses()
    {
        if (_cache.TryGetValue<List<Status>>(_cacheKey, out var statuses))
            return statuses;

        statuses = await _statuses.Find(_ => true).ToListAsync();

        _cache.Set(_cacheKey, statuses, TimeSpan.FromDays(1));

        return statuses;
    }

    public async Task CreateStatus(Status status)
    {
        await _statuses.InsertOneAsync(status);
    }
}
