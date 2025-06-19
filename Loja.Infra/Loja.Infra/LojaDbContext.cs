using Microsoft.EntityFrameworkCore;
using Loja.Models;

namespace Loja.Infra.Data;

public class LojaDbContext : DbContext
{
    public LojaDbContext(DbContextOptions<LojaDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; } = default!;
    public DbSet<Produto> Produtos { get; set; } = default!;
    public DbSet<Pedido> Pedidos { get; set; } = default!;
    public DbSet<ItemPedido> ItensPedido { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurações do Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Senha).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configurações do Produto
        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Preco).HasColumnType("decimal(10,2)");
            entity.Property(e => e.Categoria).IsRequired().HasMaxLength(100);
        });

        // Configurações do Pedido
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ValorTotal).HasColumnType("decimal(10,2)");
            entity.HasOne(e => e.Usuario)
                  .WithMany(u => u.Pedidos)
                  .HasForeignKey(e => e.UsuarioId);
        });

        // Configurações do ItemPedido
        modelBuilder.Entity<ItemPedido>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PrecoUnitario).HasColumnType("decimal(10,2)");
            entity.HasOne(e => e.Pedido)
                  .WithMany(p => p.Itens)
                  .HasForeignKey(e => e.PedidoId);
            entity.HasOne(e => e.Produto)
                  .WithMany()
                  .HasForeignKey(e => e.ProdutoId);
        });

        // SEED DE DADOS - Usuários padrão
        modelBuilder.Entity<Usuario>().HasData(
            new Usuario
            {
                Id = 1,
                Nome = "Administrador",
                Email = "admin@loja.com",
                Senha = "123456",
                DataCadastro = new DateTime(2024, 1, 1),
                Ativo = true
            },
            new Usuario
            {
                Id = 2,
                Nome = "Usuario Teste",
                Email = "teste@loja.com",
                Senha = "123456",
                DataCadastro = new DateTime(2024, 1, 1),
                Ativo = true
            }
        );

        base.OnModelCreating(modelBuilder);
    }
}