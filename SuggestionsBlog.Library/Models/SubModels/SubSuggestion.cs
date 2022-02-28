namespace SuggestionsBlog.Library.Models.SubModels;

public class SubSuggestion
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Title { get; set; }

    public SubSuggestion()
    {

    }

    public SubSuggestion(Suggestion sug)
    {
        Id = sug.Id;
        Title = sug.Title;
    }
}
