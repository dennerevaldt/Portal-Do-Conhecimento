using MySql.Data.MySqlClient;
using PDS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PDS.Models.Repository
{
    public class TeachersRepository
    {
        public static void Create(Teachers teacher)
        {
            #region Insert Person
            
            MySqlCommand cmmPerson = new MySqlCommand();
            StringBuilder sqlPerson = new StringBuilder();

            cmmPerson.Parameters.AddWithValue("@firstName", teacher.firstName);
            cmmPerson.Parameters.AddWithValue("@lastName", teacher.lastName);
            cmmPerson.Parameters.AddWithValue("@gender", teacher.gender);
            cmmPerson.Parameters.AddWithValue("@accountType", teacher.accountType);
            cmmPerson.Parameters.AddWithValue("@dateOfBirth", teacher.dateOfBirth);
            cmmPerson.Parameters.AddWithValue("@city", teacher.city);
            cmmPerson.Parameters.AddWithValue("@country", teacher.country);
            cmmPerson.Parameters.AddWithValue("@urlImageProfile", teacher.urlImageProfile);
            cmmPerson.Parameters.AddWithValue("@idAccount", teacher.Account.idAccount);

            sqlPerson.Append("CALL insertPerson(@firstName, @lastName, @gender, @accountType, @dateOfBirth, @city, @country, @urlImageProfile, @idAccount)");

            cmmPerson.CommandText = sqlPerson.ToString();

            Int64 idReturnPerson = ADOMySQL.MySQL.ExecuteEscalar(cmmPerson);

            #endregion

            #region Insert Teacher

            MySqlCommand cmmTeacher = new MySqlCommand();
            StringBuilder sqlTeacher = new StringBuilder();

            cmmTeacher.Parameters.AddWithValue("@idPerson", idReturnPerson);

            sqlTeacher.Append("CALL insertTeacher(@idPerson)");

            cmmTeacher.CommandText = sqlTeacher.ToString();

            ADOMySQL.MySQL.ExecuteNonQuery(cmmTeacher);

            #endregion
        }
    }
}