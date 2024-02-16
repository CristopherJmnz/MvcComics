namespace MvcComics.Models
{
    public class BuscadorComic
    {
        public List<Comic> Comics {  get; set; }
        public Comic Comic { get; set; }
        public BuscadorComic()
        {
            this.Comics=new List<Comic>();
        }
    }
}
