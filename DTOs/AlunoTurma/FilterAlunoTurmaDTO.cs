namespace WebApi.DTO
{
    public class FilterAlunoTurmaDTO
    {
        public int? AlunoId { get; set; }
        public int? TurmaId { get; set; }
        public int? Page { get; set; }
        public int? Limit { get; set; }
    }
}