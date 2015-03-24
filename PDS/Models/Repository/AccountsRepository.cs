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
        /// Método para criar conta de usuário.
        /// </summary>
        /// <param name="account">Objeto Account.</param>
        /// <returns>Int64 idAccount.</returns>
        public static Int64 Create(Accounts account)
        {
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@email", account.email);
            cmm.Parameters.AddWithValue("@password", account.password);
            cmm.Parameters.AddWithValue("@acessToken", account.acessToken);

            sql.Append("CALL insertAccount(@email, @password, @acessToken)");

            cmm.CommandText = sql.ToString();

            Int64 idAccountReturn = ADOMySQL.MySQL.ExecuteEscalar(cmm);

            return idAccountReturn;
        }

        /// <summary>
        /// Método para excluir conta de usuário.
        /// </summary>
        /// <param name="email">String email.</param>
        public static void Delete(string email)
        {
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@email", email);

            sql.Append("CALL deleteAccount(@email)");

            cmm.CommandText = sql.ToString();

            ADOMySQL.MySQL.ExecuteNonQuery(cmm);
        }

        /// <summary>
        /// Método para retornar os dados de usuário.
        /// </summary>
        /// <param name="email">String Email.</param>
        /// <returns>Objeto object.</returns>
        public static object GetUserData(string email)
        {
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@email", email);

            sql.Append("CALL getDataUser(@email)");

            cmm.CommandText = sql.ToString();

            try
            {
                ADOMySQL.MySQL.Conectar();

                dr = ADOMySQL.MySQL.ExecuteReader(cmm);

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

                ADOMySQL.MySQL.Desconectar();
            }
            catch (Exception)
            {
                dr.Close();
                ADOMySQL.MySQL.Desconectar();
                throw;
            }

            return objeto;
        }

        /// <summary>
        /// Método para retornar dados da conta do usuário.
        /// </summary>
        /// <param name="email">String email.</param>
        /// <returns>Object Accounts.</returns>
        public static Accounts GetDataAccount(string email)
        {
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            Accounts account = new Accounts();

            cmm.Parameters.AddWithValue("@email", email);

            sql.Append("CALL getDataAccount(@email)");

            cmm.CommandText = sql.ToString();

            try
            {
                ADOMySQL.MySQL.Conectar();
                dr = ADOMySQL.MySQL.ExecuteReader(cmm);

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

                ADOMySQL.MySQL.Desconectar();
            }
            catch (Exception)
            {
                dr.Close();
                ADOMySQL.MySQL.Desconectar();
                throw;
            }

            return account;
        }

        /// <summary>
        /// Método para retornar a existência de um email já armazenado.
        /// </summary>
        /// <param name="email">String Email.</param>
        /// <returns>True or False.</returns>
        public static bool GetEmail(string email)
        {
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@email", email);

            sql.Append("CALL getEmail(@email)");

            cmm.CommandText = sql.ToString();

            Int64 result = ADOMySQL.MySQL.ExecuteEscalar(cmm);

            if (result == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Método para atualizar a senha do usuário.
        /// </summary>
        /// <param name="email">String email.</param>
        /// <param name="password">String password.</param>
        public static void UpdateUserPassword(string email, string password)
        {
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@email", email);
            cmm.Parameters.AddWithValue("@password", password);

            sql.Append("CALL updateUserPassword(@email, @password)");

            cmm.CommandText = sql.ToString();

            ADOMySQL.MySQL.ExecuteNonQuery(cmm);
        }

        /// <summary>
        /// Método para fazer update dos dados da conta.
        /// </summary>
        /// <param name="account">Accounts account.</param>
        public static void UpdateAccount(Accounts account)
        {
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@idAccount", account.idAccount);
            cmm.Parameters.AddWithValue("@email", account.email);

            sql.Append("CALL updateAccount(@idAccount,@email)");

            cmm.CommandText = sql.ToString();

            ADOMySQL.MySQL.ExecuteNonQuery(cmm);
        }

        /// <summary>
        /// Método para fazer update dos dados pessoais.
        /// </summary>
        /// <param name="person"Dynamic person.></param>
        public static void UpdatePerson(dynamic person)
        {
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

            ADOMySQL.MySQL.ExecuteNonQuery(cmm);
            
        }

        /// <summary>
        /// Método para criptografar senha de usuário. Recebe como parametro uma string. Retorna a senha criptografada em formato string.
        /// </summary>
        /// <param name="password">String password.</param>
        /// <returns>Password encrypted.</returns>
        public static string EncryptPassword(string password)
        {
            return Crypter.MD5.Crypt(password);
        }

        /// <summary>
        /// Método para checar se a senha digitada pelo usuário confere com a senha criptografada salva. Recebe como parametro a senha digitada e o hash do banco. Retorna verdadeiro ou falso.
        /// </summary>
        /// <param name="password">String password.</param>
        /// <param name="hash">String hash.</param>
        /// <returns>True or False.</returns>
        public static bool CheckPassword(string password, string hash)
        {
            return Crypter.CheckPassword(password, hash);
        }

        /// <summary>
        /// Método para alterar caminho da imagem no DB.
        /// </summary>
        /// <param name="urlImage">String url.</param>
        /// <param name="idAccount">Int64 idAccount.</param>
        public static void UpdateUrlImage(string urlImage, Int64 idAccount)
        {
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.Add("@url", urlImage);
            cmm.Parameters.Add("@idAccount", idAccount);

            sql.Append("CALL updateUrlImage(@url,@idAccount)");

            cmm.CommandText = sql.ToString();

            ADOMySQL.MySQL.ExecuteNonQuery(cmm);
        }

    }
}