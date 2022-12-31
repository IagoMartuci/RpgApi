using Microsoft.AspNetCore.Mvc;
using RpgApi.Data;
using System.Threading.Tasks;
using RpgApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class PersonagemHabilidadesController : ControllerBase
    {
        private readonly DataContext _context;
        public PersonagemHabilidadesController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddPersonagemHabilidadesAsync(PersonagemHabilidade novoPersonagemHabilidade)
        {
            try
            {
                Personagem personagem = await _context.Personagens
                    .Include(p => p.Arma)
                    .Include(p => p.PersonagemHabilidades).ThenInclude(ps => ps.Habilidade)
                    .FirstOrDefaultAsync(p => p.Id == novoPersonagemHabilidade.PersonagemId);

                if (personagem == null)
                {
                    throw new System.Exception("Personagem não encontrado para o Id informado");
                }

                Habilidade habilidade = await _context.Habilidades
                    .FirstOrDefaultAsync(h => h.Id == novoPersonagemHabilidade.HabilidadeId);

                if (habilidade == null)
                {
                    throw new System.Exception("Habilidade não encontrada");
                }

                PersonagemHabilidade ph = new PersonagemHabilidade();
                ph.Personagem = personagem;
                ph.Habilidade = habilidade;
                await _context.PersonagemHabilidades.AddAsync(ph);
                int linhasAfetadas = await _context.SaveChangesAsync();
                return Ok(linhasAfetadas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //DESAFIOS:

        [HttpGet("{personagemId}")] //DESAFIO 5 - OK
        public async Task<IActionResult> GetHabilidadesPersonagem(int personagemId)
        {
            try
            {
                /*
                List<PersonagemHabilidade> phLista = new List<PersonagemHabilidade>();
                phLista = await _context.PersonagemHabilidades
                .Include(p => p.Personagem)
                .Include(h => h.Habilidade)
                .Where(p => p.Personagem.Id == personagemId).ToListAsync();
                return Ok(phLista); //PQ A LISTA SAI DUPLICADA?
                */

                Personagem id = await _context.Personagens
                .Include(ph => ph.PersonagemHabilidades)
                .ThenInclude(h => h.Habilidade)
                .FirstOrDefaultAsync(pBusca => pBusca.Id == personagemId);
                return Ok(id); //ASSIM NÃO SAI DUPLICADO
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetHabilidades")] //DESAFIO 6 - OK
        public async Task<IActionResult> GetHabilidades()
        {
            try
            {
                List<Habilidade> habilidades = new List<Habilidade>();
                habilidades = await _context.Habilidades
                .Include(ph => ph.PersonagemHabilidades) //Para mostrar o id dos personagens que possuem as habilidades
                .ToListAsync();
                return Ok(habilidades);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DeletePersonagemHabilidade")] //DESAFIO 7 - OK
        public async Task<IActionResult> DeleteAsync(PersonagemHabilidade ph)
        {
            try
            {
                PersonagemHabilidade phRemover = await _context.PersonagemHabilidades
                    .FirstOrDefaultAsync(phBusca => phBusca.PersonagemId == ph.PersonagemId
                    && phBusca.HabilidadeId == ph.HabilidadeId);

                if(phRemover == null)
                {
                    throw new System.Exception("Personagem ou Habilidade não encontrados");
                }

                _context.PersonagemHabilidades.Remove(phRemover);
                int linhasAfetadas = await _context.SaveChangesAsync();
                return Ok(linhasAfetadas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}