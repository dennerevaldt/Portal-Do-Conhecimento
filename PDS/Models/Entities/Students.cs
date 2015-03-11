using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDS.Models.Entities
{
    public class Students : Persons
    {
        public Int32 idStudent { get; set; }

        List<Classes> Classes { get; set; }
    }
}