namespace WebApi.DTO
{
    public class FillTurmaDTO
    {
        public required int Id { get; set; }
        public required int Codigo { get; set; }
        public required int Nivel { get; set; }
        public required int QuantidadeMaxima { get; set; }

    }
}