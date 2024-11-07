
using CFA.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CFA.DataAccess.Configuration
{
    public class TipoDocumentoConfiguration : IEntityTypeConfiguration<TipoDocumento>
    {
        public void Configure(EntityTypeBuilder<TipoDocumento> builder)
        {
            builder.ToTable("TipoDocumento");

            builder.HasKey(e => e.IdTipoDocumento);

            builder.Property(e => e.IdTipoDocumento)
                .HasMaxLength(2)
                .IsFixedLength();

            builder.Property(e => e.Descripcion)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}