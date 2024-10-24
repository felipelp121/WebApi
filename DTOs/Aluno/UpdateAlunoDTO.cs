namespace WebApi.DTO
{
    public class UpdateAlunoDTO
    {
        public required int Id { get; set; }
        public string? CPF { get; set; }
        public string? Email { get; set; }
    }
}