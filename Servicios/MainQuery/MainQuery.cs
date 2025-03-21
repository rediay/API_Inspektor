using Inspektor_API_REST.Models;
using Inspektor_API_REST.Models.ServicesAdditional;
using Inspektor_API_REST.Servicios.ServicesAdditional;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Inspektor_API_REST.Servicios.MainQuery
{
    public class MainQuery : IMainQuery
    {
        private listasrestrictivas_riskconsultingcolombia_comContext _db;
        protected readonly IServicesAdditional _servicesAdditional;
        public MainQuery(listasrestrictivas_riskconsultingcolombia_comContext db, IServicesAdditional servicesAdditional)
        {
            _db = db;
            _servicesAdditional = servicesAdditional;
        }
        public async Task<DatosCantidadConsultas> ValidateQueryQuantity(string company)
        {
            DatosCantidadConsultas data = new DatosCantidadConsultas();
            try
            {
                var dtInformation = _db.DatosCantidadConsultas.FromSqlRaw($"EXEC CantidadConsultas {company}").ToList();

                data = dtInformation.FirstOrDefault();

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<cXsHttpResponse<Procuraduria>> QueryProcuraduria(string identification, int? typeDocument, HttpRequest Request, List<string> RolePermission)
        {
            try
            {
                cXsHttpResponse<Procuraduria> result = new cXsHttpResponse<Procuraduria>();
                if (RolePermission.Contains("OnlyProcuraduria")) 
                {
                   result = await _servicesAdditional.makeProcuraduriaRequest(identification, typeDocument, Request);
                }
                else
                {
                    result.HasError = true;
                    result.ErrorMessage = "No tiene permisos para realizar esta consulta";
                }
               

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<cXsHttpResponse<IEnumerable<RamaJudicial>>> QueryRamaJudicial(string name, HttpRequest Request, List<string> RolePermission)
        {
            try
            {
                cXsHttpResponse<IEnumerable<RamaJudicial>> result = new cXsHttpResponse<IEnumerable<RamaJudicial>>();
                if (RolePermission.Contains("OnlyRama"))
                {
                    result = await _servicesAdditional.makeRamaJudicialRequest(name, Request);
                }
                else
                {
                    result.HasError = true;
                    result.ErrorMessage = "No tiene permisos para realizar esta consulta";
                }


                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<cXsHttpResponse<IEnumerable<RamaJudicialJEMPS>>> QueryRamaJudicialJEMPS(string identification, HttpRequest Request, List<string> RolePermission)
        {
            try
            {
                cXsHttpResponse<IEnumerable<RamaJudicialJEMPS>> result = new cXsHttpResponse<IEnumerable<RamaJudicialJEMPS>>();
                if (RolePermission.Contains("OnlyRamaJEPMS"))
                {
                    result = await _servicesAdditional.makeRamaJudicialJEMPSRequest(identification, Request);                    
                }
                else
                {
                    result.HasError = true;
                    result.ErrorMessage = "No tiene permisos para realizar esta consulta";
                }


                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
