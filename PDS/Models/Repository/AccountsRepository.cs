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
        /// Metodo para criptografar senha de usuario. Recebe como parametro uma string. Retorna a senha criptografada em formato string.
        /// </summary>
        /// <param name="password">String password.</param>
        /// <returns>Password encrypted.</returns>
        public static string EncryptPassword(string password)
        {
            return Crypter.MD5.Crypt(password);
        }

        /// <summary>
        /// Metodo para checar se a senha digitada pelo usuario confere com a senha criptografa salva. Recebe como parametro a senha digitada e o hash do banco. Retornar verdadeiro ou falso.
        /// </summary>
        /// <param name="password">String password.</param>
        /// <param name="hash">String hash.</param>
        /// <returns>True or False.</returns>
        public static bool CheckPassword(string password, string hash)
        {
            return Crypter.CheckPassword(password, hash);
        }
    }
}