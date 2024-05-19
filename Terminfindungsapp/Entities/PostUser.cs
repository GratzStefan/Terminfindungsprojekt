using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminfindungsapp
{
    class PostUser
    {
        public PostUser() { }
        public PostUser(string username, string password, string firstname, string lastname) 
        {
            this.username = username;
            this.password = password;
            this.firstname = firstname;
            this.lastname = lastname;
        }
        public string username { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
    }
}
