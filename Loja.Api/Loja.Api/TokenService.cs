using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Loja.DTO;

namespace Loja.Api;

public class TokenService
{
    public string Gerar(UsuarioDTO usuario)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(GetTokenDescriptor(usuario));
        return handler.WriteToken(token);
    }

    private static SecurityTokenDescriptor GetTokenDescriptor(UsuarioDTO usuario)
    {
        return new SecurityTokenDescriptor
        {
            Subject = GerarClaims(usuario),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = GetCredentials(),
        };
    }

    private static SigningCredentials GetCredentials()
    {
        var key = Encoding.ASCII.GetBytes(Config.Instancia.ChavePrivada);

        return new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature
        );
    }

    private static ClaimsIdentity GerarClaims(UsuarioDTO usuario)
    {
        var ci = new ClaimsIdentity();

        ci.AddClaim(new Claim(ClaimTypes.Name, usuario.Email));
        ci.AddClaim(new Claim("Id", usuario.Id.ToString()));

        return ci;
    }
}