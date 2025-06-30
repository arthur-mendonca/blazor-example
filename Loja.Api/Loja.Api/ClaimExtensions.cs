using System.Security.Claims;

namespace Loja.Api
{
    public static class ClaimExtensions
    {
        public static int GetId(this ClaimsPrincipal user)
        {
            try
            {
                return Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == "Id")?.Value ?? "0");
            }
            catch
            {
                return 0;
            }
        }
    }
}