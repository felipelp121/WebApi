namespace WebApi.DTO
{
    public class FillAlunoDTO
    {
        public required int Id { get; set; }
        public string? Nome { get; set; }
        public required string CPF { get; set; }
        public required string Email { get; set; }
    }
}
