using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Api_Macorrati.DTOs;

namespace Web_Api_Macorrati.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizaController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signManager;

        public AutorizaController(UserManager<IdentityUser> userManager,
                                  SignInManager<IdentityUser> signManager)
        {
            _userManager = userManager;
            _signManager = signManager; 
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "AutorizaController :: Acessado em : "
                + DateTime.Now.ToLongDateString(); 
        }

        [HttpPost("Register")]
        public async Task<ActionResult> RegisterUser([FromBody] UsuarioDTO model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState.Values.SelectMany(e => e.Errors)); 
            //}

            var user = new IdentityUser
            {
               UserName = model.Email, 
               Email = model.Email, 
               EmailConfirmed = true 
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors); 
            }

            await _signManager.SignInAsync(user, false);
            return Ok(); 
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UsuarioDTO userInfo)
        {
            //verificar as credenciais do usuário e retornar um valor
            var result = await _signManager.PasswordSignInAsync(userInfo.Email,
                userInfo.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(); 
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login Inválido...");
                return BadRequest(ModelState); 
            }
        }
    }
}
