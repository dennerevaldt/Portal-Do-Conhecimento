using Facebook;
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
        private string host = "http://portaldoconhecimento.azurewebsites.net";
        //private string host = "http://localhost:51918";

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
            parameters.redirect_uri = host +"/site/returnfb";
            parameters.response_type = "code";
            parameters.display = "page";

            var extendedPermissions = "user_about_me,user_activities,email,user_birthday,user_location";
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
                parameters.redirect_uri = host +"/site/returnfb";
                parameters.client_secret = "317c89ff3be71b66261db0d4275b6425";
                parameters.code = oauthResult.Code;

                dynamic result = _fb.Get("/oauth/access_token", parameters);

                var accessToken = result.access_token;

                //TODO: Guardar no banco
                Session.Add("FbUserToken", accessToken);

                Dictionary<string, string> data = detailsofuser();

                FormsAuthentication.SetAuthCookie(data["id"], false);

                Response.Cookies["userInfo"]["id"] = encrypt(data["id"]);
                Response.Cookies["userInfo"]["first_name"] = encrypt(data["first_name"]);
                Response.Cookies["userInfo"]["middle_name"] = encrypt(data["middle_name"]);
                Response.Cookies["userInfo"]["last_name"] = encrypt(data["last_name"]);
                Response.Cookies["userInfo"]["email"] = encrypt(data["email"]);
                Response.Cookies["userInfo"]["birthday"] = encrypt(data["birthday"]);
                Response.Cookies["userInfo"]["gender"] = encrypt(data["gender"]);
                Response.Cookies["userInfo"]["location"] = encrypt(data["location"]);
                Response.Cookies["userInfo"]["locale"] = encrypt(data["locale"]);

                setcookie("userImage", data["picture_url"]);


                return Redirect("/account/confirmaccount");
            }
            else
            {
                // tratar
                return View("home");
            }

            
        }

        public Dictionary<string, string> detailsofuser()
        {
            //array data
            var dataArray = new Dictionary<string, string>();

            if (Session["FbuserToken"] != null)
            {
                var _fb = new FacebookClient(Session["FbuserToken"].ToString());

                //get user data
                dynamic data = _fb.Get("me");
                dataArray["id"] = data.id;
                dataArray["first_name"] = data.first_name;
                dataArray["middle_name"] = data.middle_name;
                dataArray["last_name"] = data.last_name;
                dataArray["gender"] = data.gender;
                dataArray["email"] = data.email;
                dataArray["birthday"] = data.birthday;

                try
                {
                    dataArray["location"] = data.location.name;
                }
                catch (Exception)
                {
                    dataArray["location"] = "No Informed";
                }

                dataArray["locale"] = data.locale;

                //get user profile picture
                dynamic urlImage = _fb.Get("me/picture?redirect=0&height=200&width=200&type=normal");
                dataArray["picture_url"] = urlImage.data.url;

            }

            return dataArray;
        }
        #endregion

        // Encrypt string
        public string encrypt(string value)
        {
            if (value != null)
            {
                var encValue = Server.UrlTokenEncode(UTF8Encoding.UTF8.GetBytes(value));
                return encValue;
            }
            else
            {
                var encValue = Server.UrlTokenEncode(UTF8Encoding.UTF8.GetBytes("No Informed"));
                return encValue;
            }
        }

        // Set Cookie
        public void setcookie(string key, string value)
        {
            var encValue = Server.UrlTokenEncode(UTF8Encoding.UTF8.GetBytes(value));
            var c = new HttpCookie(key, encValue) { Expires = DateTime.Now.AddDays(7) };
            Response.Cookies.Add(c);
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