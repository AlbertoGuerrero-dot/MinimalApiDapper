namespace MinimalAPIPeliculas.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Foto { get; set; }
        public DateTime FechaNacmiento { get; set; }
    }
}
