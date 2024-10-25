using WebApi.DTO;
using WebApi.Model;

namespace WebApi.Repository.Interface
{   
    public interface ITurmaRepository
    {
        Task CreateTurmaAsync(Turma turma);
        Task UpdateTurmaAsync(Turma turma);
        Task DeleteTurmaAsync(Turma turma);
        Task<Turma?> FindTurmaByIdAsync(int id);
        Task<Turma?> FindTurmaByCodigoAsync(int codigo);
        Task<(IEnumerable<FillTurmaDTO> Turmas, int TotalItems)> FilterTurmasAsync(FilterTurmaDTO turmaDTO, int page, int limit);
    }
}