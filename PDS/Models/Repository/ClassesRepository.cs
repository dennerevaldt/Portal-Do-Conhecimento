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
    public class ClassesRepository
    {
        public MySqlDataReader dr;

        /// <summary>
        /// Cria uma nova turma para o professor.
        /// </summary>
        /// <param name="newClass">Classes newClass.</param>
        public void Create(Classes newClass)
        {
            MySQL database = MySQL.GetInstancia();
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@idDiscipline", newClass.discipline.idDiscipline);
            cmm.Parameters.AddWithValue("@name", newClass.name);

            sql.Append("CALL insertClass(@idDiscipline,@name)");

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
        /// Método para retornar uma lista de turmas, através do ID do professor(a).
        /// </summary>
        /// <param name="idTeacher">Int64 idTeacher.</param>
        /// <returns>List Classes.</returns>
        public List<Classes> GetAll(Int64 idDisc)
        {
            MySQL database = MySQL.GetInstancia();
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            List<Classes> listClasses = new List<Classes>();

            cmm.Parameters.AddWithValue("@idDisc", idDisc);

            sql.Append("CALL getAllClasses(@idDisc)");

            cmm.CommandText = sql.ToString();

            try
            {
                dr = database.ExecuteReader(cmm);

                while (dr.Read())
                {
                    listClasses.Add(
                        new Classes
                        {
                            idClass = Convert.ToInt64(dr["idClasse"]),
                            name = Convert.ToString(dr["name"]),
                            discipline = new Disciplines{
                                idDiscipline = Convert.ToInt64(dr["idDiscipline"]),
                                name = Convert.ToString(dr["nameDiscipline"])
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

            return listClasses;
        }

        /// <summary>
        /// Método para deletar uma turma.
        /// </summary>
        /// <param name="idClass">Int64 idClass.</param>
        public void Delete(Int64 idClass)
        {
            MySQL database = MySQL.GetInstancia();
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@idClass", idClass);

            sql.Append("CALL deleteClass(@idClass)");

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
        /// Método para fazer o update de uma turma.
        /// </summary>
        /// <param name="idClass">Int64 idClass.</param>
        /// <param name="name">String name.</param>
        public void Update(Int64 idClass, string name)
        {
            MySQL database = MySQL.GetInstancia();
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@idClass", idClass);
            cmm.Parameters.AddWithValue("@name", name);

            sql.Append("CALL updateClass(@idClass, @name)");

            cmm.CommandText = sql.ToString();

            try
            {
                database.BeginWork();
                database.ExecuteNonQuery(cmm);
                database.CommitWork();
            }
            catch (Exception)
            {
                database.RollBack();
                throw;
            }
        }

        /// <summary>
        /// Método para retornar as informações de um turma específica.
        /// </summary>
        /// <param name="idClass">Int64 idClass.</param>
        /// <returns>List Classes.</returns>
        public Classes GetOne(Int64 idClass)
        {
            MySQL database = MySQL.GetInstancia();
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            List<ClassesStudents> cStudents = new List<ClassesStudents>();
            Classes classe = new Classes();
            List<ClassesPublicationsTeachers> listClassesPubTeac = new List<ClassesPublicationsTeachers>();
            List<PublicationsTeachers> listPublicationsTeachers = new List<PublicationsTeachers>();

            cmm.Parameters.AddWithValue("@idClass", idClass);

            sql.Append("CALL getOneClass(@idClass)");

            cmm.CommandText = sql.ToString();

            try
            {
                dr = database.ExecuteReader(cmm);

                if (dr.FieldCount > 6)
                {
                    while (dr.Read())
                    {
                        cStudents.Add(
                        new ClassesStudents
                        {
                            student = new Students
                            {
                                idPerson = (Int64)dr["idPerson"],
                                accountType = Convert.ToChar(dr["accountType"]),
                                firstName = (string)dr["firstName"],
                                lastName = (string)dr["lastName"],
                                gender = Convert.ToChar(dr["gender"]),
                                dateOfBirth = (DateTime)dr["dateOfBirth"],
                                city = (string)dr["city"],
                                country = (string)dr["country"],
                                urlImageProfile = (string)dr["urlImageProfile"],
                                idStudent = (Int64)dr["idStudent"]
                            },
                            stars = (int)dr["stars"]
                        });

                    }

                    while (dr.HasRows)
                    {
                        classe =
                            new Classes
                            {
                                idClass = Convert.ToInt64(dr["idClasse"]),
                                name = Convert.ToString(dr["name"]),

                                discipline = new Disciplines
                                {
                                    idDiscipline = Convert.ToInt64(dr["idDiscipline"]),
                                    name = Convert.ToString(dr["name"])
                                },

                                classesStudents = cStudents
                            };


                        dr.NextResult();
                    }

                    dr.Close();
                }
                else
                {
                    while (dr.Read())
                    {
                        classe =
                            new Classes
                            {
                                idClass = Convert.ToInt64(dr["idClasse"]),
                                name = Convert.ToString(dr["name"]),

                                discipline = new Disciplines
                                {
                                    idDiscipline = Convert.ToInt64(dr["idDiscipline"]),
                                    name = Convert.ToString(dr["name"])
                                },

                                classesStudents = new List<ClassesStudents>()
                            };
                    }

                    dr.Close();
                }

                

            }
            catch (Exception ex)
            {
                dr.Close();
                throw ex;
            }

            return classe;
        }

        /// <summary>
        /// Método para inserir uma nova mensagem para turma.
        /// </summary>
        /// <param name="idClasse">Int64 idClasse.</param>
        /// <param name="message">String message.</param>
        public void InviteMessage(Int64 idClasse, string message)
        {
            MySQL database = MySQL.GetInstancia();
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@idClasse", idClasse);
            cmm.Parameters.AddWithValue("@message", message);

            sql.Append("CALL insertMessageClasse(@idClasse,@message)");

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
    }
}