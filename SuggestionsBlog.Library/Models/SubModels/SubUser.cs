namespace SuggestionsBlog.Library.Models.SubModels;

public class SubUser
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string DisplayName { get; set; }

    public SubUser()
    {

    }

    public SubUser(User user)
    {
        Id = user.Id;
        DisplayName = user.DisplayName;
    }
}
