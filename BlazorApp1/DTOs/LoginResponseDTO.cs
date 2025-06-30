namespace BlazorApp1.Models
{
    // DTO para receber a resposta completa da API
    public class LoginResponseDTO
    {
        public string Message { get; set; }
        public string Token { get; set; }
        public UsuarioDTO Usuario { get; set; }
    }
}