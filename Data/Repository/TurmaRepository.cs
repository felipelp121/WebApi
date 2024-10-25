using WebApi.Data;
using WebApi.Repository.Interface;
using WebApi.Model;
using WebApi.DTO;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Repository
{
    public class TurmaRepository(ApplicationDbContext context) : ITurmaRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task CreateTurmaAsync(Turma turma)
        {
            _context.Turmas.Add(turma);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTurmaAsync(Turma turma)
        {
            _context.Turmas.Update(turma);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteTurmaAsync(Turma turma)
        {
            _context.Turmas.Remove(turma);
            await _context.SaveChangesAsync();
        }


        public async Task<Turma?> FindTurmaByIdAsync(int id)
        {
            return await _context.Turmas.FindAsync(id);
        }
        public async Task<Turma?> FindTurmaByCodigoAsync(int codigo)
        {
            return await _context.Turmas.FirstOrDefaultAsync(turma => turma.Codigo == codigo);
        }

        public async Task<(IEnumerable<FillTurmaDTO> Turmas, int TotalItems)> FilterTurmasAsync(FilterTurmaDTO turmaDTO, int page, int limit)
        {
            var query = _context.Turmas.AsQueryable();

                if (turmaDTO.Id.HasValue)
                {
                    query = query.Where(turma => turma.Id == turmaDTO.Id.Value);
                }

                if (turmaDTO.Codigo.HasValue)
                {
                    query = query.Where(turma => turma.Codigo == turmaDTO.Codigo.Value);
                }

                if (turmaDTO.Nivel.HasValue)
                {
                    query = query.Where(turma => turma.Nivel == turmaDTO.Nivel.Value);
                }

                var totalItems = await query.CountAsync();

                var turmas = await query.Select(turma => new FillTurmaDTO
                {
                    Id = turma.Id,
                    Codigo = turma.Codigo,
                    Nivel = turma.Nivel,
                    QuantidadeMaxima = turma.QuantidadeMaxima
                })
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToListAsync();

            return (turmas, totalItems);
        }
    }
}