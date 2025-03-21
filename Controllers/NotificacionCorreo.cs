using Inspektor_API_REST.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Linq;
using System.Net;

namespace Inspektor_API_REST.Controllers
{
    /// <summary>
    /// 
    /// </summary>    
    public class NotificacionCorreo
    {
        //private listasrestrictivas_riskconsultingcolombia_comContext _db;
        public NotificacionCorreo()
        {
            //_db = new listasrestrictivas_riskconsultingcolombia_comContext();
        }

        MailMessage mms = new MailMessage();
        SmtpClient smt = new SmtpClient();

        public CorreosDestino SeleccionNotificaciones(string IdEmpresa)
        {
            listasrestrictivas_riskconsultingcolombia_comContext _db = new listasrestrictivas_riskconsultingcolombia_comContext ();
            CorreosDestino? correoDestino = new CorreosDestino();
            try
            {
                correoDestino = _db.CorreosDestinos.FromSqlInterpolated($" SELECT * FROM Notificaciones.CorreosDestino WHERE IdEmpresa = {IdEmpresa}").ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return correoDestino;
        }

        public bool EnviarCorreo(string IdConsultaEmpresa, string Prioridad, string Numeiden, string Nombre, string Para, string NombreUsuario, string idUsuario, string idEmpresa)
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
            Mensaje = Mensaje + "Para obtener mayor información, lo invitamos a consulta esta coincidencia en nuestra aplicación Inspektor® https://inspektor.datalaft.com/ accediendo por la opción Consulta Reporte e ingresando el número de consulta <B>" + IdConsultaEmpresa + "</B>";
            Mensaje = Mensaje + "<br>" + " <br>";
            Mensaje = Mensaje + "Este es un mensaje enviado automáticamente por Inspektor®, por favor no responderlo.";

            string Asunto = "Notificación de coincidencia Prioridad " + Prioridad + "       Consulta Individual No. " + IdConsultaEmpresa;

            try
            {
                mms.From = new MailAddress(De);
                //for para enviar correos a lista de destinatarios
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
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                smt.Credentials = new NetworkCredential(De, Pss);
                //if (ConfigurationManager.AppSettings["enableSsl"].ToString() == "S")
                smt.EnableSsl = true;
                //else
                //    smt.EnableSsl = false;
                InsertCorreosEnviados(Asunto, Para, Mensaje, idUsuario, idEmpresa, "1");
                smt.Send(mms);
                return true;
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        private String InsertCorreosEnviados(string Asunto, string Para, string Detalle, string idUsuario, string idEmpresa, string estado = "0")
        {
            listasrestrictivas_riskconsultingcolombia_comContext _db = new listasrestrictivas_riskconsultingcolombia_comContext();
            try
            {
                var correosEnviados = _db.Database.ExecuteSqlInterpolated($"EXECUTE dbo.agregarCorreosEnviados {Convert.ToInt32(idEmpresa)},{Asunto},{Para},{Detalle},{Convert.ToInt32(idUsuario)},{estado.Equals("1")}");                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return idEmpresa.ToString().Trim();
        }
    }
}
