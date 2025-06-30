namespace BlazorApp1.Models
{
    // DTO para enviar os dados de login para a API
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}