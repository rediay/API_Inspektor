using Inspektor_API_REST.Models;
using Inspektor_API_REST.Models.Auth;
using Inspektor_API_REST.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Inspektor_API_REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private listasrestrictivas_riskconsultingcolombia_comContext _db;
        private readonly IConfiguration _config;

        protected readonly IAuthService authService;
        public AuthController(listasrestrictivas_riskconsultingcolombia_comContext db, IConfiguration config, IAuthService authService)
        {
            _db = db;
            _config = config.GetSection("webservicesOptions"); ;
            this.authService = authService;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(Login login)
        {
            if (!string.IsNullOrEmpty(login.User) && !string.IsNullOrEmpty(login.Password))
            {
                var result = await ValidateCredentials(login.User, login.Password);
                if (result.IsValid)
                {
                    var resultToken = await authService.LoginToken(result.DataUser);

                    return Ok(new { Token = resultToken });
                }

                return Unauthorized(result.Message);
            }
            return BadRequest("Por favor ingrese un usuario y contraseña");

        }
        private async Task<AuthenticationResponse> ValidateCredentials(string user, string password)
        {
            AuthenticationResponse responseAutenticcation = new AuthenticationResponse();
            try
            {
                if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password))
                {
                    Usuarios userQuery = await authService.GetDataUser(user.ToString(), password.ToString());

                    bool validateUser = userQuery != null;
                    if (validateUser)
                    {
                        if (userQuery.Bloqueado)
                        {
                            responseAutenticcation.IsValid = false;
                            responseAutenticcation.Message = "El usuario registrado con esas credenciales actualmente se encuentra bloqueado. Contacte al administrador para revisar el caso.";
                        }
                        responseAutenticcation.IsValid = validateUser;

                        if (validateUser)
                        {
                            responseAutenticcation.DataUser = userQuery;
                        }
                    }
                    else
                    {
                        responseAutenticcation.IsValid = false;
                        responseAutenticcation.Message = "El nombre de usuario o la contraseña son incorrectos. Por favor, inténtalo de nuevo.";
                    }

                }
            }
            catch (Exception ex)
            {

                responseAutenticcation.IsValid = false;
                responseAutenticcation.Message = _config["serviceError"];
            }
            
            return responseAutenticcation;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenRefresh refreshToken)
        {
            var result = await authService.RefreshToken(refreshToken.Refresh_token);

            if (result.Succeeded)
                return Ok(new { token = result.Data });

            return BadRequest();
        }
    }
}
