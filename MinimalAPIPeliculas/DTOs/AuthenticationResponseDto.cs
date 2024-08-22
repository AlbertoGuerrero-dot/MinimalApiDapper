namespace MinimalAPIPeliculas.DTOs
{
    public class AuthenticationResponseDto
    {
        public string Token { get; set; } = null!;
        public DateTime Expiracion { get; set; }
    }
}
