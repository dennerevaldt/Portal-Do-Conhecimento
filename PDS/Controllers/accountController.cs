using Newtonsoft.Json;
using PDS.Models.Entities;
using PDS.Models.Repository;
using PDS.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
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
        private static string path = "";

        /// <summary>
        /// Action para criar uma nova conta de Teacher.
        /// </summary>
        /// <param name="inputFile">HttpPostedFileBase file.</param>
        /// <param name="form">FormCollection form.</param>
        [HttpPost]
        public void createaccountteacher(HttpPostedFileBase inputFile, FormCollection form)
        {
            try
            {
                // Insert Account

                Accounts account = new Accounts();
                account.email = form["inputEmailT"];
                account.password = AccountsRepository.EncryptPassword(form["inputPasswordT"]);
                account.acessToken = form["inputAcessToken"];

                Int64 idAccount = AccountsRepository.Create(account);

                // Insert Teacher

                Teachers teacher = new Teachers();
                teacher.Account = new Accounts();
                teacher.Account.idAccount = idAccount;
                teacher.firstName = form["inputFirstNameT"];
                teacher.lastName = form["inputLastNameT"];
                teacher.gender = Convert.ToChar(form["inputGender"]);

                DateTime dataOrg = Convert.ToDateTime(form["inputBirthday"]);
                string formatForMySql = dataOrg.ToString("yyyy-MM-dd HH:mm");
                DateTime dateValue = DateTime.Parse(formatForMySql);
                teacher.dateOfBirth = dateValue;

                teacher.accountType = Convert.ToChar("T");
                teacher.city = form["inputCityT"];
                teacher.country = form["inputCountryT"];

                if (inputFile != null)
                {
                    string extension = Path.GetExtension(inputFile.FileName);
                    path = Path.Combine(Server.MapPath("~/App_Data/Uploads/ImagesProfile/Teachers"), idAccount.ToString()+extension);
                    inputFile.SaveAs(path);

                    teacher.urlImageProfile = path;
                }
                else
                {
                    teacher.urlImageProfile = "/Content/images/noPhoto.png";
                }

                

                TeachersRepository.Create(teacher);

                // Return Sucess
                objectToSerializeSuc = new ReturnJson { success = true, message = "Contra criada com sucesso, faça login e aproveite!", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));

            }
            catch (Exception)
            {
                // Return Error
                objectToSerializeSuc = new ReturnJson { success = false, message = "Ops. Estamos com problemas, tente novamente.", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
            }
            
        }

        /// <summary>
        /// Action para criar uma nova conta de Student.
        /// </summary>
        /// <param name="inputFile">HttpPostedFileBase file.</param>
        /// <param name="form">FormCollection form.</param>
        [HttpPost]
        public void createaccountstudent(HttpPostedFileBase inputFile, FormCollection form)
        {
            try
            {
                // Insert Account

                Accounts account = new Accounts();
                account.email = form["inputEmail"];
                account.password = AccountsRepository.EncryptPassword(form["inputPassword"]);
                account.acessToken = form["inputAcessToken"];

                Int64 idAccount = AccountsRepository.Create(account);

                // Insert Teacher

                Students student = new Students();
                student.Account = new Accounts();
                student.Account.idAccount = idAccount;
                student.firstName = form["inputFirstName"];
                student.lastName = form["inputLastName"];
                student.gender = Convert.ToChar(form["inputGender"]);

                DateTime dataOrg = Convert.ToDateTime(form["inputBirthday"]);
                string formatForMySql = dataOrg.ToString("yyyy-MM-dd HH:mm");
                DateTime dateValue = DateTime.Parse(formatForMySql);
                student.dateOfBirth = dateValue;
                

                student.accountType = Convert.ToChar("S");
                student.city = form["inputCity"];
                student.country = form["inputCountry"];

                if (inputFile != null)
                {
                    string extension = Path.GetExtension(inputFile.FileName);
                    path = Path.Combine(Server.MapPath("~/App_Data/Uploads/ImagesProfile/Students"), idAccount.ToString() + extension);
                    inputFile.SaveAs(path);

                    student.urlImageProfile = path;
                }
                else
                {
                    student.urlImageProfile = "/Content/images/noPhoto.png";
                }
               

                StudentsRepository.Create(student);

                // Return Sucess
                objectToSerializeSuc = new ReturnJson { success = true, message = "Contra criada com sucesso, faça login e aproveite!", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));

            }
            catch (Exception)
            {
                // Return Error
                objectToSerializeSuc = new ReturnJson { success = false, message = "Ops. Estamos com problemas, tente novamente mais tarde.", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
            }

        }

        /// <summary>
        /// Action para confirmar dados retornados do facebook para criação da conta de Teacher.
        /// </summary>
        /// <param name="form">FormCollection form.</param>
        [HttpPost]
        public void confirmaccountteacher(FormCollection form)
        {
            try
            {
                // Insert Account

                Accounts account = new Accounts();
                account.email = form["email"];
                account.password = "";
                account.acessToken = form["acessToken"];

                Int64 idAccount = AccountsRepository.Create(account);

                // Insert Teacher

                Teachers teacher = new Teachers();
                teacher.Account = new Accounts();
                teacher.Account.idAccount = idAccount;
                teacher.firstName = form["firstName"];
                teacher.lastName = form["lastName"];
                teacher.gender = Convert.ToChar(form["gender"]);

                DateTime dataOrg = Convert.ToDateTime(form["dateOfBirth"]);
                string formatForMySql = dataOrg.ToString("yyyy-MM-dd HH:mm");
                DateTime dateValue = DateTime.Parse(formatForMySql);
                teacher.dateOfBirth = dateValue;

                teacher.accountType = Convert.ToChar("T");
                teacher.city = form["location"];
                teacher.country = form["country"];
                teacher.urlImageProfile = form["urlImageProfile"];
                teacher.Account.acessToken = form["acessToken"];

                TeachersRepository.Create(teacher);

                // Return Sucess
                objectToSerializeSuc = new ReturnJson { success = true, message = "Conta criada com sucesso! Você está sendo redirecionado...", returnUrl = null, location = "/home/index" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));

            }
            catch (Exception)
            {
                // Return Error
                objectToSerializeSuc = new ReturnJson { success = false, message = "Ops. Estamos com problemas, tente novamente.", returnUrl = null, location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
            }

        }

        /// <summary>
        /// Action para confirmar dados retornados do facebook para criação da conta de Student.
        /// </summary>
        /// <param name="form">FormCollection form.</param>
        [HttpPost]
        public void confirmaccountstudent(FormCollection form)
        {
            try
            {
                // Insert Account

                Accounts account = new Accounts();
                account.email = form["email"];
                account.password = "";
                account.acessToken = form["acessToken"];

                Int64 idAccount = AccountsRepository.Create(account);

                // Insert Teacher

                Students student = new Students();
                student.Account = new Accounts();
                student.Account.idAccount = idAccount;
                student.firstName = form["firstName"];
                student.lastName = form["lastName"];
                student.gender = Convert.ToChar(form["gender"]);

                DateTime dataOrg = Convert.ToDateTime(form["dateOfBirth"]);
                string formatForMySql = dataOrg.ToString("yyyy-MM-dd HH:mm");
                DateTime dateValue = DateTime.Parse(formatForMySql);
                student.dateOfBirth = dateValue;

                student.accountType = Convert.ToChar("S");
                student.city = form["location"];
                student.country = form["locale"];
                student.urlImageProfile = form["urlImageProfile"];

                StudentsRepository.Create(student);

                // Return Sucess
                objectToSerializeSuc = new ReturnJson { success = true, message = "Conta criada com sucesso! Você está sendo redirecionado...", returnUrl = null, location = "/home/index" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));

            }
            catch (Exception)
            {
                // Return Error
                objectToSerializeSuc = new ReturnJson { success = false, message = "Ops. Estamos com problemas, tente novamente mais tarde.", returnUrl = null, location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
            }

        }


        /// <summary>
        /// Action para retorna view com dados para confirmação de conta.
        /// </summary>
        /// <returns>View ConfirmAccount.</returns>
        public ActionResult confirmaccount()
        {
            return View("confirmaccount");
        }

        /// <summary>
        /// Action para validar login e permitir permissão do usuário ao portal.
        /// </summary>
        /// <param name="form">FormCollection form.</param>
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
                    Accounts account = new Accounts();

                    account = AccountsRepository.GetDataAccount(inputEmail);

                    if (account != null)
                    {
                        if (AccountsRepository.CheckPassword(inputPassword,account.password))
                        {
                            if (inputRememberme == "true")
                            {
                                //Autenticação do usuário, cookie indestrutível.
                                FormsAuthentication.SetAuthCookie(account.email, true);
                                CreateCookieInfoUser(AccountsRepository.GetUserData(account.email));
                            }
                            else
                            {
                                //Autenticação do usuário, destrói cookie ao fechar o navegador.
                                FormsAuthentication.SetAuthCookie(account.email, false);
                                CreateCookieInfoUser(AccountsRepository.GetUserData(account.email));
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

        /// <summary>
        /// Action para requisitar troca de senha.
        /// </summary>
        /// <param name="form">FormCollection form.</param>
        [HttpPost]
        public void changekey(FormCollection form)
        {
            string email = form["email"].ToLower(); 

            if (AccountsRepository.GetEmail(email))
            {
                dynamic account = AccountsRepository.GetUserData(email);

                if (account.Account.password != null && account.Account.password != string.Empty)
                {
                    string emailC = encrypt(account.Account.email);
                    string passwordC = account.Account.password;

                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                    mail.To.Add("dnrevaldt@gmail.com");
                    mail.From = new MailAddress(email);
                    mail.Subject = "Mensagem do Portal";
                    string Body = "<a href=http://localhost:51918/account/changekeyconfirm?email=" + emailC + "&password=" + passwordC +"> << Alterar Senha >> </a>";
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
                        var objectToSerialize = new ReturnJson { success = true, message = "Obrigado, foi enviado para o seu email a alteração de senha." };
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
                    var objectToSerialize = new ReturnJson { success = false, message = "Sua conta não está vinculada a nenhuma senha, use o facebook para logar ao portal." };
                    Response.Write(JsonConvert.SerializeObject(objectToSerialize));
                }
                

            }
            else
            {
                var objectToSerialize = new ReturnJson { success = false, message = "Ops.. estamos com problemas. Verifique seu email e tente novamente." };
                Response.Write(JsonConvert.SerializeObject(objectToSerialize));
            }

        }

        /// <summary>
        /// Action para alterar a senha do usuário.
        /// </summary>
        /// <param name="email">String email.</param>
        /// <param name="password">String Password.</param>
        /// <returns>View changekeyconfirm.</returns>
        [HttpGet]
        public ActionResult changekeyconfirm(string email, string password)
        {
            try
            {
                var pEmail = Server.UrlTokenDecode(email);
                string emailD = System.Text.UTF8Encoding.UTF8.GetString(pEmail);

                dynamic account = AccountsRepository.GetUserData(emailD);

                if (account.Account.password == password)
                {
                    ViewBag.email = emailD;
                    return View("changekeyconfirm");
                }
            }
            catch (Exception)
            {
                return View("Error");
            }

            return View("Error");
                    
        }

        /// <summary>
        /// Alterar senha do usuário.
        /// </summary>
        /// <param name="form">FormCollection form.</param>
        [HttpPost]
        public void changekeyconfirmed(FormCollection form)
        {
            try
            {
                string email = form["email"];
                string password = AccountsRepository.EncryptPassword(form["password"]);

                AccountsRepository.UpdateUserPassword(email, password);

                objectToSerializeSuc = new ReturnJson { success = true, message = "Senha alterada com sucesso, faça login e aproveite!", returnUrl = null, location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
            }
            catch (Exception)
            {
                objectToSerializeSuc = new ReturnJson { success = false, message = "Ops.. estamo com problemas. Tente novamente.", returnUrl = null, location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
            }
            
        }

        /// <summary>
        /// Cria o cookie com todas informações do usuário.
        /// </summary>
        /// <param name="userInfo">Dynamic userInfo.</param>
        private void CreateCookieInfoUser(dynamic userInfo)
        {
            Response.Cookies["userInfo"]["id_account"] = userInfo.Account.idAccount.ToString("D9");
            Response.Cookies["userInfo"]["email"] = encrypt(userInfo.Account.email);
            Response.Cookies["userInfo"]["password"] = encrypt(userInfo.Account.password);
            Response.Cookies["userInfo"]["acessToken"] = encrypt(userInfo.Account.acessToken);
            Response.Cookies["userInfo"]["id_person"] = encrypt(userInfo.idPerson.ToString("D9"));
            if (userInfo.accountType.ToString() == "T")
            {
                Response.Cookies["userInfo"]["id_type_account"] = encrypt(userInfo.idTeacher.ToString("D9"));
            }
            else
            {
                Response.Cookies["userInfo"]["id_type_account"] = encrypt(userInfo.idStudent.ToString("D9"));
            }
            Response.Cookies["userInfo"]["first_name"] = encrypt(userInfo.firstName);
            Response.Cookies["userInfo"]["last_name"] = encrypt(userInfo.lastName);
            Response.Cookies["userInfo"]["account_type"] = encrypt(userInfo.accountType.ToString());
            Response.Cookies["userInfo"]["birthday"] = encrypt(userInfo.dateOfBirth.ToString("dd/MM/yyyy"));
            Response.Cookies["userInfo"]["gender"] = encrypt(userInfo.gender.ToString());
            Response.Cookies["userInfo"]["location"] = encrypt(userInfo.city);
            Response.Cookies["userInfo"]["locale"] = encrypt(userInfo.country);

            setcookie("userImage", userInfo.urlImageProfile);
        }

        /// <summary>
        /// Criptografa string.
        /// </summary>
        /// <param name="value">String value.</param>
        /// <returns>String encrypted.</returns>
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

        /// <summary>
        /// Método para deslogar usuário e destruir cookies existentes.
        /// </summary>
        /// <returns>Redirect to Home.</returns>
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

            return RedirectToAction("home", "site");
        }

        /// <summary>
        /// Método para setar cookie criptografado.
        /// </summary>
        /// <param name="key">String key.</param>
        /// <param name="value">String value.</param>
        public void setcookie(string key, string value)
        {
            var encValue = Server.UrlTokenEncode(UTF8Encoding.UTF8.GetBytes(value));
            var c = new HttpCookie(key, encValue) { Expires = DateTime.Now.AddDays(7) };
            Response.Cookies.Add(c);
        }
    }
}