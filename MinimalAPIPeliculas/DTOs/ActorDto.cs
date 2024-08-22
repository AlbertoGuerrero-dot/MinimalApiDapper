namespace MinimalAPIPeliculas.DTOs
{
    public class ActorDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Foto { get; set; }
        public DateTime FechaNacmiento { get; set; }
    }
}
