namespace MusicLibrary.Models;

public class User
{
    public User(string email, string password, UserType userType)
    {
        Email = email;
        Password = password;
        UserType = userType;
    }

    public int Id { get; }
    public string Email { get; }
    public string Password { get; }
    public UserType UserType { get; }
}