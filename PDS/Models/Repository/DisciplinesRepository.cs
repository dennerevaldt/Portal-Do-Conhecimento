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
    public class DisciplinesRepository
    {
        public MySqlDataReader dr;

        /// <summary>
        /// Método para adicionar uma nova disciplina ao Professor.
        /// </summary>
        /// <param name="discipline">Disciplines discipline.</param>
        public void Create(Disciplines discipline)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("name", discipline.name);
            cmm.Parameters.AddWithValue("idTeacher", discipline.teacher.idTeacher);

            sql.Append("CALL insertDiscipline(@name, @idTeacher)");

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
        /// Método para retornar uma lista de disciplinas, através do ID do professor(a).
        /// </summary>
        /// <param name="idTeacher">Int64 idTeacher.</param>
        /// <returns>List Disciplines.</returns>
        public List<Disciplines> GetAll(Int64 idTeacher)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            List<Disciplines> listDisciplines = new List<Disciplines>();

            cmm.Parameters.AddWithValue("@idTeacher", idTeacher);

            sql.Append("CALL getAllDisciplines(@idTeacher)");

            cmm.CommandText = sql.ToString();

            try
            {
                dr = database.ExecuteReader(cmm);

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

                dr.Close();

            }
            catch (Exception ex)
            {
                dr.Close();
                throw ex;
            }

            return listDisciplines;
        }

        /// <summary>
        /// Método para deletar uma disciplina.
        /// </summary>
        /// <param name="idDiscipline">Int64 idDiscipline.</param>
        public void Delete(Int64 idDiscipline)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@idDiscipline", idDiscipline);

            sql.Append("CALL deleteDiscipline(@idDiscipline)");

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
        /// Método para fazer o update de uma disciplina.
        /// </summary>
        /// <param name="idDiscipline">Int64 idDiscipline.</param>
        /// <param name="name">String name.</param>
        public void Update(Int64 idDiscipline, string name)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();

            cmm.Parameters.AddWithValue("@idDiscipline", idDiscipline);
            cmm.Parameters.AddWithValue("@name", name);

            sql.Append("CALL updateDiscipline(@idDiscipline, @name)");

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
    }
}