﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminfindungsapp
{
    public class Organization
    {
        public Organization() { }
        public Organization(string name, string creatorid)
        {
            this.name = name;
            this.creatorid = creatorid;
        }
        public string id { get; set; }
        public string name { get; set; }

        public string creatorid { get; set; }
    }
}
