using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTO;
using WebApi.Model;

namespace WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]

    public class AlunoTurmaController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        private readonly int defaultFilterLimit = 10;
        private readonly int defaultFilterPage = 1;


        [HttpPost]
        public async Task<IActionResult> EnrollAlunoTurma(EnrollAlunoTurmaDTO alunoTurmaDTO)
        {
            try
            {
                var alunoTurmaExistente = await _context.AlunoTurmas.FirstOrDefaultAsync(
                    alunoTurma => alunoTurma.AlunoId == alunoTurmaDTO.AlunoId 
                    && alunoTurma.TurmaId == alunoTurmaDTO.TurmaId
                    );

                if (alunoTurmaExistente != null)
                {
                    return Conflict("Aluno já cadastrado nessa turma");
                }

                var turma = await _context.Turmas.FindAsync(alunoTurmaDTO.TurmaId);

                if (turma == null)
                {
                    return NotFound("Turma não encontrada");
                }

                var enrollAmount = _context.AlunoTurmas.Where(alunoTurma => alunoTurma.TurmaId == alunoTurmaDTO.TurmaId).Count();

                if (enrollAmount >= turma.QuantidadeMaxima)
                {
                    return BadRequest("Matrícula negada, a turma não possui vaga");
                }

                var alunoTurma = new AlunoTurmas(alunoTurmaDTO.AlunoId, alunoTurmaDTO.TurmaId);

                _context.AlunoTurmas.Add(alunoTurma);
                await _context.SaveChangesAsync();

                return Ok("Aluno matrículado com sucesso na turma");
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao matricular um Aluno em uma Turma");
            }
        }

        [HttpDelete]

        public async Task<ActionResult> DeleteEnrollAlunoTurma(EnrollAlunoTurmaDTO alunoTurmaDTO)
        {
            var alunoTurmas = _context.AlunoTurmas.Where(
                alunoTurma => alunoTurma.AlunoId == alunoTurmaDTO.AlunoId 
                && alunoTurma.TurmaId == alunoTurmaDTO.TurmaId
                ).ToList();
            
            if (alunoTurmas == null)
            {
                return BadRequest("Não existe matrículas associadas a esse aluno com essa turma");
            }

            _context.AlunoTurmas.RemoveRange(alunoTurmas);
            await _context.SaveChangesAsync();

            return Ok("Exclusão de matrícula realizada com sucesso");

        }


        [HttpPost("filter")]
        public async Task<IActionResult> FilterAlunoTurma([FromBody] FilterAlunoTurmaDTO alunoTurmaDTO)
        {
            try
            {
                var query = _context.AlunoTurmas.AsQueryable();

                if (alunoTurmaDTO.AlunoId.HasValue)
                {
                    query = query.Where(alunoTurma => alunoTurma.AlunoId == alunoTurmaDTO.AlunoId.Value);
                }

                if (alunoTurmaDTO.TurmaId.HasValue)
                {
                    query = query.Where(alunoTurma => alunoTurma.TurmaId == alunoTurmaDTO.TurmaId.Value);
                }

                
                int page = alunoTurmaDTO.Page ?? defaultFilterPage;
                int limit = alunoTurmaDTO.Limit ?? defaultFilterLimit;

                var totalItems = await query.CountAsync();

                var alunoTurmas = await query.Select(alunoTurma => new FillAlunoTurmaDTO
                {
                    AlunoId = alunoTurma.AlunoId,
                    TurmaId = alunoTurma.TurmaId,
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
                    Data = alunoTurmas
                });
            }
            catch (Exception)
            {
                return StatusCode(500,"Erro ao buscar turmas");
            } 
        }
    }
}