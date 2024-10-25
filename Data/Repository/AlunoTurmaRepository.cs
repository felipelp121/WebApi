using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTO;
using WebApi.Model;
using WebApi.Repository.Interface;

namespace WebApi.Repository
{
    public class AlunoTurmaRepository(ApplicationDbContext context) : IAlunoTurmaRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task CreateAlunoTurmaAsync(AlunoTurmas alunoTurmas)
        {
            _context.AlunoTurmas.Add(alunoTurmas);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAlunoTurmaAsync(AlunoTurmas alunoTurmas)
        {
            _context.AlunoTurmas.Update(alunoTurmas);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAlunoTurmaAsync(AlunoTurmas alunoTurmas)
        {
            _context.AlunoTurmas.Remove(alunoTurmas);
            await _context.SaveChangesAsync();
        }


        public async Task<AlunoTurmas?> FindAlunoTurmaByIdAsync(int id)
        {
            return await _context.AlunoTurmas.FindAsync(id);
        }

        public async Task<(IEnumerable<FillAlunoTurmaDTO> Alunos, int TotalItems)> FilterAlunoTurmaAsync(FilterAlunoTurmaDTO filterAlunoTurmaDTO, int page, int limit)
        {
            var query = _context.AlunoTurmas.AsQueryable();

            if (filterAlunoTurmaDTO.AlunoId.HasValue)
            {
                query = query.Where(alunoTurma => alunoTurma.AlunoId == filterAlunoTurmaDTO.AlunoId.Value);
            }

            if (filterAlunoTurmaDTO.TurmaId.HasValue)
            {
                query = query.Where(alunoTurma => alunoTurma.TurmaId == filterAlunoTurmaDTO.TurmaId.Value);
            }

            var totalItems = await query.CountAsync();

            var alunoTurmas = await query.Select(alunoTurma => new FillAlunoTurmaDTO
            {
                AlunoId = alunoTurma.AlunoId,
                TurmaId = alunoTurma.TurmaId,
            })
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return (alunoTurmas, totalItems);
        }

        public async Task<AlunoTurmas?> GetEnrollAlunoTurma(EnrollAlunoTurmaDTO enrollAlunoTurmaDTO)
        {
            return await _context.AlunoTurmas.FirstOrDefaultAsync(
                alunoTurma => alunoTurma.AlunoId == enrollAlunoTurmaDTO.AlunoId 
                && alunoTurma.TurmaId == enrollAlunoTurmaDTO.TurmaId
                );
        }
        public List<AlunoTurmas>? GetAllEnrollAlunoById(int alunoId)
        {
            return [.. _context.AlunoTurmas.Where(alunoTurma => alunoTurma.AlunoId == alunoId)];
        }

        public int TurmaEnrollAmountById(int turmaId)
        {
            return _context.AlunoTurmas.Where(alunoTurma => alunoTurma.TurmaId == turmaId).Count();
        }
    }
}