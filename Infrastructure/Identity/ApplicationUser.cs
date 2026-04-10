using Microsoft.AspNetCore.Identity;
using Infrastructure.Identity;

namespace Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string? ProfileImageUrl { get; set; }
    public string? Bio { get; set; }
}