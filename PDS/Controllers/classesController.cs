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
    public class classesController : Controller
    {
        #region Atributos Globais
            private ReturnJson objectToSerializeErr;
            private ReturnJson objectToSerializeSuc;
        #endregion

        public ActionResult index()
        {
            return View();
        }

        public ActionResult manageclass()
        {
            return View("manageclass");
        }

        /// <summary>
        /// Adiciona uma nova turma.
        /// </summary>
        /// <param name="form"></param>
        public void create(FormCollection form)
        {
            try
            {
                Classes newClass = new Classes();
                newClass.name = form["name"];
                newClass.discipline = new Disciplines();
                newClass.discipline.idDiscipline = Int64.Parse(form["idDiscipline"]);

                ClassesRepository repClasses = new ClassesRepository();
                repClasses.Create(newClass);

                objectToSerializeSuc = new ReturnJson { success = true, message = "Turma adicionada com sucesso.", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
            }
            catch (Exception)
            {
                objectToSerializeErr = new ReturnJson { success = false, message = "Ops, estamos com problemas. Lembre-se de cadastrar uma disciplina antes de cadastrar uma Turma. Tente novamente.", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
            }
        }

        /// <summary>
        /// Faz um get em todas turmas do professor logado.
        /// </summary>
        [HttpGet]
        public void getall(Int64 id)
        {
            Int64 idDisc = id;

            ClassesRepository repClass = new ClassesRepository();

            List<Classes> listClasses = repClass.GetAll(idDisc);

            Response.Write(JsonConvert.SerializeObject(listClasses));

        }

        ///<summary>
        ///Deleta uma turma através do id da mesma.
        ///</summary>
        ///<param name="id">Int id.</param>
        [HttpGet]
        public void delete(int id)
        {
            try
            {
                Int64 idClasse = Convert.ToInt64(id);
                ClassesRepository repClasses = new ClassesRepository();

                repClasses.Delete(idClasse);

                objectToSerializeSuc = new ReturnJson { success = true, message = "Turma deletada.", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
            }
            catch (Exception)
            {
                objectToSerializeSuc = new ReturnJson { success = false, message = "Ops, estamos com problemas. Tente novamente.", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
            }
        }

        ///<summary>
        /// Fazer update em uma turma.
        /// </summary>
        /// <param name="form"></param>
        [HttpPost]
        public void update(FormCollection form)
        {
            try
            {
                Int64 idClass = Int64.Parse(form["idClass"]);
                string name = form["name"];

                ClassesRepository repClasses = new ClassesRepository();
                repClasses.Update(idClass, name);

                objectToSerializeSuc = new ReturnJson { success = true, message = "", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
            }
            catch (Exception)
            {
                objectToSerializeErr = new ReturnJson { success = false, message = "Ops, estamos com problemas. Tente novamente.", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
            }

        }

        /// <summary>
        /// Método para retornar uma turma.
        /// </summary>
        /// <param name="form">FormCollection form.</param>
        public void getoneclass(FormCollection form)
        {
            Int64 idClass = Int64.Parse(form["idClass"]);

            ClassesRepository repClass = new ClassesRepository();

            List<Classes> listClasses = repClass.GetOne(idClass);

            Response.Write(JsonConvert.SerializeObject(listClasses));
        }

        public void insertstudentinclasse(FormCollection form)
        {
            try
            {
                Int64 idStudent = Int64.Parse(form["idStudent"]);
                Int64 idClass = Int64.Parse(form["idClass"]);

                StudentsRepository repStd = new StudentsRepository();
                repStd.InsertStudentInClasse(idStudent,idClass);

            }
            catch (Exception)
            {
                objectToSerializeErr = new ReturnJson { success = false, message = "Ops, estamos com problemas. Lembre-se de cadastrar uma disciplina antes de cadastrar uma Turma. Tente novamente.", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
            }
        }
    }
}