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
    public class TeachersRepository
    {
        private static MySqlDataReader dr;

        /// <summary>
        /// Método para inserir novo Teacher ao DB.
        /// </summary>
        /// <param name="teacher">Object teacher.</param>
        public void Create(Teachers teacher, MySQL database)
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

            //Int64 idReturnPerson = ADOMySQL.MySQL.ExecuteEscalar(cmmPerson);

            #endregion

            #region Insert Teacher

            //MySqlCommand cmmTeacher = new MySqlCommand();
            //StringBuilder sqlTeacher = new StringBuilder();

            //cmmTeacher.Parameters.AddWithValue("@idPerson", idReturnPerson);

            //sqlTeacher.Append("CALL insertTeacher(@idPerson)");

            //cmmTeacher.CommandText = sqlTeacher.ToString();

            //ADOMySQL.MySQL.ExecuteNonQuery(cmmTeacher);

            #endregion

            try
            {
                Int64 idReturnPerson = database.ExecuteScalar(cmmPerson);

                #region Insert Teacher
                MySqlCommand cmmTeacher = new MySqlCommand();
                StringBuilder sqlTeacher = new StringBuilder();

                cmmTeacher.Parameters.AddWithValue("@idPerson", idReturnPerson);

                sqlTeacher.Append("CALL insertTeacher(@idPerson)");

                cmmTeacher.CommandText = sqlTeacher.ToString();
                #endregion

                database.ExecuteNonQuery(cmmTeacher);
            }
            catch (Exception)
            {                
                throw;
            }
        }

        /// <summary>
        /// Método para inserir um novo seguidor ao professor.
        /// </summary>
        /// <param name="idFollower">Int64 idFollower.</param>
        /// <param name="idFollowing">Int64 idFollowing.</param>
        public void Follow(Int64 idFollower, Int64 idFollowing)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@idFollower", idFollower);
            cmm.Parameters.AddWithValue("@idFollowing", idFollowing);

            sql.Append("CALL insertFollower(@idFollower,@idFollowing)");

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
        /// Método para deletar um seguidor do professor.
        /// </summary>
        /// <param name="idFollower">Int64 idFollower.</param>
        /// <param name="idFollowing">Int64 idFollowing.</param>
        public void UnFollow(Int64 idFollower, Int64 idFollowing)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@idFollower", idFollower);
            cmm.Parameters.AddWithValue("@idFollowing", idFollowing);

            sql.Append("CALL deleteFollower(@idFollower,@idFollowing)");

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
        /// Método para checar se professor já segue outro professor.
        /// </summary>
        /// <param name="idFollower">Int64 idFollower.</param>
        /// <param name="idFollowing">Int64 idFollowing.</param>
        /// <returns></returns>
        public bool CheckFollow(Int64 idFollower, Int64 idFollowing)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            Int64 idReturn;

            cmm.Parameters.AddWithValue("@idFollower", idFollower);
            cmm.Parameters.AddWithValue("@idFollowing", idFollowing);

            sql.Append("CALL checkFollower(@idFollower,@idFollowing)");

            cmm.CommandText = sql.ToString();

            try
            {
                database.BeginWork();

                idReturn = database.ExecuteScalar(cmm);

                database.CommitWork();

            }
            catch (Exception)
            {
                database.RollBack();
                throw;
            }

            if (idReturn == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Método para pesquisar professores no sistema pelo nome.
        /// </summary>
        /// <param name="name">String name.</param>
        /// <returns>List Teachers.</returns>
        public List<Teachers> SearchTeacher(string name)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            List<Teachers> teachers = new List<Teachers>();

            cmm.Parameters.AddWithValue("@name", name);

            sql.Append("CALL searchTeacher(@name)");

            cmm.CommandText = sql.ToString();

            try
            {
                dr = database.ExecuteReader(cmm);

                while (dr.Read())
                {
                    teachers.Add(new Teachers
                                    {
                                        idTeacher = Convert.ToInt64(dr["idTeacher"]),
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

            return teachers;
        }

        /// <summary>
        /// Método para buscar seguidores proprios do professor no sistema.
        /// </summary>
        /// <param name="idFollower">Int64 idFollower.</param>
        /// <returns>List Teachers.</returns>
        public List<Teachers> GetFollowers(Int64 idFollower)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            List<Teachers> teachers = new List<Teachers>();

            cmm.Parameters.AddWithValue("@idFollower", idFollower);

            sql.Append("CALL getFollowers(@idFollower)");

            cmm.CommandText = sql.ToString();

            try
            {
                dr = database.ExecuteReader(cmm);

                while (dr.Read())
                {
                    teachers.Add(new Teachers
                    {
                        urlImageProfile = Convert.ToString(dr["urlImageProfile"]),
                        Account = new Accounts
                        {
                            idAccount = Convert.ToInt64(dr["idAccount"]),
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

            return teachers;
        }

        /// <summary>
        /// Método para buscar seguidores de professores no sistema.
        /// </summary>
        /// <param name="idFollower">Int64 idFollower.</param>
        /// <returns>List Teachers.</returns>
        public List<Teachers> GetFollowersOther(Int64 idFollower)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            List<Teachers> teachers = new List<Teachers>();

            cmm.Parameters.AddWithValue("@idFollower", idFollower);

            sql.Append("CALL getFollowersOther(@idFollower)");

            cmm.CommandText = sql.ToString();

            try
            {
                dr = database.ExecuteReader(cmm);

                while (dr.Read())
                {
                    teachers.Add(new Teachers
                    {
                        urlImageProfile = Convert.ToString(dr["urlImageProfile"]),
                        Account = new Accounts
                        {
                            idAccount = Convert.ToInt64(dr["idAccount"]),
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

            return teachers;
        }

        /// <summary>
        /// Método para retornar as informações de um professor, conforme disciplina.
        /// </summary>
        /// <param name="idDiscipline">Int64 idDiscipline.</param>
        /// <returns>Teachers teacher.</returns>
        public Teachers GetOne(Int64 idDiscipline)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            Teachers teacher = new Teachers();

            cmm.Parameters.AddWithValue("@idDiscipline", idDiscipline);

            sql.Append("CALL getOneTeacher(@idDiscipline)");

            cmm.CommandText = sql.ToString();

            try
            {
                dr = database.ExecuteReader(cmm);

                while (dr.Read())
                {
                    teacher =
                    (new Teachers
                        {
                            idTeacher = (Int64)dr["idTeacher"],
                            firstName = (string)dr["firstName"],
                            lastName = (string)dr["lastName"],
                            urlImageProfile = Convert.ToString(dr["urlImageProfile"]),
                            Account = new Accounts
                            {
                                idAccount = Convert.ToInt64(dr["idAccount"]),
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

            return teacher;
        }
    }
}