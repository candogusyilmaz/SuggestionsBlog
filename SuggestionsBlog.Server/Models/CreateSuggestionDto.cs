using System.ComponentModel.DataAnnotations;

namespace SuggestionsBlog.Server.Models;

public class CreateSuggestionDto
{
    [Required]
    [MaxLength(74)]
    public string Title { get; set; }

    [Required]
    [MinLength(1)]
    [Display(Name = "Category")]
    public string CategoryId { get; set; }
    
    [MaxLength(499)]
    public string Description { get; set; }
}
