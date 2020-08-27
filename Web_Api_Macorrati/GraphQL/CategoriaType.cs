using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Api_Macorrati.Models;

namespace Web_Api_Macorrati.GraphQL
{
    //Definindo qual entidade será mapeada para o type
    public class CategoriaType : ObjectGraphType<Categoria>
    {
        public CategoriaType()
        {
            //campos do type
            Field(x => x.CategoriaId);
            Field(x => x.Nome);
            Field(x => x.ImagemUrl);

            Field<ListGraphType<CategoriaType>>("categorias"); 
        }
    }
}
