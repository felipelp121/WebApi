using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DTO;
using WebApi.Model;
using WebApi.Repository.Interface;

namespace WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]

    public class AlunoController(
        IAlunoRepository alunoRepository, 
        IAlunoTurmaRepository alunoTurmaRepository,
        ITurmaRepository turmaRepository
        ) : ControllerBase
    {
        private readonly IAlunoRepository _alunoRepository = alunoRepository;
        private readonly IAlunoTurmaRepository _alunoTurmaRepository = alunoTurmaRepository;
        private readonly ITurmaRepository _turmaRepository = turmaRepository;
        private readonly int defaultFilterLimit = 10;
        private readonly int defaultFilterPage = 1;

        [HttpPost]
        public async Task<IActionResult> CreateAluno(CreateAlunoDTO alunoDTO)
        {
            try
            {
                var turma = await _turmaRepository.FindTurmaByCodigoAsync(alunoDTO.CodigoTurma);

                if (turma == null)
                {
                    return NotFound("Turma não encontrada");
                }

                var aluno = new Aluno
                {
                    Nome = alunoDTO.Nome,
                    CPF = alunoDTO.CPF,
                    Email = alunoDTO.Email
                };

                await _alunoRepository.CreateAlunoAsync(aluno);

                var enrollAmount = _alunoTurmaRepository.TurmaEnrollAmountById(turma.Id);

                if (enrollAmount >= turma.QuantidadeMaxima)
                {
                    return BadRequest("Matrícula negada, a turma não possui vaga");
                }

                var alunoTurma = new AlunoTurmas(aluno.Id, turma.Id, aluno, turma);

                await _alunoTurmaRepository.CreateAlunoTurmaAsync(alunoTurma);

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

                return StatusCode(500, "Erro ao criar o aluno.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAluno(UpdateAlunoDTO alunoDTO)
        {
            try
            {
                var aluno = await _alunoRepository.FindAlunoByIdAsync(alunoDTO.Id);

                if (aluno == null)
                {
                    return NotFound("Aluno não encontrado.");
                }

                if (!string.IsNullOrEmpty(alunoDTO.CPF)) aluno.CPF = alunoDTO.CPF;
                if (!string.IsNullOrEmpty(alunoDTO.Email)) aluno.Email = alunoDTO.Email;

                await _alunoRepository.UpdateAlunoAsync(aluno);

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

                return StatusCode(500, "Erro ao atualizar o aluno.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAluno(int id)
        {
            try
            {
                var aluno = await _alunoRepository.FindAlunoByIdAsync(id);

                if (aluno == null)
                {
                    return NotFound("Aluno não encontrado.");
                }

                var allEnrollAluno = _alunoTurmaRepository.GetAllEnrollAlunoById(id);

                await _alunoRepository.DeleteAlunoAsync(aluno, allEnrollAluno);

                return Ok("Aluno removido com sucesso.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao remover o aluno");
            }
        }


        [HttpPost("filter")]
        public async Task<IActionResult> FilterAluno([FromBody] FilterAlunoDTO alunoDTO)
        {
            try
            {
                int page = alunoDTO.Page ?? defaultFilterPage;
                int limit = alunoDTO.Limit ?? defaultFilterLimit;

                var (alunos, totalItems) = await _alunoRepository.FilterAlunosAsync(alunoDTO, page, limit);

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
                return StatusCode(500, "Erro ao buscar alunos");
            }
        }

    }
}

