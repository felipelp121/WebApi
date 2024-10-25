using Microsoft.AspNetCore.Mvc;
using WebApi.Repository.Interface;
using WebApi.DTO;
using WebApi.Model;

namespace WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]

    public class TurmaController(
        ITurmaRepository turmaRepository,
        IAlunoTurmaRepository alunoTurmaRepository
        ) : ControllerBase
    {
        private readonly ITurmaRepository _turmaRepository = turmaRepository;
        private readonly IAlunoTurmaRepository _alunoTurmaRepository = alunoTurmaRepository;

        private readonly int defaultFilterLimit = 10;
        private readonly int defaultFilterPage = 1;

        [HttpPost]
        public async Task<IActionResult> CreateTurma(CreateTurmaDTO turmaDTO)
        {
            try
            {
                var turma = new Turma(turmaDTO.Codigo, turmaDTO.Nivel, turmaDTO.QuantidadeMaxima);
                await _turmaRepository.CreateTurmaAsync(turma);

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
               var turma = await _turmaRepository.FindTurmaByIdAsync(turmaDTO.Id);
        
                if (turma == null)
                {
                    return NotFound("Turma não encontrada.");
                }

                if (turmaDTO.Codigo.HasValue) turma.Codigo = (int)turmaDTO.Codigo;
                if (turmaDTO.Nivel.HasValue) turma.Nivel = (int)turmaDTO.Nivel;
                if (turmaDTO.QuantidadeMaxima.HasValue) turma.QuantidadeMaxima = (int)turmaDTO.QuantidadeMaxima;

                await _turmaRepository.UpdateTurmaAsync(turma);

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
                var turma = await _turmaRepository.FindTurmaByIdAsync(id);
                
                if (turma == null)
                {
                    return NotFound("Turma não encontrada.");
                }

                var alunoTurmas = _alunoTurmaRepository.TurmaEnrollAmountById(id);

                if (alunoTurmas > 0)
                {
                    return BadRequest("Exclusão negada, essa turma possui alunos");
                }

                await _turmaRepository.DeleteTurmaAsync(turma);

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
                int page = turmaDTO.Page ?? defaultFilterPage;
                int limit = turmaDTO.Limit ?? defaultFilterLimit;

                var (turmas, totalItems) = await _turmaRepository.FilterTurmasAsync(turmaDTO, page, limit);

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