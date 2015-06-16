using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDS.Models.Entities
{
    public class StudentsClasses
    {
        public Classes objClass { get; set; }
        public Int64 idStudent { get; set; }
        public int stars { get; set; }
    }
}