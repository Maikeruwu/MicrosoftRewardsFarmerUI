using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftRewardsFarmerUI {
    internal class Account {
        public string Username { get; set; }
        public string Password { get; set; }

        public Account(string username, string password) {
            Username = username;
            Password = password;
        }
    }
}
