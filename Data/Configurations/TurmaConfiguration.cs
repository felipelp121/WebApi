using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WebApi.Model;

namespace WebApi.Configuration
{
    public class TurmaConfiguration : IEntityTypeConfiguration<Turma>
    {
        public void Configure(EntityTypeBuilder<Turma> builder)
        {
            builder.HasKey(turma => turma.Id);
            builder.Property(turma => turma.Codigo).IsRequired();
            builder.HasIndex(turma => turma.Codigo).IsUnique();
            builder.Property(turma => turma.Nivel).IsRequired();
            builder.Property(turma => turma.QuantidadeMaxima).IsRequired();
        }
    }
}