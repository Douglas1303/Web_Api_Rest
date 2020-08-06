using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Api_Macorrati.Services
{
    public class MeuServico : IMeuServico
    {
        public string Saudacao(string nome)
        {
            return $"Bem vindo, {nome} \n\n{DateTime.Now}"; 
        }
    }
}
