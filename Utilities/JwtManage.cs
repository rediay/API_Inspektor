using Inspektor_API_REST.Models;
using Inspektor_API_REST.Models.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Inspektor_API_REST.Utilities
{
    public class JwtManage
    {
        private readonly JwtOption jwtOptions;

        public JwtManage(IOptions<JwtOption> jwtOptions)
        {
            this.jwtOptions = jwtOptions.Value;
        }

        public Token GenerateToken<TUser>(TUser user, List<Permisos> permissions) where TUser : Usuarios
        {
            var token = new Token
            {
                Expires_in = jwtOptions.AccessValidFor.TotalMilliseconds,
                Access_token = CreateToken(user, jwtOptions.AccessExpiration, jwtOptions.AccessSigningCredentials, permissions),
                Refresh_token = CreateToken(user, jwtOptions.RefreshExpiration, jwtOptions.RefreshSigningCredentials, permissions)
            };

            return token;
        }
        private string CreateToken<TUser>(TUser user, DateTime expiration, SigningCredentials credentials, List<Permisos> permissions) where TUser : Usuarios
        {
            var claimsIdentity = GenerateClaimsIdentity(user, permissions);

            var manejoToken = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var tokenSeguridadJwt = manejoToken.CreateJwtSecurityToken(
                audience: jwtOptions.Audience,
                issuer: jwtOptions.Issuer,
                subject: claimsIdentity,
                notBefore: DateTime.UtcNow,
                expires: expiration,
                signingCredentials: credentials
                );
            var stringTokenJwt = manejoToken.WriteToken(tokenSeguridadJwt);
            return stringTokenJwt;
        }

        public ClaimsPrincipal GetPrincipal(string token, bool isAccessToken = true)
        {
            var key = new SymmetricSecurityKey(isAccessToken ? jwtOptions.AccessSecret : jwtOptions.RefreshSecret);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        private static ClaimsIdentity GenerateClaimsIdentity(Usuarios user, List<Permisos> permissions)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Usuario),
                new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString()),
                new Claim("CompanyId", user.IdEmpresa.ToString()),
            };

            if (permissions != null)
            {
                foreach (var permission in permissions)
                {
                    claims.Add(new Claim(ClaimTypes.Role, permission.Tag));
                }
            }

            return new ClaimsIdentity(claims);
        }
    }

}
