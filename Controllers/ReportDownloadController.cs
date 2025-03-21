using Inspektor_API_REST.Models;
using Inspektor_API_REST.Models.Report;
using Inspektor_API_REST.Servicios.GenerateReport;
using Inspektor_API_REST.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Text.Json;
using System.Threading.Tasks;
using ThirdParty.Json.LitJson;

namespace Inspektor_API_REST.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportDownloadController : ControllerBase
    {
        protected readonly IGenerateReport _generateReport;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ReportDownloadController(IGenerateReport generateReport, IWebHostEnvironment webHostEnvironment)
        {
            _generateReport = generateReport;
            _webHostEnvironment = webHostEnvironment;
        }
        /// <summary>
        /// Método de la consulta principal del API. Trae toda la información de listas relacionada con la información ingresada como parámetros.
        /// </summary>
        /// <param name="objetoConsulta"></param>
        /// <returns></returns>
        [HttpGet("getReport/{idConsulta}")]
        public async Task<IActionResult> GetReport(decimal idConsulta)
        {
            try
            {
                FilesHelper filesHelper = new FilesHelper();
                string contentRootPath = _webHostEnvironment.ContentRootPath;
                var makeReportHelper = new MakeReportHelper();

                var result = await _generateReport.getQuery(idConsulta, Request);
                var usuarioToken = filesHelper.UsuarioAutorizado(Request);
                var listas = await _generateReport.LoadList("1", usuarioToken.IdCompany.ToString());

                //DataTable infoDocumentoNombre = await _generateReport.DetailConsultationConsultation(idConsulta.ToString(), usuarioToken.IdCompany.ToString());

                if (result == null)
                {
                    return BadRequest("No se ha encontrado información para la consulta realizada");
                }

                var jsonReport = await _generateReport.AzureReportDownload(idConsulta);

                var image = await _generateReport.imageBase64Company(result.Responsable);

                ObjetoRespuesta reportStorage = JsonSerializer.Deserialize<ObjetoRespuesta>(jsonReport);

                //object[] resp = new object[3];
                var resp = makeReportHelper.generarReporteIndividualAPI(reportStorage, result, Response, contentRootPath, image, listas);
                
                if (resp != null)
                {
                    byte[]? bytes = resp[0] as byte[];
                    string? mimeType = resp[1] as string;
                    string? namepdf = resp[2] as string;
                    string? extension = resp[3] as string;
                    return File(bytes, mimeType, namepdf + "." + extension);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest("Ha ocurrido un error en la apliación: " + ex.Message);
            }


        }
    }
}
