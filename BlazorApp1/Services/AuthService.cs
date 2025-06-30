using System.Text.Json;
using BlazorApp1.Helpers;
using Microsoft.JSInterop;

namespace BlazorApp1.Services
{
    public class AuthService
    {
        private readonly IJSRuntime _jsRuntime;

        public AuthService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<bool> LoginAsync(string email, string senha)
        {
            try
            {
                Console.WriteLine($"Tentando login para: {email}");

                var loginDto = new { Email = email, Senha = senha };

                // Usando ApiBackend.PostAsync
                var response = await ApiBackend.PostAsync<LoginResponse, object>("usuarios/login", loginDto);

                Console.WriteLine($"Resposta da API: {response?.Message}");

                if (response?.Token != null)
                {
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "token", response.Token);
                    if (response.Usuario != null)
                    {
                        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "user", JsonSerializer.Serialize(response.Usuario));
                    }

                    Console.WriteLine("Login realizado com sucesso!");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no AuthService: {ex.Message}");
                throw; // Relança a exceção para ser capturada no Login.razor
            }
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            try
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "token");
                return !string.IsNullOrEmpty(token);
            }
            catch
            {
                return false;
            }
        }

        public async Task LogoutAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "token");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "user");
        }
    }

    public class LoginResponse
    {
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public Usuario? Usuario { get; set; }
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
    }
}