using Inspektor_API_REST.Models;
using Inspektor_API_REST.Models.Auth;
using Inspektor_API_REST.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inspektor_API_REST.Servicios
{
    public class AuthService<TUser> : IAuthService
        where TUser : Models.Usuarios, new()
    {
        protected readonly UserManager<TUser> userManager;
        protected readonly JwtManage jwtManager;
        private listasrestrictivas_riskconsultingcolombia_comContext _db;
        public AuthService(JwtManage jwtManager, listasrestrictivas_riskconsultingcolombia_comContext db)
        {
            this.jwtManager = jwtManager;
            _db = db;
        }

        public async Task<Usuarios> GetDataUser(string usr, string psswd)
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

        private async Task<List<Permisos>> GetDataPermisos(int IdUser)
        {
            try
            {
                var result = _db.Permisos.FromSqlInterpolated($"EXEC Seguridad.ConsultaPermisos {IdUser}")
                                       .AsEnumerable().ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Token> LoginToken(Usuarios user)
        {
            var permissions = await GetDataPermisos(user.IdUsuario);
            var token = jwtManager.GenerateToken(user, permissions);
            return token;
        }

        public async Task<AuthResult<Token>> RefreshToken(string refreshToken)
        {
            var refreshTokenNew = refreshToken;
            if (string.IsNullOrEmpty(refreshTokenNew))
                return AuthResult<Token>.UnauthorizedResult;

            try
            {
                var principal = jwtManager.GetPrincipal(refreshTokenNew, isAccessToken: false);
                var userId = principal.GetUserId();
                var user = await _db.Usuarios.FindAsync(userId);

                if (user != null && user.IdUsuario > 0)
                {
                    var permissions = await GetDataPermisos(userId);
                    var token = jwtManager.GenerateToken(user, permissions);
                    return AuthResult<Token>.TokenResult(token);
                }
            }
            catch (Exception)
            {
                return AuthResult<Token>.UnauthorizedResult;
            }

            return AuthResult<Token>.UnauthorizedResult;
        }
    }
}
