using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTO;
using WebApi.Model;

namespace WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]

    public class TurmaController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        private readonly int defaultFilterLimit = 10;
        private readonly int defaultFilterPage = 1;

        [HttpPost]
        public async Task<IActionResult> CreateTurma(CreateTurmaDTO turmaDTO)
        {
            try
            {
                var turma = new Turma(turmaDTO.Codigo, turmaDTO.Nivel, turmaDTO.QuantidadeMaxima);
                _context.Turmas.Add(turma);
                await _context.SaveChangesAsync();

                return Ok("Turma Cadastrada com sucesso!");
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro inesperado ao tentar criar uma turma.");
            }
        }

        [HttpPut]
        public async Task <IActionResult> UpdateTurma(UpdateTurmaDTO turmaDTO)
        {
            try 
            {
               var turma = await _context.Turmas.FindAsync(turmaDTO.Id);
        
                if (turma == null)
                {
                    return NotFound("Turma não encontrada.");
                }

                if (turmaDTO.Codigo.HasValue) turma.Codigo = (int)turmaDTO.Codigo;
                if (turmaDTO.Nivel.HasValue) turma.Nivel = (int)turmaDTO.Nivel;
                if (turmaDTO.QuantidadeMaxima.HasValue) turma.QuantidadeMaxima = (int)turmaDTO.QuantidadeMaxima;

                _context.Turmas.Update(turma);
                await _context.SaveChangesAsync();

                return Ok("Turma atualizada com sucesso.");
            }
            catch (Exception)
            {
                return StatusCode(500,"Erro ao atualizar a turma.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTurma(int id)
        {
            try
            {
                var turma = await _context.Turmas.FindAsync(id);
                
                if (turma == null)
                {
                    return NotFound("Turma não encontrada.");
                }

                var alunoTurmas = _context.AlunoTurmas.Where(alunoTurma => alunoTurma.TurmaId == id).Count();

                if (alunoTurmas > 0)
                {
                    return BadRequest("Exclusão negada, essa turma possui alunos");
                }

                _context.Turmas.Remove(turma);
                await _context.SaveChangesAsync();

                return Ok("Turma removida com sucesso.");
            }
            catch (Exception)
            {
                return StatusCode(500,"Erro ao remover a turma");
            }
        }

        [HttpPost("filter")]
        public async Task<IActionResult> FilterTurma([FromBody] FilterTurmaDTO turmaDTO)
        {
            try
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

                
                int page = turmaDTO.Page ?? defaultFilterPage;
                int limit = turmaDTO.Limit ?? defaultFilterLimit;

                var totalItems = await query.CountAsync();

                var turmas = await query.Select(turma => new FillTurmaDTO
                {
                    Id = turma.Id,
                    Codigo = turma.Codigo,
                    Nivel = turma.Nivel,
                    QuantidadeMaxima = turma.QuantidadeMaxima
                })
                    .Skip((page - defaultFilterPage) * limit)
                    .Take(limit)
                    .ToListAsync();

                
                return Ok(new 
                { 
                    Page = page,
                    Limit = limit,
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling((double)totalItems / limit),
                    Data = turmas
                });
            }
            catch (Exception)
            {
                return StatusCode(500,"Erro ao buscar turmas");
            } 
        }
    }
}