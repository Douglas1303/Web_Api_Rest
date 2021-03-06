﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Web_Api_Macorrati.Context;

namespace Web_Api_Macorrati.Repository
{
    //Classe concreta que implementa os metodos
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context; 
        }

        public IQueryable<T> Get()
        {
            return _context.Set<T>().AsNoTracking(); 
        }

        public async Task<T> GetById(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(predicate);
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity); 
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity); 
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified; 
            _context.Set<T>().Update(entity); 
        }

        public List<T> LocalizaPagina<Tipo>(int numeroPagina, int quantidadeRegistros) where Tipo : class
        {
            return _context.Set<T>()
                .Skip(quantidadeRegistros * (numeroPagina - 1))
                .Take(quantidadeRegistros).ToList(); 
        }

        public int GetTotalRegistros()
        {
            return _context.Set<T>().AsNoTracking().Count(); 
        }
    }
}
