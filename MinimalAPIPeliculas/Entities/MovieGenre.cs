namespace MinimalAPIPeliculas.Entities
{
    public class MovieGenre
    {
        public int PeliculaId { get; set; }
        public int GeneroId { get; set; }
        public Genre Genre { get; set; } = null!;
        public Movie Movie { get; set; } = null!;
    }
}
