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
            string title = "User menu";
            string[] options = { "Get all songs", "Add new song", "Edit song", "Find songs", "Log out" };
            if (_loggedUser.UserType == UserType.Admin) 
                Array.Copy(options, new string[]{ "Get all users", "Add new user", "Edit user", "Delete user", "Find user", "Log out" }, options.Length);
            var userOption = ShowOptions(title, options);
            UserOptions(userOption);
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
                userOption = int.Parse(ReadLine());

            } while (userOption > options.Length || userOption <= 0);
            return userOption;
        }

        private void UserOptions(int userOption)
        {
           
            switch (userOption)
            {
                case 1:
                    WriteLine("All your songs: ");
                    _songController.ShowAllSongs();
                    Console.ReadKey();
                    Console.Clear();
                    UserMenu();
                    break;
                case 2:
                    WriteLine("Add new song: ");
                    Console.ReadKey();
                    Console.Clear();
                    UserMenu();
                    break;
                case 3:
                    break;
                default:
                    WriteLine("No such option"); break;
            }
            if (userOption != 3)
            {
                if (ReturnToMenu()) MainMenu();
            }
        }

        public void DoOptions(int userOption)
        {
            switch (userOption)
            {
                case 1:
                    WriteLine("You choose \"1\"");
                    var email = "";
                    var pass = "";
                    do
                    {
                        Write("Enter your email: ");
                        email = GetEmailFromUser();
                        Write("Enter your password: ");
                        pass = GetPasswordFromUser();
                        _loggedUser = _userController.Login(email, pass);
                    } while (_loggedUser == null);
                    Console.Clear();
                    UserMenu();
                    break;
                case 2:
                    WriteLine("You choose \"2\"");
                    var flag = true;
                    do
                    {
                        Write("Enter your email: ");
                        email = GetEmailFromUser();
                        Write("Enter your password: ");
                        pass = GetPasswordFromUser();
                        var userType = GetUserTypeFromUser();
                        _loggedUser = new User(email, pass, userType);
                        flag = !_userController.Registration(_loggedUser);
                    } while (flag);
                    Console.Clear();
                    UserMenu();
                    break;
                case 3:
                    break;
                default:
                    WriteLine("No such option"); break;
            }
            if (userOption != 3)
            {
                if (ReturnToMenu()) MainMenu();
            }
        }

        private UserType GetUserTypeFromUser()
        {
            WriteLine("Select user type by number:");
            var values = Enum.GetValues(typeof(UserType));
            foreach (UserType type in values)
            {
                WriteLine($"[{(int)type}] - {type}");
            }
            string s = ReadLine();
            int.TryParse(s, out int intType);
            return (UserType)intType;
        }

        private string GetPasswordFromUser()
        {
            string userString = null;
            do
            {
                userString = ReadLine();
            } while (!isPasswordValid(userString));

            return userString;
        }

        private string GetEmailFromUser()
        {
            string userString = null;
            do
            {
                userString = ReadLine();
            } while (!isValid(userString));
            
            return userString;
        }

        private static bool ReturnToMenu()
        {
            string YorN;
            do
            {
                WriteLine("Want to return to the main menu? (Y/N):");
                YorN = ReadLine();
            } while (!(YorN.ToLower() == "y" || YorN.ToLower() == "n"));
            return (YorN.ToLower() == "y");
        }

        private static bool isValid(string email)
        {
            if (email != null)
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                if (match.Success)
                {
                    WriteLine(email + " is correct");
                    return true;
                }
            }
            WriteLine(email + " is incorrect");
            return false;
        }

        private static bool isPasswordValid(string password)
        {
            if (password != null)
            {
                if (password.Length >= 8)
                {
                    WriteLine("password is correct");
                    return true;
                }
            }
            WriteLine("password is incorrect. Too short, at least 8 characters needed");
            return false;
        }
    }

}
