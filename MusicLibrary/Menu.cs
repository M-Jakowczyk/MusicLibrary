using MusicLibrary.Controllers;
using MusicLibrary.Db;
using MusicLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Console;

namespace MusicLibrary
{
    public class Menu
    {
        private User _loggedUser;
       // private string[] options;
        private static UserController _userController = new UserController();
        private static SongController _songController = new SongController();

        
        public void MainMenu()
        {
            string title = "Main menu";
            string[] options = { "Log in", "Sign up", "Exit" };
            var userOption = ShowOptions(title, options);
            DoOptions(userOption);
        }

        public void UserMenu() 
        {
            int userOption;
            string title = "User menu";
            string[] options = { "Get all songs", "Add new song", "Edit song", "Delete song", "Log out" };

            if (_loggedUser.UserType == UserType.Admin)
            {
                AdminMenu();
            }
            else
            {
                userOption = ShowOptions(title, options);
                SongModify(userOption);
            }
        }

        private void AdminMenu()
        {
            string title = "Admin menu";
            string[] options = { "Song modify", "User modify", "Log out" };
            var userOption = ShowOptions(title, options);
            Console.Clear();
            if (userOption == 1)
            {
                string[] songOptions = { "Get all songs", "Add new song", "Edit song", "Delete song", "Return" };
                userOption = ShowOptions("Song modify", songOptions);
                SongModify(userOption);
            }
            else if (userOption == 2)
            {
                string[] userOptions = { "Get all users", "Add new user", "Edit user", "Delete user", "Return" };
                userOption = ShowOptions("User modify", userOptions);
                UserModify(userOption);
            }
            else
            {
                _loggedUser = null;
                if (ReturnToMenu()) MainMenu();
                return;
            }
        }

        private int ShowOptions(string nameMenu, string[] options)
        {
            int userOption;
            WriteLine($"---[ {nameMenu} ]---");
            if (_loggedUser != null) { WriteLine($"Current user login [{_loggedUser.Email}]"); }
            do
            {
                int i = 1;
                foreach (string name in options)
                {
                    WriteLine($"{i}. {name}");
                    i++;
                }
                WriteLine($"Enter a value (from 1 to {options.Length}): ");
                int.TryParse(ReadLine(), out userOption);

            } while (userOption > options.Length | userOption <= 0);
            return userOption;
        }

        private void SongModify(int userOption)
        {
           
            switch (userOption)
            {
                case 1:
                    WriteLine("All your songs: ");
                    _songController.ShowSongs(_loggedUser);
                    Console.ReadKey();
                    Console.Clear();
                    UserMenu();
                    return;
                case 2:
                    WriteLine("Add new song: ");
                    _songController.AddNewSong(_loggedUser);
                    // Console.ReadKey();
                    Console.Clear();
                    UserMenu();
                    return;
                case 3:
                    WriteLine("Edit song: ");
                    _songController.EditSong(_loggedUser);
                    // Console.ReadKey();
                    Console.Clear();
                    UserMenu();
                    return;
                case 4:
                    WriteLine("Delete song: ");
                    _songController.RemoveSong(_loggedUser);
                    // Console.ReadKey();
                    Console.Clear();
                    UserMenu();
                    return;
                case 5:
                    Console.Clear();
                    if(_loggedUser.UserType == UserType.Admin) AdminMenu();
                    else 
                    {
                        _loggedUser = null;
                        if (ReturnToMenu()) MainMenu();
                    }
                    return;
                default:
                    WriteLine("No such option"); break;
            }
            if (userOption != 5)
            {
                if (ReturnToMenu()) 
                {
                    if (_loggedUser.UserType == UserType.Admin) AdminMenu();
                    UserMenu();
                }
            }
        }

        private void UserModify(int userOption)
        {
           switch (userOption)
            {
                case 1:
                    WriteLine("All users list: ");
                    _userController.ShowAllUsers();
                    Console.ReadKey();
                    Console.Clear();
                    UserMenu();
                    return;
                case 2:
                    WriteLine("Add new user: ");
                    _userController.AddUser();
                    // Console.ReadKey();
                    Console.Clear();
                    UserMenu();
                    return;
                case 3:
                    WriteLine("Edit user: ");
                    _userController.Edit();
                    // Console.ReadKey();
                    Console.Clear();
                    UserMenu();
                    return;
                case 4:
                    WriteLine("Delete user: ");
                    _userController.Remove();
                    // Console.ReadKey();
                    Console.Clear();
                    UserMenu();
                    return;
                case 5:
                    Console.Clear();
                    AdminMenu();
                    return;
                default:
                    WriteLine("No such option"); break;
            }
            if (userOption != 5)
            {
                if (ReturnToMenu()) AdminMenu();
            }
        }

        public void DoOptions(int userOption)
        {
            Console.Clear();
            switch (userOption)
            {
                case 1:
                    WriteLine("Log in: ");
                    var user = _userController.EnterUserData();
                    _loggedUser = _userController.Login(user.Email, user.Password);
                    Console.Clear();
                    if (_loggedUser != null) UserMenu();
                    else MainMenu();
                    return;
                case 2:
                    WriteLine("Registration: ");
                    var newUser = _userController.AddUser();
                    _loggedUser = _userController.Login(newUser.Email, newUser.Password);
                    Console.Clear();
                    if (_loggedUser != null) UserMenu();
                    else MainMenu();
                    return;
                case 3:
                    Console.WriteLine("Goodbye!");
                    Console.ReadKey();
                    return;
                default:
                    WriteLine("No such option"); break;
            }
            if (userOption != 3)
            {
                if (ReturnToMenu()) MainMenu();
            }
        }

        

        private static bool ReturnToMenu()
        {
            string YorN;
            do
            {
                WriteLine("Do you want to return to the menu? (Y/N):");
                YorN = ReadLine();
            } while (!(YorN.ToLower() == "y" | YorN.ToLower() == "n"));
            return (YorN.ToLower() == "y");
        }

        
    }

}
