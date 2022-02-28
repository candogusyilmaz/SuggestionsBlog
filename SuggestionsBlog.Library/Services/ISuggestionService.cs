

namespace SuggestionsBlog.Library.Services;

public interface ISuggestionService
{
    Task CreateSuggestion(Suggestion suggestion);
    Task<List<Suggestion>> GetAllApprovedSuggestions();
    Task<List<Suggestion>> GetAllSuggestions();
    Task<List<Suggestion>> GetAllSuggestionsWaitingForApproval();
    Task<Suggestion> GetSuggestionById(string id);
    Task<List<Suggestion>> GetSuggestionsByUserId(string userId);
    Task UpdateSuggestion(Suggestion suggestion);
    Task VoteSuggestion(string suggestionId, string userId);
}