using Newtonsoft.Json;
using PDS.Models.Entities;
using PDS.Models.Repository;
using PDS.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PDS.Controllers
{
    public class accountController : Controller
    {
        private static ReturnJson objectToSerializeErr;
        private static ReturnJson objectToSerializeSuc;

        // Create account
        [HttpPost]
        public void createaccountteacher(FormCollection form)
        {
            Accounts account = new Accounts();
            account.email = form["email"];
            account.password = form["password"];
            account.acessToken = form["acessToken"];

            Int64 idAccount = AccountsRepository.Create(account);

        }

        // Confirm data account
        public ActionResult confirmaccount()
        {
            return View("confirmaccount");
        }

        // Login 
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

                if (inputEmail != null || inputPassword != null)
                {
                    //userReturn = UsuariosRepositorio.GetUser(user);

                    //if (userReturn != null)
                    if (inputEmail == "dd@email")
                    {
                        if (inputRememberme == "true")
                        {
                            //Autenticação do usuário, cookie indestrutível.
                            FormsAuthentication.SetAuthCookie("Denner", true);
                        }
                        else
                        {
                            //Autenticação do usuário, destrói cookie ao fechar o navegador.
                            FormsAuthentication.SetAuthCookie("Denner", false);
                        }

                        objectToSerializeSuc = new ReturnJson { success = true, message = "Entrando...", returnUrl = returnUrlStr, location = "/home/index" };
                        Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));

                    }
                    else
                    {
                        objectToSerializeErr = new ReturnJson { success = false, message = "Email ou Senha incorretos. Verifique-os e tente novamente." };
                        Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
                    }
                }
                else
                {
                    objectToSerializeErr = new ReturnJson { success = false, message = "os campos são obrigatórios." };
                    Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
                }
            }
            catch (Exception)
            {
                objectToSerializeErr = new ReturnJson { success = false, message = "Serviço atualmente indisponível, tente novamente em alguns minutos." };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
            }

        }

        // Logoff
        public ActionResult logoff()
        {
            // Rotina para remover autenticação do usuário
            System.Web.Security.FormsAuthentication.SignOut();

            // Rotina para remover cookie de dados do usuário logado
            if (Request.Cookies["userInfo"] != null)
            {
                HttpCookie myCookie = new HttpCookie("userInfo");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }

            if (Request.Cookies["userImage"] != null)
            {
                HttpCookie myCookie = new HttpCookie("userImage");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }

            return Redirect("/site/home");
        }

        // Set Cookie
        public void setcookie(string key, string value)
        {
            var encValue = Server.UrlTokenEncode(UTF8Encoding.UTF8.GetBytes(value));
            var c = new HttpCookie(key, encValue) { Expires = DateTime.Now.AddDays(7) };
            Response.Cookies.Add(c);
        }
    }
}