using WebApi.DTO;
using WebApi.Model;

namespace WebApi.Repository.Interface
{    public interface IAlunoRepository
    {
        Task CreateAlunoAsync(Aluno aluno);
        Task UpdateAlunoAsync(Aluno aluno);
        Task DeleteAlunoAsync(Aluno aluno, List<AlunoTurmas>? alunoTurmas);
        Task<Aluno?> FindAlunoByIdAsync(int id);
        Task<(IEnumerable<FillAlunoDTO> Alunos, int TotalItems)> FilterAlunosAsync(FilterAlunoDTO alunoDTO, int page, int limit);
    }
}