using Inspektor_API_REST.Models;
using Inspektor_API_REST.Servicios.GenerateReport;
using Inspektor_API_REST.Servicios.MainQuery;
using Inspektor_API_REST.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Inspektor_API_REST.Controllers
{
    //[Authorize(Policy = "ConsultaListas")]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultaPrincipalController : ControllerBase
    {
        private listasrestrictivas_riskconsultingcolombia_comContext _db;
        private readonly IConfiguration _config;
        protected readonly IMainQuery _mainQuery;
        protected readonly IGenerateReport _generateReport;
        ObjetoConsulta objetoConsulta = new ObjetoConsulta();
        string coincidenciasPrioridad;

        public ConsultaPrincipalController(listasrestrictivas_riskconsultingcolombia_comContext db, IConfiguration config, IMainQuery mainQuery, IGenerateReport generateReport)
        {
            _db = db;
            _config = config;
            _mainQuery = mainQuery;
            _generateReport = generateReport;
        }

        /// <summary>
        /// Método de la consulta principal del API. Trae toda la información de listas relacionada con la información ingresada como parámetros.
        /// </summary>
        /// <param name="objetoConsulta"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ConsultaPrincipal(ObjetoConsulta objetoConsulta)
        {

            if (string.IsNullOrEmpty(objetoConsulta.Nombre) && string.IsNullOrEmpty(objetoConsulta.Identificacion))
                return BadRequest("Nombre o documento son necesarios para realizar consulta.");

            bool tienePrioridad_4 = objetoConsulta.TienePrioridad_4 != null ? (bool)objetoConsulta.TienePrioridad_4 : Convert.ToBoolean(_config.GetSection("MailNotificationSender")["prioridad_4"]);
            string cantidadPalabras = !string.IsNullOrEmpty(objetoConsulta.CantidadPalabras) ? objetoConsulta.CantidadPalabras : _config.GetSection("MailNotificationSender")["cantidadPalabras"];


            ObjetoRespuesta objetoRespuesta = new ObjetoRespuesta();
            String nombreBusqueda = string.IsNullOrEmpty(objetoConsulta.Nombre) ? "" : ReemplazarCaracteres(objetoConsulta.Nombre.Trim().Replace("  ", " ").ToUpper());
            String identificacion = string.IsNullOrEmpty(objetoConsulta.Identificacion) ? "" : objetoConsulta.Identificacion.Trim();

            var usuarioToken = UsuarioAutorizado();

            var relacionEmpresaUsuario = (_db.Usuarios.Where(User => User.Usuario == usuarioToken).FirstOrDefault());
            relacionEmpresaUsuario.TrimAllStrings();
            objetoConsulta.IdUsuario = relacionEmpresaUsuario.IdUsuario;
            objetoConsulta.IdEmpresa = relacionEmpresaUsuario.IdEmpresa;

            objetoRespuesta.Usuario = relacionEmpresaUsuario;

            var datosCantidadConsultas = await _mainQuery.ValidateQueryQuantity(objetoConsulta.IdEmpresa.ToString());

            if (datosCantidadConsultas.IdPlan != 1 && (datosCantidadConsultas.TotalConsultasRealizadas > datosCantidadConsultas.ConsultasContratadas))
            {
                return BadRequest("La cantidad de consultas asignadas al plan contrato ha llegado al límite.");
            }
            else
            {
                try
                {
                    Consultas? consulta = _db.Consultas.FromSqlInterpolated($"EXECUTE dbo.agregarConsultaProcuraduriaAll2021 {objetoConsulta.IdUsuario},{objetoConsulta.IdEmpresa},{objetoConsulta.IdUsuario},1,NULL,NULL,NULL").ToList().FirstOrDefault();



                    GuardarDetalleConsulta(consulta, objetoConsulta);
                    GuardarConsultaListas(consulta, objetoConsulta);

                    var nombreListasP = nombreBusqueda.Trim().Replace("'", "''");
                    nombreListasP = nombreListasP.Replace("&amp;", "&");
                    nombreListasP = nombreListasP.Replace("&AMP;", "&");
                    nombreListasP = ReemplazarCaracteres(nombreListasP);
                    var cantPalabras = nombreListasP.Split(" ").Length;
                    var cantLetras = String.IsNullOrEmpty(nombreListasP) ? 0 : nombreListasP.Trim().Length;

                    objetoRespuesta.NumConsulta = consulta.IdConsultaEmpresa;
                    objetoRespuesta.Nombre = objetoConsulta.Nombre;
                    objetoRespuesta.NumDocumento = objetoConsulta.Identificacion;

                    IEnumerable<Listas> listas = new List<Listas>();

                    if (!string.IsNullOrEmpty(nombreBusqueda) && !string.IsNullOrEmpty(identificacion))
                    {
                        listas = _db.Listas.FromSqlInterpolated($"EXECUTE dbo.SP_ConsultaPrioridad1ApiV3 {nombreBusqueda},{identificacion},{objetoConsulta.IdEmpresa}").ToList();
                        coincidenciasPrioridad = listas.Count() != 0 ? "1" : string.Empty;
                    }
                    if (!string.IsNullOrEmpty(identificacion))
                    {
                        IEnumerable<Listas> listasP2 = _db.Listas.FromSqlInterpolated($"EXECUTE dbo.SP_ConsultaPrioridad2ApiV3 {identificacion},{Global.notIn(listas)},{objetoConsulta.IdEmpresa}").ToList();
                        listas = listas.Concat(listasP2).ToList();
                        coincidenciasPrioridad = coincidenciasPrioridad == string.Empty && listasP2.Count() != 0 ? "2" : coincidenciasPrioridad;
                    }
                    if (!string.IsNullOrEmpty(nombreBusqueda))
                    {
                        IEnumerable<Listas> listasP3 = _db.Listas.FromSqlInterpolated($"EXECUTE dbo.SP_ConsultaPrioridad3ApiV3 {nombreBusqueda},{Global.notIn(listas)},{objetoConsulta.IdEmpresa}").ToList();
                        listas = listas.Concat(listasP3).ToList();
                        coincidenciasPrioridad = coincidenciasPrioridad == string.Empty && listasP3.Count() != 0 ? "3" : coincidenciasPrioridad;
                    }

                    if (tienePrioridad_4 && !string.IsNullOrEmpty(nombreBusqueda))
                    {
                        IEnumerable<Listas> listasP4 = _db.Listas.FromSqlInterpolated($"EXECUTE dbo.SP_ConsultaPrioridad4ApiV3 {nombreBusqueda},{Convert.ToInt32(cantidadPalabras)},{Global.notIn(listas)},{objetoConsulta.IdEmpresa}").ToList();
                        listas = listas.Concat(listasP4).ToList();
                        coincidenciasPrioridad = coincidenciasPrioridad == string.Empty && listasP4.Count() != 0 ? "4" : coincidenciasPrioridad;
                    }

                    foreach (var lista in listas)
                    {
                        _db.Database.ExecuteSqlInterpolated($"EXECUTE Listas.SP_InsertarDetalleConsultaLista {consulta.IdConsulta}, {objetoConsulta.IdUsuario.ToString()}, {lista.Prioridad}, {lista.IdLista}");
                    }

                    DefinirGrupoListas(ref objetoRespuesta, listas);

                    IEnumerable<ListasPropias> listasPropias = _db.ListasPropias.FromSqlInterpolated($"EXECUTE Listas.ConsultaListasPropia {nombreListasP},{identificacion},{objetoConsulta.IdEmpresa},{cantLetras},{cantPalabras}").ToList();
                    GuardarListasPropias(listasPropias, consulta.IdConsulta, objetoConsulta.IdUsuario.ToString());
                    coincidenciasPrioridad = coincidenciasPrioridad == string.Empty && listasPropias.Count() != 0 ? "5" : coincidenciasPrioridad;

                    objetoRespuesta.Listas_propias = listasPropias;
                    objetoRespuesta.CantCoincidencias = listas.Count() + listasPropias.Count();

                    List<string> RolePermission = UsuarioAutorizadoPermisos();

                    if (objetoConsulta.Procuraduria.Equals(true))
                    {
                        objetoRespuesta.procuraduria = await _mainQuery.QueryProcuraduria(objetoConsulta.Identificacion, objetoConsulta.TipoDocumento, Request, RolePermission);
                    }
                    if (objetoConsulta.RamaJEPMS.Equals(true))
                    {
                        objetoRespuesta.ramaJudicialJEPMS = await _mainQuery.QueryRamaJudicialJEMPS(objetoConsulta.Identificacion, Request, RolePermission);
                    }
                    if (objetoConsulta.RamaJudicial.Equals(true))
                    {
                        objetoRespuesta.ramaJudicial = await _mainQuery.QueryRamaJudicial(objetoConsulta.Nombre, Request, RolePermission);
                    }

                    var jsonData = JsonConvert.SerializeObject(objetoRespuesta);

                    await _generateReport.AzureReportUpload(jsonData, objetoRespuesta.NumConsulta);

                    string correoRemitente = _config.GetSection("MailNotificationSender")["from"].ToString();

                    if (!string.IsNullOrEmpty(correoRemitente))
                    {
                        CargaInfoNotificaciones(coincidenciasPrioridad, objetoRespuesta.NumConsulta.ToString(), objetoConsulta.Identificacion, objetoConsulta.Nombre);
                    }

                    return new ObjectResult(objetoRespuesta);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// Reemplazo de caracteres especiales en el nombre ingresado para consulta
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        private String ReemplazarCaracteres(String cadena)
        {
            return cadena.Replace("'", "''").Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace("Ñ", "N").Replace("ñ", "n");
        }

        /// <summary>
        /// Extrae del token el usuario al cual fue asignado.
        /// </summary>
        /// <returns></returns>
        private string UsuarioAutorizado()
        {
            Request.Headers.TryGetValue("Authorization", out var headerValue);

            var authHeader = headerValue.ToString().StartsWith("Bearer ") ? headerValue.ToString().Substring(7) : headerValue.ToString();
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadToken(authHeader);
            var token = jsonToken as JwtSecurityToken;

            string usuario = token.Claims.First(c => c.Type == "name").Value.ToString().Trim();

            return usuario;
        }

        private void GuardarListasPropias(IEnumerable<ListasPropias> listasPropias, decimal IdConsulta, string IdUsuario)
        {
            foreach (var listaPropia in listasPropias)
            {
                _db.Database.ExecuteSqlInterpolated($"EXECUTE Listas.AgregarDetalleConsultaListasPropias {IdConsulta}, {IdUsuario}, {listaPropia.TipoDocumento}, {listaPropia.DocumentoIdentidad}, {listaPropia.NombreCompleto}, {listaPropia.FuenteConsulta},{listaPropia.TipoPersona},{listaPropia.Alias}, {listaPropia.Delito}, {listaPropia.Zona}, {listaPropia.Link}, {listaPropia.OtraInformacion}, {listaPropia.IdListaPropia},{listaPropia.FechaRegistro}");
            }
        }

        private List<string> UsuarioAutorizadoPermisos()
        {
            Request.Headers.TryGetValue("Authorization", out var headerValue);

            var authHeader = headerValue.ToString().StartsWith("Bearer ") ? headerValue.ToString().Substring(7) : headerValue.ToString();
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadToken(authHeader);
            var token = jsonToken as JwtSecurityToken;

            var roles = token.Claims
                    .Where(c => c.Type == "role")
                    .Select(c => c.Value)
                    .ToList();

            return roles;
        }

        private void GuardarConsultaListas(Consultas? consulta, ObjetoConsulta objetoConsulta)
        {
            _db.Database.ExecuteSqlInterpolated($"EXECUTE Listas.InsertaConsultaLista {consulta.IdConsulta},{objetoConsulta.Nombre},{objetoConsulta.Identificacion},{""},{""},{"WS-Inspektor"}, {objetoConsulta.IdUsuario}");
        }

        /// <summary>
        /// Almacena los detalles de la consulta realizada
        /// </summary>
        /// <param name="consulta"></param>
        /// <param name="consultaIndividual"></param>
        private void GuardarDetalleConsulta(Consultas? consulta, ObjetoConsulta consultaIndividual)
        {
            DetalleConsulta detalleConsulta = new DetalleConsulta()
            {
                IdConsulta = consulta.IdConsulta,
                NombreConsulta = consultaIndividual.Nombre,
                IdentificacionConsulta = consultaIndividual.Identificacion,
                DocumentoCoincidencia = String.Empty,
                NombreCoincidencia = String.Empty,
                NombreLista = String.Empty,
                Delito = String.Empty,
                Peps = String.Empty,
                FechaActualizacion = consulta.FechaConsulta,
                Validado = false,
                TipoIdentificacion = String.Empty,
                TipoVinculo = String.Empty,
                AnoRenovacion = String.Empty,
                EstadoMatricula = String.Empty,
                Embargado = String.Empty,
                Liquidacion = String.Empty,
                Afiliado = String.Empty,
                Multas = String.Empty,
                Proponente = String.Empty
            };
            _db.DetalleConsulta.Add(detalleConsulta);
            _db.SaveChanges();
        }

        /// <summary>
        /// Método que permite agrupar todos los resultados de las listas asociadas en el grupo lista del objeto respuesta.
        /// </summary>
        /// <param name="objetoRespuesta"></param>
        /// <param name="listas"></param>
        private void DefinirGrupoListas(ref ObjetoRespuesta objetoRespuesta, IEnumerable<Listas> listas)
        {
            objetoRespuesta.Listas = Array.Empty<Listas>();

            foreach (Listas lista in listas)
            {
                objetoRespuesta.Listas = objetoRespuesta.Listas.Append(lista);
            }
        }

        /// <summary>
        /// Método que llama los datos de correo del usuario del servicio
        /// </summary>
        /// <param name="IdEmpresa"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private CorreosDestino SeleccionNotificaciones(string IdEmpresa)
        {
            CorreosDestino? correoDestino = new CorreosDestino();
            try
            {
                correoDestino = (from destinatario in _db.CorreosDestinos where destinatario.IdEmpresa.ToString() == IdEmpresa select destinatario).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return correoDestino;
        }

        /// <summary>
        /// Método que procesa la información para enviar un correo de notificación al consultante
        /// </summary>
        /// <param name="Coincidencias"></param>
        /// <param name="IdConsultaEmpresa"></param>
        /// <param name="Numeiden"></param>
        /// <param name="Nombre"></param>
        private void CargaInfoNotificaciones(string coincPrioridad, string IdConsultaEmpresa, string Numeiden, string Nombre)
        {
            CorreosDestino correosDestino = new CorreosDestino();

            var IdEmpresa = _config.GetSection("MailNotificationSender")["companyId"].ToString();
            var usuarioAutorizado = UsuarioAutorizado();

            var dataConsultante = (from usuario in _db.Usuarios where usuario.Usuario == usuarioAutorizado select new { usuario.IdUsuario, usuario.IdEmpresa }).FirstOrDefault();
            var nombreUsuario = (from usuario in _db.Usuarios where usuario.IdUsuario == dataConsultante.IdUsuario select new { usuario.Nombres, usuario.Apellidos }).FirstOrDefault();
            string nombreCompleto = nombreUsuario.Nombres.ToString().Trim() + " " + nombreUsuario.Apellidos.ToString().Trim();
            string idUsuario = dataConsultante.IdUsuario.ToString();
            string idEmpresa = dataConsultante.IdEmpresa.ToString();

            correosDestino = SeleccionNotificaciones(idEmpresa);

            if (correosDestino != null)
            {
                if (coincPrioridad == "1" && correosDestino.Prioridad1.ToString().Trim() == "True")
                {
                    EnviarCorreo(IdConsultaEmpresa, coincPrioridad, Numeiden, Nombre, correosDestino.CorreosPrioridad1.ToString().Trim(), nombreCompleto, idUsuario, idEmpresa);
                }
                else if (coincPrioridad == "2" && correosDestino.Prioridad2.ToString().Trim() == "True")
                {
                    EnviarCorreo(IdConsultaEmpresa, coincPrioridad, Numeiden, Nombre, correosDestino.CorreosPrioridad2.ToString().Trim(), nombreCompleto, idUsuario, idEmpresa);
                }
                else if (coincPrioridad == "3" && correosDestino.Prioridad3.ToString().Trim() == "True")
                {
                    EnviarCorreo(IdConsultaEmpresa, coincPrioridad, Numeiden, Nombre, correosDestino.CorreosPrioridad3.ToString().Trim(), nombreCompleto, idUsuario, idEmpresa);
                }
                else if (coincPrioridad == "4" && correosDestino.Prioridad4.ToString().Trim() == "True")
                {
                    EnviarCorreo(IdConsultaEmpresa, coincPrioridad, Numeiden, Nombre, correosDestino.CorreosPrioridad4.ToString().Trim(), nombreCompleto, idUsuario, idEmpresa);
                }
                else if (coincPrioridad == "5" && correosDestino.Prioridad1.ToString().Trim() == "True")
                {
                    EnviarCorreo(IdConsultaEmpresa, "Listas Propias", Numeiden, Nombre, correosDestino.CorreosPrioridad1.ToString().Trim(), nombreCompleto, idUsuario, idEmpresa);
                }
            }
        }

        MailMessage mms = new MailMessage();
        SmtpClient smt = new SmtpClient();

        /// <summary>
        /// Define y envia el formato del correo de notificación.
        /// </summary>
        /// <param name="IdConsultaEmpresa"></param>
        /// <param name="Prioridad"></param>
        /// <param name="Numeiden"></param>
        /// <param name="Nombre"></param>
        /// <param name="Para"></param>
        /// <param name="NombreUsuario"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        private bool EnviarCorreo(string IdConsultaEmpresa, string Prioridad, string Numeiden, string Nombre, string Para, string NombreUsuario, string idUsuario, string idEmpresa)
        {
            var configManager = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string De = configManager.GetSection("MailNotificationSender")["from"].ToString();
            string Pss = configManager.GetSection("MailNotificationSender")["pwd"].ToString();
            string Mensaje = "";
            Mensaje = "Se encontraron coincidencias de Prioridad " + Prioridad + " para la consulta realizada con los siguientes parámetros de búsqueda:" + "<br>";
            Mensaje = Mensaje + "<br>";
            Mensaje = Mensaje + " Número de Identificación : <B>" + Numeiden + "</B><br>";
            Mensaje = Mensaje + " Nombre : <B>" + Nombre + "</B><br>";
            Mensaje = Mensaje + " Nombre de Usuario Consulta : " + NombreUsuario + "<br>";
            Mensaje = Mensaje + "<br>";
            Mensaje = Mensaje + "Para obtener mayor información, lo invitamos a consultar esta coincidencia en nuestra aplicación Inspektor® https://inspektor.datalaft.com/ accediendo por la opción Consulta Reporte e ingresando el número de consulta <B>" + IdConsultaEmpresa + "</B>";
            Mensaje = Mensaje + "<br>" + " <br>";
            Mensaje = Mensaje + "Este es un mensaje enviado automáticamente por Inspektor®, por favor no responderlo.";

            string Asunto = $"Notificación de coincidencia Prioridad {Prioridad} Consulta Individual No. " + IdConsultaEmpresa;

            try
            {
                mms.From = new MailAddress(De);

                string[] strSeparator = new string[] { ";" };
                string[] arrName = Para.Split(strSeparator, StringSplitOptions.None);
                int i = arrName.Length;
                for (int j = 0; j < i; j++)
                {
                    mms.To.Add(new MailAddress(arrName[j]));
                }

                mms.Body = Mensaje;
                mms.IsBodyHtml = true;
                mms.Subject = Asunto;
                smt.Host = configManager.GetSection("MailNotificationSender")["host"].ToString();
                smt.Port = Convert.ToInt16(configManager.GetSection("MailNotificationSender")["port"].ToString());
                smt.Credentials = new NetworkCredential(De, Pss);
                smt.EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                InsertCorreosEnviados(Asunto, Para, Mensaje, idUsuario, idEmpresa, "1");

                return true;
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        /// <summary>
        /// Método que guarda registro del correo de notificación enviado. No se invoca directamente a la tabla involucrada debido a que la respuesta del SP no retorna una respuesta con la estructura de dicha tabla.
        /// </summary>
        /// <param name="Asunto"></param>
        /// <param name="Para"></param>
        /// <param name="Detalle"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private String InsertCorreosEnviados(string Asunto, string Para, string Detalle, string idUsuario, string idEmpresa, string estado = "0")
        {
            try
            {
                var idEmpresaParam = new SqlParameter("@IdEmpresa", Convert.ToInt32(idEmpresa));
                var asuntoParam = new SqlParameter("@Asunto", Asunto ?? (object)DBNull.Value);
                var paraParam = new SqlParameter("@Para", Para ?? (object)DBNull.Value);
                var detalleParam = new SqlParameter("@Detalle", Detalle ?? (object)DBNull.Value);
                var idUsuarioParam = new SqlParameter("@IdUsuario", Convert.ToInt32(idUsuario));
                var estadoParam = new SqlParameter("@Estado", DBNull.Value);

                var correosEnviados = _db.Database.ExecuteSqlRaw(
                    "EXECUTE dbo.agregarCorreosEnviados @IdEmpresa, @Asunto, @Para, @Detalle, @IdUsuario, @Estado",
                    idEmpresaParam, asuntoParam, paraParam, detalleParam, idUsuarioParam, estadoParam
                );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return idEmpresa.ToString().Trim();
        }
    }
}
