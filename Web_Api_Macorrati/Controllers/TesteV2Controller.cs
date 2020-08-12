using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web_Api_Macorrati.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/teste")]
    [ApiController]
    public class TesteV2Controller : ControllerBase
    {
        public ActionResult Get()
        {
            return Content("<html><body><h2>Teste v2 Controller - V: 2.0 </h2></body></html>", "text/html");
        }
    }
}
