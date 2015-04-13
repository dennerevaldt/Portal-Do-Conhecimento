using ADOMySQL;
using CryptSharp;
using MySql.Data.MySqlClient;
using PDS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PDS.Models.Repository
{
    public class AccountsRepository
    {
        #region Atributos Staticos
        
        public static MySqlDataReader dr;
        public static object objeto;

        #endregion

        /// <summary>
        /// Método para criar conta de Professor.
        /// </summary>
        /// <param name="account">Objeto Account.</param>
        /// <returns>Int64 idAccount.</returns>
        public Int64 CreateTeacher(Accounts account, Teachers teacher)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@email", account.email);
            cmm.Parameters.AddWithValue("@password", account.password);
            cmm.Parameters.AddWithValue("@acessToken", account.acessToken);

            sql.Append("CALL insertAccount(@email, @password, @acessToken)");

            cmm.CommandText = sql.ToString();

            Int64 idAccountReturn = 0;

            try
            {
                database.BeginWork();

                idAccountReturn = database.ExecuteScalar(cmm);

                teacher.Account.idAccount = idAccountReturn;

                TeachersRepository repTeacher = new TeachersRepository();
                repTeacher.Create(teacher, database);

                database.CommitWork();
            }
            catch (Exception)
            {
                database.RollBack();
                throw;
            }

            return idAccountReturn;
        }

        /// <summary>
        /// Método para criar conta de Estudante.
        /// </summary>
        /// <param name="account">Objeto Account.</param>
        /// <returns>Int64 idAccount.</returns>
        public Int64 CreateStudent(Accounts account, Students student)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@email", account.email);
            cmm.Parameters.AddWithValue("@password", account.password);
            cmm.Parameters.AddWithValue("@acessToken", account.acessToken);

            sql.Append("CALL insertAccount(@email, @password, @acessToken)");

            cmm.CommandText = sql.ToString();

            Int64 idAccountReturn = 0;

            try
            {
                database.BeginWork();

                idAccountReturn = database.ExecuteScalar(cmm);

                student.Account.idAccount = idAccountReturn;

                StudentsRepository repStudents = new StudentsRepository();
                repStudents.Create(student, database);

                database.CommitWork();

            }
            catch (Exception)
            {
                database.RollBack();
                throw;
            }

            //Int64 idAccountReturn = ADOMySQL.MySQL.ExecuteEscalar(cmm);

            return idAccountReturn;
        }

        /// <summary>
        /// Método para excluir conta de usuário.
        /// </summary>
        /// <param name="email">String email.</param>
        public void Delete(string email)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@email", email);

            sql.Append("CALL deleteAccount(@email)");

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
        /// Método para retornar os dados de usuário.
        /// </summary>
        /// <param name="email">String Email.</param>
        /// <returns>Objeto object.</returns>
        public object GetUserData(string email)
        {
            MySQL database = MySQL.GetInstancia("root","123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@email", email);

            sql.Append("CALL getDataUser(@email)");

            cmm.CommandText = sql.ToString();

            try
            {
                dr = database.ExecuteReader(cmm);

                while (dr.Read())
                {
                    if (dr["accountType"].ToString() == "T")
                    {
                        objeto = new Teachers
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
                        };
                    }
                    else
                    {
                        objeto = new Students
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

                        };
                    }
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                dr.Close();
                throw ex;
            }

            return objeto;
        }

        /// <summary>
        /// Método para retornar dados da conta do usuário.
        /// </summary>
        /// <param name="email">String email.</param>
        /// <returns>Object Accounts.</returns>
        public Accounts GetDataAccount(string email)
        {
            MySQL database = MySQL.GetInstancia("root","123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            Accounts account = new Accounts();

            cmm.Parameters.AddWithValue("@email", email);

            sql.Append("CALL getDataAccount(@email)");

            cmm.CommandText = sql.ToString();

            try
            {
                dr = database.ExecuteReader(cmm);

                while (dr.Read())
                {
                    account = new Accounts
                    {
                        idAccount = Convert.ToInt64(dr["idAccount"]),
                        email = Convert.ToString(dr["email"]),
                        password = Convert.ToString(dr["password"]),
                        acessToken = Convert.ToString(dr["acessToken"])
                    };
                    
                }

                dr.Close();

            }
            catch (Exception)
            {
                dr.Close();
                throw;
            }

            return account;
        }

        /// <summary>
        /// Método para retornar a existência de um email já armazenado.
        /// </summary>
        /// <param name="email">String Email.</param>
        /// <returns>True or False.</returns>
        public bool GetEmail(string email)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@email", email);

            sql.Append("CALL getEmail(@email)");

            cmm.CommandText = sql.ToString();

            Int64 result = 0;

            try
            {
                 result = database.ExecuteScalar(cmm);

                 if (result == 0)
                 {
                     return false;
                 }
                 else
                 {
                     return true;
                 }
            }
            catch (Exception ex)
            {           
                throw ex;
            }
            
        }

        /// <summary>
        /// Método para atualizar a senha do usuário.
        /// </summary>
        /// <param name="email">String email.</param>
        /// <param name="password">String password.</param>
        public void UpdateUserPassword(string email, string password)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@email", email);
            cmm.Parameters.AddWithValue("@password", password);

            sql.Append("CALL updateUserPassword(@email, @password)");

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
        /// Método para fazer update dos dados da conta.
        /// </summary>
        /// <param name="account">Accounts account.</param>
        public void UpdateAccount(Accounts account)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@idAccount", account.idAccount);
            cmm.Parameters.AddWithValue("@email", account.email);

            sql.Append("CALL updateAccount(@idAccount,@email)");

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
        /// Método para fazer update dos dados pessoais.
        /// </summary>
        /// <param name="person"Dynamic person.></param>
        public void UpdatePerson(dynamic person)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@idPerson", person.idPerson);
            cmm.Parameters.AddWithValue("@firstName", person.firstName);
            cmm.Parameters.AddWithValue("@lastName", person.lastName);
            cmm.Parameters.AddWithValue("@gender", person.gender);
            cmm.Parameters.AddWithValue("@dateOfBirth", person.dateOfBirth);
            cmm.Parameters.AddWithValue("@location", person.city);
            cmm.Parameters.AddWithValue("@country", person.country);

            sql.Append("CALL updatePerson(@idPerson,@firstName,@lastName,@gender,@dateOfBirth,@location,@country)");

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
        /// Método para criptografar senha de usuário. Recebe como parametro uma string. Retorna a senha criptografada em formato string.
        /// </summary>
        /// <param name="password">String password.</param>
        /// <returns>Password encrypted.</returns>
        public string EncryptPassword(string password)
        {
            return Crypter.MD5.Crypt(password);
        }

        /// <summary>
        /// Método para checar se a senha digitada pelo usuário confere com a senha criptografada salva. Recebe como parametro a senha digitada e o hash do banco. Retorna verdadeiro ou falso.
        /// </summary>
        /// <param name="password">String password.</param>
        /// <param name="hash">String hash.</param>
        /// <returns>True or False.</returns>
        public bool CheckPassword(string password, string hash)
        {
            return Crypter.CheckPassword(password, hash);
        }

        /// <summary>
        /// Método para alterar caminho da imagem no DB.
        /// </summary>
        /// <param name="urlImage">String url.</param>
        /// <param name="idAccount">Int64 idAccount.</param>
        public void UpdateUrlImage(string urlImage, Int64 idAccount)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@url", urlImage);
            cmm.Parameters.AddWithValue("@idAccount", idAccount);

            sql.Append("CALL updateUrlImage(@url,@idAccount)");

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
        /// Método para buscar os dados do perfil do usuário.
        /// </summary>
        /// <param name="idAccount">Int64 idAccount.</param>
        /// <returns></returns>
        public object GetPerfil(Int64 idAccount)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@idAccount", idAccount);

            sql.Append("CALL getDataPerfil(@idAccount)");

            cmm.CommandText = sql.ToString();

            try
            {
                dr = database.ExecuteReader(cmm);

                while (dr.Read())
                {
                    if (dr["accountType"].ToString() == "T")
                    {
                        objeto = new Teachers
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
                        };
                    }
                    else
                    {
                        objeto = new Students
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

                        };
                    }
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                dr.Close();
                throw ex;
            }

            return objeto;
        }

    }
}