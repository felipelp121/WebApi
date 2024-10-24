using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi
{
    [Route("api/[controller]")]
    [ApiController]

    public class AlunoTurmaController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;


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
    }
}