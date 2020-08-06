using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Api_Macorrati.Context;
using Web_Api_Macorrati.Models;

namespace Web_Api_Macorrati.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext contexto) : base(contexto) 
        {

        }

        public IEnumerable<Produto> GetProdutosPorPreco()
        {
            return Get().OrderBy(p => p.Preco).ToList(); 
        }
    }
}
