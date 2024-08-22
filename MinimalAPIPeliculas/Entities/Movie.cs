namespace MinimalAPIPeliculas.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public bool Encines {  get; set; }  
        public DateTime FechaLanzamiento { get; set; }
        public string? Poster {  get; set; }
        public List<Commentary> Comentarios { get; set; } = new List<Commentary>();
        public List<MovieGenre> Genres { get; set; } = new List<MovieGenre>();
        public List<MovieActor> Actors { get; set; } = new List<MovieActor>();
    }
}
