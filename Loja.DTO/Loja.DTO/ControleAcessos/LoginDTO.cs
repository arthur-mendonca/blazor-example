using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loja.DTO.ControleAcessos
{
    public record struct LoginDTO(string Email, string Senha)
    {
    }

}