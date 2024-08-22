namespace MinimalAPIPeliculas.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public bool Encines { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        public string? Poster { get; set; }
        public List<ComentaryDto> Comentarios { get; set; } = new List<ComentaryDto>();
        public List<GenreDto> Genres { get; set; } = new List<GenreDto>();
        public List<MovieActorDto> Actors { get; set; } = new List<MovieActorDto>();
    }
}
