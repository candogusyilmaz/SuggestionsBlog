
using SuggestionsBlog.Library.Models;

namespace SuggestionsBlog.Library.Services;

public interface IUserService
{
    Task CreateUser(User user);
    Task<User> GetUserById(string id);
    Task<User> GetUserByObjectId(string objectIdentifier);
    Task<List<User>> GetUsers();
    Task UpdateUser(User user);
}