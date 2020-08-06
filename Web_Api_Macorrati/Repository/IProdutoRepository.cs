using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Api_Macorrati.Models;

namespace Web_Api_Macorrati.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosPorPreco(); 
    }
}
