using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi
{
    public class AlunoConfiguration : IEntityTypeConfiguration<Aluno>
    {
        public void Configure(EntityTypeBuilder<Aluno> builder)
        {
            builder.HasKey(aluno => aluno.Id);
            builder.Property(aluno => aluno.Nome).IsRequired().HasMaxLength(100);
            builder.Property(aluno => aluno.Email).IsRequired().HasMaxLength(50);
            builder.HasIndex(aluno => aluno.Email).IsUnique();
            builder.Property(aluno => aluno.CPF).IsRequired().HasMaxLength(11);
            builder.HasIndex(aluno => aluno.CPF).IsUnique();
        }
    }
}
