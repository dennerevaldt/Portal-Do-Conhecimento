using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PDS.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mail;
using System.Web.Mvc;
using System.Web.Security;

namespace PDS.Controllers
{
    public class siteController : Controller
    {
        private static ReturnJson objectToSerializeErr;
        private static ReturnJson objectToSerializeSuc;

        // Index
        public ActionResult home(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                TempData["ReturnUrl"] = returnUrl;
            }
            return View();
        }

        [HttpPost]
        public void login(FormCollection form)
        {
            try
            {
                
                var inputRememberme = form["inputRememberme"];
                var inputEmail = form["inputEmail"];
                var inputPassword = form["inputPassword"];

                object returnUrl = string.Empty;
                TempData.TryGetValue("ReturnUrl", out returnUrl);
                string returnUrlStr = returnUrl as string;

                //userReturn = UsuariosRepositorio.GetUser(user);

                //if (userReturn != null)
                if(inputEmail=="dd@email")
                {
                     if(inputRememberme == "true")
                     {
                            //Autenticação do usuário, cookie indestrutível.
                            FormsAuthentication.SetAuthCookie("Denner", true);
                     }
                     else
                     {
                            //Autenticação do usuário, destrói cookie ao fechar o navegador.
                            FormsAuthentication.SetAuthCookie("Denner", false);
                     }

                     objectToSerializeSuc = new ReturnJson { success = true, message = "Login autorizado.", returnUrl = returnUrlStr, location = "/home/index" };
                     Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
                  
                }
                else
                {
                     objectToSerializeErr = new ReturnJson { success = false, message = "Email ou Senha incorretos. Verifique-os e tente novamente." };
                     Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
                }

            }
            catch (Exception)
            {
                objectToSerializeErr = new ReturnJson { success = false, message = "Serviço atualmente indisponível, tente novamente em alguns minutos." };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
            }

        }

        // Set Cookie
        public void setcookie(string key, string value)
        {
            var encValue = Server.UrlTokenEncode(UTF8Encoding.UTF8.GetBytes(value));
            var c = new HttpCookie(key, encValue) { Expires = DateTime.Now.AddDays(7) };
            Response.Cookies.Add(c);
        }

        // LogOff
        public ActionResult logoff()
        {
            // Rotina para remover autenticação do usuário
            System.Web.Security.FormsAuthentication.SignOut();

            return Redirect("/site/home");
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
                    var objectToSerialize = new ReturnJson { success = true, message = "Obrigado, mensagem enviada com sucesso." };
                    Response.Write(JsonConvert.SerializeObject(objectToSerialize));
                }
                catch (Exception)
                {
                    var objectToSerialize = new ReturnJson { success = false, message = "Ops.. estamos com problemas. Verifique sua conexão e tente novamente." };
                    Response.Write(JsonConvert.SerializeObject(objectToSerialize));
                }

            }
            else
            {
                var objectToSerialize = new ReturnJson { success = false, message = "Ops.. estamos com problemas. Tenta novamente." };
                Response.Write(JsonConvert.SerializeObject(objectToSerialize));
            }
            
        }
    }
}