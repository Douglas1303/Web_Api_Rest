using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Api_Macorrati.Context;
using Web_Api_Macorrati.DTOs;
using Web_Api_Macorrati.Models;
using Web_Api_Macorrati.Repository;

namespace Web_Api_Macorrati.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiConventionType(typeof(DefaultApiConventions))] // Documenta todos os tipos de retornos no swagger
    [Produces("application/json")]
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper; 

        public CategoriasController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper; 
 
        }

        [AllowAnonymous]
        [HttpGet("teste")]
        public string GetTeste()
        {
            return $"CategoriasController - {DateTime.Now.ToLongDateString().ToString()}"; 
        }

        /// <summary>
        /// Obtém os produtos relacionados para cada categoria
        /// </summary>
        /// <returns>Objetos Categoria e respectivo Objetos Produtos</returns>
        [HttpGet("produtos")] //Retornas as categorias e os produtos 
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasProdutos();

            var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);

            return categoriasDTO; 
        }

        /// <summary>
        /// Retorna uma coleção de objetos Categoria
        /// </summary>
        /// <returns>Lista de Categorias</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            try
            {
                var categorias = await _uof.CategoriaRepository.Get().ToListAsync();

                var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);
                //throw new Exception(); //lançar uma exception para teste
                return categoriasDTO;
            }
            catch (Exception)
            {

                return BadRequest(); 
            }
        }
        [HttpGet("Paginacao")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetPaginacao(int pag=1, int reg=5)
        {
            try
            {
                if (reg > 99)
                    reg = 5; 

                var categorias =  _uof.CategoriaRepository.LocalizaPagina<Categoria>(pag, reg).ToList();

                var totalDeRegistros = _uof.CategoriaRepository.GetTotalRegistros();
                var numeroPaginas = (int)Math.Ceiling((double)totalDeRegistros / reg);

                Response.Headers["X-Total-Registros"] = totalDeRegistros.ToString();
                Response.Headers["X-Numeros-Paginas"] = numeroPaginas.ToString(); 

                var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);
                //throw new Exception(); //lançar uma exception para teste
                return categoriasDTO;
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        /// <summary>
        /// Obter uma categoria pelo id 
        /// </summary>
        /// <param name="id">Codigo sa categoria</param>
        /// <returns>Objetos Categoria</returns>
        [HttpGet("{id}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound();
            }

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return categoriaDTO;
        }

        /// <summary>
        /// Inclui uma nova categoria
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     POST api/categorias
        ///     {
        ///        "categoriaId": 1,
        ///        "nome": "categoria1",
        ///        "imagemUrl": "http://teste.net/1.jpg"
        ///     }
        /// </remarks>
        /// <param name="categoriaDto">objeto Categoria</param>
        /// <returns>O objeto Categoria incluida</returns>
        /// <remarks>Retorna um objeto Categoria incluído</remarks>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoriaDTO categoriaDto)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto); 

            _uof.CategoriaRepository.Add(categoria);
            await _uof.Commit(); 

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria); 

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                return BadRequest();
            }

            var categoria = _mapper.Map<Categoria>(categoriaDto); 

            _uof.CategoriaRepository.Update(categoria); 
            await _uof.Commit();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetById(c => c.CategoriaId == id); 

            if (categoria == null) 
            {
                return NotFound(); 
            }

            _uof.CategoriaRepository.Delete(categoria);
            await _uof.Commit();

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria); 

            return categoriaDTO; 
        }

    }
}
