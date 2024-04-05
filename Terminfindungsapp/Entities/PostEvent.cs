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
        public PostEvent(string titel, string description, string datetimestart, string datetimeend, string organizationid)
        {
            this.titel = titel;
            this.description = description;
            this.datetimestart = datetimestart;
            this.datetimeend = datetimeend;
            this.organizationid = organizationid;
            
        }
        public string id { get; set; }
        public string titel { get; set; }
        public string description { get; set; }
        public string datetimestart { get; set; }
        public string datetimeend { get; set; }
        public string organizationid { get; set; }

    }
}
