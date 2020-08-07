using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Web_Api_Macorrati.Context;
using Web_Api_Macorrati.DTOs;
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
        private readonly IMapper _mapper; 

        public ProdutosController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper; 
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPrecos()
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDTO; 
        }

        [HttpGet]
        //[ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {  
            var produtos = _uof.ProdutoRepository.Get().ToList();

            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
            
            return produtosDTO; 
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            var produto =  _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound(); //404
            }

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto); 

            return produtoDTO; 
        }

        [HttpPost]
        public ActionResult Post([FromBody]ProdutoDTO produtoDto)
        {
            var produto = _mapper.Map<Produto>(produtoDto); 

            _uof.ProdutoRepository.Add(produto);
            _uof.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto); 

            return new CreatedAtRouteResult("ObterProduto", new {id = produto.ProdutoId }, produtoDTO);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ProdutoDTO produtoDto)
        {
            if(id != produtoDto.ProdutoId)
            {
                return BadRequest(); 
            }

            var produto = _mapper.Map<Produto>(produtoDto); //Mapeio produtoDto para Produto

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit(); 

            return Ok(); 
        }

        [HttpDelete("{id}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id); 

            if (produto == null)
            {
                return NotFound(); 
            }

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();

            //retorna:
            var produtoDTO = _mapper.Map<ProdutoDTO>(produto); 

            return produtoDTO; 
        }
    }
}
