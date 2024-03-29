using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminfindungsapp
{
    class PostOrganization
    {
        public PostOrganization() { }
        public PostOrganization(string name)
        {
            this.name = name;
        }
        public string name { get; set; }
    }
}
