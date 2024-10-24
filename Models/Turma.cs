namespace WebApi
{
    public class Turma
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public int Nivel { get; set; }
        public int QuantidadeMaxima { get; set; }

        public ICollection<AlunoTurmas> AlunoTurmas { get; set; } = [];

        public Turma(){}
        public Turma(int codigo, int nivel, int qtdMaxima)
        {
            Codigo = codigo;
            Nivel = nivel;
            QuantidadeMaxima = qtdMaxima;
        }
    }
}