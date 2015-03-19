using Newtonsoft.Json;
using PDS.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace PDS.Controllers
{
    [Authorize]
    public class homeController : Controller
    {
        /// <summary>
        /// Action da página home do portal.
        /// </summary>
        /// <returns>Home portal.</returns>
        public ActionResult index()
        {
            return View();
        }
        
        /// <summary>
        /// Método para enviar por email convites para usar Portal.
        /// </summary>
        /// <param name="form">FormCollection form.</param>
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
    }
}