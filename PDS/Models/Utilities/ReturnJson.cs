using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDS.Models.Utilities
{
    public class ReturnJson
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string returnUrl { get; set; }
        public string location { get; set; }
    }
}