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
    public class ClassesPublicationsTeachersRepository
    {
        public MySqlDataReader dr;

        /// <summary>
        /// Método para fazer um insert na tabela ClassesPublicationsTeachers.
        /// </summary>
        /// <param name="idPublication">Int64 idPublication.</param>
        /// <param name="idClasse">Int64 idClasse.</param>
        public void Create(Int64 idPublication, Int64 idClasse)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@idPublication", idPublication );
            cmm.Parameters.AddWithValue("@idClasse", idClasse);

            sql.Append("CALL insertClassesPublicationTeachers(@idPublication, @idClasse)");

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
        /// Método para retornar as publicações de um turma específica.
        /// </summary>
        /// <param name="idClass">Int64 idClass.</param>
        /// <returns>List ClassesPublicationsTeachers.</returns>
        public List<ClassesPublicationsTeachers> GetPublications(Int64 idClass)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            List<ClassesStudents> cStudents = new List<ClassesStudents>();
            List<Classes> listClasses = new List<Classes>();
            List<ClassesPublicationsTeachers> listClassesPubTeac = new List<ClassesPublicationsTeachers>();

            cmm.Parameters.AddWithValue("@idClass", idClass);

            sql.Append("CALL getPublications(@idClass)");

            cmm.CommandText = sql.ToString();

            try
            {
                dr = database.ExecuteReader(cmm);

                while (dr.Read())
                {
                    listClassesPubTeac.Add(
                    new ClassesPublicationsTeachers
                    {
                        publicationsTeachers = new PublicationsTeachers
                        {
                            idPublication = (Int64)dr["idPublication"],
                            datePublication = (DateTime)dr["datePublication"],
                            textPublication = (string)dr["textPublication"],

                            attachment = new Attachments
                            {
                                idAttachment = (Int64)dr["idAttachment"],
                                urlAttachment = (string)dr["urlAttachment"]
                            }
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

            return listClassesPubTeac;
        }
    }
}