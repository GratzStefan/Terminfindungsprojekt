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
        public PostEvent(string titel, string description, DateTime datetimestart, DateTime datetimeend, string organizationid)
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
        public DateTime datetimestart { get; set; }
        public DateTime datetimeend { get; set; }
        public string organizationid { get; set; }

    }
}
