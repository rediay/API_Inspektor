using Inspektor_API_REST.Models.Report;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Threading.Tasks;

namespace Inspektor_API_REST.Servicios.GenerateReport
{
    public interface IGenerateReport
    {
        Task<ObjetRequest> getQuery(decimal idConsulta, HttpRequest Request);
        Task<string> imageBase64Company(int idUsuario);
        Task<string> AzureReportDownload(decimal idConsultaReport);
        Task AzureReportUpload(string jsonData, decimal idConsultaReport);
        Task<DataTable> DetailConsultationConsultation(string idCompanyConsultation, string IdCompany);
        Task<DataTable> LoadList(string idStatus, string IdCompany);

    }
}
