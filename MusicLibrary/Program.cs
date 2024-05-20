// See https://aka.ms/new-console-template for more information

using System.Data.SqlClient;
using MusicLibrary;
using MusicLibrary.Db;
using MusicLibrary.Models;

//var newUser = new User("ttt@2p.pl", "password1", UserType.Regular);
//var mySong = new Song("Tower", "Luna", "Non", 2024, newUser.Id);
Start();

static void Start()
{
    Console.WriteLine("Hello, SongLibrary!");
    SetupDb.Start();
    Menu menu = new Menu();
    menu.MainMenu();
}

