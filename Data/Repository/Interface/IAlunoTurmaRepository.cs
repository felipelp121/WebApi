using WebApi.DTO;
using WebApi.Model;

namespace WebApi.Repository.Interface
{
    public interface IAlunoTurmaRepository
    {
        Task CreateAlunoTurmaAsync(AlunoTurmas alunoTurma);
        Task UpdateAlunoTurmaAsync(AlunoTurmas alunoTurma);
        Task DeleteAlunoTurmaAsync(AlunoTurmas alunoTurma);
        Task<AlunoTurmas?> FindAlunoTurmaByIdAsync(int id);
        Task<(IEnumerable<FillAlunoTurmaDTO> Alunos, int TotalItems)> FilterAlunoTurmaAsync(FilterAlunoTurmaDTO filterAlunoTurmaDTO, int page, int limit);

        Task<AlunoTurmas?> GetEnrollAlunoTurma(EnrollAlunoTurmaDTO enrollAlunoTurmaDTO);
        int TurmaEnrollAmountById(int turmaId);
        List<AlunoTurmas>? GetAllEnrollAlunoById(int alunoId);
    }
}