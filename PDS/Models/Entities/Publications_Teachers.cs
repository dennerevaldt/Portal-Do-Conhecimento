using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDS.Models.Entities
{
    public class Publications_Teachers
    {
        public Int32 idPublication { get; set; }

        public DateTime datePublication { get; set; }

        public Teachers Teacher { get; set; }

        public Attachments Attachment { get; set; }

        public Classes Class { get; set; }
    }
}