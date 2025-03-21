using Inspektor_API_REST.Models;
using Inspektor_API_REST.Models.Auth;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;

namespace Inspektor_API_REST.Utilities
{
    public class FilesHelper
    {
        public static Boolean saveConsultaFile(ObjetoRespuesta consultaIndividual)
        {
            try
            {
                var jsondata = JsonConvert.SerializeObject(consultaIndividual);

                string path = Path.Combine(Directory.GetCurrentDirectory(), $"Files/Consultas/{consultaIndividual.NumConsulta}.json");
                System.IO.File.WriteAllText(path, jsondata);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static ObjetoRespuesta getConsultaIndividual(string idConsulta)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"Files/Consultas/{idConsulta}.json");

            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                ObjetoRespuesta consultaIndividual = JsonConvert.DeserializeObject<ObjetoRespuesta>(json);
                return consultaIndividual;
            }
        }
        public AuthUsuario UsuarioAutorizado(HttpRequest Request)
        {
            AuthUsuario authUsuario = new AuthUsuario();
            Request.Headers.TryGetValue("Authorization", out var headerValue);

            var authHeader = headerValue.ToString().StartsWith("Bearer ") ? headerValue.ToString().Substring(7) : headerValue.ToString();
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadToken(authHeader);
            var token = jsonToken as JwtSecurityToken;

            //authUsuario.NameUser = token.Claims.First(c => c.Type == "name").Value.ToString().Trim();
            //authUsuario.IdUser = token.Claims.First(c => c.Type == "nameid").Value.ToString().Trim();
            //authUsuario.IdCompany = int.Parse(token.Claims.First(c => c.Type == "CompanyId").Value.ToString().Trim());

            var nameClaim = token.Claims.FirstOrDefault(c => c.Type == "name");
            var nameIdClaim = token.Claims.FirstOrDefault(c => c.Type == "nameid");
            var companyIdClaim = token.Claims.FirstOrDefault(c => c.Type == "CompanyId");

            if (nameClaim != null)
            {
                authUsuario.NameUser = nameClaim.Value.ToString().Trim();
            }

            if (nameIdClaim != null)
            {
                authUsuario.IdUser = nameIdClaim.Value.ToString().Trim();
            }

            if (companyIdClaim != null)
            {
                authUsuario.IdCompany = int.Parse(companyIdClaim.Value.ToString().Trim());
            }

            return authUsuario;
        }
    }
}
