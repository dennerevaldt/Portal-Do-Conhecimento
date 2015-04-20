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
    public class StudentsRepository
    {
        MySqlDataReader dr;

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

        /// <summary>
        /// Método para retornar os dados de somente um Estudante.
        /// </summary>
        /// <param name="idStudent">Int64 idStudent.</param>
        /// <returns>Student student.</returns>
        public Students GetOne(Int64 idStudent)
        {
            MySQL database = MySQL.GetInstancia();
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            Students student = new Students();

            cmm.Parameters.AddWithValue("@idStudent", idStudent);

            sql.Append("CALL getOneStudent(@idStudent)");

            cmm.CommandText = sql.ToString();

            try
            {
                dr = database.ExecuteReader(cmm);

                while (dr.Read())
                {
                    student = new Students
                    {
                        idPerson = (Int64)dr["idPerson"],
                        accountType = (char)dr["accountType"],
                        firstName = (string)dr["firstName"],
                        lastName = (string)dr["lastName"],
                        gender = (char)dr["gender"],
                        dateOfBirth = (DateTime)dr["dateOfBirth"],
                        city = (string)dr["city"],
                        country = (string)dr["country"],
                        urlImageProfile = (string)dr["urlImageProfile"],
                        idStudent = (Int64)dr["idStudent"]
                    };

                }

                dr.Close();

            }
            catch (Exception ex)
            {
                dr.Close();
                throw ex;
            }

            return student;
        }

        /// <summary>
        /// Método para pesquisar estudantes no sistema pelo nome.
        /// </summary>
        /// <param name="name">String name.</param>
        /// <returns>List Students.</returns>
        public List<Students> SearchStudent(string name)
        {
            MySQL database = MySQL.GetInstancia();
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            List<Students> students = new List<Students>();

            cmm.Parameters.AddWithValue("@name", name);

            sql.Append("CALL searchStudent(@name)");

            cmm.CommandText = sql.ToString();

            try
            {
                dr = database.ExecuteReader(cmm);

                while (dr.Read())
                {
                    students.Add(new Students
                    {
                        idStudent = Convert.ToInt64(dr["idStudent"]),
                        idPerson = Convert.ToInt64(dr["idPerson"]),
                        firstName = Convert.ToString(dr["firstName"]),
                        lastName = Convert.ToString(dr["lastName"]),
                        gender = Convert.ToChar(dr["gender"]),
                        accountType = Convert.ToChar(dr["accountType"]),
                        dateOfBirth = Convert.ToDateTime(dr["dateOfBirth"]),
                        city = Convert.ToString(dr["city"]),
                        country = Convert.ToString(dr["country"]),
                        urlImageProfile = Convert.ToString(dr["urlImageProfile"]),
                        Account = new Accounts
                        {
                            idAccount = Convert.ToInt64(dr["idAccount"]),
                            email = Convert.ToString(dr["email"]),
                            password = Convert.ToString(dr["password"]),
                            acessToken = Convert.ToString(dr["acessToken"])
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

            return students;
        }

        /// <summary>
        /// Método para inserir um novo aluno a uma nova turma.
        /// </summary>
        /// <param name="idStudent">Int64 idStudent.</param>
        /// <param name="idClass">Int64 idClass.</param>
        public void InsertStudentInClasse(Int64 idStudent, Int64 idClass)
        {
            MySQL database = MySQL.GetInstancia();
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@idStudent", idStudent);
            cmm.Parameters.AddWithValue("@idClass", idClass);

            sql.Append("CALL insertStudentInClass(@idStudent,@idClass)");

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
        /// Método para retornar o número de novas mensagens do aluno.
        /// </summary>
        /// <param name="idStudent">Int64 idStudent.</param>
        /// <returns>Int64 numMessages.</returns>
        public Int64 GetNumMessages(Int64 idStudent)
        {
            MySQL database = MySQL.GetInstancia();
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            Int64 numMessages = 0;

            cmm.Parameters.AddWithValue("@idStudent", idStudent);

            sql.Append("CALL getNumMessagesStudent(@idStudent)");

            cmm.CommandText = sql.ToString();

            try
            {
                database.BeginWork();
                numMessages = database.ExecuteScalar(cmm);
                database.CommitWork();
            }
            catch (Exception ex)
            {
                database.RollBack();
                throw ex;
            }

            return numMessages;
        }

        /// <summary>
        /// Método para retornar as turmas do aluno para concatenar com as mensagens.
        /// </summary>
        /// <param name="idStudent">Int64 idStudent.</param>
        /// <returns>List Classes.</returns>
        public List<Classes> getClasseMessage(Int64 idStudent)
        {
            MySQL database = MySQL.GetInstancia();
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            List<Classes> classes = new List<Classes>();

            cmm.Parameters.AddWithValue("@idStudent", idStudent);

            sql.Append("CALL getMessagesClasses(@idStudent)");

            cmm.CommandText = sql.ToString();

            try
            {
                dr = database.ExecuteReader(cmm);

                while (dr.Read())
                {

                    classes.Add(
                        new Classes
                        {
                            idClass = (Int64)dr["idClasse"],
                            name = (string)dr["nameClasse"],

                            discipline = new Disciplines
                            {
                                idDiscipline = (Int64)dr["idDiscipline"],
                                name = (string)dr["nameDiscipline"],

                                teacher = new Teachers
                                {
                                    idTeacher = (Int64)dr["idTeacher"],
                                    firstName = (string)dr["firstName"],
                                    urlImageProfile = (string)dr["urlImageProfile"]
                                }
                            }
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

            return classes;
        }

        /// <summary>
        /// Método para retornar as mensagens das turmas de um aluno para concatenar com as turmas.
        /// </summary>
        /// <param name="idStudent">Int64 idStudent.</param>
        /// <returns>List MessagesClasses.</returns>
        public List<MessagesClasse> getMessagesClasse(Int64 idStudent)
        {
            MySQL database = MySQL.GetInstancia();
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            List<MessagesClasse> newList = new List<MessagesClasse>();

            cmm.Parameters.AddWithValue("@idStudent", idStudent);

            sql.Append("CALL getMessagesClasses(@idStudent)");

            cmm.CommandText = sql.ToString();

            try
            {
                dr = database.ExecuteReader(cmm);

                while (dr.Read())
                {
                    newList.Add(
                        new MessagesClasse
                        {
                            idMessage = (Int64)dr["idMessage"],
                            message = (string)dr["message"],
                            idClasse = (Int64)dr["idClasse"]
                        });
                }

                dr.Close();

            }
            catch (Exception ex)
            {
                dr.Close();
                throw ex;
            }

            return newList;
        }

        /// <summary>
        /// Método para verificar a existência de um aluno na turma.
        /// </summary>
        /// <param name="idStudent">Int64 idStudent.</param>
        /// <returns>bool true or false.</returns>
        public bool CheckStudentInClasse(Int64 idStudent, Int64 idClass)
        {
            MySQL database = MySQL.GetInstancia();
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            bool state;
            Int64 numReturn = 0;

            cmm.Parameters.AddWithValue("@idStudent", idStudent);
            cmm.Parameters.AddWithValue("@idClass", idClass);


            sql.Append("CALL checkStudentInClasse(@idStudent,@idClass)");

            cmm.CommandText = sql.ToString();

            try
            {
                numReturn = database.ExecuteScalar(cmm);
                
                if (numReturn == 1)
                {
                    state = true;
                }
                else
                {
                    state = false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return state;
        }


    }
}