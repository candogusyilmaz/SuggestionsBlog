using Microsoft.Extensions.Caching.Memory;

namespace SuggestionsBlog.Library.Services;

public class MongoCategoryService : ICategoryService
{
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<Category> _categories;
    private const string _cacheKey = "Cache_AllCategories";

    public MongoCategoryService(IDbConnection db, IMemoryCache cache)
    {
        _cache = cache;
        _categories = db.CategoryCollection;
    }

    public async Task<List<Category>> GetAllCategories()
    {
        if (_cache.TryGetValue<List<Category>>(_cacheKey, out var categories))
            return categories;

        categories = await _categories.Find(_ => true).ToListAsync();

        _cache.Set(_cacheKey, categories, TimeSpan.FromDays(1));

        return categories;
    }

    public async Task CreateCategory(Category category)
    {
        await _categories.InsertOneAsync(category);
    }
}
