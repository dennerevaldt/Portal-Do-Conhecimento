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
            MySQL database = MySQL.GetInstancia("root", "123456");
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
        public List<Classes> GetAll(Int64 idTeacher)
        {
            MySQL database = MySQL.GetInstancia("root", "123456");
            MySqlCommand cmm = new MySqlCommand();
            StringBuilder sql = new StringBuilder();
            List<Classes> listClasses = new List<Classes>();

            cmm.Parameters.AddWithValue("@idTeacher", idTeacher);

            sql.Append("CALL getAllClasses(@idTeacher)");

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
            MySQL database = MySQL.GetInstancia("root", "123456");
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
            MySQL database = MySQL.GetInstancia("root", "123456");
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
    }
}