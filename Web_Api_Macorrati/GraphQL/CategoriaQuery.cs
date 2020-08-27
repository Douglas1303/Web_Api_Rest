using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web_Api_Macorrati.Repository;

namespace Web_Api_Macorrati.GraphQL
{
    //mapeamos os campos para uma consulta
    //para uma chamada no repositorio(CategoriaRepository)
    public class CategoriaQuery : ObjectGraphType
    {
        public CategoriaQuery(IUnitOfWork _context)
        {
            //Esse metodo vai retornar um objeto categoria
            Field<CategoriaType>("categoria",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType>() { Name = "id" }),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    return _context.CategoriaRepository.GetById(c => c.CategoriaId == id);
                });

            //retorna uma lista de categorias
            Field<ListGraphType<CategoriaType>>("categorias",
                resolve: context => {
                    return _context.CategoriaRepository.Get(); 
                });
        }
    }
}
