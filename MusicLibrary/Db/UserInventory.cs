using System.Data;
using System.Data.SqlClient;
using MusicLibrary.Models;

namespace MusicLibrary.Db;

public class UserInventory
{
    private string _connectionString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "_connectionString.txt");
    //private string _connectionString = "Server=ROG;Database=MusicLibrary;Trusted_Connection=True;";
    public void CreateNewUser(User user)
    {
        var query = "INSERT INTO Users VALUES(@Email, @Password, @UserType)";
        var parameters = new[]
        {
            new SqlParameter("@Email", SqlDbType.NVarChar) { Value = user.Email },
            new SqlParameter("@Password", SqlDbType.NVarChar) { Value = user.Password },
            new SqlParameter("@UserType", SqlDbType.Int) { Value = user.UserType }
        };
        ChangDb(query, parameters);
    }

    public void DeleteByEmail(string Email)
    {
        var query = "DELETE FROM Users WHERE Email = @Email";
        var parameters = new[]
        {
            new SqlParameter("@Email", SqlDbType.NVarChar) { Value = Email }
        };
        ChangDb(query, parameters);
    }
    
    public void DeleteById(int userId)
    {
        var query = "DELETE FROM Users WHERE Id = @userId";
        var parameters = new[]
        {
            new SqlParameter("@userId", SqlDbType.NVarChar) { Value = userId }
        };
        ChangDb(query, parameters);
    }

    public void UpdateUser(User user, int userId)
    {
        var query = "UPDATE Users SET Email = @Email, Password = @Password, Type = @UserType WHERE Id = @userId; ";
        var parameters = new[]
        {
            new SqlParameter("@Email", SqlDbType.NVarChar) { Value = user.Email },
            new SqlParameter("@Password", SqlDbType.NVarChar) { Value = user.Password },
            new SqlParameter("@UserType", SqlDbType.Int) { Value = user.UserType },
            new SqlParameter("@userId", SqlDbType.Int) { Value = userId }
        };
        ChangDb(query, parameters);
    }

    public User[] GetAllUsers()
    {
        var query = "SELECT * FROM Users";
        var result = SelectFromDb(query, Array.Empty<SqlParameter>());

        return result;
    }

    public User GetUserByEmail(string email)
    {
        var query = "SELECT * FROM Users WHERE Email = @Email";
        var parameters = new[]
        {
            new SqlParameter("@Email", SqlDbType.NVarChar) { Value = email }
        };
        var result = SelectFromDb(query, parameters);
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
        var query = "SELECT * FROM Users WHERE Email = @email";
        var parameters = new[]
        {
            new SqlParameter("@email", SqlDbType.VarChar) { Value = email },
        };
        var result = SelectFromDb(query, parameters);

        return result.Length != 0;
    }

    private void ChangDb(string query, SqlParameter[] parameters)
    {
        var sqlConnection = new SqlConnection(_connectionString);
        sqlConnection.Open();

        var sqlCommand = new SqlCommand(query, sqlConnection);
        if (parameters.Length > 0)
        {
            sqlCommand.Parameters.AddRange(parameters);
        }
        sqlCommand.ExecuteNonQuery();

        sqlCommand.Dispose();
        sqlConnection.Dispose();
    }

    private User[] SelectFromDb(string query, SqlParameter[] parameters)
    {
        var result = new User[0];
        var sqlConnection = new SqlConnection(_connectionString);
        sqlConnection.Open();

        var sqlCommand = new SqlCommand(query, sqlConnection);
        if (parameters.Length > 0)
        {
            sqlCommand.Parameters.AddRange(parameters);
        }
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