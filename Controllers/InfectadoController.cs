using System;
using System.Collections.Generic;
using API.Data.Collections;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MongoDB.Driver;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectadoController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Infectado> _infectadosCollection;

        public InfectadoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.Cpf, dto.Nome, dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            _infectadosCollection.InsertOne(infectado);
            
            return StatusCode(201, "Infectado adicionado com sucesso");
        }

        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();
            
            var infectadosDto = new List<InfectadoDto>();

            foreach(var infectado in infectados){
                var dto = new InfectadoDto{
                    Cpf = infectado.Cpf,
                    Nome = infectado.Nome,
                    DataNascimento = infectado.DataNascimento,
                    Sexo = infectado.Sexo,
                    Latitude = infectado.Localizacao.Latitude,
                    Longitude = infectado.Localizacao.Longitude
                };
                infectadosDto.Add(dto);
            } 

            return Ok(infectadosDto);
        }

        [HttpGet("{Cpf}")]
        public ActionResult ObterInfectado([FromRoute] string Cpf)
        {
            var filter = Builders<Infectado>.Filter.Eq(x => x.Cpf, Cpf);            
            var infectados = _infectadosCollection.Find(filter).ToList();

            if(infectados == null || infectados.Count < 1)
                return NoContent();      

            var infectadosDto = new List<InfectadoDto>();

            foreach(var infectado in infectados){
                var dto = new InfectadoDto{
                    Cpf = infectado.Cpf,
                    Nome = infectado.Nome,
                    DataNascimento = infectado.DataNascimento,
                    Sexo = infectado.Sexo,
                    Latitude = infectado.Localizacao.Latitude,
                    Longitude = infectado.Localizacao.Longitude
                };
                infectadosDto.Add(dto);
            }              

            return Ok(infectadosDto);
        }

        [HttpPut("{Cpf}")]
        public ActionResult AtualizarInfectado([FromRoute] string Cpf, [FromBody]InfectadoDto dto)
        {

            var filter = Builders<Infectado>.Filter.Eq(x => x.Cpf, Cpf);

            if(filter == null)
                return NoContent();

            var infectado = new Infectado(dto.Cpf, dto.Nome, dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);
         
            _infectadosCollection.ReplaceOne(filter, infectado);
            return Ok(infectado);
        }

        [HttpDelete("{Cpf}")]
        public ActionResult DeletarInfectado([FromRoute] string Cpf)
        {

            var filter = Builders<Infectado>.Filter.Eq(x => x.Cpf, Cpf);

            if(filter == null)
                return NoContent();

            _infectadosCollection.DeleteOne(filter);
            return Ok();
        }

    }
}