using MusicLibrary.Db;
using MusicLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    }
}
