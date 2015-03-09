﻿using Facebook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PDS.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
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

            ViewBag.UrlFb = getfacebookloginurl();

            return View();
        }

        #region Login Facebook
        public string getfacebookloginurl()
        {
            dynamic parameters = new ExpandoObject();
            parameters.client_id = "860133764025276";
            parameters.redirect_uri = "http://portaldoconhecimento.azurewebsites.net/site/returnfb";
            parameters.response_type = "code";
            parameters.display = "page";

            var extendedPermissions = "user_about_me,read_stream,publish_stream";
            parameters.scope = extendedPermissions;

            var _fb = new FacebookClient();
            var url = _fb.GetLoginUrl(parameters);

            return url.ToString();
        }

        public ActionResult returnfb()
        {
            var _fb = new FacebookClient();
            FacebookOAuthResult oauthResult;

            _fb.TryParseOAuthCallbackUrl(Request.Url, out oauthResult);

            if (oauthResult.IsSuccess)
            {
                //Pega o Access Token "permanente"
                dynamic parameters = new ExpandoObject();
                parameters.client_id = "860133764025276";
                parameters.redirect_uri = "http://portaldoconhecimento.azurewebsites.net/site/returnfb";
                parameters.client_secret = "317c89ff3be71b66261db0d4275b6425";
                parameters.code = oauthResult.Code;

                dynamic result = _fb.Get("/oauth/access_token", parameters);

                var accessToken = result.access_token;

                //TODO: Guardar no banco
                Session.Add("FbUserToken", accessToken);
            }
            else
            {
                // tratar
            }

            Dictionary<string, string> data = detailsofuser();

            FormsAuthentication.SetAuthCookie(data["id"], false);

            Response.Cookies["userInfo"]["first_name"] = data["first_name"];
            Response.Cookies["userInfo"]["last_name"] = data["last_name"];

            setcookie("userImage", data["picture_url"]);


            return Redirect("/home/index");
        }

        public Dictionary<string, string> detailsofuser()
        {
            //array dados
            var dataArray = new Dictionary<string, string>();

            if (Session["FbuserToken"] != null)
            {
                var _fb = new FacebookClient(Session["FbuserToken"].ToString());

                //detalhes do usuario completo
                //var request = _fb.Get("me");

                //get separado dados
                dynamic data = _fb.Get("me?fields=first_name,middle_name,last_name,id,gender,picture");
                string idUser = data.id;
                dataArray["id"] = data.id;
                dataArray["first_name"] = data.first_name;
                dataArray["middle_name"] = data.middle_name;
                dataArray["last_name"] = data.last_name;
                dataArray["gender"] = data.gender;
                dataArray["picture_url"] = data.picture.data.url;

                //foto usuário grande
                //WebResponse response = null;
                //string pictureUrl = string.Empty;
                //try
                //{
                //    WebRequest req = WebRequest.Create(string.Format("https://graph.facebook.com/" + idUser + "/picture?type=large"));
                //    response = req.GetResponse();
                //    dataArray["picture_url"] = response.ResponseUri.ToString();

                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}
                //finally
                //{
                //    if (response != null) response.Close();
                //}

            }

            return dataArray;
        }
        #endregion



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

        // Set Cookie
        public void setcookie(string key, string value)
        {
            var encValue = Server.UrlTokenEncode(UTF8Encoding.UTF8.GetBytes(value));
            var c = new HttpCookie(key,encValue) { Expires = DateTime.Now.AddDays(7) };
            Response.Cookies.Add(c);
        }

        // LogOff
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