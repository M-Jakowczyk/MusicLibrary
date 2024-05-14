namespace MusicLibrary.Models;

public class Song
{
    public Song(string title, string author, string album, int year)
    {
        Title = title;
        Author = author;
        Album = album;
        Year = year; 
    }
    
    public Song(int id, string title, string author, string album, int year)
    : this(title,author,album,year) {
        Id = id; 
    }
    
    public Song(string title, string author, string album, int year, int usersId)
        : this(title,author,album,year) {
        UsersId = usersId; 
    }

    public int Id { get; }
    public string Title { get; }
    public string Author { get; }
    public string Album { get; }
    public int Year { get; }
    public int UsersId { get; set; }
}