using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi
{
    public class AlunoTurmasConfiguration : IEntityTypeConfiguration<AlunoTurmas>
    {
        public void Configure(EntityTypeBuilder<AlunoTurmas> builder)
        {
            builder.HasKey(alunoTurmas => alunoTurmas.Id);
            builder.HasOne(alunoTurmas => alunoTurmas.Aluno)
                .WithMany(aluno => aluno.AlunoTurmas)
                .HasForeignKey(alunoTurmas => alunoTurmas.AlunoId);

            builder.HasOne(alunoTurmas => alunoTurmas.Turma)
                .WithMany(turma => turma.AlunoTurmas)
                .HasForeignKey(alunoTurmas => alunoTurmas.TurmaId);
        }
    }
}