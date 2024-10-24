namespace WebApi.Model
{
    public class AlunoTurmas
    {
        public int Id { get; set;}
        public int AlunoId { get; set;}
        public int TurmaId { get; set;}

        public Aluno? Aluno { get; set;}
        public Turma? Turma { get; set;}

        public AlunoTurmas(){}

        public AlunoTurmas(int alunoId, int turmaId) : this()
        {
            AlunoId = alunoId;
            TurmaId = turmaId;
        }
        public AlunoTurmas(int alunoId, int turmaId, Aluno aluno, Turma turma) : this()
        {
            AlunoId = alunoId;
            TurmaId = turmaId;
            Aluno = aluno;
            Turma = turma;
        }

    }
}