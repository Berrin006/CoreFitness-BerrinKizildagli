using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Account;

public class AccountDetailsViewModel
{
    [Required(ErrorMessage = "First name is required")]
    [Display(Name = "First name")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required")]
    [Display(Name = "Last name")]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    [Display(Name = "Email address")]
    public string Email { get; set; } = null!;

    [Display(Name = "Phone number")]
    public string? PhoneNumber { get; set; }

    public IEnumerable<Domain.Aggregates.GymClasses.GymClass> BookedClasses { get; set; } = new List<Domain.Aggregates.GymClasses.GymClass>();
}