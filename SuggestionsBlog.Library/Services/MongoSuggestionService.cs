using Microsoft.Extensions.Caching.Memory;

namespace SuggestionsBlog.Library.Services;

public class MongoSuggestionService : ISuggestionService
{
    private readonly IDbConnection _db;
    private readonly IUserService _userService;
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<Suggestion> _suggestions;
    private const string _cacheKey = "Cache_AllSuggestions";

    public MongoSuggestionService(IDbConnection db, IUserService userService, IMemoryCache cache)
    {
        _db = db;
        _userService = userService;
        _cache = cache;
        _suggestions = db.SuggestionCollection;
    }

    public async Task<List<Suggestion>> GetAllSuggestions()
    {
        if (_cache.TryGetValue<List<Suggestion>>(_cacheKey, out var suggestions))
            return suggestions;

        suggestions = await _suggestions.Find(x => x.Archived == false).ToListAsync();

        _cache.Set(_cacheKey, suggestions, TimeSpan.FromMinutes(1));

        return suggestions;
    }

    public async Task<List<Suggestion>> GetAllApprovedSuggestions()
    {
        return (await GetAllSuggestions()).Where(x => x.ApprovedForRelease).ToList();
    }

    public async Task<List<Suggestion>> GetAllSuggestionsWaitingForApproval()
    {
        return (await GetAllSuggestions()).Where(x => x.ApprovedForRelease == false && x.Rejected == false).ToList();
    }

    public async Task<Suggestion> GetSuggestionById(string id)
    {
        return await _suggestions.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Suggestion>> GetSuggestionsByUserId(string userId)
    {
        if(!_cache.TryGetValue<List<Suggestion>>(userId, out var suggestions))
        {
            suggestions = await _suggestions.Find(x => x.Author.Id == userId).ToListAsync();

            _cache.Set(userId, suggestions, TimeSpan.FromMinutes(5));
        }

        return suggestions;
    }

    public async Task UpdateSuggestion(Suggestion suggestion)
    {
        await _suggestions.ReplaceOneAsync(x => x.Id == suggestion.Id, suggestion);
        _cache.Remove(_cacheKey);
    }

    public async Task VoteSuggestion(string suggestionId, string userId)
    {
        var client = _db.Client;

        using var session = await client.StartSessionAsync();

        try
        {
            session.StartTransaction();

            var db = client.GetDatabase(_db.DbName);
            var suggestions = db.GetCollection<Suggestion>(_db.SuggestionCollectionName);
            var suggestion = await suggestions.Find(x => x.Id == suggestionId).FirstAsync();

            bool upvoted = suggestion.UserVotes.Add(userId);

            if (!upvoted)
            {
                suggestion.UserVotes.Remove(userId);
            }

            await suggestions.ReplaceOneAsync(session, x => x.Id == suggestionId, suggestion);

            var users = db.GetCollection<User>(_db.UserCollectionName);
            var user = await _userService.GetUserById(userId);

            if (upvoted)
            {
                user.VotedSuggestions.Add(new SubSuggestion(suggestion));
            }
            else
            {
                user.VotedSuggestions.Remove(user.VotedSuggestions.First(x => x.Id == suggestionId));
            }

            await users.ReplaceOneAsync(session, x => x.Id == user.Id, user);

            await session.CommitTransactionAsync();

            _cache.Remove(_cacheKey);
        }
        catch (Exception)
        {
            await session.AbortTransactionAsync();
            throw;
        }
    }

    public async Task CreateSuggestion(Suggestion suggestion)
    {
        var client = _db.Client;

        using var session = await client.StartSessionAsync();

        try
        {
            session.StartTransaction();
            var db = client.GetDatabase(_db.DbName);
            var suggestions = db.GetCollection<Suggestion>(_db.SuggestionCollectionName);
            await suggestions.InsertOneAsync(session, suggestion);

            var users = db.GetCollection<User>(_db.UserCollectionName);
            var user = await _userService.GetUserById(suggestion.Author.Id);
            user.OwnedSuggestions.Add(new SubSuggestion(suggestion));

            await users.ReplaceOneAsync(x => x.Id == user.Id, user);

            await session.CommitTransactionAsync();
        }
        catch (Exception)
        {
            await session.AbortTransactionAsync();
            throw;
        }
    }
}