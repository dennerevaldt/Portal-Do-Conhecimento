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
        /// <summary>
        /// Método para inserir uma nova publicação.
        /// </summary>
        /// <param name="post">PublicationTeachers post.</param>
        /// <returns>Int64 idPublication.</returns>
        public Int64 Create(string textPublication, Int64 idTeacher)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
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

    }
}