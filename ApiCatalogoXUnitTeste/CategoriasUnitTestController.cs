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
    public class CategoriasUnitTestController
    {
        private IMapper mapper;
        private IUnitOfWork repository; 

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString = "Server=Localhost;Database=ApiMacorrati;Uid=root;Pwd=244005";

        static CategoriasUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql(connectionString)
                .Options; 
        }

        public CategoriasUnitTestController()
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

        //Testar método GET
        [Fact]
        public void GetCategorias_Return_OkResult()
        {
            //arrange (Preparação)
            var controller = new CategoriasController(repository, mapper);

            //act 
            var data = controller.Get();

            //assert - verifica se o tipo retornado é uma lista
            Assert.IsType<List<CategoriaDTO>>(data.Value); 
        }

        //Get - BadRequest
        [Fact]
        public void GetCategrias_Return_BadRequestResult()
        {
            //Arrange 
            var controller = new CategoriasController(repository, mapper);

            //Act
            var data = controller.Get();

            //Assert 
            Assert.IsType<BadRequestResult>(data.Result); 
        }

        //GET que retorna uma lista de objetos categoria
        [Fact]
        public void GetCategorias_MatchResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);

            //Act 
            var data = controller.Get();

            //Assert 
            Assert.IsType<List<CategoriaDTO>>(data.Value);
            var cat = data.Value.Should().BeAssignableTo<List<CategoriaDTO>>().Subject;

            Assert.Equal("Bebidas", cat[0].Nome);
            Assert.Equal("http://www.teste.com/Imagens/1.jpg", cat[0].ImagemUrl);

            Assert.Equal("Sobremesa", cat[2].Nome);
            Assert.Equal("http://www.teste.com/Imagens/1.jpg", cat[2].ImagemUrl);
        }

        //Get por id - Retornar um objeto CategoriaDTO
        [Fact]
        public void GetCategoriaById_Return_OkResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);

            var catId = 13;


            //Act 
            var data = controller.Get(catId);

            //Assert
            Assert.IsType<CategoriaDTO>(data.Value); 
        }

        //Get por id -- NotFound
        [Fact]
        public void GetCategoriaById_ReturnNotFound()
        {
            //arrange 
            var controller = new CategoriasController(repository, mapper);
            var catId = 999; //Passo um id que não existe no banco

            //Act
            var data = controller.Get(catId);

            //Assert
            Assert.IsType<NotFoundResult>(data.Result); 
        }

        //Post - CreatedResult
        [Fact]
        public void Post_Categoria_AddValidData_Return_CreatedResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);

            var cat = new CategoriaDTO() { Nome = "Testando un", ImagemUrl = "testc.jpg" };

            //Act
            var data = controller.Post(cat);

            //Assert
            Assert.IsType<CreatedAtRouteResult>(data); 
        }

        //Put - alterar um objeto categoria 
        [Fact]
        public void Put_Categoria_Update_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            var catId = 19;

            //Act  
            var existingPost = controller.Get(catId);
            
            var result = existingPost.Value.Should().BeAssignableTo<CategoriaDTO>().Subject;
            
            var catDto = new CategoriaDTO();
            catDto.CategoriaId = catId;
            catDto.Nome = "Categoria Atualizada - Testes 1";
            catDto.ImagemUrl = result.ImagemUrl;

            var updatedData = controller.Put(catId, catDto);

            //Assert  
            Assert.IsType<OkResult>(updatedData);
        }

        [Fact]
        public void Put_Categoria_Update_InvalidData_Return_BadRequest()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            var catId = 1000;

            //Act  
            var existingPost = controller.Get(catId);
            
            var result = existingPost.Value.Should().BeAssignableTo<CategoriaDTO>().Subject;

            var catDto = new CategoriaDTO();
            catDto.CategoriaId = result.CategoriaId;
            catDto.Nome = "Categoria Atualizada - Testes 1 com nome muiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiitttttttttttttttttttttttttttttttttooooooooooooooo looooooooooooooooooooooooooooooooooooooooooooooonnnnnnnnnnnnnnnnnnnnnnnnnnnngo";
            catDto.ImagemUrl = result.ImagemUrl;

            var data = controller.Put(catId, catDto);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }
        //=======================================Delete ===================================
        [Fact]
        public void Delete_Categoria_Return_OkResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            var catId = 20;

            //Act  
            var data = controller.Delete(catId);

            //Assert  
            Assert.IsType<CategoriaDTO>(data.Value);
        }

    }
}
