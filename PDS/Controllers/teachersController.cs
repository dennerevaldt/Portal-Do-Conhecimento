using Newtonsoft.Json;
using PDS.Models.Entities;
using PDS.Models.Repository;
using PDS.Models.Utilities;
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
        #region Atributos Staticos

        private static ReturnJson objectToSerializeErr;
        private static ReturnJson objectToSerializeSuc;

        #endregion

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

                    objectToSerializeSuc = new ReturnJson { success = true, message = "", returnUrl = "", location = "" };
                    Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));

                }
                else
                {
                    if (type == "notFile")
                    {
                        //request cookie data user
                        HttpCookie userInfo = Request.Cookies["userInfo"];
                        var CidT = Server.UrlTokenDecode(userInfo["id_type_account"]);
                        string idTeacher = System.Text.UTF8Encoding.UTF8.GetString(CidT);

                        //insert publications teachers
                        PublicationsTeachersRepository repPubTeachers = new PublicationsTeachersRepository();
                        Int64 idPublication = repPubTeachers.Create(form["message"], Int64.Parse(idTeacher));

                        objectToSerializeSuc = new ReturnJson { success = true, message = "", returnUrl = "", location = "" };
                        Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
                    }
                    else
                    {
                        objectToSerializeErr = new ReturnJson { success = false, message = "", returnUrl = "", location = "" };
                        Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
                    }

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
        public void GetPublications(string pStartList)
        {
            //Get id Follower teacher
            HttpCookie userInfo = Request.Cookies["userInfo"];
            var idAccTp = Server.UrlTokenDecode(userInfo["id_type_account"]);
            string id_type_account = System.Text.UTF8Encoding.UTF8.GetString(idAccTp);

            try
            {
                Int64 idTeacher = Int64.Parse(id_type_account);
                Int64 numPosts = Int64.Parse(pStartList);

                PublicationsTeachersRepository repPubTeac = new PublicationsTeachersRepository();
                List<PublicationsTeachers> listPublications = repPubTeac.GetPublications(idTeacher,numPosts);

                Response.Write(JsonConvert.SerializeObject(listPublications));
            }
            catch (Exception ex)
            {            
                throw ex;
            }
        }

        /// <summary>
        /// Action para retornar o número total de posts de um professor.
        /// </summary>
        public void CountPostsFollowersTeachers()
        {

            //Get id Follower teacher
            HttpCookie userInfo = Request.Cookies["userInfo"];
            var idAccTp = Server.UrlTokenDecode(userInfo["id_type_account"]);
            string id_type_account = System.Text.UTF8Encoding.UTF8.GetString(idAccTp);

            try
            {
                Int64 idTeacher = Int64.Parse(id_type_account);

                PublicationsTeachersRepository repPubTeac = new PublicationsTeachersRepository();
                Int64 numTotal  = repPubTeac.CountPostsFollowersTeachers(idTeacher);

                Response.Write(JsonConvert.SerializeObject(numTotal));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Action para deletar do banco uma postagem e os seus anexos.
        /// </summary>
        /// <param name="id">Int64 idPublication.</param>
        [HttpGet]
        public void deletePost(string id)
        {
            try
            {
                Int64 idPublication = Int64.Parse(id);
                PublicationsTeachersRepository repPubTeac = new PublicationsTeachersRepository();

                //Get url post in DB
                string urlAttachment = repPubTeac.GetUrlAttachment(idPublication);

                //Get full url post
                string fullPath = Request.MapPath(urlAttachment);

                //Delete attachment post
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                //Delete post in DB
                repPubTeac.Delete(idPublication);

                objectToSerializeSuc = new ReturnJson { success = true, message = "", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeSuc));
            }
            catch (Exception)
            {
                objectToSerializeErr = new ReturnJson { success = false, message = "Ops, estamos com problemas. Tente novamente.", returnUrl = "", location = "" };
                Response.Write(JsonConvert.SerializeObject(objectToSerializeErr));
            }
        }
    }
}