using Microsoft.Data.SqlClient;
using MusicLibrary.Models;

namespace MusicLibrary.Db;

public class SongInventory
{
    private string _connectionString = "Server=.;Database=Test;Trusted_Connection=True;";
    
    public void AddNewSongToDb(Song song)
        {
            //Insert into Song values('Title 1', 2024, 'Author 1')
            var query = $"Insert into Song values('{song.Title}', {song.Year}, '{song.Author}')";

            ChangDb(query);
        }

        public void DeleteSongByTitle(string title)
        {
            //Insert into Song values('Title 1', 2024, 'Author 1')
            var query = $"DELETE FROM Song WHERE Title = '{title}';";

            ChangDb(query);
        }

        public Song[] GetAllSongs()
        {
            var result = new Song[0];
            //var result = new List<Song>();

            var query = "Select * from Song";

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
                //result.Add(song);
            }

            sqlCommand.Dispose();
            sqlConnection.Dispose();

            return result.ToArray();
        }

        public Song[] GetSongsByAuthor(string authorInDb)
        {
            var result = new Song[0];
            //var result = new List<Song>();

            var query = $"Select * from Song where Author = '{authorInDb}'";

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

                var song = new Song(int.Parse(id), title, author, "", int.Parse(year) );

                Array.Resize(ref result, result.Length + 1);
                result[result.Length - 1] = song;
                //result.Add(song);
            }

            sqlCommand.Dispose();
            sqlConnection.Dispose();

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
}

