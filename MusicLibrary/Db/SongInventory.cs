using MusicLibrary.Models;
using System.Data.SqlClient;

namespace MusicLibrary.Db;

public class SongInventory
{
    private string _connectionString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "_connectionString.txt");
    //private string _connectionString = "Server=ROG;Database=MusicLibrary;Trusted_Connection=True;";

    public int AddNewSongToDb(Song song)
    {
        var query = $"Insert into Songs values('{song.Title}', '{song.Author}', '{song.Album}', {song.Year})";
        ChangDb(query);
        return GetSongID(song);
    }

    private int GetSongID(Song song)
    {
        var query = $"SELECT * FROM Songs WHERE (Title = '{song.Title}' AND Author = '{song.Author}' AND Album = '{song.Album}' AND Year = {song.Year} )";
        return SelectFromDb(query)[0].Id;
    }
    public Song GetSongByID(int id)
    {
        var query = $"SELECT * FROM Songs WHERE ID = {id}";
        return SelectFromDb(query)[0];
    }
    public void DeleteSongByID(int songID)
    {
        var query = $"DELETE FROM Songs WHERE ID = {songID};";
        ChangDb(query);
    }

    public void UpdateSong(Song song, int songID)
    {
        var query = $"UPDATE Users SET Title = '{song.Title}', Author = '{song.Author}', Album = '{song.Album}', Year = {song.Year} WHERE ID = songID; ";
        ChangDb(query);
    }

    public Song[] GetAllSongs()
    {
        var query = "Select * from Songs";
        var result = SelectFromDb(query);

        return result.ToArray();
    }

    public Song[] GetSongsByAuthor(string authorInDb)
    {
        var query = $"Select * from Songs where Author = '{authorInDb}'";
        var result = SelectFromDb(query);

        return result.ToArray();
    }

    internal Song[] GetSongsByUser(int[] songIDs)
    {
        var query = $"SELECT * FROM Songs WHERE ID IN '{songIDs}' ";
        var result = SelectFromDb(query);
        return result.ToArray();
    }

    private void ChangDb(string query)
        {
            var sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();

            var sqlCommand = new SqlCommand(query, sqlConnection);

            sqlCommand.ExecuteNonQuery();

            sqlCommand.Dispose();
            sqlConnection.Dispose();
        }
    private Song[] SelectFromDb(string query)
    {
        var result = new Song[0];
        var sqlConnection = new SqlConnection(_connectionString);
        sqlConnection.Open();

        var sqlCommand = new SqlCommand(query, sqlConnection);

        var reader = sqlCommand.ExecuteReader();

        while (reader.Read())
        {
            var id = reader["Id"].ToString();
            var title = reader["Title"].ToString();
            var year = reader["Year"].ToString();
            var author = reader["Author"].ToString();

            var song = new Song(int.Parse(id), title, author, "", int.Parse(year));

            Array.Resize(ref result, result.Length + 1);
            result[result.Length - 1] = song;
        }

        sqlCommand.Dispose();
        sqlConnection.Dispose();

        return result;
    }
}

