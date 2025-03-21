using System;
using System.Threading.Tasks;

namespace Inspektor_API_REST.Servicios.Files
{
    public interface IFileShare
    {
        Task FileUploadAsync(string jsonData, decimal isConsulta);
        Task<String> FileDownloadAsync(decimal idConsultaReport);
    }
}
