namespace WebApi
{
    public class AlunoTurmas
    {
        public int Id { get; set;}
        public required int AlunoId { get; set;}
        public required int TurmaId { get; set;}

        public required Aluno Aluno { get; set;}
        public required Turma Turma { get; set;}
    }
}