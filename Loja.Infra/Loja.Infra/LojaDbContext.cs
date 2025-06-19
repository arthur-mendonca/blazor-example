using Microsoft.EntityFrameworkCore;
using Loja.Models;

namespace Loja.Infra.Data;

public class LojaDbContext : DbContext
{
    public LojaDbContext(DbContextOptions<LojaDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; } = default!;

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