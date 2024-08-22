namespace MinimalAPIPeliculas.DTOs
{
    public class CreateActorDto
    {
        public string Nombre { get; set; } = null!;
        public IFormFile? Foto { get; set; }
        public DateTime FechaNacmiento { get; set; }
    }
}
