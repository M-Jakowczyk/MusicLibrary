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
        
        public User Login(string email, string pass)
        {
            var result = _inventory.ComparePasswords(email, pass);
            if (result) return _inventory.GetUserByEmail(email);
            return null;
        }

        public bool Registration(User user)
        {
            if (!_inventory.UserExistByEmail(user.Email))
            {
                _inventory.CreateNewUser(user);
                return true;
            }
            else
            {
                Console.WriteLine("There is already a user on the provided e-mail address");
                return false;
            }
        }

        public User EnterUserData()
        {
            Console.Write("Enter email: ");
            var email = GetEmailFromUser();
            Console.Write("Enter password: ");
            var pass = GetPasswordFromUser();

            return new User(email, pass);
        }
        public User EnterUserData(bool GetType)
        {
            Console.Write("Enter email: ");
            var email = GetEmailFromUser();
            Console.Write("Enter password: ");
            var pass = GetPasswordFromUser();
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

        private string GetPasswordFromUser()
        {
            string userString = null;
            do
            {
                userString = Console.ReadLine();
            } while (!isPasswordValid(userString));

            return userString;
        }

        private string GetEmailFromUser()
        {
            string userString = null;
            do
            {
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
                    Console.WriteLine(email + " is correct");
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
                if (password.Length >= 8)
                {
                    Console.WriteLine("password is correct");
                    return true;
                }
            }
            Console.WriteLine("password is incorrect. Too short, at least 8 characters needed");
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
            throw new NotImplementedException();
        }

        internal void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
