namespace WebApi
{
    public class CreateAlunoDTO
    {
        public required string Nome { get; set; }
        public required string CPF { get; set; }
        public required string Email { get; set; }
        public required int CodigoTurma { get; set; }
    }
}