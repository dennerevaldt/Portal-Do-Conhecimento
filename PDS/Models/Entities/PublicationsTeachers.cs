using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDS.Models.Entities
{
    public class PublicationsTeachers
    {
        public Int64 idPublication { get; set; }
        public DateTime datePublication { get; set; }
        public string textPublication { get; set; }
        public Attachments attachment { get; set; }
        public Teachers teacher { get; set; }
        public List<PublicationsTeachersClasses> publicationsTeachersClasses { get; set; }
    }
}