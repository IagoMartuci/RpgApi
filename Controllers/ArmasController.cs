using Microsoft.AspNetCore.Mvc;
using RpgApi.Data;
using System.Threading.Tasks;
using RpgApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ArmasController : ControllerBase
    {
        private readonly DataContext _context;
        public ArmasController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Arma> lista = await _context.Armas.ToListAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            try
            {
                Arma arma = await _context.Armas
                    .FirstOrDefaultAsync(armaBusca => armaBusca.Id == id);

                return Ok(arma);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*[HttpPost] //ANTIGO
        public async Task<IActionResult> AddArmaAntigo(Arma novaArma)
        {
            try
            {
                if (novaArma.Dano <= 0 || novaArma.Dano > 25)
                {
                    throw new Exception("Dano não pode ser menor ou igual a 0, ou maior que 25");
                }

                Personagem p = await _context.Personagens
                    .FirstOrDefaultAsync(p => p.Id == novaArma.PersonagemId);

                if (p == null)
                {
                    throw new System.Exception("Não existe personagem com o Id informado");
                }

                await _context.Armas.AddAsync(novaArma);
                await _context.SaveChangesAsync();
                return Ok(novaArma.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/

        [HttpPost] //NOVO
        public async Task<IActionResult> Add(Arma novaArma)
        {
            try
            {
                if (novaArma.Dano == 0)
                {
                    throw new System.Exception("O dano da arma não ser 0");
                }

                Personagem personagem = await _context.Personagens
                    .FirstOrDefaultAsync(p => p.Id == novaArma.PersonagemId);

                if (personagem == null)
                    throw new System.Exception("Seu usuário não contém personagens com o Id do Personagem informado.");

                Arma buscaArma = await _context.Armas
                    .FirstOrDefaultAsync(a => a.PersonagemId == novaArma.PersonagemId);

                if (buscaArma != null)
                    throw new System.Exception("O personagem selecionado já contém uma arma atribuída a ele.");

                await _context.Armas.AddAsync(novaArma);
                await _context.SaveChangesAsync();

                return Ok(novaArma.Id);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut] //ANTIGO
        public async Task<IActionResult> UpdateArmaAntigo(Arma novaArma)
        {
            try
            {
                if (novaArma.Dano <= 0 || novaArma.Dano > 25)
                {
                    throw new Exception("Dano não pode ser menor ou igual a 0, ou maior que 25");
                }

                _context.Armas.Update(novaArma);
                int linhasAfetadas = await _context.SaveChangesAsync();
                return Ok(linhasAfetadas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*[HttpPut] //NOVO
        public async Task<IActionResult> Update(Arma novaArma)
        {
            try
            {
                if (novaArma.Dano == 0)
                {
                    throw new System.Exception("O dano da arma não ser 0");
                }

                Personagem personagem = await _context.Personagens
                    .FirstOrDefaultAsync(p => p.Id == novaArma.PersonagemId);

                if (personagem == null)
                    throw new System.Exception("Seu usuário não contém personagens com o Id do Personagem informado.");

                Arma buscaArma = await _context.Armas
                    .FirstOrDefaultAsync(a => a.PersonagemId == novaArma.PersonagemId);

                if (buscaArma != null)
                    throw new System.Exception("O personagem selecionado já contém uma arma atribuída a ele.");

                _context.Armas.Update(novaArma);
                int linhasAfetadas = await _context.SaveChangesAsync();

                return Ok(linhasAfetadas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Arma aRemover = await _context.Armas
                    .FirstOrDefaultAsync(a => a.Id == id);

                _context.Armas.Remove(aRemover);
                int linhasAfetadas = await _context.SaveChangesAsync();

                return Ok(linhasAfetadas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}