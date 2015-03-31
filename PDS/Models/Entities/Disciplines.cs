using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDS.Models.Entities
{
    public class Disciplines
    {
        public Int64 idDiscipline { get; set; }
        public string name { get; set; }
        public Teachers teacher { get; set; }
    }
}