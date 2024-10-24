using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi
{
    [Route("api/[controller]")]
    [ApiController]

    public class AlunoController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly int defaultFilterLimit = 10;
        private readonly int defaultFilterPage = 1;

        [HttpPost]
        public async Task<IActionResult> CreateAluno(AlunoCreateDTO alunoDTO)
        {
            try
            {
                var turma = await _context.Turmas.FirstOrDefaultAsync(turma => turma.Codigo == alunoDTO.CodigoTurma);

                if (turma == null)
                {
                    return NotFound("Turma não encontrada");
                }

                var aluno = new Aluno(alunoDTO.Nome, alunoDTO.CPF, alunoDTO.Email);

                _context.Alunos.Add(aluno);
                await _context.SaveChangesAsync();

                // var alunoTurmaExistente = await _context.AlunoTurmas.FirstOrDefaultAsync(
                //     alunoTurma => alunoTurma.AlunoId == aluno.Id && alunoTurma.TurmaId == turma.Id
                //     );

                // if (alunoTurmaExistente != null)
                // {
                //     return Conflict("Aluno já cadastrado nessa turma");
                // }

                var alunoTurma = new AlunoTurmas(aluno.Id, turma.Id, aluno, turma);

                _context.AlunoTurmas.Add(alunoTurma);
                await _context.SaveChangesAsync();

                return Ok("Aluno cadastrado com sucesso");

            }
            catch (DbUpdateException expection)
            {
                if (expection.InnerException != null && expection.InnerException.Message.Contains("Alunos_CPF"))
                {
                    return Conflict("CPF já cadastrado.");
                }
                if (expection.InnerException != null && expection.InnerException.Message.Contains("Alunos_Email"))
                {
                    return Conflict("E-mail já cadastrado.");
                }

                return BadRequest("Erro ao criar o aluno.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAluno(AlunoUpdateDTO alunoDTO)
        {
            try 
            {
               var aluno = await _context.Alunos.FindAsync(alunoDTO.Id);
        
                if (aluno == null)
                {
                    return NotFound("Aluno não encontrado.");
                } 

                if (!string.IsNullOrEmpty(alunoDTO.CPF)) aluno.CPF = alunoDTO.CPF;
                if (!string.IsNullOrEmpty(alunoDTO.Email)) aluno.Email = alunoDTO.Email;

                _context.Alunos.Update(aluno);
                await _context.SaveChangesAsync();

                return Ok("Aluno atualizado com sucesso.");

            }
            catch (DbUpdateException expection)
            {
                if (expection.InnerException != null && expection.InnerException.Message.Contains("Alunos_CPF"))
                {
                    return Conflict("CPF já cadastrado.");
                }
                if (expection.InnerException != null && expection.InnerException.Message.Contains("Alunos_Email"))
                {
                    return Conflict("E-mail já cadastrado.");
                }

                return BadRequest("Erro ao atualizar o aluno.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAluno(int id)
        {
            try
            {
                var aluno = await _context.Alunos.FindAsync(id);
                
                if (aluno == null)
                {
                    return NotFound("Aluno não encontrado.");
                }

                _context.Alunos.Remove(aluno);
                await _context.SaveChangesAsync();

                return Ok("Aluno removido com sucesso.");
            }
            catch (Exception)
            {
                return BadRequest("Erro ao remover o aluno");
            }
        }


        [HttpPost("filter")]
        public async Task<IActionResult> GetAluno([FromBody] AlunoFilterDTO alunoDTO)
        {
            try
            {
                var query = _context.Alunos.AsQueryable();

                if (alunoDTO.Id.HasValue)
                {
                    query = query.Where(aluno => aluno.Id == alunoDTO.Id.Value);
                }

                if (!string.IsNullOrEmpty(alunoDTO.Nome))
                {
                    query = query.Where(aluno => aluno.Nome.Contains(alunoDTO.Nome));
                }

                if (!string.IsNullOrEmpty(alunoDTO.Email))
                {
                    query = query.Where(aluno => aluno.Email.Contains(alunoDTO.Email));
                }

                
                int page = alunoDTO.Page ?? defaultFilterPage;
                int limit = alunoDTO.Limit ?? defaultFilterLimit;

                var totalItems = await query.CountAsync();

                var alunos = await query
                    .Skip((page - defaultFilterPage) * limit)
                    .Take(limit)
                    .ToListAsync();

                
                return Ok(new 
                { 
                    Page = page,
                    Limit = limit,
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling((double)totalItems / limit),
                    Data = alunos
                });
            }
            catch (Exception)
            {
                return BadRequest("Erro ao buscar alunos");
            } 
        }

    }
}
