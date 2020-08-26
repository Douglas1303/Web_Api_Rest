using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Web_Api_Macorrati.Repository
{
    //Interface - Define a assinatura dos metodos
    public interface IRepository<T> //<T> define que é um repositorio generico
    {
        IQueryable<T> Get();
        Task<T> GetById(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity); 
    }
}
