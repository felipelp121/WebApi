using Microsoft.AspNetCore.Mvc;
using WebApi.DTO;
using WebApi.Model;
using WebApi.Repository.Interface;

namespace WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]

    public class AlunoTurmaController(
        IAlunoTurmaRepository alunoTurmaRepository,
        ITurmaRepository turmaRepository
        ) : ControllerBase
    {
        private readonly IAlunoTurmaRepository _alunoTurmaRepository = alunoTurmaRepository;
        private readonly ITurmaRepository _turmaRepository = turmaRepository;

        private readonly int defaultFilterLimit = 10;
        private readonly int defaultFilterPage = 1;


        [HttpPost]
        public async Task<IActionResult> EnrollAlunoTurma(EnrollAlunoTurmaDTO enrollAlunoTurmaDTO)
        {
            try
            {
                var enroll = await _alunoTurmaRepository.GetEnrollAlunoTurma(enrollAlunoTurmaDTO);

                if (enroll != null)
                {
                    return Conflict("Aluno já cadastrado nessa turma");
                }

                var turma = await _turmaRepository.FindTurmaByIdAsync(enrollAlunoTurmaDTO.TurmaId);

                if (turma == null)
                {
                    return NotFound("Turma não encontrada");
                }

                var turmaEnrollAmount = _alunoTurmaRepository.TurmaEnrollAmountById(enrollAlunoTurmaDTO.TurmaId);

                if (turmaEnrollAmount >= turma.QuantidadeMaxima)
                {
                    return BadRequest("Matrícula negada, a turma não possui vaga");
                }

                var alunoTurma = new AlunoTurmas(enrollAlunoTurmaDTO.AlunoId, enrollAlunoTurmaDTO.TurmaId);

                await _alunoTurmaRepository.CreateAlunoTurmaAsync(alunoTurma);

                return Ok("Aluno matrículado com sucesso na turma");
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao matricular um Aluno em uma Turma");
            }
        }

        [HttpDelete]

        public async Task<ActionResult> DeleteEnrollAlunoTurma(EnrollAlunoTurmaDTO enrollAlunoTurmaDTO)
        {
            var enroll = await _alunoTurmaRepository.GetEnrollAlunoTurma(enrollAlunoTurmaDTO);
            
            if (enroll == null)
            {
                return BadRequest("Não existe matrículas associadas a esse aluno com essa turma");
            }

            await _alunoTurmaRepository.DeleteAlunoTurmaAsync(enroll);

            return Ok("Exclusão de matrícula realizada com sucesso");

        }


        [HttpPost("filter")]
        public async Task<IActionResult> FilterAlunoTurma([FromBody] FilterAlunoTurmaDTO filterAlunoTurmaDTO)
        {
            try
            {
                int page = filterAlunoTurmaDTO.Page ?? defaultFilterPage;
                int limit = filterAlunoTurmaDTO.Limit ?? defaultFilterLimit;

                var (alunoTurmas, totalItems) = await _alunoTurmaRepository.FilterAlunoTurmaAsync(filterAlunoTurmaDTO, page, limit);

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