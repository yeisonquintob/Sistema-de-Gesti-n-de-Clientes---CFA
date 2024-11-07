using CFA.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CFA.DataAccess.Configuration
{
    public class TelefonoClienteConfiguration : IEntityTypeConfiguration<TelefonoCliente>
    {
        public void Configure(EntityTypeBuilder<TelefonoCliente> builder)
        {
            builder.ToTable("TelefonosCliente");

            builder.HasKey(e => e.IdTelefono);

            builder.Property(e => e.IdTelefono)
                .UseIdentityColumn();

            builder.Property(e => e.NumeroTelefono)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(e => e.TipoTelefono)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("GETDATE()");

            // Relaciones
            builder.HasOne(d => d.Cliente)
                .WithMany(p => p.Telefonos)
                .HasForeignKey(d => d.CodigoCliente)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}