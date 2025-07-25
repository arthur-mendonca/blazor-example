﻿// <auto-generated />
using System;
using Loja.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Loja.Api.Migrations
{
    [DbContext(typeof(LojaDbContext))]
    [Migration("20250619215549_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.6");

            modelBuilder.Entity("Loja.Models.ItemPedido", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("PedidoId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("PrecoUnitario")
                        .HasColumnType("TEXT");

                    b.Property<int>("ProdutoId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantidade")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PedidoId");

                    b.HasIndex("ProdutoId");

                    b.ToTable("ItemPedido");
                });

            modelBuilder.Entity("Loja.Models.Pedido", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DataPedido")
                        .HasColumnType("TEXT");

                    b.Property<string>("EnderecoEntrega")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("ValorTotal")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Pedido");
                });

            modelBuilder.Entity("Loja.Models.Produto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Ativo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Categoria")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Descricao")
                        .HasColumnType("TEXT");

                    b.Property<int>("EstoqueQuantidade")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Imagem")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Preco")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Produto");
                });

            modelBuilder.Entity("Loja.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Ativo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CEP")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cidade")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("TEXT");

                    b.Property<string>("Endereco")
                        .HasColumnType("TEXT");

                    b.Property<string>("Estado")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Telefone")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("Loja.Models.ItemPedido", b =>
                {
                    b.HasOne("Loja.Models.Pedido", "Pedido")
                        .WithMany("Itens")
                        .HasForeignKey("PedidoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Loja.Models.Produto", "Produto")
                        .WithMany()
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pedido");

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("Loja.Models.Pedido", b =>
                {
                    b.HasOne("Loja.Models.Usuario", "Usuario")
                        .WithMany("Pedidos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Loja.Models.Pedido", b =>
                {
                    b.Navigation("Itens");
                });

            modelBuilder.Entity("Loja.Models.Usuario", b =>
                {
                    b.Navigation("Pedidos");
                });
#pragma warning restore 612, 618
        }
    }
}
