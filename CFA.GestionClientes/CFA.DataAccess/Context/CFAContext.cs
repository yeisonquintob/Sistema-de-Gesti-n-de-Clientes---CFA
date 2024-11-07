
using Microsoft.EntityFrameworkCore;
using CFA.Entities.Models;

namespace CFA.DataAccess.Context;

public class CFADbContext : DbContext
{
    public CFADbContext(DbContextOptions<CFADbContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<DireccionCliente> DireccionesCliente { get; set; }
    public DbSet<TelefonoCliente> TelefonosCliente { get; set; }
    public DbSet<TipoDocumento> TipoDocumentos { get; set; }
    public DbSet<Genero> Generos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar las entidades para que usen el esquema correcto
        modelBuilder.Entity<Cliente>().ToTable("Clientes");
        modelBuilder.Entity<DireccionCliente>().ToTable("DireccionesCliente");
        modelBuilder.Entity<TelefonoCliente>().ToTable("TelefonosCliente");
        modelBuilder.Entity<TipoDocumento>().ToTable("TipoDocumento");
        modelBuilder.Entity<Genero>().ToTable("Genero");

        // Configuraciones específicas
        modelBuilder.Entity<Cliente>()
            .HasIndex(c => new { c.TipoDocumento, c.NumeroDocumento })
            .IsUnique();

        modelBuilder.Entity<DireccionCliente>()
            .HasOne<Cliente>()
            .WithMany()
            .HasForeignKey(d => d.CodigoCliente);

        modelBuilder.Entity<TelefonoCliente>()
            .HasOne<Cliente>()
            .WithMany()
            .HasForeignKey(t => t.CodigoCliente);
    }
}