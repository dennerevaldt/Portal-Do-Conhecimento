using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDS.Models.Entities
{
    public abstract class Persons
    {
        public Int64 idPerson { get; set; }

        public string name { get; set; }

        public char gender { get; set; }

        public char accountType { get; set; }

        public DateTime dateOfBirth { get; set; }

        public string city { get; set; }

        public string country { get; set; }

        public string urlImageProfile { get; set; }

        public Accounts Account { get; set; }

    }
}