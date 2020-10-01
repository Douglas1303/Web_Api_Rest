﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Api_Macorrati.Models;
using Web_Api_Macorrati.Pagination;

namespace Web_Api_Macorrati.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters); 
        Task<IEnumerable<Produto>> GetProdutosPorPreco(); 
    }
}
