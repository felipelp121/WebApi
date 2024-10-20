namespace WebApi
{
    public class Aluno
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string CPF { get; set; }
        public required string Email { get; set; }

        public required ICollection<AlunoTurmas> AlunoTurmas { get; set; } = [];
    }
}