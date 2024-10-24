using Microsoft.EntityFrameworkCore;
using WebApi.Configuration;
using WebApi.Model;

namespace WebApi.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Turma> Turmas { get; set; }
        public DbSet<AlunoTurmas> AlunoTurmas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AlunoConfiguration());
            modelBuilder.ApplyConfiguration(new TurmaConfiguration());
            modelBuilder.ApplyConfiguration(new AlunoTurmasConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}