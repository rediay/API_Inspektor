using Inspektor_API_REST.Models;
using Inspektor_API_REST.Models.Report;
using Inspektor_API_REST.Servicios.Files;
using Inspektor_API_REST.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inspektor_API_REST.Servicios.GenerateReport
{
    public class GenerateReport : IGenerateReport
    {
        private listasrestrictivas_riskconsultingcolombia_comContext _db;
        private readonly IFileShare _fileShare;
        private readonly DatabaseHelper _databaseHelper;
        public GenerateReport(listasrestrictivas_riskconsultingcolombia_comContext db, IConfiguration config, IFileShare fileShare)
        {
            _db = db;
            _fileShare = fileShare;
            _databaseHelper = new DatabaseHelper(config);
        }
        public async Task<ObjetRequest> getQuery(decimal idConsulta, HttpRequest Request)
        {
            FilesHelper filesHelper = new FilesHelper();
            ObjetRequest result = new ObjetRequest();
            decimal idConsultaEmpresa = Convert.ToDecimal(idConsulta);

            try
            {
                var usuarioToken = filesHelper.UsuarioAutorizado(Request);

                result = SearchInfoEmpresa(usuarioToken.IdCompany, idConsultaEmpresa);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }
        public async Task<string> imageBase64Company(int idUsuario)
        {
            var relacionEmpresaUsuario = (_db.Usuarios.Where(User => User.IdUsuario == idUsuario).FirstOrDefault());
            var image = Convert.ToBase64String((_db.Empresas.Where(empresa => empresa.IdEmpresa == relacionEmpresaUsuario.IdEmpresa).Select(empresa => empresa.Imagen).FirstOrDefault()));
            return image.ToString();
        }
        private ObjetRequest SearchInfoEmpresa(int? IdEmpresa, decimal idConsultaEmpresa)
        {
            var result = new ObjetRequest { };

            if (idConsultaEmpresa != 0 && IdEmpresa != 0 && IdEmpresa != null)
            {
                result = (
                    from DtlConsulta in _db.DetalleConsulta
                    join Consulta in _db.Consultas
                    on DtlConsulta.IdConsulta equals Consulta.IdConsulta
                    where Consulta.IdConsultaEmpresa == idConsultaEmpresa && Consulta.IdEmpresa == IdEmpresa
                    orderby DtlConsulta.IdDetalleConsulta
                    select new ObjetRequest
                    {
                        idConsulta = DtlConsulta.IdConsulta,
                        idConsultaEmpresa = Consulta.IdConsultaEmpresa,
                        NombreConsulta = DtlConsulta.NombreConsulta,
                        IdentificacionConsulta = DtlConsulta.IdentificacionConsulta,
                        FechaActualizacion = DtlConsulta.FechaActualizacion.ToString(),
                        FechaConsulta = Consulta.FechaConsulta,
                        jsonReport = Consulta.jsonReport,
                        HasjsonReport = Consulta.HasjsonReport,
                        idDetalleConsulta = DtlConsulta.IdDetalleConsulta,
                        Responsable = Consulta.Responsable
                    }
                    ).FirstOrDefault();
            }

            return result;
        }
        public async Task<string> AzureReportDownload(decimal idConsultaReport)
        {
            string result = await _fileShare.FileDownloadAsync(idConsultaReport);

            return result;
        }

        public async Task AzureReportUpload(string jsonData, decimal idConsultaReport)
        {
            await _fileShare.FileUploadAsync(jsonData, idConsultaReport);

        }

        public async Task<DataTable> DetailConsultationConsultation(string idCompanyConsultation, string IdCompany)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                    new SqlParameter("@idCompanyConsultationd", SqlDbType.Int) { Value = idCompanyConsultation },
                    new SqlParameter("@IdCompany", SqlDbType.Int) { Value = IdCompany }
            };

            DataTable result = await _databaseHelper.ExecuteStoredProcedureAsync("DetalleConsultaConsolidado", parameters);

            return result;
        }

        public async Task<DataTable> LoadList(string idStatus, string IdCompany)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                    new SqlParameter("@vIdEmpresa", SqlDbType.Int) { Value = IdCompany },
                    new SqlParameter("@vEstado", SqlDbType.Int) { Value = idStatus }
            };

            DataTable result = await _databaseHelper.ExecuteStoredProcedureAsync("Listas.LoadEmpresaTiposLista", parameters);

            return result;
        }
    }
}
