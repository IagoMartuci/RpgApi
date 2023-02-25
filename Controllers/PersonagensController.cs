using Microsoft.AspNetCore.Mvc;
using RpgApi.Data;
using System.Threading.Tasks;
using RpgApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using RpgApi.Models.Enuns;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;

namespace RpgApi.Controllers
{
    [Authorize(Roles = "Jogador, Admin")] // Roles são os papeis que o usuário terá, neste caso irá permitir que tanto o admin quanto o jogador acessem a controller.
    [ApiController]
    [Route("[Controller]")] //Abstrai a necessidade de colocar a palavra Controller no endereço http.
    public class PersonagensController : ControllerBase
    {
        //Programaçao de toda a classe ficara aqui abaixo:
        private readonly DataContext _context; //Declaração do atributo
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PersonagensController(DataContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            //Inicialização do atributo a partir de um parametro
            _context = context; //Injeção de dependencia, o que é?
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        private int ObterUsuarioId()
        {
            return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        private string ObterPerfilUsuario()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }

        [HttpGet("GetByPerfil")]
        public async Task<IActionResult> GetByPerfilAsync()
        {
            try
            {
                var lista = new List<Personagem>();

                if (ObterPerfilUsuario() == "Admin")
                {
                    lista = await _context.Personagens.ToListAsync();
                }
                else
                {
                    lista = await _context.Personagens
                        .Where(p => p.Usuario.Id == ObterUsuarioId()).ToListAsync();
                }
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id) //Qual a função do async?
        {
            try //Como funciona o try/catch? Seria uma condicional, o que tem que ser programado cai no try, o que for exceção cai no catch.
            {
                Personagem p = await _context.Personagens //Qual a função do await?
                    .Include(ar => ar.Arma) //Inclui na propriedade Arma do objeto p
                    .Include(us => us.Usuario) // DESAFIO 4 - OK (Testado via UPDATE manual no SQL, vinculando Usuário Id X no Personagem Id Y)
                    .Include(ph => ph.PersonagemHabilidades) //Inclui na propriedade PersonagemHabilidade do objeto p (PersonagemHabilidade.cs)
                        .ThenInclude(h => h.Habilidade) //Inclui na lista de PersonagemHabilidade de p (Habilidade.cs)
                    .FirstOrDefaultAsync(pBusca => pBusca.Id == id);

                return Ok(p);
            }
            catch (Exception ex) //Como funciona o Exception?
            {
                return BadRequest(ex.Message); //No Postman retornou 204, e não o BadRequest, pq?
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Personagem> lista = await _context.Personagens.ToListAsync();
                return Ok(lista);
            }
            catch (Exception ex) //Qual seria a exceção neste caso já que o Método é um GetAll?
            {
                return BadRequest(ex.Message); //Qual seria a mensagem da variavel Message? Seria uma mensagem padrão ou nós que temos que definir?
            }
        }

        public async Task<bool> PersonagemExistente(string pNome)
        {
            if (await _context.Personagens.AnyAsync(x => x.Nome.ToLower() == pNome.ToLower()))
            {
                return true;
            }
            return false;
        }

        [HttpPost]
        public async Task<IActionResult> Add(Personagem novoPersonagem) //Método POST ok
        {
            try
            {
                if (novoPersonagem.Nome.Length < 3)
                {
                    throw new Exception("Nome de personagem deve conter 3 ou mais caracteres");
                }
                else if (await PersonagemExistente(novoPersonagem.Nome))
                {
                    throw new Exception("Nome de personagem já existe");
                }
                /*else if (novoPersonagem.PontosVida > 100)
                {
                    throw new Exception("Pontos de Vida não pode ser maior que 100");
                }*/
                else if (novoPersonagem.PontosVida != 100)
                {
                    throw new Exception("Pontos de Vida deve ser 100");
                }
                else if (novoPersonagem.Classe != ClasseEnum.Cavaleiro &&
                         novoPersonagem.Classe != ClasseEnum.Mago &&
                         novoPersonagem.Classe != ClasseEnum.Clerigo)
                {
                    throw new Exception("O personagem deve possuirr uma Classe");
                }
                else
                {
                    novoPersonagem.Usuario = _context.Usuarios.FirstOrDefault(uBusca => uBusca.Id == ObterUsuarioId());
                    await _context.Personagens.AddAsync(novoPersonagem);
                    await _context.SaveChangesAsync();
                    return Ok(novoPersonagem.Id);
                }
            }
            catch (Exception ex) //Quando utilizamos o using System; não precisamos colocar o System. antes do Exception
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Personagem editPersonagem) //Método PUT ok
        {
            try
            {
                /*List<Personagem> listaPersonagens = await _context.Personagens.ToListAsync();

                if (listaPersonagens.Exists(pBusca => pBusca.Nome == editPersonagem.Nome && pBusca.Id != editPersonagem.Id))
                {
                    throw new Exception("Já existe um personagem com este nome");
                }*/
                if (editPersonagem.PontosVida != 100)
                {
                    throw new Exception("Pontos de Vida deve ser 100");
                }
                if (editPersonagem.Nome.Length < 3)
                {
                    throw new Exception("Nome de personagem deve conter 3 ou mais caracteres");
                }
                if (editPersonagem.Classe != ClasseEnum.Cavaleiro &&
                    editPersonagem.Classe != ClasseEnum.Mago &&
                    editPersonagem.Classe != ClasseEnum.Clerigo)
                {
                    throw new Exception("O personagem deve possuir uma Classe");
                }
                
                editPersonagem.Usuario = _context.Usuarios.FirstOrDefault(uBusca => uBusca.Id == ObterUsuarioId());
                _context.Personagens.Update(editPersonagem);
                int linhasAfetadas = await _context.SaveChangesAsync();
                return Ok(linhasAfetadas);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) //Método DELETE ok
        {
            try
            {
                Personagem pRemover = await _context.Personagens
                    .FirstOrDefaultAsync(p => p.Id == id);

                _context.Personagens.Remove(pRemover);
                int linhasAfetadas = await _context.SaveChangesAsync();

                return Ok(linhasAfetadas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("PersonagemRandom")] //Método adicional
        public async Task<IActionResult> Sorteio()
        {
            List<Personagem> personagens =
                await _context.Personagens.ToListAsync();

            //Sorteio com numero da quantidade de personagens
            int sorteio = new Random().Next(personagens.Count);

            //Busca na lista pelo indice sorteado (não é o ID)
            Personagem p = personagens[sorteio];

            string msg =
                string.Format("No Sorteado {0}. Personagem: {1}", sorteio, p.Nome);

            return Ok(msg);
        }

        [HttpPut("RestaurarPontosVida")]
        public async Task<IActionResult> RestaurarPontosVidaAsync(Personagem p)
        {
            try
            {
                int linhasAfetadas = 0;
                Personagem pEncontrado =
                await _context.Personagens.FirstOrDefaultAsync(pBusca => pBusca.Id == p.Id);
                pEncontrado.PontosVida = 100;

                bool atualizou = await TryUpdateModelAsync<Personagem>(pEncontrado, "p",
                pAtualizar => pAtualizar.PontosVida);

                // If vai detectar e atualizar apenas as colunas que foram alteradas.
                if (atualizou)
                    linhasAfetadas = await _context.SaveChangesAsync();
                return Ok(linhasAfetadas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Método para alteração da foto
        [HttpPut("AtualizarFoto")]
        public async Task<IActionResult> AtualizarFotoAsync(Personagem p)
        {
            try
            {
                Personagem personagem = await _context.Personagens
                    .FirstOrDefaultAsync(x => x.Id == p.Id);
                personagem.FotoPersonagem = p.FotoPersonagem;
                var attach = _context.Attach(personagem);
                attach.Property(x => x.Id).IsModified = false;
                attach.Property(x => x.FotoPersonagem).IsModified = true;
                int linhasAfetadas = await _context.SaveChangesAsync();
                return Ok(linhasAfetadas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ZerarRanking")]
        public async Task<IActionResult> ZerarRankingAsync(Personagem p)
        {
            try
            {
                Personagem pEncontrado =
                await _context.Personagens.FirstOrDefaultAsync(pBusca => pBusca.Id == p.Id);

                pEncontrado.Disputas = 0;
                pEncontrado.Vitorias = 0;
                pEncontrado.Derrotas = 0;
                int linhasAfetadas = 0;

                bool atualizou = await TryUpdateModelAsync<Personagem>(pEncontrado, "p",
                pAtualizar => pAtualizar.Disputas,
                pAtualizar => pAtualizar.Vitorias,
                pAtualizar => pAtualizar.Derrotas);

                // If vai detectar e atualizar apenas as colunas que foram alteradas.
                if (atualizou)
                    linhasAfetadas = await _context.SaveChangesAsync();

                return Ok(linhasAfetadas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ZerarRankingRestaurarVidas")]
        public async Task<IActionResult> ZerarRankingRestaurarVidasAsync()
        {
            try
            {
                List<Personagem> lista =
                await _context.Personagens.ToListAsync();
                int linhasAfetadas = 0;

                foreach (Personagem p in lista)
                {
                    await ZerarRankingAsync(p);
                    await RestaurarPontosVidaAsync(p);
                    linhasAfetadas++;
                }

                await _context.SaveChangesAsync();
                return Ok(linhasAfetadas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetByUser/{userId}")]
        public async Task<IActionResult> GetByUserAsync(int userId)
        {
            try
            {
                List<Personagem> lista = await _context.Personagens
                .Where(u => u.Usuario.Id == userId)
                .ToListAsync();
                return Ok(lista);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetByPerfil/userId")]
        public async Task<IActionResult> GetByPerfilAsync(int userId)
        {
            try
            {
                Usuario usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(x => x.Id == userId);

                List<Personagem> lista = new List<Personagem>();
                if (usuario.Perfil == "Admin")
                    lista = await _context.Personagens.ToListAsync();
                else
                    lista = await _context.Personagens
                        .Where(p => p.Usuario.Id == userId).ToListAsync();

                return Ok(lista);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetByUser")]
        public async Task<IActionResult> GetByUserAsync()
        {
            try
            {
                int id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

                List<Personagem> lista = await _context.Personagens.Where(u => u.Usuario.Id == id).ToListAsync();

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}