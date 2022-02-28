using Microsoft.Extensions.Configuration;

using MongoDB.Driver;

using SuggestionsBlog.Library.Models;

namespace SuggestionsBlog.Library.DataAccess;

public class DbConnection : IDbConnection
{
    private readonly IConfiguration _config;
    private readonly IMongoDatabase _db;
    private const string _connectionId = "MongoDB";

    public string DbName { get; private set; }
    public string CategoryCollectionName { get; private set; } = "Categories";
    public string StatusCollectionName { get; private set; } = "Statuses";
    public string UserCollectionName { get; private set; } = "Users";
    public string SuggestionCollectionName { get; private set; } = "Suggestions";

    public MongoClient Client { get; private set; }
    public IMongoCollection<Category> CategoryCollection { get; private set; }
    public IMongoCollection<Status> StatusCollection { get; private set; }
    public IMongoCollection<User> UserCollection { get; private set; }
    public IMongoCollection<Suggestion> SuggestionCollection { get; private set; }

    public DbConnection(IConfiguration config)
    {
        _config = config;
        Client = new MongoClient(_config.GetConnectionString(_connectionId));
        DbName = _config["DatabaseName"];
        _db = Client.GetDatabase(DbName);

        CategoryCollection = _db.GetCollection<Category>(CategoryCollectionName);
        StatusCollection = _db.GetCollection<Status>(StatusCollectionName);
        UserCollection = _db.GetCollection<User>(UserCollectionName);
        SuggestionCollection = _db.GetCollection<Suggestion>(SuggestionCollectionName);
    }
}
