using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDS.Models.Entities
{
    public class Accounts
    {
        public Int32 idAccount { get; set; }

        public string email { get; set; }

        public string password { get; set; }

        public string acessToken { get; set; }
    }
}