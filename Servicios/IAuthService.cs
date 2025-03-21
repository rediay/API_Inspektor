using Inspektor_API_REST.Models;
using Inspektor_API_REST.Models.Auth;
using System.Threading.Tasks;

namespace Inspektor_API_REST.Servicios
{
    public interface IAuthService
    {
        Task<Token> LoginToken(Usuarios user);
        Task<AuthResult<Token>> RefreshToken(string refreshToken);
        Task<Usuarios> GetDataUser(string usr, string psswd);

    }
}
