using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Web_Api_Macorrati.Repository;


namespace Web_Api_Macorrati.GraphQL
{
    //É incluido no pipeline do request para processar
    //a requisição http usando a instância do nosso repositorio
    public class TestGraphQLMiddleware
    {
        //instancia para processar o request http
        private readonly RequestDelegate _next;

        //instancia do UnitOfWork
        private readonly IUnitOfWork _context;

        public TestGraphQLMiddleware(RequestDelegate next, IUnitOfWork context)
        {
            _next = next;
            _context = context;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.StartsWithSegments("/graphql"))
            {
                //Tenta ler o corpo do request usando um StreamReader
                using (var stream = new StreamReader(httpContext.Request.Body))
                {
                    var query = await stream.ReadToEndAsync();

                    if (!String.IsNullOrWhiteSpace(query))
                    {
                        var schema = new Schema
                        {
                            Query = new CategoriaQuery(_context)
                        };

                        var result = await new DocumentExecuter().ExecuteAsync(options =>
                        {
                            options.Schema = schema;
                            options.Query = query;
                        });
                        await WriteResult(httpContext, result);
                    }
                }
            }
            else
            {
                await _next(httpContext); 
            }
        }
        private async Task WriteResult(HttpContext httpContext, ExecutionResult result)
        {
            //Metodo não implementado 
            //var json = new DocumentWriter(indent: true).Write(result); 
            //httpContext.Response.StatusCode = 200;
            //httpContext.Response.ContentType = "application/json";
            //await httpContext.Response.WriteAsync(json); 
        }
    }
}
