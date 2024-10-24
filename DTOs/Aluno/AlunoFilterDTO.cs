namespace WebApi
{
    public class AlunoFilterDTO
    {
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public int? Page { get; set; }
        public int? Limit { get; set; }
    }
}