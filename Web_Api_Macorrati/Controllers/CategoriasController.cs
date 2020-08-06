using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Api_Macorrati.Context;
using Web_Api_Macorrati.Models;

namespace Web_Api_Macorrati.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _contexto;
        private readonly ILogger _logger; 

        public CategoriasController(AppDbContext contexto, 
                                    ILogger<CategoriasController> logger)
        {
            _contexto = contexto;
            _logger = logger;
        }

        [HttpGet("produtos")] //Retornas as categorias e os produtos 
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return _contexto.Categorias.Include(x => x.Produtos).ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return _contexto.Categorias.AsNoTracking().ToList();
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _contexto.Categorias.AsNoTracking().FirstOrDefault(c => c.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {
            _contexto.Add(categoria);
            _contexto.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            _contexto.Entry(categoria).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _contexto.Categorias.FirstOrDefault(c => c.CategoriaId == id);

            if (categoria == null) 
            {
                return NotFound(); 
            }

            _contexto.Remove(categoria);
            _contexto.SaveChanges(); 

            return categoria; 
        }

    }
}
