using System.Net.Http.Json; // Use a nova forma de fazer chamadas JSON
using System.Text.Json;

namespace BlazorApp1.Helpers
{
    public static class ApiBackend
    {
        // A URL base da sua API
        private static readonly string UrlBase = "http://localhost:5284/api/";

        // FÁBRICA PARA CRIAR O HTTPCLIENT
        // Isso será configurado no Program.cs
        public static IHttpClientFactory HttpClientFactory { get; set; }

        public static async Task<T?> PostAsync<T, K>(string complementoUrl, K objeto)
        {
            // Cria um HttpClient usando a fábrica
            var httpClient = HttpClientFactory.CreateClient();

            var urlCompleta = UrlBase + complementoUrl;

            // Faz a chamada POST
            var response = await httpClient.PostAsJsonAsync(urlCompleta, objeto);

            // Se a API retornar um erro (401, 404, 500), isso vai explodir uma exceção
            response.EnsureSuccessStatusCode();

            // Lê a resposta e deserializa
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}