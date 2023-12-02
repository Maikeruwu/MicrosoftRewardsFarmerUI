using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftRewardsFarmerUI.model
{
    internal class Account
    {
        // These fields need to be lowercase for JSON serialization, sorry for the inconsistency
        public string username { get; set; }
        public string password { get; set; }

        public Account(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
