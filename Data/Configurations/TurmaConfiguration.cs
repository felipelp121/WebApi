using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi
{
    public class TurmaConfiguration : IEntityTypeConfiguration<Turma>
    {
        public void Configure(EntityTypeBuilder<Turma> builder)
        {
            builder.HasKey(turma => turma.Id);
            builder.Property(turma => turma.Codigo).IsRequired();
            builder.Property(turma => turma.Nivel).IsRequired();
            builder.Property(turma => turma.QuantidadeMaxima).IsRequired();
        }
    }
}