using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminfindungsapp.Entities
{
    class PostRequest
    {
        public PostRequest() { }
        public string id { get; set; }
        public User user{ get; set; }
        public PostOrganization org{ get; set; }
        public RequestStatus status{ get; set; }
    }
}
