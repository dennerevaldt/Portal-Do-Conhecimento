using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDS.Models.Entities
{
    public class Classes
    {
        public Int64 idClass { get; set; }
        public string name { get; set; }
        public Disciplines discipline { get; set; }
        public List<ClassesStudents> classesStudents { get; set; }
        public List<ClassesPublicationsTeachers> classesPublicationTeachers { get; set; }

    }
}