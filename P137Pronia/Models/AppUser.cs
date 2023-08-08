using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace P137Pronia.Models;

public class AppUser:IdentityUser
{
    [Required]
    public string Fullname { get; set; }
    public DateTime BirthDate { get; set; }
}
