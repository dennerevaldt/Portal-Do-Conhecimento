using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PDS.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mail;
using System.Web.Mvc;

namespace PDS.Controllers
{
    public class siteController : Controller
    {
        // Index
        public ActionResult home()
        {
            return View();
        }

        // Contact
        [HttpPost]
        public void contact(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                string name = form["name"];
                string email = form["email"].ToLower();
                string message = form["message"];

                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                mail.To.Add("dnrevaldt@gmail.com");
                mail.From = new MailAddress(email, name);
                mail.Subject = "Mensagem do Portal";
                string Body = "Nome: " + name + "<br />" + "Email: " + email + "<br />" + "Mensagem: " + message;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                ("dnrevaldt@gmail.com", "02051994");// Enter seders User name and password
                smtp.EnableSsl = true;

                try
                {
                    smtp.Send(mail);
                    var objectToSerialize = new ReturnContact { success = true, message = "Obrigado, mensagem enviada com sucesso." };
                    Response.Write(JsonConvert.SerializeObject(objectToSerialize));
                }
                catch (Exception ex)
                {
                    var objectToSerialize = new ReturnContact { success = false, message = "Ops.. estamos com problemas. Tenta novamente mais tarde." };
                    Response.Write(JsonConvert.SerializeObject(objectToSerialize));
                    throw ex;
                }

            }
            else
            {
                var objectToSerialize = new ReturnContact { success = false, message = "Ops.. estamos com problemas. Tenta novamente." };
                Response.Write(JsonConvert.SerializeObject(objectToSerialize));
            }
            
        }
    }
}