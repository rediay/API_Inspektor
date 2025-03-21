using Inspektor_API_REST.Models.ServicesAdditional;
using Inspektor_API_REST.Servicios.ServicesAdditional;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Web;

namespace Inspektor_API_REST.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ConsultaServiciosAdicionalesController : ControllerBase
    {
        protected readonly IServicesAdditional _servicesAdditional;

        public ConsultaServiciosAdicionalesController(IServicesAdditional servicesAdditional)
        {
            _servicesAdditional = servicesAdditional;
        }

        [HttpPost]
        //[Authorize(Policy = "Procuraduria")]
        [Authorize]
        [Route("Procuraduria")]
        public async Task<IActionResult> Procuraduria(DataConsult dataConsult)
        {
            var result = await _servicesAdditional.makeProcuraduriaRequest(dataConsult.Identification, dataConsult.typeDocument, Request);

            if (!result.HasError)
                return Ok(HttpUtility.HtmlDecode(result.Data.html_response));

            return BadRequest(result.ErrorMessage);

        }
        [HttpPost]
        //[Authorize(Policy = "Rama")]
        [Authorize]
        [Route("RamaJudicial")]
        public async Task<IActionResult> RamaJudicial(DataConsult dataConsult)
        {
            var result = await _servicesAdditional.makeRamaJudicialRequest(dataConsult.Name, Request);

            if (!result.HasError)
                return Ok(result.Data);

            return BadRequest(result.ErrorMessage);

        }
        [HttpPost]
        //[Authorize(Policy = "RamaJEPMS")]
        [Authorize]
        [Route("RamaJudicialJEMPS")]
        public async Task<IActionResult> RamaJudicialJEMPS(DataConsult dataConsult)
        {
            var result = await _servicesAdditional.makeRamaJudicialJEMPSRequest(dataConsult.Identification, Request);

            if (!result.HasError)
                return Ok(result.Data);

            return BadRequest(result.ErrorMessage);

        }
    }
}
