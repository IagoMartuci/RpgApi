using Microsoft.AspNetCore.Mvc;
using RpgApi.Models;
using RpgApi.Models.Enuns;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("[Controller]")] //Rota que vamos testar a API
    public class PersonagemExemploController : ControllerBase
    {
        private static List<Personagem> personagens = new List<Personagem>()
        {
            new Personagem() { Id = 1, Nome = "Frodo", PontosVida=100, Forca=17, Defesa=23, Inteligencia=33, Classe=ClasseEnum.Cavaleiro},
            new Personagem() { Id = 2, Nome = "Sam", PontosVida=100, Forca=15, Defesa=25, Inteligencia=30, Classe=ClasseEnum.Cavaleiro},
            new Personagem() { Id = 3, Nome = "Galadriel", PontosVida=100, Forca=18, Defesa=21, Inteligencia=35, Classe=ClasseEnum.Clerigo },
            new Personagem() { Id = 4, Nome = "Gandalf", PontosVida=100, Forca=18, Defesa=18, Inteligencia=37, Classe=ClasseEnum.Mago },
            new Personagem() { Id = 5, Nome = "Hobbit", PontosVida=100, Forca=20, Defesa=17, Inteligencia=31, Classe=ClasseEnum.Cavaleiro },
            new Personagem() { Id = 6, Nome = "Celeborn", PontosVida=100, Forca=21, Defesa=13, Inteligencia=34, Classe=ClasseEnum.Clerigo },
            new Personagem() { Id = 7, Nome = "Radagast", PontosVida=100, Forca=25, Defesa=11, Inteligencia=35, Classe=ClasseEnum.Mago }
        };

        [HttpGet("GetAll")]
        public IActionResult Get()
        {
            return Ok(personagens);
        }

        [HttpGet("GetByClasse/{idClasse}")]
        public IActionResult GetByClasse(int idClasse)
        {
            if (idClasse == 1)
            {
                return Ok(personagens.FindAll(p => p.Classe == ClasseEnum.Cavaleiro));
            }
            if (idClasse == 2)
            {
                return Ok(personagens.FindAll(p => p.Classe == ClasseEnum.Mago));
            }
            if (idClasse == 3)
            {
                return Ok(personagens.FindAll(p => p.Classe == ClasseEnum.Clerigo));
            }
            return NotFound("NotFound: A classe pesquisada não existe na base de dados");
        }

        [HttpGet("GetByNome/{nomeDig}")]
        public IActionResult GetByNome(string nomeDig)
        {
            List<Personagem> listaBusca = personagens.FindAll(p => p.Nome.ToLower() == nomeDig.ToLower());
            if (listaBusca.Any())
            {
                return Ok(listaBusca);
            }
            else
            {
                return NotFound("NotFound: O nome pesquisado não existe na base de dados");
            }
        }

        [HttpPost("PostValidacao")]
        public IActionResult PostValidacao(Personagem novoPersonagem)
        {
            if (novoPersonagem.Defesa < 10 && novoPersonagem.Inteligencia > 30)
            {
                return BadRequest("Valor de defesa não pode ser menor do que 10;\nValor de iteligencia não pode ser maior do que 30.");
            }
            if (novoPersonagem.Defesa < 10)
            {
                return BadRequest("Valor de defesa não pode ser menor do que 10.");
            }
            if (novoPersonagem.Inteligencia > 30)
            {
                return BadRequest("Valor de iteligencia não pode ser maior do que 30.");
            }
            personagens.Add(novoPersonagem);
            return Ok(personagens);
        }

        [HttpPost("PostValidacaoMago")]
        public IActionResult PostValidacaoMago(Personagem novoPersonagem)
        {
            if (novoPersonagem.Classe == ClasseEnum.Mago)
            {
                if (novoPersonagem.Inteligencia < 35)
                {
                    return BadRequest("Valor da inteligencia para MAGOS não pode ser menor do que 35");
                }
            }
            personagens.Add(novoPersonagem);
            return Ok(personagens);
        }

        [HttpGet("GetClerigoMago")]
        public IActionResult GetClerigoMago()
        {
            int idClasse = 1;
            //Conversão Explicita ou CAST
            ClasseEnum ClasseConvertidaEnum = (ClasseEnum)idClasse;

            if (idClasse == 1)
            {
                personagens.RemoveAll(p => p.Classe == ClasseEnum.Cavaleiro);
            }

            List<Personagem> listaFinal = personagens.FindAll(p => p.Classe != ClasseEnum.Cavaleiro).OrderByDescending(p => p.PontosVida).ToList();
            return Ok(listaFinal);
        }

        [HttpGet("GetEstatisticas")]
        public IActionResult GetEstatisticas()
        {
            return Ok($"Quantidade de personagens: {personagens.Count}\nTotal de inteligencia: {personagens.Sum(p => p.Inteligencia)}");
        }

        [HttpGet("{idPersonagem}")]
        public IActionResult GetById(int idPersonagem)
        {
            return Ok(personagens.FirstOrDefault(p => p.Id == idPersonagem));
        }

        [HttpGet("GetOrdenado")]
        public IActionResult GetOrdem()
        {
            List<Personagem> listaFinal = personagens.OrderBy(p => p.Forca).ToList();
            return Ok(listaFinal);
        }

        [HttpGet("GetContagem")]
        public IActionResult GetQuantidade()
        {
            return Ok("Quantidade de personagens: " + personagens.Count);
        }

        [HttpGet("GetSomaForca")]
        public IActionResult GetSomaForca()
        {
            return Ok("Força somada de todos os personagens: " + personagens.Sum(p => p.Forca));
        }

        [HttpGet("GetSemCavaleiro")]
        public IActionResult GetSemCavaleiro()
        {
            List<Personagem> listaBusca = personagens.FindAll(p => p.Classe != ClasseEnum.Cavaleiro);
            return Ok(listaBusca);
        }

        [HttpGet("GetbyNomeAproximado/{nomeAprox}")]
        public IActionResult GetByNomeAproximado(string nomeAprox)
        {
            List<Personagem> listaBusca = personagens.FindAll(p => p.Nome.ToLower().Contains(nomeAprox));
            return Ok(listaBusca);
        }

        [HttpGet("GetRemovendoMago")]
        public IActionResult GetRemovendoMagos()
        {
            /*Personagem pRemove = personagens.Find(p => p.Classe == ClasseEnum.Mago);
            personagens.Remove(pRemove);
            return Ok("Personagem removido: " + pRemove.Nome);*/

            //TENTANDO REMOVER TODOS OS MAGOS DO GET DE 1X SÓ:
            int idClasse = 3;
            ClasseEnum ClasseConvertidaEnum = (ClasseEnum)idClasse;
            if (idClasse == 3)
            {
                //List<Personagem> listaRemove = personagens.FindAll(p => p.Classe == ClasseEnum.Mago).ToList();
                personagens.RemoveAll(p => p.Classe == ClasseEnum.Mago);
                //return Ok(listaRemove);
            }
            List<Personagem> listaFinal = personagens.FindAll(p => p.Classe != ClasseEnum.Mago).ToList();
            return Ok(listaFinal);
        }

        [HttpGet("GetByForca/{forcaPersonagem}")]
        public IActionResult GetByForca(int forcaPersonagem)
        {
            List<Personagem> listaFinal = personagens.FindAll(p => p.Forca == forcaPersonagem);
            return Ok(listaFinal);
        }

        [HttpPost]
        public IActionResult AddPersonagem(Personagem novoPersonagem)
        {
            if (novoPersonagem.Inteligencia == 0)
            {
                return BadRequest("Inteligencia não pode ter o valor igual a 0 (zero)!");
            }
            else
            {
                personagens.Add(novoPersonagem);
                return Ok(personagens);
            }
        }

        [HttpPut]
        public IActionResult UpdatePersonagem(Personagem p)
        {
            Personagem personagemAlterado = personagens.Find(pers => pers.Id == p.Id);
            personagemAlterado.Nome = p.Nome;
            personagemAlterado.PontosVida = p.PontosVida;
            personagemAlterado.Forca = p.Forca;
            personagemAlterado.Defesa = p.Defesa;
            personagemAlterado.Inteligencia = p.Inteligencia;
            personagemAlterado.Classe = p.Classe;
            return Ok(personagens);
        }

        [HttpDelete("{idPersonagem}")]

        public IActionResult Delete(int idPersonagem)
        {
            personagens.RemoveAll(pers => pers.Id == idPersonagem);
            return Ok(personagens);
        }
    }
}