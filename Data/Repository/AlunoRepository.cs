using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTO;
using WebApi.Model;
using WebApi.Repository.Interface;

namespace WebApi.Repository
{
    public class AlunoRepository(ApplicationDbContext context) : IAlunoRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task CreateAlunoAsync(Aluno aluno)
        {
            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAlunoAsync(Aluno aluno)
        {
            _context.Alunos.Update(aluno);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAlunoAsync(Aluno aluno, List<AlunoTurmas>? allEnrollAluno)
        {
            _context.Alunos.Remove(aluno);
            if (allEnrollAluno != null)
            {
                _context.AlunoTurmas.RemoveRange(allEnrollAluno);
            }
            await _context.SaveChangesAsync();
        }


        public async Task<Aluno?> FindAlunoByIdAsync(int id)
        {
            return await _context.Alunos.FindAsync(id);
        }

        public async Task<(IEnumerable<FillAlunoDTO> Alunos, int TotalItems)> FilterAlunosAsync(FilterAlunoDTO alunoDTO, int page, int limit)
        {
            var query = _context.Alunos.AsQueryable();

            if (alunoDTO.Id.HasValue)
                query = query.Where(aluno => aluno.Id == alunoDTO.Id.Value);

            if (!string.IsNullOrEmpty(alunoDTO.Nome))
                query = query.Where(aluno => aluno.Nome.Contains(alunoDTO.Nome));

            if (!string.IsNullOrEmpty(alunoDTO.Email))
                query = query.Where(aluno => aluno.Email.Contains(alunoDTO.Email));

            int totalItems = await query.CountAsync();

            var alunos = await query
                .Select(aluno => new FillAlunoDTO
                {
                    Id = aluno.Id,
                    Nome = aluno.Nome,
                    CPF = aluno.CPF,
                    Email = aluno.Email
                })
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return (alunos, totalItems);
        }
    }
}