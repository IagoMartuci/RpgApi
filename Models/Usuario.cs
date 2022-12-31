using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace RpgApi.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; } // [] byte = Array de byte
        public byte[] PasswordSalt { get; set; }
        public byte[] Foto { get; set; }
        public double? Latitude { get; set; } // ? significa que a variável pode receber valores null
        public double? Longitude { get; set; }
        public DateTime? DataAcesso { get; set; }

        // O que for armazenado na variável da propriedade logo abaixo de uma configuração com o uso de [], não vai para o banco de dados
        [NotMapped]
        public string PasswordString { get; set; }
        public List<Personagem> Personagens { get; set; } // Lista de Personagens definirá que um Usuário poderá possuir N personagens
        public string Perfil { get; set; }
        public string Email { get; set; }
    }
}