using Microsoft.EntityFrameworkCore.Migrations;

namespace Web_Api_Macorrati.Migrations
{
    public partial class PopulaDb : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("insert into Categorias(Nome, ImagemUrl) Values('Bebidas', 'http://www.teste.com/Imagens/1.jpg')");
            mb.Sql("insert into Categorias(Nome, ImagemUrl) Values('Lanches', 'http://www.teste.com/Imagens/1.jpg')");
            mb.Sql("insert into Categorias(Nome, ImagemUrl) Values('Sobremesa', 'http://www.teste.com/Imagens/1.jpg')");

            mb.Sql("insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId)"+ 
                "Values('Coca-Cola', 'Refrigerante de cola 350 ml', 5.45, 'teste.jpg', 50, now(), (Select categoriaId From Categorias Where Nome='Bebidas'))");

            mb.Sql("insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId)" +
              "Values('Coxinha', 'Refrigerante de cola 350 ml', 5.45, 'teste1.jpg', 50, now() , (Select categoriaId From Categorias Where Nome='Lanches'))");

            mb.Sql("insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId)" +
              "Values('Coca-Cola', 'Refrigerante de cola 350 ml', 5.45, 'teste2.jpg', 50, now(), (Select categoriaId From Categorias Where Nome='Sobremesa'))");

        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Categorias");
            mb.Sql("Delete from Produtos");
        }
    }
}
