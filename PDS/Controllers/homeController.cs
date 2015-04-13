using Newtonsoft.Json;
using PDS.Models.Entities;
using PDS.Models.Repository;
using PDS.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace PDS.Controllers
{
    public class homeController : Controller
    {
        #region Atributos Globais
            private ReturnJson objectToSerializeErr;
            private ReturnJson objectToSerializeSuc;
        #endregion

        /// <summary>
        /// Action da página home do portal.
        /// </summary>
        /// <returns>Home portal.</returns>
        [Authorize]
        public ActionResult index()
        {
            return View();
        }
  
        /// <summary>
        /// Método para enviar por email convites para usar Portal.
        /// </summary>
        /// <param name="form">FormCollection form.</param>
        [Authorize]
        [HttpPost]
        public void invitefriend(FormCollection form)
        {
            try
            {
                string email = form["email"].ToLower();

                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress("dnrevaldt@gmail.com","Portal do conhecimento");
                mail.Subject = "Mensagem do Portal";
                string Body = "<p><a href=\"http://portaldoconhecimento.com.br\"> Conheça o <b>Portal do conhecimento</b> agora! </a></p>";
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                ("dnrevaldt@gmail.com", "letitbe10");// Enter seders User name and password
                smtp.EnableSsl = true;

                try
                {
                    smtp.Send(mail);
                    var objectToSerialize = new ReturnJson { success = true, message = "Convite enviado com sucesso para seu amigo!" };
                    Response.Write(JsonConvert.SerializeObject(objectToSerialize));
                }
                catch (Exception)
                {
                    var objectToSerialize = new ReturnJson { success = false, message = "Ops.. estamos com problemas. Verifique sua conexão e tente novamente." };
                    Response.Write(JsonConvert.SerializeObject(objectToSerialize));
                }
            }
            catch (Exception)
            {
                var objectToSerialize = new ReturnJson { success = false, message = "Ops.. estamos com problemas. Verifique sua conexão e tente novamente." };
                Response.Write(JsonConvert.SerializeObject(objectToSerialize));
            }

        }

        [HttpGet]
        public ActionResult perfil(int idperfil)
        {
            try
            {
                AccountsRepository repAccount = new AccountsRepository();
                dynamic user = repAccount.GetPerfil((Int64)idperfil);

                try
                {
                    ViewBag.idTeacherPerfil = user.idTeacher;
                }
                catch (Exception)
                {
                    ViewBag.idTeacherPerfil = null;
                }

                TempData["dataPerfilUser"] = user;

                return View("perfil");
            }
            catch (Exception)
            {
                return View("Error");
            }
            
        }

        [HttpPost]
        public void follow(FormCollection form)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    Int64 idFollower = Int64.Parse(form["idFollower"]);
                    Int64 idFollowing = Int64.Parse(form["idFollowing"]);

                    TeachersRepository repTeacher = new TeachersRepository();
                    repTeacher.Follow(idFollower, idFollowing);

                    objectToSerializeSuc = new ReturnJson { success = true, message = "", returnUrl = null, location = "" };
                    Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
                }
                catch (Exception)
                {
                    objectToSerializeErr = new ReturnJson { success = false, message = "", returnUrl = null, location = "" };
                    Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
                }
                
            }
            else
            {
                Redirect("/site/home");
            }
        }

        [HttpPost]
        public void unfollow(FormCollection form)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    Int64 idFollower = Int64.Parse(form["idFollower"]);
                    Int64 idFollowing = Int64.Parse(form["idFollowing"]);

                    TeachersRepository repTeacher = new TeachersRepository();
                    repTeacher.UnFollow(idFollower, idFollowing);

                    objectToSerializeSuc = new ReturnJson { success = true, message = "", returnUrl = null, location = "" };
                    Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
                }
                catch (Exception)
                {
                    objectToSerializeErr = new ReturnJson { success = false, message = "", returnUrl = null, location = "" };
                    Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
                }

            }
            else
            {
                Redirect("/site/home");
            }
        }

        [HttpPost]
        public void checkfollow(FormCollection form)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    Int64 idFollower = Int64.Parse(form["idFollower"]);
                    Int64 idFollowing = Int64.Parse(form["idFollowing"]);

                    TeachersRepository repTeacher = new TeachersRepository();
                    bool result = repTeacher.CheckFollow(idFollower, idFollowing);

                    if (result == true)
                    {
                        objectToSerializeSuc = new ReturnJson { success = true, message = "", returnUrl = null, location = "" };
                        Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
                    }
                    else
                    {
                        objectToSerializeSuc = new ReturnJson { success = false, message = "", returnUrl = null, location = "" };
                        Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
                    }
                    
                }
                catch (Exception)
                {
                    objectToSerializeErr = new ReturnJson { success = false, message = "Erro.", returnUrl = null, location = "" };
                    Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
                }

            }
            else
            {
                Redirect("/site/home");
            }
        }

        [HttpPost]
        public void searchteacher(FormCollection form)
        {
            if (!string.IsNullOrWhiteSpace(form["name"]))
            {
                try
                {
                    TeachersRepository repTeacher = new TeachersRepository();
                    List<Teachers> listTeacher = repTeacher.SearchTeacher(form["name"]);

                    Response.Write(JsonConvert.SerializeObject(listTeacher));
                }
                catch (Exception)
                {
                    objectToSerializeErr = new ReturnJson { success = false, message = "Error in Database.", returnUrl = null, location = "" };
                    Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
                }

            }

        }

        [HttpPost]
        public void searchstudent(FormCollection form)
        {
            if (!string.IsNullOrWhiteSpace(form["name"]))
            {
                try
                {
                    StudentsRepository repStudent = new StudentsRepository();
                    List<Students> listStudent = repStudent.SearchStudent(form["name"]);

                    Response.Write(JsonConvert.SerializeObject(listStudent));
                }
                catch (Exception)
                {
                    objectToSerializeErr = new ReturnJson { success = false, message = "Error in Database.", returnUrl = null, location = "" };
                    Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
                }

            }

        }

        [HttpPost]
        public void getfollowers(FormCollection form)
        {
            if (!string.IsNullOrWhiteSpace(form["id"]))
            {
                try
                {
                    TeachersRepository repTeacher = new TeachersRepository();
                    List<Teachers> listFollowers = repTeacher.GetFollowers(Int64.Parse(form["id"]));

                    Response.Write(JsonConvert.SerializeObject(listFollowers));
                }
                catch (Exception)
                {
                    objectToSerializeErr = new ReturnJson { success = false, message = "Error in Database.", returnUrl = null, location = "" };
                    Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
                }

            }
        }

        [HttpPost]
        public void getfollowersother(FormCollection form)
        {
            if (!string.IsNullOrWhiteSpace(form["id"]))
            {
                try
                {
                    TeachersRepository repTeacher = new TeachersRepository();
                    List<Teachers> listFollowers = repTeacher.GetFollowersOther(Int64.Parse(form["id"]));

                    Response.Write(JsonConvert.SerializeObject(listFollowers));
                }
                catch (Exception)
                {
                    objectToSerializeErr = new ReturnJson { success = false, message = "Error in Database.", returnUrl = null, location = "" };
                    Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
                }

            }
        }
    }
}