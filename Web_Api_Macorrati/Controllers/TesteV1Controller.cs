using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Web_Api_Macorrati.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/teste")]
    [ApiController]
    public class TesteV1Controller : ControllerBase
    {
        public ActionResult Get()
        {
            return Content("<html><body><h2>Teste v1 Controller - V: 1.0 </h2></body></html>", "text/html");
        }

    }
}
