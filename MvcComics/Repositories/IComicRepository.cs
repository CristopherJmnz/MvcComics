using MvcComics.Models;

namespace MvcComics.Repositories
{
    public interface IComicRepository
    {
        List<Comic> GetAllComics();
        Comic FindComic(int idComic);
        List<string> GetNombreComics();
        void InsertComic(string nombre,string imagen,string descripcion);
        void UpdateComic(int idComic,string nombre, string imagen, string descripcion);
        void DeleteComic(int idComic);
    }
}
