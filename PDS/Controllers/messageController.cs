using Newtonsoft.Json;
using PDS.Models.Entities;
using PDS.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PDS.Controllers
{
    public class messageController : Controller
    {
        // GET: message
        public ActionResult index()
        {
            return View();
        }

        /// <summary>
        /// Action para retornar o número de mensagens novas de um aluno.
        /// </summary>
        /// <param name="form">FormCollection form.</param>
        public void getNumMessageStudent(FormCollection form)
        {
            try
            {
                Int64 idStudent = Int64.Parse(form["id"]);

                StudentsRepository repStudent = new StudentsRepository();
                Int64 numMessages = repStudent.GetNumMessages(idStudent);

                Response.Write(JsonConvert.SerializeObject(numMessages));
            }
            catch (Exception ex)
            {             
                throw ex;
            }
        }

        /// <summary>
        /// Action para retornar as mensagens de cada aluno.
        /// </summary>
        /// <param name="form">FormCollection form.</param>
        public void getMessagesClasses(FormCollection form)
        {
            try
            {
                Int64 idStudent = Int64.Parse(form["id"]);
                StudentsRepository repStudent = new StudentsRepository();
                List<Classes> classes = repStudent.getClasseMessage(idStudent);
                List<MessagesClasse> messages = repStudent.getMessagesClasse(idStudent);
                List<Classes> checkList = new List<Classes>();
                int countTurmas = 0;
                List<Classes> listFinal = new List<Classes>();

                //Método para concatenar as duas listas de turmas com suas respectivas mensagens
                //for (int i = 0; i < classes.Count; i++)
                //{
                //    listFinal.Add(classes[i]);
                //    listFinal[i].messagesClasse = new List<MessagesClasse>();

                //    for (int x = 0; x < messages.Count; x++)
                //    {
                //        if (messages[x].idClasse == classes[i].idClass)
                //        {
                //            MessagesClasse msg = new MessagesClasse
                //            {
                //                idClasse = messages[x].idClasse,
                //                idMessage = messages[x].idMessage,
                //                message = messages[x].message
                //            };

                //            listFinal[i].messagesClasse.Add(msg);
                //        }
                //    }
                    
                //}

                //Verifica quantas turmas diferentes existem no retorno do db.
                for (int i = 0; i < classes.Count; i++)
                {
                    if (!checkList.Exists(c => c.idClass == classes[i].idClass))
                    {
                        checkList.Add(classes[i]);
                    }
                }

                //Inicializa contador de turmas.
                countTurmas = checkList.Count;

                //Método para concatenar as duas listas de turmas com suas respectivas mensagens.
                for (int i = 0; i < countTurmas; i++)
                {
                    listFinal.Add(checkList[i]);
                    listFinal[i].messagesClasse = new List<MessagesClasse>();

                    for (int x = 0; x < messages.Count; x++)
                    {
                        if (messages[x].idClasse == checkList[i].idClass)
                        {
                            MessagesClasse msg = new MessagesClasse
                            {
                                idClasse = messages[x].idClasse,
                                idMessage = messages[x].idMessage,
                                message = messages[x].message
                            };

                            listFinal[i].messagesClasse.Add(msg);
                        }
                    }

                }

                Response.Write(JsonConvert.SerializeObject(listFinal));
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}