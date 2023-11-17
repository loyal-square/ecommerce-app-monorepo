using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MonolithServer.Models;

[Index("Username", IsUnique = true)]
[Index("Email", IsUnique = true)]
public class Profile
{
    public int Id { get; set; }
    [Required]
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [Required]
    public string Email { get; set; }
    public string StoreIdArrayString { get; set; }
    [Required]
    public string UserType { get; set; }
    public string ProfileImgUrl { get; set; }
}