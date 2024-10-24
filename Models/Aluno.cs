namespace WebApi.Model
{
    public class Aluno
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string CPF { get; set; }
        public required string Email { get; set; }

        public ICollection<AlunoTurmas> AlunoTurmas { get; set; } = [];

        public Aluno(){}

        public Aluno(string nome, string cpf, string email)
        {
            Nome = nome;
            CPF = cpf;
            Email = email;
        }
    }
}