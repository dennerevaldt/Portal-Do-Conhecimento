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
                            student = new Students
                            {
                                idStudent = (Int64)dr["idStudent"]
                            },

                            idClasse = (Int64)dr["idClasse"],

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

        /// <summary>
        /// Método para retornar as estrelas de um estudante, que posteriormente será concatenado com um objeto estudante.
        /// </summary>
        /// <param name="idStudent"></param>
        /// <returns></returns>
        public ClassesStudents GetOne(Int64 idStudent, Int64 idClasse)
        {
            MySQL database = MySQL.GetInstancia();
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            ClassesStudents classeStudent = new ClassesStudents();
            StudentsRepository repStudent = new StudentsRepository();

            cmm.Parameters.AddWithValue("@idStudent", idStudent);
            cmm.Parameters.AddWithValue("@idClasse", idClasse);

            sql.Append("CALL getOneClassesStudents(@idStudent,@idClasse)");

            cmm.CommandText = sql.ToString();

            try
            {
                dr = database.ExecuteReader(cmm);

                while (dr.Read())
                {
                    classeStudent = 
                        new ClassesStudents
                        {
                            stars = dr.IsDBNull(dr.GetOrdinal("stars")) ? 0 : (int)dr["stars"]
                        };
                }

                dr.Close();

            }
            catch (Exception ex)
            {
                dr.Close();
                throw ex;
            }

            return classeStudent;
        }

        /// <summary>
        /// Método para buscar as estrelinhas dos alunos de todas turmas de um aluno
        /// </summary>
        /// <param name="idStudent"></param>
        /// <returns></returns>
        public List<ClassesStudents> GetStarStudents(Int64 idStudent)
        {
            MySQL database = MySQL.GetInstancia();
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            List<ClassesStudents> cStudents = new List<ClassesStudents>();
            Classes classe = new Classes();
            List<ClassesPublicationsTeachers> listClassesPubTeac = new List<ClassesPublicationsTeachers>();
            List<PublicationsTeachers> listPublicationsTeachers = new List<PublicationsTeachers>();

            cmm.Parameters.AddWithValue("@idStudent", idStudent);

            sql.Append("CALL getStarsStudents(@idStudent)");

            cmm.CommandText = sql.ToString();

            try
            {
                dr = database.ExecuteReader(cmm);

                while (dr.Read())
                {
                    cStudents.Add(
                    new ClassesStudents
                    {
                        student = new Students
                        {
                            firstName = (string)dr["firstName"],
                            lastName = (string)dr["lastName"],
                            urlImageProfile = (string)dr["urlImageProfile"],
                            idStudent = (Int64)dr["idStudent"]
                        },
                        idClasse = (Int64)dr["idClasse"],
                        stars = (int)dr["stars"]
                    });
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                dr.Close();
                throw ex;
            }

            return cStudents;
   
        }
    }
}