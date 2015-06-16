using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDS.Models.Entities
{
    public class ClassesStudents
    {
        public Students student { get; set; }
        public Int64 idClasse { get; set; }
        public int stars { get; set; }
    }
}