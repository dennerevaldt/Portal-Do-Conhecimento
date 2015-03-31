using MySql.Data.MySqlClient;
using PDS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PDS.Models.Repository
{
    public class DisciplinesRepository
    {
        public static MySqlDataReader dr;

        /// <summary>
        /// Método para adicionar uma nova disciplina ao Professor.
        /// </summary>
        /// <param name="discipline">Disciplines discipline.</param>
        public static void Create(Disciplines discipline)
        {
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("name", discipline.name);
            cmm.Parameters.AddWithValue("idTeacher", discipline.teacher.idTeacher);

            sql.Append("CALL insertDiscipline(@name, @idTeacher)");

            cmm.CommandText = sql.ToString();

            ADOMySQL.MySQL.ExecuteNonQuery(cmm);

        }

        /// <summary>
        /// Método para retornar uma lista de disciplinas, através do ID do professor(a).
        /// </summary>
        /// <param name="idTeacher">Int64 idTeacher.</param>
        /// <returns>List Disciplines.</returns>
        public static List<Disciplines> GetAll(Int64 idTeacher)
        {
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            List<Disciplines> listDisciplines = new List<Disciplines>();

            cmm.Parameters.AddWithValue("@idTeacher", idTeacher);

            sql.Append("CALL getAllDisciplines(@idTeacher)");

            cmm.CommandText = sql.ToString();

            try
            {
                ADOMySQL.MySQL.Conectar();
                dr = ADOMySQL.MySQL.ExecuteReader(cmm);

                while (dr.Read())
                {
                    listDisciplines.Add(
                        new Disciplines
                        {
                            teacher = new Teachers {
                                idTeacher = Convert.ToInt64(dr["idTeacher"])
                            },

                            name = Convert.ToString(dr["name"]),
                            idDiscipline = Convert.ToInt64(dr["idDiscipline"]),                   
                        }
                    );

                }

                dr.Dispose();

                ADOMySQL.MySQL.Desconectar();
            }
            catch (Exception ex)
            {
                dr.Dispose();
                ADOMySQL.MySQL.Desconectar();
                throw ex;
            }

            return listDisciplines;
        }

        /// <summary>
        /// Método para deletar uma disciplina.
        /// </summary>
        /// <param name="idDiscipline">Int64 idDiscipline.</param>
        public static void Delete(Int64 idDiscipline)
        {
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@idDiscipline", idDiscipline);

            sql.Append("CALL deleteDiscipline(@idDiscipline)");

            cmm.CommandText = sql.ToString();

            ADOMySQL.MySQL.ExecuteNonQuery(cmm);
        }

        /// <summary>
        /// Método para fazer o update de uma disciplina.
        /// </summary>
        /// <param name="idDiscipline">Int64 idDiscipline.</param>
        /// <param name="name">String name.</param>
        public static void Update(Int64 idDiscipline, string name)
        {
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@idDiscipline", idDiscipline);
            cmm.Parameters.AddWithValue("@name", name);

            sql.Append("CALL updateDiscipline(@idDiscipline, @name)");

            cmm.CommandText = sql.ToString();

            ADOMySQL.MySQL.ExecuteNonQuery(cmm);
        }
    }
}