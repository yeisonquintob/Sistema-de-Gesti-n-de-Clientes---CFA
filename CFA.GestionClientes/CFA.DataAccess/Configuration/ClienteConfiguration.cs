
using CFA.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CFA.DataAccess.Configuration
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes");

            builder.HasKey(e => e.Codigo);

            builder.Property(e => e.Codigo)
                .UseIdentityColumn()
                .HasMaxLength(11);

            builder.Property(e => e.TipoDocumento)
                .IsRequired()
                .HasMaxLength(2);

            builder.Property(e => e.NumeroDocumento)
                .IsRequired()
                .HasMaxLength(11);

            builder.Property(e => e.Nombres)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(e => e.Apellido1)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(e => e.Apellido2)
                .HasMaxLength(30);

            builder.Property(e => e.Genero)
                .IsRequired()
                .HasMaxLength(1);

            builder.Property(e => e.FechaNacimiento)
                .IsRequired();

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("GETDATE()");

            // Relaciones
            builder.HasOne(d => d.TipoDocumentoNavigation)
                .WithMany(p => p.Clientes)
                .HasForeignKey(d => d.TipoDocumento);

            builder.HasOne(d => d.GeneroNavigation)
                .WithMany(p => p.Clientes)
                .HasForeignKey(d => d.Genero);

            // Índices
            builder.HasIndex(e => new { e.TipoDocumento, e.NumeroDocumento })
                .IsUnique()
                .HasDatabaseName("UQ_Cliente_Documento");
        }
    }
}