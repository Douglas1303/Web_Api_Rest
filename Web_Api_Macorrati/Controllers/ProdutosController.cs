using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Api_Macorrati.Context;
using Web_Api_Macorrati.DTOs;
using Web_Api_Macorrati.Filters;
using Web_Api_Macorrati.Models;
using Web_Api_Macorrati.Repository;

namespace Web_Api_Macorrati.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
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

        /// <summary>
        /// Obtém os produtos ordenados por preço na ordem ascendente
        /// </summary>
        /// <param name="none"></param>
        /// <returns>Lista de objetos Produtos ordenados por preço</returns>
        [HttpGet("menorpreco")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPrecos()
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosPorPreco();
            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDTO; 
        }

        /// <summary>
        /// Exibe uma relação dos produtos
        /// </summary>
        /// <returns>Retorna uma lista de objetos Produto</returns>
        // api/produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
        {
            try
            {
                var produtos = await _uof.ProdutoRepository.Get().ToListAsync();

                var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
                //throw new Exception();  //lançar uma exception para teste
                return produtosDTO;
            }
            catch (Exception)
            {
                return BadRequest(); 
            }
        }

        /// <summary>
        /// Obtem o produto pelo seu identificado produtoId
        /// </summary>
        /// <param name="id">Código do produto</param>
        /// <returns>Um objeto Produto</returns>
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
            var produto =  await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound(); //404
            }

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto); 

            return produtoDTO; 
        }

        /// <summary>
        /// Incluir um novo produto
        /// </summary>
        /// <param name="produtoDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]ProdutoDTO produtoDto)
        {
            var produto = _mapper.Map<Produto>(produtoDto); 

            _uof.ProdutoRepository.Add(produto);
            await _uof.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto); 

            return new CreatedAtRouteResult("ObterProduto", new {id = produto.ProdutoId }, produtoDTO);
        }

        //  api/produtos/1
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProdutoDTO produtoDto)
        {
            if(id != produtoDto.ProdutoId)
            {
                return BadRequest(); 
            }

            var produto = _mapper.Map<Produto>(produtoDto); //Mapeio produtoDto para Produto

            _uof.ProdutoRepository.Update(produto);
            await _uof.Commit(); 

            return Ok(); 
        }

        //  api/produtos/1
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {
            var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id); 

            if (produto == null)
            {
                return NotFound(); 
            }

            _uof.ProdutoRepository.Delete(produto);
            await _uof.Commit();

            //retorna:
            var produtoDTO = _mapper.Map<ProdutoDTO>(produto); 

            return produtoDTO; 
        }
    }
}
