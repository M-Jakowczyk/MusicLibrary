using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibrary.Db
{
    public class SetupDb
    {
        private static string _connectionString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "_connectionString.txt");
        //private static string _connectionString = "Server=ROG; Database=MusicLibrary; Trusted_Connection=true;";

        public static void Start()
        {
            CheckDbConnection();
            PrepareDb();
        }
        private static void CheckDbConnection()
        {
            
            var sqlConnection = new SqlConnection(_connectionString);
            try
            {
                sqlConnection.Open();
                Console.WriteLine("Checked connection to the database...");
                sqlConnection.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem with database connection...");
            }
        }

        private static void PrepareDb()
        {
            Console.WriteLine("Preparing tables in the Database...");

            string query = "IF OBJECT_ID(N'Users', N'U') IS NULL " +
                "BEGIN PRINT 'Table NOT Exists' " +
                "CREATE TABLE Users " +
                "(Id int PRIMARY KEY IDENTITY, " +
                "Email nvarchar(255) NOT NULL, " +
                "Password nvarchar(255) NOT NULL, " +
                "Type int NOT NULL) " +
                "END;";
            string query2 = "IF OBJECT_ID(N'Songs', N'U') IS NULL " +
                "BEGIN  PRINT 'Table NOT Exists' " +
                "CREATE TABLE Songs " +
                "(Id int PRIMARY KEY IDENTITY, " +
                "Title nvarchar(255) NOT NULL, " +
                "Author nvarchar(255) NOT NULL, " +
                "Album nvarchar(255) NOT NULL, " +
                "[Year] int) " +
                "END;";
            string query3 = "IF OBJECT_ID(N'UserSongs', N'U') IS NULL " +
                "BEGIN  PRINT 'Table NOT Exists' " +
                "CREATE TABLE UserSongs " +
                "(UserId int NOT NULL, " +
                "SongId int NOT NULL, " +
                "PRIMARY KEY (UserId, SongId), " +
                "FOREIGN KEY (UserId) REFERENCES Users(Id), " +
                "FOREIGN KEY (SongId) REFERENCES Songs(Id)) " +
                "END;";
            ChangDb(query);
            ChangDb(query2);
            ChangDb(query3);
        }

        private static void LoadTestData()
        {
            Console.WriteLine("Would you like to load sample data into the database?");

        }

        private static void ChangDb(string query)
        {
            var sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();

            var sqlCommand = new SqlCommand(query, sqlConnection);

            sqlCommand.ExecuteNonQuery();

            sqlCommand.Dispose();
            sqlConnection.Dispose();
        }
    }
}
