using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Api_Macorrati.Validations
{
    public class PrimeiraLetraMaiusculaAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) ; 
            {
                return ValidationResult.Success; 
            }

            var primeiraLetra = value.ToString()[0].ToString();

            if (primeiraLetra != primeiraLetra.ToUpper()) //verifico se a primeira letra [0] é de caixa alta
            {
                return new ValidationResult("A primeira letra do nome do produto deve ser maiúscula!");
            }

            return ValidationResult.Success; 
        }
    }
}
