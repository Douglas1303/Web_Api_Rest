using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Web_Api_Macorrati.Context;
using Web_Api_Macorrati.Controllers;
using Web_Api_Macorrati.DTOs;
using Web_Api_Macorrati.DTOs.Mappings;
using Web_Api_Macorrati.Repository;
using Xunit;

namespace ApiCatalogoXUnitTeste
{
    public class ProdutosUnitTestController
    {

        private IMapper mapper;
        private IUnitOfWork repository;

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString = "Server=Localhost;Database=ApiMacorrati;Uid=root;Pwd=244005";

        static ProdutosUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql(connectionString)
                .Options;
        }

        public ProdutosUnitTestController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            mapper = config.CreateMapper();

            var context = new AppDbContext(dbContextOptions);

            repository = new UnitOfWork(context);
        }


        //Testes unitários 

        //Get - Lista de produtos
        [Fact]
        public void GetProdutos_Return_OKResult()
        {
            //Arrange 
            var controller = new ProdutosController(repository, mapper);

            //Act
            var data = controller.Get();

            //Assert
            Assert.IsType<List<ProdutoDTO>>(data.Value); 
        }

        //Get - BadRequest
        [Fact]
        public void GetProdutos_Return_BadRequest()
        {
            //Arrange
            var controller = new ProdutosController(repository, mapper);

            //Act
            var data = controller.Get();

            //Assert
            Assert.IsType<BadRequestResult>(data.Result); 
        }

        //GET que retorna uma lista de objetos produtos
        /*
        [Fact]
        public void GetProdutos_MatchResult()
        {
            //Arrange
            var controller = new ProdutosController(repository, mapper);

            //Act 
            var data = controller.Get();

            //Assert 
            Assert.IsType<List<ProdutoDTO>>(data.Value);
            var cat = data.Value.Should().BeAssignableTo<List<ProdutoDTO>>().Subject;

            //dados de categoria
            Assert.Equal("Bebidas", cat[0].Nome);
            Assert.Equal("http://www.teste.com/Imagens/1.jpg", cat[0].ImagemUrl);

            Assert.Equal("Sobremesa", cat[2].Nome);
            Assert.Equal("http://www.teste.com/Imagens/1.jpg", cat[2].ImagemUrl);
        }
        */
        //Get por id - Retornar um objeto ProdutoDTO
        [Fact]
        public void GetProdutoById_Return_OkResult()
        {
            //Arrange
            var controller = new ProdutosController(repository, mapper);

            var catId = 2;


            //Act 
            var data = controller.Get(catId);

            //Assert
            Assert.IsType<ProdutoDTO>(data.Value);
        }

        //Get por id -- NotFound
        [Fact]
        public void GetProdutoById_ReturnNotFound()
        {
            //arrange 
            var controller = new ProdutosController(repository, mapper);
            var catId = 999; //Passo um id que não existe no banco

            //Act
            var data = controller.Get(catId);

            //Assert
            Assert.IsType<NotFoundResult>(data.Result);
        }

        //Post - CreatedResult
        [Fact]
        public void Post_produto_AddValidData_Return_CreatedResult()
        {
            //Arrange
            var controller = new ProdutosController(repository, mapper);

            var cat = new ProdutoDTO() 
            { 
              Nome = "macarrão",
              Descricao = "macarrão para teste", 
              Preco = 5, 
              ImagemUrl = "testandoProduto.jpg", 
              CategoriaId = 13           
            };

            //Act
            var data = controller.Post(cat);

            //Assert
            Assert.IsType<CreatedAtRouteResult>(data);
        }
        //Put - alterar um objeto produto 
        [Fact]
        public void Put_Produto_Update_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new ProdutosController(repository, mapper);
            var catId = 9;

            //Act  
            var existingPost = controller.Get(catId);

            var result = existingPost.Value.Should().BeAssignableTo<ProdutoDTO>().Subject;

            var catDto = new ProdutoDTO();
            catDto.ProdutoId = catId;
            catDto.Nome =  "macarrão alterado";
            catDto.Descricao = "macarrão esta alterado";
            catDto.Preco = 4; 
            catDto.ImagemUrl = result.ImagemUrl;

            var updatedData = controller.Put(catId, catDto);

            //Assert  
            Assert.IsType<OkResult>(updatedData);
        }

        //=======================================Delete ===================================
        [Fact]
        public void Delete_Produto_Return_OkResult()
        {
            //Arrange  
            var controller = new ProdutosController(repository, mapper);
            var catId = 9;

            //Act  
            var data = controller.Delete(catId);

            //Assert  
            Assert.IsType<ProdutoDTO>(data.Value);
        }
    }
}
