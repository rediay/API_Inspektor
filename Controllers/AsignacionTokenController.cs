using Inspektor_API_REST.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.Data.SqlClient;
using Inspektor_API_REST.Utilities;

namespace Inspektor_API_REST.Controllers
{
    [AllowAnonymous]
    [Route("api/AsignacionToken")]
    public class AsignacionTokenController : ControllerBase
    {
        private listasrestrictivas_riskconsultingcolombia_comContext _db;
        private readonly IConfiguration _config;
        private RespuestaToken respuesta = new RespuestaToken();
        private TokenJWTUsuario tokenJWT = new TokenJWTUsuario();
        List<Usuarios> usuarios = new List<Usuarios>();

        public AsignacionTokenController(listasrestrictivas_riskconsultingcolombia_comContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        /// <summary>
        /// Método que permite crear y asignar un token JWT para los usuarios que quieran hacer uso del servicio.
        /// </summary>
        /// <param name="nuevoUsuarioToken"></param>
        /// <returns>Devuelve el token JWT en formato string, previamente almacenado.</returns>
        [HttpPost]
        [Route("NuevoTokenJWT")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GeneradorNuevoToken(NuevoUsuarioToken nuevoUsuarioToken)
        {
            CrearToken crearToken = new CrearToken();
            var configManager = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var riskConIT_id = configManager.GetSection("JWTParameters")["RiskConIT_id"];
            var rolAdmin_id = configManager.GetSection("JWTParameters")["RolAdmin_id"];

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Usuarios usuario = await ObtenerDatosUsuario(nuevoUsuarioToken.Usuario.ToString(), nuevoUsuarioToken.Contrasena.ToString());
            Usuarios usuarioNuevoToken = await ConsultaNuevoUsuario(nuevoUsuarioToken.IdUsuarioToken);

            if (usuario == null && usuarioNuevoToken == null)
            {
                return BadRequest("Usuario no encontrado. Por favor revise los datos ingresados");
            }
            else if (usuarioNuevoToken == null || usuario == null)
            {
                return BadRequest("No hay registrado ningún usuario con el ID ingresado o sus credenciales no son correctas. Por favor revise los datos ingresados");
            }

            bool validezUsuario = !usuarioNuevoToken.Bloqueado;

            if (usuario.IdEmpresa == Convert.ToInt32(riskConIT_id) && usuario.IdRol == Convert.ToInt32(rolAdmin_id))
            {
                if (validezUsuario)
                {
                    if (!ComprobacionTokenCreado(usuarioNuevoToken.IdUsuario))
                    {                      
                        var token = crearToken.GeneradorTokenJWT(usuarioNuevoToken.Usuario);
                        respuesta.TraeResultados = true;
                        respuesta.Error = false;
                        respuesta.Data = token;

                        tokenJWT.TokenJWT = token;
                        tokenJWT.IdUsuario = usuarioNuevoToken.IdUsuario;
                        tokenJWT.FechaCreacion = DateTime.Now;
                        _db.TokenJWTUsuarios.Add(tokenJWT);
                        _db.SaveChanges();

                        return Ok(respuesta);
                    }
                    else
                    {
                        return Ok("Este usuario ya cuenta con una llave asignada");
                    }
                }
                else
                {
                    respuesta.TraeResultados = false;
                    respuesta.Error = true;
                    respuesta.Data = "El usuario registrado con esas credenciales actualmente se encuentra bloqueado. Contacte al administrador para revisar el caso.";
                    return Ok(respuesta);
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Método generador del token. toma como identificadores parametros almacenados en "appsettings.json" definiendo la vigencia de cada token según el valor definido en
        /// el parámetro "ExpirationTime". Está definido a 1 año por defecto.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public static string GeneradorTokenJWT(string usuario)
        {
            var configManager = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var llaveSecreta = configManager.GetSection("JWTParameters")["SecretKey"];
            var tokenPublico = configManager.GetSection("JWTParameters")["AuditionTokenJWT"];
            var tokenEdicion = configManager.GetSection("JWTParameters")["IssuerTokenJWT"];
            var tiempoVigencia = configManager.GetSection("JWTParameters")["ExpirationTime"];

            var llaveSeguridad = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(llaveSecreta));
            var credenciales = new SigningCredentials(llaveSeguridad, SecurityAlgorithms.HmacSha256Signature);

            ClaimsIdentity peticionIdentidad = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, usuario) });

            var manejoToken = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var tokenSeguridadJwt = manejoToken.CreateJwtSecurityToken(
                audience: tokenPublico,
                issuer: tokenEdicion,
                subject: peticionIdentidad,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(tiempoVigencia)),
                signingCredentials: credenciales
                );
            var stringTokenJwt = manejoToken.WriteToken(tokenSeguridadJwt);
            return stringTokenJwt;
        }

        /// <summary>
        /// Método que comprueba si el ID del usuario que se está ingresando para la creación de un token ya tiene uno asignado en la BD
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        private bool ComprobacionTokenCreado(int idUsuario)
        {
            bool existeToken = false;
            int token = (from tokenAsignado in _db.TokenJWTUsuarios where tokenAsignado.IdUsuario == idUsuario select tokenAsignado.IdUsuario).FirstOrDefault();
            existeToken = token == 0 ? false : true;

            return existeToken;
        }

        /// <summary>
        /// Método que comprueba si hay registro de usuario que coincida con el ID ingresado para la asignación de token
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        private async Task<Usuarios> ConsultaNuevoUsuario(int idUsuario)
        {
            try
            {
                var nUsuario = (from nuevoUsuario in _db.Usuarios where nuevoUsuario.IdUsuario == idUsuario select nuevoUsuario).FirstOrDefaultAsync();
                return await nUsuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que busca los datos ingresados como creador del token
        /// </summary>
        /// <param name="usr"></param>
        /// <param name="psswd"></param>
        /// <returns></returns>
        private async Task<Usuarios> ObtenerDatosUsuario(string usr, string psswd)
        {
            try
            {
                var result = _db.Usuarios.FromSqlInterpolated($"EXEC autenticarUsuarioWeb {usr}, {psswd}")
                                       .AsEnumerable().FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
