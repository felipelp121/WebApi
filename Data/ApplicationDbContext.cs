using Microsoft.EntityFrameworkCore;

namespace WebApi
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Turma> Turmas { get; set; }
        public DbSet<AlunoTurmas> AlunoTurmas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AlunoConfiguration());
            modelBuilder.ApplyConfiguration(new TurmaConfiguration());
            modelBuilder.ApplyConfiguration(new AlunoTurmasConfiguration());
            base.OnModelCreating(modelBuilder);

            // // Tabela Aluno
            // modelBuilder.Entity<Aluno>(entity =>
            // {
            //     entity.HasKey(aluno => aluno.Id);
            //     entity.Property(aluno => aluno.Nome).IsRequired().HasMaxLength(100);
            //     entity.Property(aluno => aluno.CPF).IsRequired().HasMaxLength(11);
            //     entity.HasIndex(aluno => aluno.CPF).IsUnique();
            // });

            // // Tabela Turma
            // modelBuilder.Entity<Turma>(entity =>
            // {
            //     entity.HasKey(turma => turma.Id);
            //     entity.Property(turma => turma.Codigo).IsRequired();
            //     entity.Property(turma => turma.Level).IsRequired();
            //     entity.Property(turma => turma.Quantidade).IsRequired();
            //     entity.Property(turma => turma.QuantidadeMaxima).IsRequired();
            // });

            // // Tabela AlunoTurma
            // modelBuilder.Entity<AlunoTurma>(entity =>
            // {
            //     entity.HasKey(alunoTurmas => alunoTurmas.Id);
            //     entity.HasOne(alunoTurmas => alunoTurmas.Aluno)
            //         .WithMany(aluno => aluno.AlunoTurmas)
            //         .HasForeignKey(alunoTurmas => alunoTurmas.AlunoId);

            //     entity.HasOne(alunoTurmas => alunoTurmas.Turma)
            //         .WithMany(turma => turma.AlunoTurmas)
            //         .HasForeignKey(alunoTurmas => alunoTurmas.TurmaId);
            // });
        }
    }
}