
using CFA.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CFA.DataAccess.Configuration
{
    public class DireccionClienteConfiguration : IEntityTypeConfiguration<DireccionCliente>
    {
        public void Configure(EntityTypeBuilder<DireccionCliente> builder)
        {
            builder.ToTable("DireccionesCliente");

            builder.HasKey(e => e.IdDireccion);

            builder.Property(e => e.IdDireccion)
                .UseIdentityColumn();

            builder.Property(e => e.Direccion)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.TipoDireccion)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("GETDATE()");

            // Relaciones
            builder.HasOne(d => d.Cliente)
                .WithMany(p => p.Direcciones)
                .HasForeignKey(d => d.CodigoCliente)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
