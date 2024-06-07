using MusicLibrary.Db;
using MusicLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicLibrary.Controllers
{
    public class UserController
    {
        private static UserInventory _inventory = new UserInventory();

        public UserController()
        {
        }

        public User Login(string email, string pass)
        {
            var result = _inventory.ComparePasswords(email, pass);
            if (result) return _inventory.GetUserByEmail(email);
            else
            {
                Console.WriteLine("Invalid e-mail or password. Please try again.");
                Console.ReadKey();
            }
            return null;
        }

        public bool Registration(User user)
        {
            if (_inventory.UserExistByEmail(user.Email))
            {
                Console.WriteLine("There is already a user on the provided e-mail address");
                Console.ReadKey();
                return false;
            }
            else
            {
                _inventory.CreateNewUser(user);
                return true;
            }
        }

        public User LogOut()
        {
            return null;
        }

        public User EnterUserData()
        {
            var email = GetEmailFromUser("Enter email: ");
            var pass = GetPasswordFromUser("Enter password: ");

            return new User(email, pass);
        }
        public User EnterUserData(bool GetType)
        {
            string pass, pass2;
            var email = GetEmailFromUser("Enter email: ");
            do {
                pass = GetPasswordFromUser("Enter password: ");
                pass2 = GetPasswordFromUser("Enter password again: ");
                if (!pass.Equals(pass2)) Console.WriteLine("Passwords are not equal");
            } while((!pass.Equals(pass2)) || (pass == ""));
            var userType = GetUserTypeFromUser();

            return new User(email, pass, userType);
        }

            public User AddUser()
        {
            var user = EnterUserData(true);
            if (Registration(user)) return user;
            else return null;
        }

        private UserType GetUserTypeFromUser()
        {
            Console.WriteLine("Select user type by number:");
            var values = Enum.GetValues(typeof(UserType));
            foreach (UserType type in values)
            {
                Console.WriteLine($"[{(int)type}] - {type}");
            }
            string s = Console.ReadLine();
            int.TryParse(s, out int intType);
            return (UserType)intType;
        }

        private string GetPasswordFromUser(String text)
        {
            string userString = null;
            do
            {
                Console.WriteLine(text);
                userString = Console.ReadLine();
            } while (!isPasswordValid(userString));
            return userString;
        }

        private string GetEmailFromUser(String text)
        {
            string userString = null;
            do
            {
                Console.WriteLine(text);
                userString = Console.ReadLine();
            } while (!isValid(userString));
            return userString;
        }

        private static bool isValid(string email)
        {
            if (email != null)
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                if (match.Success)
                {
                    //Console.WriteLine(email + " is correct");
                    return true;
                }
            }
            Console.WriteLine(email + " is incorrect");
            return false;
        }

        private static bool isPasswordValid(string password)
        {
            if (password != null)
            {
                if (password.Length >= 5)
                {
                    //Console.WriteLine("password is correct");
                    return true;
                }
            }
            Console.WriteLine("password is incorrect. Too short, at least 5 characters needed");
            return false;
        }

        internal void ShowAllUsers()
        {
            foreach (var user in _inventory.GetAllUsers())
            {
                Console.WriteLine($"[{user.Id}] {user.Email} - /{user.UserType}/" );
            }
        }

        internal void Edit()
        {
            ShowAllUsers();
            Console.WriteLine("Choose user to edit (by Id):");
            int.TryParse(Console.ReadLine(), out var userId);
            var updatedUser = EnterUserData(true);
            _inventory.UpdateUser(updatedUser, userId);
        }

        internal void Remove()
        {
            ShowAllUsers();
            Console.WriteLine("Choose user to delete (by Id):");
            int.TryParse(Console.ReadLine(), out var userId);
            _inventory.DeleteById(userId);
        }
    }
}
