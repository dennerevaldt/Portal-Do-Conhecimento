﻿using ADOMySQL;
using MySql.Data.MySqlClient;
using PDS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PDS.Models.Repository
{
    public class StudentsRepository
    {
        /// <summary>
        /// Método para inserir novo Student no DB.
        /// </summary>
        /// <param name="student">Object student.</param>
        public void Create(Students student, MySQL database)
        {
            #region Insert Person

            MySqlCommand cmmPerson = new MySqlCommand();
            StringBuilder sqlPerson = new StringBuilder();

            cmmPerson.Parameters.AddWithValue("@firstName", student.firstName);
            cmmPerson.Parameters.AddWithValue("@lastName", student.lastName);
            cmmPerson.Parameters.AddWithValue("@gender", student.gender);
            cmmPerson.Parameters.AddWithValue("@accountType", student.accountType);
            cmmPerson.Parameters.AddWithValue("@dateOfBirth", student.dateOfBirth);
            cmmPerson.Parameters.AddWithValue("@city", student.city);
            cmmPerson.Parameters.AddWithValue("@country", student.country);
            cmmPerson.Parameters.AddWithValue("@urlImageProfile", student.urlImageProfile);
            cmmPerson.Parameters.AddWithValue("@idAccount", student.Account.idAccount);

            sqlPerson.Append("CALL insertPerson(@firstName, @lastName, @gender, @accountType, @dateOfBirth, @city, @country, @urlImageProfile, @idAccount)");

            cmmPerson.CommandText = sqlPerson.ToString();

            //Int64 idReturnPerson = ADOMySQL.MySQL.ExecuteEscalar(cmmPerson);

            #endregion

            #region Insert Student

            //MySqlCommand cmmStudent = new MySqlCommand();
            //StringBuilder sqlStudent = new StringBuilder();

            //cmmStudent.Parameters.AddWithValue("@idPerson", idReturnPerson);

            //sqlStudent.Append("CALL insertStudent(@idPerson)");

            //cmmStudent.CommandText = sqlStudent.ToString();

            //ADOMySQL.MySQL.ExecuteNonQuery(cmmStudent);

            #endregion

            try
            {
                Int64 idReturnPerson = database.ExecuteScalar(cmmPerson);

                #region Insert Student
                MySqlCommand cmmStudent = new MySqlCommand();
                StringBuilder sqlStudent = new StringBuilder();

                cmmStudent.Parameters.AddWithValue("@idPerson", idReturnPerson);

                sqlStudent.Append("CALL insertStudent(@idPerson)");

                cmmStudent.CommandText = sqlStudent.ToString();
                #endregion

                database.ExecuteNonQuery(cmmStudent);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}