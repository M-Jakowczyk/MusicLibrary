using Microsoft.Data.SqlClient;
using MusicLibrary.Models;

namespace MusicLibrary.Db;

public class UserInventory
{

    private string _connectionString = "Server=.;Database=Test;Trusted_Connection=True;";
    public void CreateNewUser(User user)
    {
        var query = $"INSERT INTO User VALUES('{user.Email}', '{user.Password}', '{user.UserType}')";
        ChangDb(query);
    }

    public void DeleteByEmail(string email)
    {
        var query = $"DELETE FROM User WHERE Title = '{email}';";
        ChangDb(query);
    }

    public void UpdateUser(User user, int userId)
    {
        var query = $"UPDATE User SET Email = {user.Email}, Password = {user.Password}, UserType = {user.UserType} WHERE Id = userId; ";
        ChangDb(query);
    }

    public User[] GetAllUsers()
    {
        var query = "SELECT * FROM User";
        var result = SelectFromDb(query);

        return result;
    }

    public User GetUserByEmail(string email)
    {
        var query = $"SELECT * FROM User WHERE Email = '{email}'";
        var result = SelectFromDb(query);

        return result[0];
    }

    public bool ComparePasswords(string email, string inputPass)
    {
        var user = GetUserByEmail(email);
        return user.Password == inputPass;
    }

    public bool UserExistByEmail(string email)
    {
        var query = $"SELECT * FROM User WHERE Email = '{email}'";
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
            var id = reader["Id"].ToString();
            var email = reader["Email"].ToString();
            var password = reader["Password"].ToString();
            var userType = reader["UserType"].ToString();

            var user = new User(int.Parse(id), email, password, (UserType)Enum.Parse(typeof(UserType), userType, true));
            Array.Resize(ref result, result.Length + 1);
            result[result.Length - 1] = user;
        }

        sqlCommand.Dispose();
        sqlConnection.Dispose();

        return result;
    } 
}