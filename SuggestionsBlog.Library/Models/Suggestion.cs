namespace SuggestionsBlog.Library.Models;

public class Suggestion
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Title { get; set; }
    public SubUser Author { get; set; }
    public string Description { get; set; }
    public string OwnerNotes { get; set; }
    public Category Category { get; set; }
    public Status Status { get; set; }

    public bool ApprovedForRelease { get; set; } = false;
    public bool Archived { get; set; } = false;
    public bool Rejected { get; set; } = false;

    public HashSet<string> UserVotes { get; set; } = new();
    public DateTime DateCreated { get; init; } = DateTime.UtcNow;
}
