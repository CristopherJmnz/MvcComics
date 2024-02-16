using Microsoft.AspNetCore.Http.HttpResults;
using MvcComics.Models;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace MvcComics.Repositories
{
    #region PROCEDURES ORACLE
//    create or replace procedure SP_INSERT_COMIC
//    (p_NOMBRE COMICS.NOMBRE%TYPE, p_IMAGEN COMICS.IMAGEN%TYPE, p_DESCRIPCION COMICS.DESCRIPCION%TYPE)
//AS
//Begin

//    insert into comics values((SELECT MAX(IDCOMIC) FROM COMICS) + 1 ,p_nombre,p_imagen,p_descripcion);
//  commit;
//end SP_INSERT_COMIC;
    #endregion
    public class ComicsRepositoryOracle : IComicRepository
    {
        private OracleConnection cn;
        private OracleCommand com;
        private DataTable tablaComics;

        public ComicsRepositoryOracle()
        {
            string connectionString = @"User Id=SYSTEM;Password=oracle;Data Source=LOCALHOST:1521/XE; Persist Security Info=True";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            string sql = "select * from Comics";
            this.tablaComics = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(sql, this.cn);
            adapter.Fill(this.tablaComics);
        }
        public void DeleteComic(int idComic)
        {
            string sql = "delete from comics where idcomic=:idcomic";
            OracleParameter pamId = new OracleParameter(":idcomic", idComic);
            this.com.Parameters.Add(pamId);
            this.com.CommandText = sql;
            this.com.CommandType = CommandType.Text;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public Comic FindComic(int idComic)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           where datos.Field<int>("IDCOMIC") == idComic
                           select datos;
            var row = consulta.First();
            Comic com = new Comic
            {
                Descripcion = row.Field<string>("DESCRIPCION"),
                IdComic = row.Field<int>("IDCOMIC"),
                Imagen = row.Field<string>("IMAGEN"),
                Nombre = row.Field<string>("NOMBRE")
            };
            return com;
        }

        public List<Comic> GetAllComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;
            List<Comic> comics = new List<Comic>();
            foreach (var row in consulta)
            {
                Comic com = new Comic
                {
                    Descripcion = row.Field<string>("DESCRIPCION"),
                    IdComic = row.Field<int>("IDCOMIC"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Nombre = row.Field<string>("NOMBRE")
                };
                comics.Add(com);
            }
            return comics;
        }

        public void InsertComic(string nombre, string imagen, string descripcion)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;
            int maximoId = consulta.Max(x => x.Field<int>("IDCOMIC")) + 1;
            string sql = "Insert into comics values(:id,:nombre,:imagen,:descripcion)";
            OracleParameter pamId = new OracleParameter(":?", maximoId);
            this.com.Parameters.Add(pamId);
            OracleParameter pamNombre = new OracleParameter(":?", nombre);
            this.com.Parameters.Add(pamNombre);
            OracleParameter pamImagen = new OracleParameter(":?", imagen);
            this.com.Parameters.Add(pamImagen);
            OracleParameter pamDescripcion = new OracleParameter(":?", descripcion);
            this.com.Parameters.Add(pamDescripcion);
            this.com.CommandText = sql;
            this.com.CommandType = CommandType.Text;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void InsertComicProcedure(string nombre, string imagen, string descripcion)
        {
            this.com.CommandText = "SP_INSERT_COMIC";
            this.com.CommandType = CommandType.StoredProcedure;
            OracleParameter pamNombre = new OracleParameter(":?", nombre);
            this.com.Parameters.Add(pamNombre);
            OracleParameter pamImagen = new OracleParameter(":?", imagen);
            this.com.Parameters.Add(pamImagen);
            OracleParameter pamDescripcion = new OracleParameter(":?", descripcion);
            this.com.Parameters.Add(pamDescripcion);
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void UpdateComic(int idComic, string nombre, string imagen, string descripcion)
        {
            throw new NotImplementedException();
        }
    }
}
