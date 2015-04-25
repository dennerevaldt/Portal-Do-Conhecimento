using Newtonsoft.Json;
using PDS.Models.Entities;
using PDS.Models.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PDS.Controllers
{
    public class teachersController : Controller
    {
        /// <summary>
        /// Action para fazer o upload de um post.
        /// </summary>
        /// <param name="file">HttpPostedFileBase file.</param>
        /// <param name="form">FormCollection form.</param>
        [HttpPost]
        public void UploadPost(HttpPostedFileBase file, FormCollection form)
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

                    //prepare url attachment
                    string path = "/Content/Uploads/Posts/Teachers/" + idPublication + type;
                    file.SaveAs(Path.Combine(Server.MapPath(path)));

                    //insert attachment
                    AttachmentsRepository repAtt = new AttachmentsRepository();
                    repAtt.Create(path, idPublication);

                }
                else
                {
                    //request cookie data user
                    HttpCookie userInfo = Request.Cookies["userInfo"];
                    var CidT = Server.UrlTokenDecode(userInfo["id_type_account"]);
                    string idTeacher = System.Text.UTF8Encoding.UTF8.GetString(CidT);

                    //insert publications teachers
                    PublicationsTeachersRepository repPubTeachers = new PublicationsTeachersRepository();
                    Int64 idPublication = repPubTeachers.Create(form["message"], Int64.Parse(idTeacher));
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Action para retornar as publicações de professores seguidores.
        /// </summary>
        [HttpPost]
        public void GetPublications()
        {
            //Get id Follower teacher
            HttpCookie userInfo = Request.Cookies["userInfo"];
            var idAccTp = Server.UrlTokenDecode(userInfo["id_type_account"]);
            string id_type_account = System.Text.UTF8Encoding.UTF8.GetString(idAccTp);

            try
            {
                PublicationsTeachersRepository repPubTeac = new PublicationsTeachersRepository();
                List<PublicationsTeachers> listPublications = repPubTeac.GetPublications(Int64.Parse(id_type_account));

                Response.Write(JsonConvert.SerializeObject(listPublications));
            }
            catch (Exception ex)
            {            
                throw ex;
            }
        }
    }
}