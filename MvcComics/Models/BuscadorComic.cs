namespace MvcComics.Models
{
    public class BuscadorComic
    {
        public List<Comic> comics {  get; set; }
        public Comic comic { get; set; }
        public BuscadorComic()
        {
            this.comics=new List<Comic>();
        }
    }
}
