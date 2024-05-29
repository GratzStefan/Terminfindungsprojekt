using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminfindungsapp.Entities
{
    class GroupedEvent
    {
        public GroupedEvent(DateTime date)
        {
            this.date = date;
        }

        public DateTime date;
        public List<Event> events = new List<Event>();
    }
}
