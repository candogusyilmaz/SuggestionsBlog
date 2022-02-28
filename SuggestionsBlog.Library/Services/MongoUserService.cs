namespace SuggestionsBlog.Library.Services;

public class MongoUserService : IUserService
{
    private readonly IMongoCollection<User> _users;

    public MongoUserService(IDbConnection db)
    {
        _users = db.UserCollection;
    }

    public async Task<List<User>> GetUsers()
    {
        var results = await _users.FindAsync(_ => true);

        return results.ToList();
    }

    public async Task<User> GetUserById(string id)
    {
        var result = await _users.FindAsync(s => s.Id == id);

        return result.FirstOrDefault();
    }

    public async Task<User> GetUserByObjectId(string objectId)
    {
        return await _users.Find(s => s.ObjectIdentifier == objectId).FirstOrDefaultAsync();
    }

    public async Task CreateUser(User user)
    {
        await _users.InsertOneAsync(user);
    }

    public async Task UpdateUser(User user)
    {
        await _users.ReplaceOneAsync(x => x.Id == user.Id, user, options: new ReplaceOptions { IsUpsert = true });
    }
}
