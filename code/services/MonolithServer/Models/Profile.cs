namespace MonolithServer.Models;

public class Profile
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string StoreIdArrayString { get; set; }
    public string UserType { get; set; }
    public string ProfileImgUrl { get; set; }
}