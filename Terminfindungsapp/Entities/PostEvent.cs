using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminfindungsapp.Entities
{
    public class PostEvent
    {
        public PostEvent() { }
        public PostEvent(string titel, string description, string datetime, string organizationid)
        {
            this.titel = titel;
            this.description = description;
            this.datetime = datetime;
            this.organizationid = organizationid;
            
        }
        public string id { get; set; }
        public string titel { get; set; }

        public string description { get; set; }
        public string datetime { get; set; }
        public string organizationid { get; set; }

    }
}
