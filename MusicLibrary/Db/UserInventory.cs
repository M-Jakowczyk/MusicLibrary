using System.Data.SqlClient;
using MusicLibrary.Models;

namespace MusicLibrary.Db;

public class UserInventory
{

    private string _connectionString = "Server=ROG;Database=MusicLibrary;Trusted_Connection=True;";
    public void CreateNewUser(User user)
    {
        var query = $"INSERT INTO Users VALUES('{user.Email}', '{user.Password}', '{(int)user.UserType}')";
        ChangDb(query);
    }

    public void DeleteByEmail(string email)
    {
        var query = $"DELETE FROM Users WHERE Email = '{email}';";
        ChangDb(query);
    }

    public void UpdateUser(User user, int userId)
    {
        var query = $"UPDATE Users SET Email = {user.Email}, Password = {user.Password}, Type = {user.UserType} WHERE ID = userId; ";
        ChangDb(query);
    }

    public User[] GetAllUsers()
    {
        var query = "SELECT * FROM Users";
        var result = SelectFromDb(query);

        return result;
    }

    public User GetUserByEmail(string email)
    {
        var query = $"SELECT * FROM Users WHERE Email = '{email}'";
        var result = SelectFromDb(query);

        if (result.Length > 0) return result[0];
        else return null;
    }

    public bool ComparePasswords(string email, string inputPass)
    {
        var user = GetUserByEmail(email);
        if (user != null) return user.Password == inputPass;
        else return false;
    }

    public bool UserExistByEmail(string email)
    {
        var query = $"SELECT * FROM Users WHERE Email = '{email}'";
        var result = SelectFromDb(query);

        return result.Length != 0;
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

    private User[] SelectFromDb(string query)
    {
        var result = new User[0];
        var sqlConnection = new SqlConnection(_connectionString);
        sqlConnection.Open();

        var sqlCommand = new SqlCommand(query, sqlConnection);

        var reader = sqlCommand.ExecuteReader();

        while (reader.Read())
        {
            var id = reader["ID"].ToString();
            var email = reader["Email"].ToString();
            var password = reader["Password"].ToString();
            var userType = reader["Type"].ToString();

            var user = new User(int.Parse(id), email, password, (UserType)Enum.ToObject(typeof(UserType), int.Parse(userType)));
            Array.Resize(ref result, result.Length + 1);
            result[result.Length - 1] = user;
        }

        sqlCommand.Dispose();
        sqlConnection.Dispose();

        return result;
    } 
}