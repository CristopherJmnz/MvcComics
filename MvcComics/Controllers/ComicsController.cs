using Microsoft.AspNetCore.Mvc;
using MvcComics.Models;
using MvcComics.Repositories;

namespace MvcComics.Controllers
{
    public class ComicsController : Controller
    {
        private IComicRepository repo;
        public ComicsController(IComicRepository repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            List<Comic>comics=this.repo.GetAllComics();
            return View(comics);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Comic comic)
        {
            this.repo.InsertComic(comic.Nombre, comic.Imagen, comic.Descripcion);
            return RedirectToAction("Index");
        }

        public IActionResult CreateProcedure()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProcedure(Comic comic)
        {
            this.repo.InsertComicProcedure(comic.Nombre, comic.Imagen, comic.Descripcion);
            return RedirectToAction("Index");
        }

        public IActionResult Buscador()
        {
            List<Comic> comics = this.repo.GetAllComics();
            BuscadorComic busc=new BuscadorComic();
            busc.Comics = comics;
            return View(busc);
        }

        [HttpPost]
        public IActionResult Buscador(int idComic)
        {
            Comic comic=this.repo.FindComic(idComic);
            List<Comic> comics = this.repo.GetAllComics();
            BuscadorComic busc = new BuscadorComic();
            busc.Comics = comics;
            busc.Comic = comic;
            return View(busc);
        }

        [Route("deletePost")]
        public IActionResult Delete(int idComic)
        {
            Comic comic=this.repo.FindComic(idComic);
            return View(comic);
        }

        [Route("deletePost")]
        [HttpPost]
        public IActionResult DeletePost(int idComic)
        {
            this.repo.DeleteComic(idComic);
            return RedirectToAction("Index");
        }
    }
}
