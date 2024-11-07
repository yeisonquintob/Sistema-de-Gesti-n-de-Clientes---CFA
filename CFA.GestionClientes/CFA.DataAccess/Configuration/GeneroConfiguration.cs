
using CFA.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CFA.DataAccess.Configuration
{
    public class GeneroConfiguration : IEntityTypeConfiguration<Genero>
    {
        public void Configure(EntityTypeBuilder<Genero> builder)
        {
            builder.ToTable("Genero");

            builder.HasKey(e => e.IdGenero);

            builder.Property(e => e.IdGenero)
                .HasMaxLength(1)
                .IsFixedLength();

            builder.Property(e => e.Descripcion)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}