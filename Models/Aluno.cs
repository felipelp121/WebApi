namespace WebApi
{
    public class Aluno
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }

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