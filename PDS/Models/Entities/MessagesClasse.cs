using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDS.Models.Entities
{
    public class MessagesClasse
    {
        public Int64 idMessage { get; set; }
        public Int64 idClasse { get; set; }
        public string message { get; set; }
    }
}