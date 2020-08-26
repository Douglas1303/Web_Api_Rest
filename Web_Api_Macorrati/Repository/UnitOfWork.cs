using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Api_Macorrati.Context;

namespace Web_Api_Macorrati.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ProdutoRepository _produtoRepository;
        private CategoriaRepository _categoriaRepository;
        public AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProdutoRepository ProdutoRepository 
        {   
            //Se a instância for nula passo uma nova instância 
            get
            {
                return _produtoRepository = _produtoRepository ?? new ProdutoRepository(_context); 
            }
        }

        public ICategoriaRepository CategoriaRepository 
        {
            get
            {
                return _categoriaRepository = _categoriaRepository ?? new CategoriaRepository(_context); 
            }
        }


        public async Task Commit()
        {
            await _context.SaveChangesAsync(); 
        }

        public void Dispose()
        {
            _context.Dispose(); 
        }
    }
}
