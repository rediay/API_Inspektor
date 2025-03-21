using Inspektor_API_REST.Models;
using Inspektor_API_REST.Models.ServicesAdditional;
using Inspektor_API_REST.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Inspektor_API_REST.Servicios.ServicesAdditional
{
    public class ServicesAdditional : IServicesAdditional
    {
        private listasrestrictivas_riskconsultingcolombia_comContext _db;
        protected IConfigurationSection configuration;
        protected readonly HttpClient _httpClient;
        public ServicesAdditional(listasrestrictivas_riskconsultingcolombia_comContext db, HttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _db = db;
            _httpClient = httpClientFactory.CreateClient();
            this.configuration = configuration.GetSection("webservicesOptions");
        }
        public async Task<cXsHttpResponse<Procuraduria>> makeProcuraduriaRequest(string identification, int? typeDocument, HttpRequest Request)
        {
            var infoService = _db.Servicios.FirstOrDefault(x => x.IdServicio == 13);

            string url = $"{infoService.URL_SRV.Trim()}{typeDocument}&NumeroIdentificacion={identification}";

            cXsHttpResponse<Procuraduria> procuraduria = new cXsHttpResponse<Procuraduria>();
            try
            {
                if (infoService.Estado)
                {
                    AddLogAdditionalServices(1, identification, typeDocument, Request);
                    RequestAditionalServiceParams parameters = new RequestAditionalServiceParams()
                    {
                        url = url,
                        method = HttpMethod.Get
                    };
                    var json = await MakeHttpRequest(parameters);
                    if (json != null)
                    {
                        if (json.Contains(",\"not_criminal_records\":false"))
                            json = json.Replace(",\"not_criminal_records\":false", "");
                        //json = json.Replace(",\"not_criminal_records\":false", "");
                        procuraduria.Data = JsonConvert.DeserializeObject<Procuraduria>(json);

                        if (procuraduria.Data.not_criminal_records != null && !procuraduria.Data.not_criminal_records.ToString().Equals("False"))
                        {
                            procuraduria.HasError = true;
                            procuraduria.ErrorMessage = procuraduria.Data.not_criminal_records.message.ToString().Trim();
                        }
                    }
                    else
                    {
                        procuraduria.HasError = true;
                        procuraduria.ErrorMessage = this.configuration["servicenodata"];
                    }

                    //if(procuraduria.Data.data.Count == 0)
                    //{
                    //    procuraduria.HasError = true;
                    //    procuraduria.ErrorMessage = this.configuration["serviceNoData"];
                    //}
                }
                else
                {
                    procuraduria.HasError = true;
                    procuraduria.ErrorMessage = this.configuration["serviceDisabled"];
                }

            }
            catch (Exception ex)
            {
                procuraduria.HasError = true;
                procuraduria.ErrorMessage = this.configuration["serviceError"];
                return procuraduria;
            }
            return procuraduria;
        }
        public async Task<cXsHttpResponse<IEnumerable<RamaJudicialJEMPS>>> makeRamaJudicialJEMPSRequest(string document, HttpRequest Request)
        {
            var infoService = _db.Servicios.FirstOrDefault(x => x.IdServicio == 14);
            string url = $"{infoService.URL_SRV.Trim() + document}";

            cXsHttpResponse<IEnumerable<RamaJudicialJEMPS>> ramaJudicials = new cXsHttpResponse<IEnumerable<RamaJudicialJEMPS>>();

            try
            {
                if (infoService.Estado)
                {
                    AddLogAdditionalServices(14, document, null, Request);
                    RequestAditionalServiceParams parameters = new RequestAditionalServiceParams()
                    {
                        url = url,
                        method = HttpMethod.Get,
                        token = this.configuration["ramaProcToken"]
                    };
                    var json = await MakeHttpRequest(parameters);

                    ramaJudicials.Data = JsonConvert.DeserializeObject<List<RamaJudicialJEMPS>>(json);

                    if (ramaJudicials.Data.Count() == 0)
                    {
                        ramaJudicials.HasError = true;
                        ramaJudicials.ErrorMessage = "NO EXISTE INFORMACIÓN CON LOS PARÁMETROS DE LA BÚSQUEDA";
                    }

                }
                else
                {
                    ramaJudicials.HasError = true;
                    ramaJudicials.ErrorMessage = this.configuration["serviceDisabled"];
                }

            }
            catch (Exception ex)
            {
                ramaJudicials.HasError = true;
                ramaJudicials.ErrorMessage = this.configuration["serviceError"];
                return ramaJudicials;
            }
            return ramaJudicials;
        }
        public async Task<cXsHttpResponse<IEnumerable<RamaJudicial>>> makeRamaJudicialRequest(string name, HttpRequest Request)
        {
            //string url = $"https://0415-181-54-0-253.ngrok-free.app/api/judiciary_complete?Denomination={name}";
            var infoService = _db.Servicios.FirstOrDefault(x => x.IdServicio == 2);
            string url = $"{infoService.URL_SRV.Trim() + name}";
            cXsHttpResponse<IEnumerable<RamaJudicial>> ramaJudicials = new cXsHttpResponse<IEnumerable<RamaJudicial>>();

            try
            {
                if (infoService.Estado)
                {
                    AddLogAdditionalServices(2, name, null, Request);
                    RequestAditionalServiceParams parameters = new RequestAditionalServiceParams()
                    {
                        url = url,
                        method = HttpMethod.Get,
                        token = this.configuration["ramaProcToken"]
                    };
                    var json = await MakeHttpRequest(parameters);

                    cXsHttpResponseRama<IEnumerable<RamaJudicial>>  ramaJudicialNew = JsonConvert.DeserializeObject<cXsHttpResponseRama<IEnumerable<RamaJudicial>>>(json);
                    string message = ramaJudicialNew.Message;

                    if (ramaJudicialNew.HasError)
                    {
                        ramaJudicials.HasError = true;
                        ramaJudicials.ErrorMessage = message;
                        return ramaJudicials;
                    }

                    ramaJudicials.Data = ramaJudicialNew.Data;
                    ramaJudicials.ErrorMessage = message;
                    ramaJudicials.HasError = false;

                    if (ramaJudicials.Data.Count() == 0)
                    {
                        ramaJudicials.HasError = true;
                        ramaJudicials.ErrorMessage = "NO EXISTE INFORMACIÓN CON LOS PARÁMETROS DE LA BÚSQUEDA";
                    }
                }
                else
                {
                    ramaJudicials.HasError = true;
                    ramaJudicials.ErrorMessage = this.configuration["serviceDisabled"];
                }

            }
            catch (Exception ex)
            {
                ramaJudicials.HasError = true;
                ramaJudicials.ErrorMessage = this.configuration["serviceError"];
                return ramaJudicials;
            }
            return ramaJudicials;
        }
        private async Task<string> MakeHttpRequest(RequestAditionalServiceParams parameters)
        {
            var request = new HttpRequestMessage(parameters.method, parameters.url);

            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            if (!string.IsNullOrEmpty(parameters.token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", parameters.token);
                request.Headers.Authorization = new AuthenticationHeaderValue(parameters.token);
            }

            if (parameters.content != null)
            {
                request.Content = parameters.content;
            }

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return data;
            }

            return null;
        }
        private void AddLogAdditionalServices(int idService, string? identificationOrName, int? typeDocument, HttpRequest Request)
        {
            FilesHelper filesHelper = new FilesHelper();
            LogAdditionalServices logAdditionalServices = new LogAdditionalServices();

            var userToken = filesHelper.UsuarioAutorizado(Request);

            var infoUser = (_db.Usuarios.Where(User => User.Usuario == userToken.NameUser).FirstOrDefault());

            logAdditionalServices.Data = identificationOrName;
            logAdditionalServices.Datatype = typeDocument.ToString();
            logAdditionalServices.Ip = GetClientIpAddress();
            logAdditionalServices.IdServicio = idService;
            logAdditionalServices.IdUsuario = infoUser.IdUsuario;
            logAdditionalServices.IdEmpresa = infoUser.IdEmpresa;
            logAdditionalServices.FechaConsulta = DateTime.Now;

            InsertLogRecord(logAdditionalServices);
        }
        private string GetClientIpAddress()
        {

            IPAddress ipAddress = Dns.GetHostAddresses(Dns.GetHostName())[1];

            string ip = ipAddress.ToString();

            return ip;
        }
        private void InsertLogRecord(LogAdditionalServices log)
        {
            _db.LogAdditionalServices.Add(log);
            _db.SaveChanges();
        }

    }
}
