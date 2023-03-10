using Microsoft.AspNetCore.Mvc;
using RpgApi.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RpgApi.Models;
using RpgApi.Utils;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace RpgApi.Controllers
{
    [Authorize] //Acessa a controller somente após validação do token
    [ApiController]
    [Route("[Controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public UsuariosController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private string CriarToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.UserName),
                new Claim(ClaimTypes.Role, usuario.Perfil)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("ConfiguracaoToken:Chave").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> UsuarioExistente(string userName)
        {
            if (await _context.Usuarios.AnyAsync(x => x.UserName.ToLower() == userName.ToLower()))
            {
                return true;
            }
            return false;
        }

        [AllowAnonymous] //Permite acessar o método seguinte sem validação do token
        [HttpPost("Registrar")]
        public async Task<IActionResult> RegistrarUsuario(Usuario user)
        {
            try
            {
                if (await UsuarioExistente(user.UserName))
                {
                    throw new System.Exception("Nome de usuário já existe");  //Se colocar o using System; não precisa colocar o System na linha do código.
                }

                Criptografia.CriarPasswordHash(user.PasswordString, out byte[] hash, out byte[] salt);
                user.PasswordString = string.Empty;
                user.PasswordHash = hash;
                user.PasswordSalt = salt;
                await _context.Usuarios.AddAsync(user);
                await _context.SaveChangesAsync();
                return Ok(user.Id);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public DateTime PegaHoraBrasilia() => TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

        [AllowAnonymous]
        [HttpPost("Autenticar")]
        public async Task<IActionResult> AutenticarUsuario(Usuario credenciais)
        {
            try
            {
                Usuario usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(x => x.UserName.ToLower().Equals(credenciais.UserName.ToLower()));

                if (usuario is null)
                {
                    throw new System.Exception("Usuário não encontrado");
                }
                else if (!Criptografia.VerificarPasswordHash(credenciais.PasswordString, usuario.PasswordHash, usuario.PasswordSalt))
                {
                    throw new System.Exception("Senha incorreta");
                }
                else
                {
                    //usuario.DataAcesso = DateTime.Now;
                    usuario.DataAcesso = PegaHoraBrasilia();
                    _context.Usuarios.Update(usuario);
                    await _context.SaveChangesAsync(); //Confirma a alteração no banco

                    usuario.PasswordHash = null; //Null para que a senha não transite pelo JSON
                    usuario.PasswordSalt = null; //Null para que a senha não transite pelo JSON
                    usuario.Token = CriarToken(usuario); 
                    return Ok(usuario); //Retorna o obejto usuario inteiro, e não somente o Id como anteriormente
                    //return Ok(usuario.Id);
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //DESAFIOS:

        [HttpPut("AlterarSenha")] //DESAFIO 1
        public async Task<IActionResult> AlterarSenha(Usuario credenciais)
        {
            try
            {
                Usuario usuario = await _context.Usuarios //Busca o usuário no banco através do login
                    .FirstOrDefaultAsync(x => x.UserName.ToLower().Equals(credenciais.UserName.ToLower()));

                if (usuario == null) //Se não achar nenhum usuário pelo login, retorna a mensagem
                {
                    throw new System.Exception("Usuário não encontrado");
                }
                else
                {
                    Criptografia.CriarPasswordHash(credenciais.PasswordString, out byte[] hash, out byte[] salt);
                    usuario.PasswordHash = hash; //Se o usuário existir, executa a criptografia
                    usuario.PasswordSalt = salt; //guardando o hash e o salt nas propriedades do usuário

                    _context.Usuarios.Update(usuario);
                    int linhasAfetadas = await _context.SaveChangesAsync(); //Confirma a alteração no banco
                    return Ok(linhasAfetadas); //Retorna as linhas afetadas (geralmente sempre 1 linha mesmo)
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAll")] //DESAFIO 2 - OK
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Usuario> lista = await _context.Usuarios.ToListAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> GetUsuario(int usuarioId)
        {
            try
            {
                //List exigirá o using System.Collections.Generic
                Usuario usuario = await _context.Usuarios //Busca o usuário no banco através do Id
                    .FirstOrDefaultAsync(x => x.Id == usuarioId);
                return Ok(usuario);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetByLogin/{login}")]
        public async Task<IActionResult> GetUsuario(string login)
        {
            try
            {
                //List exigirá o using System.Collections.Generic
                Usuario usuario = await _context.Usuarios //Busca o usuário no banco através do login
                    .FirstOrDefaultAsync(x => x.UserName.ToLower() == login.ToLower());

                return Ok(usuario);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Método para alteração da geolocalização
        [HttpPut("AtualizarLocalizacao")]
        public async Task<IActionResult> AtualizarLocalizacao(Usuario u)
        {
            try
            {
                Usuario usuario = await _context.Usuarios //Busca o usuário no banco através do Id
                    .FirstOrDefaultAsync(x => x.Id == u.Id);

                usuario.Latitude = u.Latitude;
                usuario.Longitude = u.Longitude;

                var attach = _context.Attach(usuario);
                attach.Property(x => x.Id).IsModified = false;
                attach.Property(x => x.Latitude).IsModified = true;
                attach.Property(x => x.Longitude).IsModified = true;

                int linhasAfetadas = await _context.SaveChangesAsync(); //Confirma a alteração no banco
                return Ok(linhasAfetadas); //Retorna as linhas afetadas (Geralmente sempre 1 linha msm)
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Método para alteração do e-mail
        [HttpPut("AtualizarEmail")]
        public async Task<IActionResult> AtualizarEmail(Usuario u)
        {
            try
            {
                Usuario usuario = await _context.Usuarios //Busca o usuário no banco através do Id
                    .FirstOrDefaultAsync(x => x.Id == u.Id);

                usuario.Email = u.Email;

                var attach = _context.Attach(usuario);
                attach.Property(x => x.Id).IsModified = false;
                attach.Property(x => x.Email).IsModified = true;

                int linhasAfetadas = await _context.SaveChangesAsync(); //Confirma a alteração no banco
                return Ok(linhasAfetadas); //Retorna as linhas afetadas (Geralmente sempre 1 linha msm)
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Método para alteração da foto
        [HttpPut("AtualizarFoto")]
        public async Task<IActionResult> AtualizarFoto(Usuario u)
        {
            try
            {
                Usuario usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(x => x.Id == u.Id);

                usuario.Foto = u.Foto;

                var attach = _context.Attach(usuario);
                attach.Property(x => x.Id).IsModified = false;
                attach.Property(x => x.Foto).IsModified = true;
                
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