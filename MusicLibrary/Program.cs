// See https://aka.ms/new-console-template for more information

using Microsoft.Data.SqlClient;
using MusicLibrary;
using MusicLibrary.Db;
using MusicLibrary.Models;

//var newUser = new User("ttt@2p.pl", "password1", UserType.Regular);
//var mySong = new Song("Tower", "Luna", "Non", 2024, newUser.Id);
Start();

static void Start()
{
    Console.WriteLine("Hello, SongLibrary!");
    CheckDbConnection();
    Menu menu = new Menu();
    menu.MainMenu();
}

static void CheckDbConnection()
{
    string _connectionString = "Server=ROG;Database=MusicLibrary;Trusted_Connection=True;";
    var sqlConnection = new SqlConnection(_connectionString);
    var flag = sqlConnection.StatisticsEnabled;
    if (flag) Console.WriteLine("Database connected...");
    Console.WriteLine("Problem with database connection...");
}