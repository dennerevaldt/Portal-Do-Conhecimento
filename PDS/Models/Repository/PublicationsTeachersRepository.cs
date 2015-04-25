using ADOMySQL;
using MySql.Data.MySqlClient;
using PDS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PDS.Models.Repository
{
    public class PublicationsTeachersRepository
    {
        public static MySqlDataReader dr;

        /// <summary>
        /// Método para inserir uma nova publicação.
        /// </summary>
        /// <param name="post">PublicationTeachers post.</param>
        /// <returns>Int64 idPublication.</returns>
        public Int64 Create(string textPublication, Int64 idTeacher)
        {
            MySQL database = MySQL.GetInstancia();
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            Int64 idReturn;

            cmm.Parameters.AddWithValue("@datePublication", DateTime.Now);
            cmm.Parameters.AddWithValue("@textPublication", textPublication);
            cmm.Parameters.AddWithValue("@idTeacher", idTeacher);

            sql.Append("CALL insertPublicationsTeachers(@datePublication, @textPublication, @idTeacher)");

            cmm.CommandText = sql.ToString();

            try
            {
                database.BeginWork();
                idReturn = database.ExecuteScalar(cmm);
                database.CommitWork();
            }
            catch (Exception ex)
            {
                database.RollBack();
                throw ex;
            }

            return idReturn;

        }

        /// <summary>
        /// Método para deletar a postagem de um professor.
        /// </summary>
        /// <param name="idPublication">Int64 idPublication.</param>
        public void Delete(Int64 idPublication)
        {
            MySQL database = MySQL.GetInstancia();
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@idPublication", idPublication);

            sql.Append("CALL deletePostTeacher(@idPublication)");

            cmm.CommandText = sql.ToString();

            try
            {
                database.BeginWork();
                database.ExecuteNonQuery(cmm);
                database.CommitWork();
            }
            catch (Exception ex)
            {
                database.RollBack();
                throw ex;
            }
        }

        /// <summary>
        /// Método para retornar a url de um anexo de uma postagem.
        /// </summary>
        /// <param name="idPublication">Int64 idPublication.</param>
        /// <returns>String url.</returns>
        public string GetUtlAttachment(Int64 idPublication)
        {
            MySQL database = MySQL.GetInstancia(); ;
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            Accounts account = new Accounts();
            string url = string.Empty;

            cmm.Parameters.AddWithValue("@idPublication", idPublication);

            sql.Append("CALL getUrlAttachment(@idPublication)");

            cmm.CommandText = sql.ToString();

            try
            {
                dr = database.ExecuteReader(cmm);

                while (dr.Read())
                {
                    url = (string)dr["urlAttachment"];
                }

                dr.Close();

            }
            catch (Exception)
            {
                dr.Close();
                throw;
            }

            return url;
        }

        /// <summary>
        /// Método para buscar todas publicações dos seguidores que um professor segue.
        /// </summary>
        /// <param name="idFollower"></param>
        /// <returns></returns>
        public List<PublicationsTeachers> GetPublications(Int64 idFollower)
        {
            MySQL database = MySQL.GetInstancia();
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            List<PublicationsTeachers> listPubTeac = new List<PublicationsTeachers>();

            cmm.Parameters.AddWithValue("@idFollower", idFollower);

            sql.Append("CALL getPublicationsTeacherProfile(@idFollower)");

            cmm.CommandText = sql.ToString();

            try
            {
                dr = database.ExecuteReader(cmm);

                while (dr.Read())
                {
                    listPubTeac.Add(
                    new PublicationsTeachers
                    {
                        idPublication = (Int64)dr["idPublication"],
                        datePublication = (DateTime)dr["datePublication"],
                        textPublication = (string)dr["textPublication"],

                        teacher = new Teachers 
                        {
                            firstName = (string)dr["firstName"],
                            lastName = (string)dr["lastName"],
                            urlImageProfile = (string)dr["urlImageProfile"]
                        },

                        attachment = new Attachments
                        {
                            idAttachment = dr.IsDBNull(dr.GetOrdinal("idAttachment")) ? 0 : dr.GetInt64(dr.GetOrdinal("idAttachment")),     
                            urlAttachment = dr.IsDBNull(dr.GetOrdinal("urlAttachment")) ? "" : dr.GetString(dr.GetOrdinal("urlAttachment"))
                        }

                    });
                }

                dr.Close();

            }
            catch (Exception ex)
            {
                dr.Close();
                throw ex;
            }

            return listPubTeac;
        }

    }
}