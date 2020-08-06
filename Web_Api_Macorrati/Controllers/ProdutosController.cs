using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Api_Macorrati.Context;
using Web_Api_Macorrati.Filters;
using Web_Api_Macorrati.Models;

namespace Web_Api_Macorrati.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _contexto;
        public ProdutosController(AppDbContext contexto)
        {
            _contexto = contexto;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            return await _contexto.Produtos.AsNoTracking().ToListAsync();
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> Get(int id)
        {
            var produto = await _contexto.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound(); //404
            }

            return produto; 
        }

        [HttpPost]
        public ActionResult Post([FromBody]Produto produto)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState); 
            //}

            _contexto.Produtos.Add(produto);
            _contexto.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto", new {id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Produto produto)
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest(); 
            }

            _contexto.Entry(produto).State = EntityState.Modified;

            _contexto.SaveChanges();

            return Ok(); 
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            var produto = _contexto.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound(); 
            }

            _contexto.Remove(produto);
            _contexto.SaveChanges();

            return produto; 
        }
    }
}
