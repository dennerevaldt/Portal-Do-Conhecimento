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
    public class ClassesStudentsRepository
    {
        MySqlDataReader dr;

        /// <summary>
        /// Método para retornar os alunos de uma turma.
        /// </summary>
        /// <param name="idClass">Int64 idClass.</param>
        /// <returns>List ClassesStudents.</returns>
        public List<ClassesStudents> GetAll(Int64 idClass)
        {
            MySQL database = MySQL.GetInstancia();
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            List<ClassesStudents> listClassesStudents = new List<ClassesStudents>();
            StudentsRepository repStudent = new StudentsRepository();

            cmm.Parameters.AddWithValue("@idClass", idClass);

            sql.Append("CALL getAllClassesStudents(@idClass)");

            cmm.CommandText = sql.ToString();

            try
            {
                dr = database.ExecuteReader(cmm);

                while (dr.Read())
                {
                    listClassesStudents.Add(
                        new ClassesStudents
                        {
                            student = repStudent.GetOne((Int64)dr["idStudent"]),                           
                            stars = dr.IsDBNull(dr.GetOrdinal("stars")) ? 0 : (int)dr["stars"]
                        }
                    );

                }

                dr.Close();

            }
            catch (Exception ex)
            {
                dr.Close();
                throw ex;
            }

            return listClassesStudents;
        }
    }
}