namespace WebApi
{
    public class Turma
    {
        public int Id { get; set; }
        public required int Codigo { get; set; }
        public required int Nivel { get; set; }
        public required int QuantidadeMaxima { get; set; }

        public required ICollection<AlunoTurmas> AlunoTurmas { get; set; } = [];
    }
}