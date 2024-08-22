using Microsoft.AspNetCore.Identity;

namespace MinimalAPIPeliculas.Entities
{
    public class Commentary
    {
        public int Id { get; set; }
        public string? Cuerpo { get; set; }
        public int PeliculaId { get; set; }
        public string UserId { get; set; } = null!;
        public IdentityUser user { get; set; } = null!;
    }
}
