using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminfindungsapp.Entities
{
    class Request
    {
        public Request() { }
        public string id { get; set; }
        public User user{ get; set; }
        public Organization org{ get; set; }
        public RequestStatus status{ get; set; }
    }
}
