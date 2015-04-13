using Newtonsoft.Json;
using PDS.Models.Entities;
using PDS.Models.Repository;
using PDS.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PDS.Controllers
{
    public class disciplinesController : Controller
    {
        #region Atributos Globais

        private ReturnJson objectToSerializeErr;
        private ReturnJson objectToSerializeSuc;

        #endregion

        public ActionResult index()
        {
            return View();
        }

        /// <summary>
        /// Cria uma nova disciplina.
        /// </summary>
        /// <param name="form">FormCollection form.</param>
        [HttpPost]
        public void create(FormCollection form)
        {
            Disciplines discipline = new Disciplines();
            discipline.name = form["name"];
            discipline.teacher = new Teachers();
            discipline.teacher.idTeacher = Int64.Parse(form["idTeacher"]);

            try
            {
                DisciplinesRepository repDisc = new DisciplinesRepository();
                repDisc.Create(discipline);

                objectToSerializeSuc = new ReturnJson { success = true, message = "Disciplina adicionada com sucesso.", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
            }
            catch (Exception)
            {
                objectToSerializeSuc = new ReturnJson { success = false, message = "Ops, estamos com problemas. Tente novamente.", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
            }
        }

        /// <summary>
        /// Faz um get em todas disciplinas do professor logado.
        /// </summary>
        [HttpPost]
        public void getall()
        {

                HttpCookie userInfo = Request.Cookies["userInfo"];
                var CidTeacher = Server.UrlTokenDecode(userInfo["id_type_account"]);
                string id_teacher = System.Text.UTF8Encoding.UTF8.GetString(CidTeacher);

                Int64 idTeacher = Int64.Parse(id_teacher);

                DisciplinesRepository repDisc = new DisciplinesRepository();

                List<Disciplines> listDisciplines = repDisc.GetAll(idTeacher);

                Response.Write(JsonConvert.SerializeObject(listDisciplines));

                      
        }

        /// <summary>
        /// Action para Get do nome da disciplina.
        /// </summary>
        /// <param name="idDisc">Int idDiscipline.</param>
        public void getname(int idDisc)
        {
            DisciplinesRepository repDisc = new DisciplinesRepository();

            Disciplines discipline = repDisc.GetName(idDisc);

            Response.Write(JsonConvert.SerializeObject(discipline));
        }

        /// <summary>
        /// Deleta uma disciplina através do id da mesma.
        /// </summary>
        /// <param name="id">Int id.</param>
        [HttpGet]
        public void delete(int id)
        {
            try
            {
                Int64 idDiscipline = Convert.ToInt64(id);
                DisciplinesRepository repDisc = new DisciplinesRepository();
                repDisc.Delete(idDiscipline);

                objectToSerializeSuc = new ReturnJson { success = true, message = "Disciplina deletada.", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
            }
            catch (Exception)
            {
                objectToSerializeSuc = new ReturnJson { success = false, message = "Ops, estamos com problemas. Tente novamente.", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
            }       
        }

        /// <summary>
        /// Fazer update em uma disciplina.
        /// </summary>
        /// <param name="form"></param>
        [HttpPost]
        public void update(FormCollection form)
        {
            try
            {
                Int64 idDiscipline = Int64.Parse(form["idDiscipline"]);
                string name = form["name"];

                DisciplinesRepository repDisc = new DisciplinesRepository();
                repDisc.Update(idDiscipline, name);

                objectToSerializeSuc = new ReturnJson { success = true, message = "", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
            }
            catch (Exception)
            {
                objectToSerializeSuc = new ReturnJson { success = false, message = "Ops, estamos com problemas. Tente novamente.", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
            }
            
        }

    }
}