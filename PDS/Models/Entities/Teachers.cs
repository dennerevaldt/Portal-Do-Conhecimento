using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDS.Models.Entities
{
    public class Teachers :Persons
    {
        public Int32 idTeacher { get; set; }

        public List<Disciplines> Disciplines { get; set; }

        public List<Followers> Followers { get; set; }
    }
}