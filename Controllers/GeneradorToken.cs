using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;

namespace Inspektor_API_REST.Controllers
{
    internal static class GeneradorToken
    {
        public static string GeneradorTokenJWT(string usuario)
        {
            var configManager = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var llaveSecreta = configManager.GetSection("JWTParameters")["SecretKey"];
            var tokenPublico = configManager.GetSection("JWTParameters")["AuditionTokenJWT"];
            var tokenEdicion = configManager.GetSection("JWTParameters")["IssuerTokenJWT"];
            var tiempoVigencia = configManager.GetSection("JWTParameters")["ExpirationTime"];

            var llaveSeguridad = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(llaveSecreta));
            var credenciales = new SigningCredentials(llaveSeguridad, SecurityAlgorithms.HmacSha256Signature);

            ClaimsIdentity peticionIdentidad = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, usuario) });

            //Creación del token para el usuario
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
    }
}
