using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class InfectadoDto
    {

        [Required]
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Sexo { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}