namespace MinimalAPIPeliculas.Entities
{
    public class MovieActor
    {
        public int ActorId { get; set; }
        public Actor Actor { get; set; } = null!;
        public int PeliculaId { get; set; }
        public Movie Pelicula { get; set; } = null!;
        public int Orden { get; set; }
        public string Personaje { get; set; } = null!;
    }
}
