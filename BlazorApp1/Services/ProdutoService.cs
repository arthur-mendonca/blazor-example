using BlazorApp1.Models;
using System.Collections.Generic;
using System.Linq;

namespace BlazorApp1.Services
{
    public class ProdutoService
    {
        private List<Produto> produtos = new List<Produto>
        {
            new Produto { Id = 1, Nome = "Celular 1", Preco = 1999.99m, Imagem = "/img/cel1.webp" },
            new Produto { Id = 2, Nome = "Celular 2", Preco = 2999.99m, Imagem = "/img/cel2.webp" },
            new Produto { Id = 3, Nome = "Celular 3", Preco = 3999.99m, Imagem = "/img/cel3.webp" },
            new Produto { Id = 4, Nome = "Celular 4", Preco = 4999.99m, Imagem = "/img/cel4.webp" },
            new Produto { Id = 5, Nome = "Celular 5", Preco = 5999.99m, Imagem = "/img/cel5.webp" },
            new Produto { Id = 6, Nome = "Celular 6", Preco = 6999.99m, Imagem = "/img/cel6.webp" }
        };

        public List<Produto> GetProdutos() => produtos;

        public Produto GetProdutoById(int id) => produtos.FirstOrDefault(p => p.Id == id);
    }
}