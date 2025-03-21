using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Session;

namespace Inspektor_API_REST.Controllers
{
    internal class ValidadorToken : DelegatingHandler
    {
        private static bool IntentoRecuperacionToken(HttpRequestMessage peticion, out string token)
        {
            token = null;
            IEnumerable<string> encabezadoAutorizacion;
            if (!peticion.Headers.TryGetValues("Authorization", out encabezadoAutorizacion) || encabezadoAutorizacion.Count() > 1)
            {
                return false;
            }
            var bearerToken = encabezadoAutorizacion.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage peticion, CancellationToken cancelacionToken)
        {
            var httpContext = new HttpContextAccessor().HttpContext;
            HttpStatusCode codigoEstado;
            var configManager = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string token;

            //Comprobacion de la existencia del token
            if (!IntentoRecuperacionToken(peticion, out token))
            {
                codigoEstado = HttpStatusCode.Unauthorized;
                return base.SendAsync(peticion, cancelacionToken);
            }

            try
            {
                var llaveSecreta = configManager.GetSection("JWTParameters")["SecretKey"];
                var tokenPublico = configManager.GetSection("JWTParameters")["AuditionTokenJWT"];
                var tokenEdicion = configManager.GetSection("JWTParameters")["IssuerTokenJWT"];
                var llaveSeguridad = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(llaveSecreta));

                SecurityToken tokenSeguridad;
                var manejoToken = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                TokenValidationParameters parametrosValidacion = new TokenValidationParameters()
                {
                    ValidAudience = tokenPublico,
                    ValidIssuer = tokenEdicion,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.ValidadorVigencia,
                    IssuerSigningKey = llaveSeguridad
                };

                // Extraer y asignar principal y usuario actual
                Thread.CurrentPrincipal = manejoToken.ValidateToken(token, parametrosValidacion, out tokenSeguridad);
                //string strSesion = httpContext.Session.SetString(token, "xxx");
                httpContext.User = manejoToken.ValidateToken(token, parametrosValidacion, out tokenSeguridad);

                return base.SendAsync(peticion, cancelacionToken);
            }
            catch (SecurityTokenValidationException)
            {
                codigoEstado = HttpStatusCode.Unauthorized;
            }
            catch (Exception)
            {
                codigoEstado = HttpStatusCode.InternalServerError;
            }

            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(codigoEstado) { });
        }

        public bool ValidadorVigencia(DateTime? despuesDe, DateTime? expiracion, SecurityToken tokenSeguridad, TokenValidationParameters parametrosValidacion)
        {
            if (expiracion != null)
            {
                if (DateTime.UtcNow < expiracion) { return true; }
            }
            return false;
        }
    }
}
