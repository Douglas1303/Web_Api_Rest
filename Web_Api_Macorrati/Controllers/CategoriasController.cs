using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Api_Macorrati.Context;
using Web_Api_Macorrati.Models;
using Web_Api_Macorrati.Repository;

namespace Web_Api_Macorrati.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof; 

        public CategoriasController(IUnitOfWork context)
        {
            _uof = context;
 
        }

        [HttpGet("produtos")] //Retornas as categorias e os produtos 
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return _uof.CategoriaRepository.GetCategoriasProdutos().ToList(); 
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return _uof.CategoriaRepository.Get().ToList();
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {
            _uof.CategoriaRepository.Add(categoria);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            _uof.CategoriaRepository.Update(categoria); 
            _uof.Commit();

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id); 

            if (categoria == null) 
            {
                return NotFound(); 
            }

            _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit(); 

            return categoria; 
        }

    }
}
