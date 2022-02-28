
namespace SuggestionsBlog.Library.Services;

public interface ICategoryService
{
    Task CreateCategory(Category category);
    Task<List<Category>> GetAllCategories();
}