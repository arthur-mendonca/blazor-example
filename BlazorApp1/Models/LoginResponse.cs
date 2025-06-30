namespace BlazorApp1.Models
{
    public class LoginResponse
    {
        public string Message { get; set; } = default!;
        public Usuario Usuario { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}