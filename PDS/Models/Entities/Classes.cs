using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDS.Models.Entities
{
    public class Classes
    {
        public Int32 idClass { get; set; }

        public string name { get; set; }

        public List<Students> Students { get; set; } 
    }
}