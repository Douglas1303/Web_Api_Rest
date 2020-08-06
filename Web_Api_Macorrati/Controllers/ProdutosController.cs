using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Web_Api_Macorrati.Context;
using Web_Api_Macorrati.Filters;
using Web_Api_Macorrati.Models;
using Web_Api_Macorrati.Repository;

namespace Web_Api_Macorrati.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        public ProdutosController(IUnitOfWork context)
        {
            _uof = context;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPrecos()
        {
            return _uof.ProdutoRepository.GetProdutosPorPreco().ToList(); 
        }

        [HttpGet]
        //[ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            return _uof.ProdutoRepository.Get().ToList();
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto =  _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound(); //404
            }

            return produto; 
        }

        [HttpPost]
        public ActionResult Post([FromBody]Produto produto)
        {
            _uof.ProdutoRepository.Add(produto);
            _uof.Commit(); 

            return new CreatedAtRouteResult("ObterProduto", new {id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Produto produto)
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest(); 
            }

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit(); 

            return Ok(); 
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id); 

            if (produto == null)
            {
                return NotFound(); 
            }

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit(); 

            return produto; 
        }
    }
}
