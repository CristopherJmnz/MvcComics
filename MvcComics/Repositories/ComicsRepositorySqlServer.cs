using Microsoft.AspNetCore.Http.HttpResults;
using MvcComics.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MvcComics.Repositories
{
    #region PROCEDURES SQL
//    create procedure SP_INSERT_COMIC
//(@NOMBRE NVARCHAR(600),@IMAGEN NVARCHAR(600),@DESCRIPCION NVARCHAR(600))
//AS
//    declare @maxId int
//    select @maxId=Max(COMICS.IDCOMIC) FROM COMICS;
//    insert into comics values(@maxId+1, @nombre, @imagen, @descripcion);
//    GO
    #endregion
    public class ComicsRepositorySqlServer : IComicRepository
    {
        private SqlConnection cn;
        private SqlCommand com;
        private DataTable tablaComics;

        public ComicsRepositorySqlServer()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=COMICS;Persist Security Info=True;User ID=sa;Password='';";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            string sql = "select * from comics";
            this.tablaComics = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter(sql,this.cn);
            ad.Fill(this.tablaComics);
        }

        public void DeleteComic(int idComic)
        {
            
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
                    Descripcion=row.Field<string>("DESCRIPCION"),
                    IdComic= row.Field<int>("IDCOMIC"),
                    Imagen=row.Field<string>("IMAGEN"),
                    Nombre= row.Field<string>("NOMBRE")
                };
                comics.Add(com);
            }
            return comics;
        }

        public List<string> GetNombreComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;
            List<string> nombre = new List<string>();
            foreach (var row in consulta)
            {
                nombre.Add(row.Field<string>("NOMBRE"));
            }
            return nombre;
        }

        public void InsertComic(string nombre, string imagen, string descripcion)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;
            int maximoId = consulta.Max(x=>x.Field<int>("IDCOMIC")) +1;
            string sql = "Insert into comics values(@id,@nombre,@imagen,@descripcion)";
            this.com.Parameters.AddWithValue("@id",maximoId);
            this.com.Parameters.AddWithValue("@nombre",nombre);
            this.com.Parameters.AddWithValue("@imagen",imagen);
            this.com.Parameters.AddWithValue("@descripcion",descripcion);
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
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@imagen", imagen);
            this.com.Parameters.AddWithValue("@descripcion", descripcion);
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
