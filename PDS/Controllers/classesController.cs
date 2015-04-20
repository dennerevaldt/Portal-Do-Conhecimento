using Newtonsoft.Json;
using PDS.Models.Entities;
using PDS.Models.Repository;
using PDS.Models.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
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
            try
            {
                Int64 idClass = Int64.Parse(form["idClass"]);

                ClassesRepository repClass = new ClassesRepository();
                TeachersRepository repTeacher = new TeachersRepository();
                ClassesPublicationsTeachersRepository repClPub = new ClassesPublicationsTeachersRepository();

                Classes classeReturn = repClass.GetOne(idClass);

                classeReturn.classesPublicationTeachers =
                    new List<ClassesPublicationsTeachers>
                    (
                        repClPub.GetPublications(idClass)
                    );


                //Get Teacher
                classeReturn.discipline.teacher = new Teachers();

                classeReturn.discipline.teacher =
                    (
                        repTeacher.GetOne(classeReturn.discipline.idDiscipline)
                    );

                Response.Write(JsonConvert.SerializeObject(classeReturn));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para inserir estudante em uma turma.
        /// </summary>
        /// <param name="form">FormCollection form.</param>
        public void insertstudentinclasse(FormCollection form)
        {
            try
            {
                Int64 idStudent = Int64.Parse(form["idStudent"]);
                Int64 idClass = Int64.Parse(form["idClass"]);
                StudentsRepository repStd = new StudentsRepository();

                if(!repStd.CheckStudentInClasse(idStudent,idClass))
                {
                      repStd.InsertStudentInClasse(idStudent,idClass);

                      objectToSerializeSuc = new ReturnJson { success = true, message = "Aluno adicionado com sucesso.", returnUrl = "", location = "" };
                      Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
                }
                else
                {
                    objectToSerializeErr = new ReturnJson { success = false, message = "O aluno já está adicionado a turma.", returnUrl = "", location = "" };
                    Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
                }

            }
            catch (Exception)
            {
                objectToSerializeErr = new ReturnJson { success = false, message = "Ops, estamos com problemas. Lembre-se de cadastrar uma disciplina antes de cadastrar uma Turma. Tente novamente.", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
            }
        }

        /// <summary>
        /// Action para fazer o upload de um post.
        /// </summary>
        /// <param name="file">HttpPostedFileBase file.</param>
        /// <param name="form">FormCollection form.</param>
        [HttpPost]
        public void uploadPost(HttpPostedFileBase file, FormCollection form)
        {
            try
            {
                #region Get Type File
                
                string type;

                if (file != null)
                {
                    type = Path.GetExtension(file.FileName);
                }
                else
                {
                    type = "notFile";
                }

                #endregion

                if (type == ".jpg" || type == ".JPG" || type == ".png" || type == ".PNG" || type == ".jpeg" || type == ".JPEG" ||
                    type == ".doc" || type == ".docx" || type == ".txt" || type == ".pdf" || type == ".PDF" || type == ".ppt" ||
                    type == ".zip" || type == ".rar")
                {

                    //request cookie data user
                    HttpCookie userInfo = Request.Cookies["userInfo"];
                    var CidT = Server.UrlTokenDecode(userInfo["id_type_account"]);
                    string idTeacher = System.Text.UTF8Encoding.UTF8.GetString(CidT);

                    //insert publications teachers
                    PublicationsTeachersRepository repPubTeachers = new PublicationsTeachersRepository();
                    Int64 idPublication = repPubTeachers.Create(form["message"], Int64.Parse(idTeacher));

                    //insert classes publications teacher
                    ClassesPublicationsTeachersRepository repClassesPubTec = new ClassesPublicationsTeachersRepository();
                    repClassesPubTec.Create(idPublication, Int64.Parse(form["idClasse"]));

                    //prepare url attachment
                    string path = "/Content/Uploads/Posts/Teachers/" + idPublication + type;
                    file.SaveAs(Path.Combine(Server.MapPath(path)));

                    //insert attachment
                    AttachmentsRepository repAtt = new AttachmentsRepository();
                    repAtt.Create(path, idPublication);
                    
                }

            }
            catch (Exception ex)
            {            
                throw ex;
            }

        }

        /// <summary>
        /// Action para fazer download de um arquivo.
        /// </summary>
        /// <param name="form">FormCollection form.</param>
        /// <returns>File file.</returns>
        [HttpPost]
        public FileResult download(FormCollection form)
        {
            try
            {
                string url = form["url"];
                string path = Path.Combine(Server.MapPath(url));
                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string dateNow = DateTime.Now.ToShortDateString();
                string nameDown = "teste.jpg";


                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, nameDown);
            }
            catch (Exception ex)
            {
                throw ex;
            }      
        }

        /// <summary>
        /// Action para enviar uma nova mensagem para turma.
        /// </summary>
        /// <param name="form">FormCollection form.</param>
        [HttpPost]
        public void invitemessage(FormCollection form)
        {
            try
            {
                Int64 idClasse = Int64.Parse(form["id"]);
                string message = form["message"];

                ClassesRepository repClasses = new ClassesRepository();
                repClasses.InviteMessage(idClasse, message);

                objectToSerializeSuc = new ReturnJson { success = true, message = "Mensagem enviada com sucesso.", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
            }
            catch (Exception)
            {
                objectToSerializeErr = new ReturnJson { success = false, message = "Ops, estamos com problemas. Tente novamente.", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
            }
        }

        /// <summary>
        /// Action para retornar as postagens de cada aluno.
        /// </summary>
        /// <param name="form">FormCollection form.</param>
        [HttpPost]
        public void getpoststeachers(FormCollection form)
        {
            try
            {
                Int64 idStudent = Int64.Parse(form["id"]);
                ClassesPublicationsTeachersRepository repPubTeac = new ClassesPublicationsTeachersRepository();
                List<ClassesPublicationsTeachers> listPub = repPubTeac.GetPostsTeachers(idStudent);

                Response.Write(JsonConvert.SerializeObject(listPub));
            }
            catch (Exception)
            {
                
                throw;
            }

        }
    }
}