using Newtonsoft.Json;
using PDS.Models.Repository;
using PDS.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PDS.Controllers
{
    public class studentsClassesController : Controller
    {
        #region Atributos Staticos       
        private static ReturnJson objectToSerializeErr;
        private static ReturnJson objectToSerializeSuc;
        #endregion

        public void Update(FormCollection form)
        {
            try
            {
                Int64 idStudent = Int64.Parse(form["idStudent"]);
                Int64 idClasse = Int64.Parse(form["idClass"]);
                int stars = int.Parse(form["stars"]);

                StudentsClassesRepository repStdCls = new StudentsClassesRepository();
                repStdCls.AddStars(idStudent, idClasse, stars);

                //Response.Write(JsonConvert.SerializeObject("Ok"));

            }
            catch (Exception ex)
            {               
                throw ex;
            }
        }

        public void Delete(FormCollection form)
        {
            try
            {
                Int64 idStudent = Int64.Parse(form["idStudent"]);
                Int64 idClasse = Int64.Parse(form["idClass"]);
                int stars = int.Parse(form["stars"]);

                StudentsClassesRepository repStdCls = new StudentsClassesRepository();
                repStdCls.RemoveStars(idStudent, idClasse, stars);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}