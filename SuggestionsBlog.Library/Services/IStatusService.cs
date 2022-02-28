
namespace SuggestionsBlog.Library.Services;

public interface IStatusService
{
    Task CreateStatus(Status status);
    Task<List<Status>> GetAllStatuses();
}