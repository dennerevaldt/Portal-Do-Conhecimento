using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDS.Models.Entities
{
    public class Disciplines
    {
        public Int32 idDiscipline { get; set; }

        public string name { get; set; }

        public Teachers Teacher { get; set; }

        public Classes Class { get; set; }

    }
}