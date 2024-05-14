// See https://aka.ms/new-console-template for more information

using MusicLibrary.Models;

var newUser = new User("ttt@2p.pl", "password1", UserType.Regular);
var mySong = new Song("Tower", "Luna", "Non", 2024, newUser.Id);
Console.WriteLine("Hello, World!");

static bool Login()
{
    return true;
}

static void Registration()
{
    
}

static void Start()
{
    
}