namespace WebApi
{
    public class AlunoUpdateDTO
    {
        public required int Id { get; set; }
        public string? CPF { get; set; }
        public string? Email { get; set; }
    }
}