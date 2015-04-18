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
    public class AttachmentsRepository
    {
        /// <summary>
        /// Método para fazer um insert na tabela Attachments.
        /// </summary>
        /// <param name="urlAttachment">String urlAttachment.</param>
        /// <param name="idPublication">Int64 idPublication.</param>
        public void Create(string urlAttachment, Int64 idPublication)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            Int64 idReturn;

            cmm.Parameters.AddWithValue("@urlAttachment", urlAttachment);
            cmm.Parameters.AddWithValue("@idPublication", idPublication);

            sql.Append("CALL insertAttachment(@urlAttachment, @idPublication)");

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

        }
    }
}