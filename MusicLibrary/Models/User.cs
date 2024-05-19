namespace MusicLibrary.Models;

public class User
{
    public User(string email, string password, UserType userType)
    {
        Email = email;
        Password = password;
        UserType = userType;
    }

    public User(int id, string email, string password, UserType userType)
    : this(email, password, userType)
    {
            Id = id;
    }

    public int Id { get; }
    public string Email { get; }
    public string Password { get; }
    public UserType UserType { get; }
}