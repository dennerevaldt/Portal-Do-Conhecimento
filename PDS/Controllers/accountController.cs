using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PDS.Models.Entities;
using PDS.Models.Repository;
using PDS.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
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
        private static string path = string.Empty;
        private static string extension = string.Empty;
        private static dynamic person;
        private static string type;


        /// <summary>
        /// Action para retornar painel de gerenciamento da conta de usuário.
        /// </summary>
        /// <returns>View manage.</returns>
        [Authorize]
        public ActionResult manage()
        {
            return View("manage");
        }

        /// <summary>
        /// Action para alterar dados cadastrais do usuário.
        /// </summary>
        /// <param name="form">FormCollection form.</param>
        [Authorize]
        public void changedata(FormCollection form)
        {
            try
            {
                Accounts account = new Accounts();
                account.email = form["email"];
                account.idAccount = Convert.ToInt64(form["idAccount"]);

                AccountsRepository.UpdateAccount(account);

                if (form["accountType"] == "T")
                {
                    Teachers teacher = new Teachers();

                    teacher.idPerson = Convert.ToInt64(form["idPerson"]);
                    teacher.firstName = form["firstName"];
                    teacher.lastName = form["lastName"];
                    teacher.gender = Convert.ToChar(form["gender"]);

                    DateTime dataOrg = Convert.ToDateTime(form["dateOfbirth"]);
                    string formatForMySql = dataOrg.ToString("yyyy-MM-dd HH:mm");
                    DateTime dateValue = DateTime.Parse(formatForMySql);
                    teacher.dateOfBirth = dateValue;

                    teacher.city = form["location"];
                    teacher.country = form["country"];

                    AccountsRepository.UpdatePerson(teacher);
                }
                else
                {
                    Students student = new Students();

                    student.idPerson = Convert.ToInt64(form["idPerson"]);
                    student.firstName = form["firstName"];
                    student.lastName = form["lastName"];
                    student.gender = Convert.ToChar(form["gender"]);

                    DateTime dataOrg = Convert.ToDateTime(form["dateOfbirth"]);
                    string formatForMySql = dataOrg.ToString("yyyy-MM-dd HH:mm");
                    DateTime dateValue = DateTime.Parse(formatForMySql);
                    student.dateOfBirth = dateValue;

                    student.city = form["location"];
                    student.country = form["country"];

                    AccountsRepository.UpdatePerson(student);
                }

                objectToSerializeSuc = new ReturnJson { success = true, message = "Dados atualizados com sucesso.", returnUrl = null, location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
                
            }
            catch (Exception)
            {
                objectToSerializeSuc = new ReturnJson { success = false, message = "Ops.. estamos com problemas, verifique todos os campos e tente novamente.", returnUrl = null, location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
            }
        }

        /// <summary>
        /// Método para altera a imagem do perfil.
        /// </summary>
        /// <param name="inputFile">HttpostedFileBase inputFile.</param>
        [Authorize]
        [HttpPost]
        public void changephoto(HttpPostedFileBase inputFile)
        {
            try
            {
                HttpCookie cookie_image = Request.Cookies["userImage"];
                var decValueImage = Server.UrlTokenDecode(cookie_image.Value);
                string url_image = System.Text.UTF8Encoding.UTF8.GetString(decValueImage);

                HttpCookie cookie_userInfo = Request.Cookies["userInfo"];
                var decValueUser = Server.UrlTokenDecode(cookie_userInfo["account_type"]);
                string typeAccount = System.Text.UTF8Encoding.UTF8.GetString(decValueUser);

                var decId = Server.UrlTokenDecode(cookie_userInfo["id_account"]);
                string idAccount = System.Text.UTF8Encoding.UTF8.GetString(decId);

                if (typeAccount == "T")
                {
                    if (url_image != "/Content/images/noPhoto.png")
                    {                                     
                        string fullPath = Request.MapPath(url_image);

                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }

                        Image image = System.Drawing.Image.FromStream(inputFile.InputStream);

                        Image imgNew = new System.Drawing.Bitmap(image, new System.Drawing.Size(200, 200));

                        imgNew.Save(Path.Combine(Server.MapPath(url_image)));

                    }
                    else
                    {
                        Image image = System.Drawing.Image.FromStream(inputFile.InputStream);

                        Image imgNew = new System.Drawing.Bitmap(image, new System.Drawing.Size(200, 200));

                        string path = "/Content/Uploads/ImagesProfile/Teachers/" + idAccount + ".jpg";

                        imgNew.Save(Path.Combine(Server.MapPath(path)));

                        AccountsRepository.UpdateUrlImage(path, Convert.ToInt64(idAccount));

                        setcookie("userImage", path);
                    }
                }
                else
                {
                    if (url_image != "/Content/images/noPhoto.png")
                    {
                        string fullPath = Request.MapPath(url_image);

                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }

                        Image image = System.Drawing.Image.FromStream(inputFile.InputStream);

                        Image imgNew = new System.Drawing.Bitmap(image, new System.Drawing.Size(200, 200));

                        imgNew.Save(Path.Combine(Server.MapPath(url_image)));
                    }
                    else
                    {
                        Image image = System.Drawing.Image.FromStream(inputFile.InputStream);

                        Image imgNew = new System.Drawing.Bitmap(image, new System.Drawing.Size(200, 200));

                        string path = "/Content/Uploads/ImagesProfile/Students/" + idAccount + ".jpg";

                        imgNew.Save(Path.Combine(Server.MapPath(path)));

                        AccountsRepository.UpdateUrlImage(path, Convert.ToInt64(idAccount));

                        setcookie("userImage", path);
                    }
                }

                objectToSerializeSuc = new ReturnJson { success = true, message = "Imagem alterada com sucesso.", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
            }
            catch (Exception)
            {
                objectToSerializeSuc = new ReturnJson { success = false, message = "Estamos com problemas, verifique a imagem escolhida e tente novamente.", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
            }
            
        }

        /// <summary>
        /// Action para deletar conta de usuário.
        /// </summary>
        /// <param name="form">FormCollection form.</param>
        [Authorize]
        [HttpPost]
        public void deleteaccount(FormCollection form)
        {
            try
            {
                string email = form["email"];
                AccountsRepository.Delete(email);

                HttpCookie cookie_userInfo = Request.Cookies["userInfo"];
                var decId = Server.UrlTokenDecode(cookie_userInfo["id_account"]);
                string idAccount = System.Text.UTF8Encoding.UTF8.GetString(decId);

                var decValueUser = Server.UrlTokenDecode(cookie_userInfo["account_type"]);
                string typeAccount = System.Text.UTF8Encoding.UTF8.GetString(decValueUser);

                HttpCookie cookie_image = Request.Cookies["userImage"];
                var decValueImage = Server.UrlTokenDecode(cookie_image.Value);
                string url_image = System.Text.UTF8Encoding.UTF8.GetString(decValueImage);

                string fullPath = Request.MapPath(url_image);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                

                objectToSerializeSuc = new ReturnJson { success = true, message = null, returnUrl = null, location = "/site/home" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));

            }
            catch (Exception)
            {
                objectToSerializeSuc = new ReturnJson { success = false, message = "Ops.. Estamos com problemas, tente novamente.", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
            }
        }

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
                if (AccountsRepository.GetEmail(form["inputEmailT"].ToLower()) == false)
                {
                    if (inputFile != null)
                    {
                        // get type file
                        type = Path.GetExtension(inputFile.FileName);
                    }
                    else
                    {
                        type = "notImage";
                    }

                    if(type == ".jpg" || type == ".png" || type == "notImage")
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

                        DateTime dataOrg = Convert.ToDateTime(form["inputBirthdayT"]);
                        string formatForMySql = dataOrg.ToString("yyyy-MM-dd HH:mm");
                        DateTime dateValue = DateTime.Parse(formatForMySql);
                        teacher.dateOfBirth = dateValue;

                        teacher.accountType = Convert.ToChar("T");
                        teacher.city = form["inputCityT"];
                        teacher.country = form["inputCountryT"];

                        if (inputFile != null)
                        {
                            Image image = System.Drawing.Image.FromStream(inputFile.InputStream);

                            Image imgNew = new System.Drawing.Bitmap(image, new System.Drawing.Size(200,200));

                            imgNew.Save(Path.Combine(Server.MapPath("~/Content/Uploads/ImagesProfile/Teachers"),idAccount.ToString()+".jpg"));

                            teacher.urlImageProfile = "/Content/Uploads/ImagesProfile/Teachers/"+idAccount.ToString()+".jpg";
                        }
                        else
                        {
                            teacher.urlImageProfile = "/Content/images/noPhoto.png";
                        }
            
                        TeachersRepository.Create(teacher);

                        dynamic user = AccountsRepository.GetUserData(form["inputEmailT"].ToLower());

                        CreateCookieInfoUser(user);
                        FormsAuthentication.SetAuthCookie(form["inputEmailT"].ToLower(), false);

                        // Return Sucess
                        objectToSerializeSuc = new ReturnJson { success = true, message = "Contra criada com sucesso. Entrando...", returnUrl = "", location = "/home/index" };
                        Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
                    }
                    else
                    {
                        objectToSerializeSuc = new ReturnJson { success = false, message = "Formato de imagem não permitido, tente novamente.", returnUrl = "", location = "" };
                        Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
                    }
                }
                else
                {
                    objectToSerializeSuc = new ReturnJson { success = false, message = "Email já existente no portal, informe outro e tente novamente.", returnUrl = "", location = "" };
                    Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
                }
            }
            catch (Exception)
            {
                // Return Error
                objectToSerializeSuc = new ReturnJson { success = false, message = "Ops. Estamos com problemas, verifique os campos e tente novamente.", returnUrl = "", location = "" };
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
                if (AccountsRepository.GetEmail(form["inputEmail"].ToLower()) == false)
                {
                    if(inputFile != null){
                        // get type file
                        type = Path.GetExtension(inputFile.FileName);
                    }
                    else
                    {
                        type = "notImage";
                    }

                    if (type == ".jpg" || type == ".png" || type == "notImage")
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
                            Image image = System.Drawing.Image.FromStream(inputFile.InputStream);

                            Image imgNew = new System.Drawing.Bitmap(image, new System.Drawing.Size(200, 200));

                            imgNew.Save(Path.Combine(Server.MapPath("~/Content/Uploads/ImagesProfile/Students"), idAccount.ToString() + ".jpg"));

                            student.urlImageProfile = "/Content/Uploads/ImagesProfile/Students/" + idAccount.ToString() + ".jpg";

                        }
                        else
                        {
                            student.urlImageProfile = "/Content/images/noPhoto.png";
                        }

                        StudentsRepository.Create(student);

                        dynamic user = AccountsRepository.GetUserData(form["inputEmail"].ToLower());

                        CreateCookieInfoUser(user);
                        FormsAuthentication.SetAuthCookie(form["inputEmail"].ToLower(), false);

                        // Return Sucess
                        objectToSerializeSuc = new ReturnJson { success = true, message = "Contra criada com sucesso. Entrando...", returnUrl = "", location = "/home/index" };
                        Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
                    }
                    else
                    {
                        objectToSerializeSuc = new ReturnJson { success = false, message = "Formato de imagem não permitido, tente novamente.", returnUrl = "", location = "" };
                        Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
                    }
                }
                else
                {
                    objectToSerializeSuc = new ReturnJson { success = false, message = "Email já existente no portal, informe outro e tente novamente.", returnUrl = "", location = "" };
                    Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
                }
            }
            catch (Exception)
            {
                // Return Error
                objectToSerializeSuc = new ReturnJson { success = false, message = "Ops. Estamos com problemas, verifique os campos e tente novamente.", returnUrl = "", location = "" };
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
                teacher.Account.acessToken = form["acessToken"];

                teacher.urlImageProfile = "/Content/Uploads/ImagesProfile/Teachers/" + idAccount.ToString() + ".jpg";
                string url = (form["urlImageProfile"].ToString());
                string localSave = Path.Combine(Server.MapPath("~/Content/Uploads/ImagesProfile/Teachers"), idAccount.ToString() + ".jpg");

                WebClient client = new WebClient();
                client.DownloadFile(url, localSave);

                TeachersRepository.Create(teacher);

                dynamic userInfo = AccountsRepository.GetUserData(account.email);

                Response.Cookies["userInfo"]["id_account"] = encrypt(userInfo.Account.idAccount.ToString("D1"));
                Response.Cookies["userInfo"]["email"] = encrypt(userInfo.Account.email);
                Response.Cookies["userInfo"]["acessToken"] = encrypt(userInfo.Account.acessToken);
                Response.Cookies["userInfo"]["id_person"] = encrypt(userInfo.idPerson.ToString("D1"));
                Response.Cookies["userInfo"]["id_type_account"] = encrypt(userInfo.idTeacher.ToString("D1"));
                Response.Cookies["userInfo"]["first_name"] = encrypt(userInfo.firstName);
                Response.Cookies["userInfo"]["last_name"] = encrypt(userInfo.lastName);
                Response.Cookies["userInfo"]["account_type"] = encrypt(userInfo.accountType.ToString());
                Response.Cookies["userInfo"]["birthday"] = encrypt(userInfo.dateOfBirth.ToString("dd/MM/yyyy"));
                Response.Cookies["userInfo"]["gender"] = encrypt(userInfo.gender.ToString());
                Response.Cookies["userInfo"]["location"] = encrypt(userInfo.city);
                Response.Cookies["userInfo"]["locale"] = encrypt(userInfo.country);

                setcookie("userImage", userInfo.urlImageProfile);

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
                student.country = form["country"];

                student.urlImageProfile = "/Content/Uploads/ImagesProfile/Students/" + idAccount.ToString() + ".jpg" ;
                string url = (form["urlImageProfile"].ToString());
                string localSave = Path.Combine(Server.MapPath("~/Content/Uploads/ImagesProfile/Students"), idAccount.ToString() + ".jpg");

                WebClient client = new WebClient();
                client.DownloadFile(url, localSave);
                
                StudentsRepository.Create(student);

                dynamic userInfo = AccountsRepository.GetUserData(account.email);

                Response.Cookies["userInfo"]["id_account"] = encrypt(userInfo.Account.idAccount.ToString("D1"));
                Response.Cookies["userInfo"]["email"] = encrypt(userInfo.Account.email);
                Response.Cookies["userInfo"]["acessToken"] = encrypt(userInfo.Account.acessToken);
                Response.Cookies["userInfo"]["id_person"] = encrypt(userInfo.idPerson.ToString("D1"));
                Response.Cookies["userInfo"]["id_type_account"] = encrypt(userInfo.idStudent.ToString("D1"));
                Response.Cookies["userInfo"]["first_name"] = encrypt(userInfo.firstName);
                Response.Cookies["userInfo"]["last_name"] = encrypt(userInfo.lastName);
                Response.Cookies["userInfo"]["account_type"] = encrypt(userInfo.accountType.ToString());
                Response.Cookies["userInfo"]["birthday"] = encrypt(userInfo.dateOfBirth.ToString("dd/MM/yyyy"));
                Response.Cookies["userInfo"]["gender"] = encrypt(userInfo.gender.ToString());
                Response.Cookies["userInfo"]["location"] = encrypt(userInfo.city);
                Response.Cookies["userInfo"]["locale"] = encrypt(userInfo.country);

                setcookie("userImage", userInfo.urlImageProfile);

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

                    if (account.email != null && account.password != null && account.password != string.Empty)
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
                    else
                    {
                        objectToSerializeErr = new ReturnJson { success = false, message = "Nenhuma senha cadastrada para este email, tente fazer login com Facebook." };
                        Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
                    }
                    
                }
                else
                {
                    objectToSerializeErr = new ReturnJson { success = false, message = "Os campos são obrigatórios." };
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
                    mail.To.Add(email);
                    mail.From = new MailAddress("dnrevaldt@gmail.com","Portal do conhecimento");
                    mail.Subject = "Mensagem do Portal";
                    string Body = "<p>Alterar senha do <b>Portal do Conhecimento</b></p> <br /> <a href=http://localhost:51918/account/changekeyconfirm?email=" + emailC + "&password=" + passwordC + "> << Alterar Senha >> </a>";
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

                objectToSerializeSuc = new ReturnJson { success = true, message = "Senha alterada com sucesso.", returnUrl = null, location = "" };
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
            Response.Cookies["userInfo"]["id_account"] = encrypt(userInfo.Account.idAccount.ToString("D1"));
            Response.Cookies["userInfo"]["email"] = encrypt(userInfo.Account.email);
            Response.Cookies["userInfo"]["password"] = encrypt(userInfo.Account.password);
            Response.Cookies["userInfo"]["acessToken"] = encrypt(userInfo.Account.acessToken);
            Response.Cookies["userInfo"]["id_person"] = encrypt(userInfo.idPerson.ToString("D1"));
            if (userInfo.accountType.ToString() == "T")
            {
                Response.Cookies["userInfo"]["id_type_account"] = encrypt(userInfo.idTeacher.ToString("D1"));
            }
            else
            {
                Response.Cookies["userInfo"]["id_type_account"] = encrypt(userInfo.idStudent.ToString("D1"));
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
        [Authorize]
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