using MusicLibrary.Models;
using System.Data;
using System.Data.SqlClient;

namespace MusicLibrary.Db;

public class SongInventory
{
    private string _connectionString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "_connectionString.txt");
    //private string _connectionString = "Server=ROG;Database=MusicLibrary;Trusted_Connection=True;";

    public int AddNewSongToDb(Song song)
    {
        var query = "Insert into Songs values(@Title, @Author, @Album, @Year)";
        var parameters = new[]
        {
            new SqlParameter("@Title", SqlDbType.VarChar) { Value = song.Title },
            new SqlParameter("@Author", SqlDbType.VarChar) { Value = song.Author },
            new SqlParameter("@Album", SqlDbType.VarChar) { Value = song.Album },
            new SqlParameter("@Year", SqlDbType.Int) { Value = song.Year }
        };
        ChangeDb(query, parameters);
        return GetSongID(song);
    }

    private int GetSongID(Song song)
    {
        var query = "SELECT * FROM Songs WHERE (Title = @Title AND Author = @Author AND Album = @Album AND Year = @Year )";
        var parameters = new[]
        {
            new SqlParameter("@Title", SqlDbType.VarChar) { Value = song.Title },
            new SqlParameter("@Author", SqlDbType.VarChar) { Value = song.Author },
            new SqlParameter("@Album", SqlDbType.VarChar) { Value = song.Album },
            new SqlParameter("@Year", SqlDbType.Int) { Value = song.Year }
        };
        return SelectFromDb(query, parameters)[0].Id;
    }
    public Song GetSongByID(int id)
    {
        var query = "SELECT * FROM Songs WHERE ID = @Id";
        var parameters = new[]
        {
            new SqlParameter("@Id", SqlDbType.VarChar) { Value = id}
        };
        return SelectFromDb(query, parameters)[0];
    }
    public void DeleteSongByID(int songID)
    {
        var query = $"DELETE FROM Songs WHERE ID = @songID;";
        var parameters = new[]
        {
            new SqlParameter("@songID", SqlDbType.VarChar) { Value = songID}
        };
        ChangeDb(query, parameters);
    }

    public void UpdateSong(Song song, int songID)
    {
        var query = $"UPDATE Users SET Title = @Title, Author = @Author, Album = @Album, Year = @Year WHERE Id = @songID; ";
        var parameters = new[]
        {
            new SqlParameter("@Title", SqlDbType.VarChar) { Value = song.Title },
            new SqlParameter("@Author", SqlDbType.VarChar) { Value = song.Author },
            new SqlParameter("@Album", SqlDbType.VarChar) { Value = song.Album },
            new SqlParameter("@Year", SqlDbType.Int) { Value = song.Year },
            new SqlParameter("@songID", SqlDbType.Int) { Value = songID }
        };
        ChangeDb(query, parameters);
    }

    public Song[] GetAllSongs()
    {
        var query = "Select * from Songs";
        var result = SelectFromDb(query,new SqlParameter[]{});

        return result.ToArray();
    }

    public Song[] GetSongsByUser(int userId)
    {
        string query = @"
                        SELECT Songs.Id, Songs.Title, Songs.Author, Songs.Album, Songs.Year
                        FROM Songs
                        INNER JOIN UserSongs ON Songs.Id = UserSongs.SongId
                        WHERE UserSongs.UserId = @UserId";
        var parameters = new[]
        {
            new SqlParameter("@UserId", SqlDbType.Int) { Value = userId }
        };
        return SelectFromDb(query, parameters);
    }

    public void AssignSongToUser(int userId, int songId)
    {
        string query = "INSERT INTO UserSongs (UserId, SongId) VALUES (@UserId, @SongId)";
        var parameters = new[]
        {
            new SqlParameter("@UserId", SqlDbType.Int) { Value = userId },
            new SqlParameter("@SongId", SqlDbType.Int) { Value = songId }
        };
        ChangeDb(query, parameters);
    }

    // public Song[] GetSongsByAuthor(string authorInDb)
    // {
    //     var query = "Select * from Songs where Author = @authorInDb";
    //     var parameters = new[]
    //     {
    //         new SqlParameter("@authorInDb", SqlDbType.VarChar) { Value = authorInDb}
    //     };
    //     var result = SelectFromDb(query, parameters);
    //
    //     return result.ToArray();
    // }

    internal Song[] GetSongsByUser(int[] songIDs)
    {
        var query = $"SELECT * FROM Songs WHERE ID IN @songIDs ";
        var parameters = new[]
        {
            new SqlParameter("@songIDs", SqlDbType.VarChar) { Value = songIDs}
        };
        var result = SelectFromDb(query, parameters);
        return result.ToArray();
    }

    private void ChangeDb(string query, SqlParameter[] parameters)
        {
            var sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();

            var sqlCommand = new SqlCommand(query, sqlConnection);
            if (parameters != null)
            {
                sqlCommand.Parameters.AddRange(parameters);
            }
            sqlCommand.ExecuteNonQuery();

            sqlCommand.Dispose();
            sqlConnection.Dispose();
        }
    private Song[] SelectFromDb(string query, SqlParameter[] parameters)
    {
        var result = new Song[0];
        var sqlConnection = new SqlConnection(_connectionString);
        sqlConnection.Open();

        var sqlCommand = new SqlCommand(query, sqlConnection);
        if (parameters != null)
        {
            sqlCommand.Parameters.AddRange(parameters);
        }

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

