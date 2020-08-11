using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Web_Api_Macorrati.Validations;

namespace Web_Api_Macorrati.Models
{
    [Table("Produtos")]
    public class Produto : IValidatableObject
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório!")]
        [StringLength(80, ErrorMessage = "O nome deve ter entre 5 e 20 caracteres!", MinimumLength = 5)]
        //[PrimeiraLetraMaiuscula]
        public string Nome { get; set; }

        [Required]
        [MaxLength(300)]
        [StringLength(300, ErrorMessage = "A descrição deve conter no máximo {1} caracteres!")]
        public string Descricao { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(8,2)")]
        [Range(1, 1000, ErrorMessage = "O preço deve estar entre {1} e {2}")]
        public decimal Preco { get; set; }

        [Required]
        [MaxLength(300)]
        public string ImagemUrl { get; set; }

        public float Estoque { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now; 

        public int CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(this.Nome))
            {
                var primeiraLetra = this.Nome[0].ToString();

                if (primeiraLetra != primeiraLetra.ToUpper())
                {
                    yield return new ValidationResult("A primeira letra do produto deve ser maiúscula",
                        new[]
                        { 
                            nameof(this.Nome)
                        });
                }
            }

            if (this.Estoque <= 0)
            {
                yield return new ValidationResult("O estoque deve ser maior do que 0",
                        new[]
                        {
                            nameof(this.Estoque)
                        });
            }
        }
    }
}
