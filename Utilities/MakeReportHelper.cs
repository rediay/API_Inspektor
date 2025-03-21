using Inspektor_API_REST.Models;
using Inspektor_API_REST.Models.Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Reporting.NETCore;
using Newtonsoft.Json;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using ThirdParty.Json.LitJson;
using DataTable = System.Data.DataTable;
using LocalReport = Microsoft.Reporting.NETCore.LocalReport;
using ReportDataSource = Microsoft.Reporting.NETCore.ReportDataSource;
using ReportParameter = Microsoft.Reporting.NETCore.ReportParameter;
using Warning = Microsoft.Reporting.NETCore.Warning;

namespace Inspektor_API_REST.Utilities
{
    public class MakeReportHelper
    {
        public Object[] generarReporteIndividualAPI(ObjetoRespuesta dataIndi, ObjetRequest dataEnca, HttpResponse response, string contentRootPath, string image, DataTable listas_seleccionadas)
        {
            //try
            //{

            
            using var ReportViewerReporteConsolidadoNew = new LocalReport();
            ReportViewerReporteConsolidadoNew.DataSources.Clear();
            ReportViewerReporteConsolidadoNew.EnableExternalImages = true;
            ReportViewerReporteConsolidadoNew.EnableHyperlinks = true;
                //DateTime dateTime = DateTime.ParseExact(dataEnca.FechaConsulta.ToString().Trim(), "MMM  d yyyy h:mmtt", CultureInfo.InvariantCulture);
                //string formattedDate = dateTime.ToString("yyyy/MM/dd");
            //string formattedDateClean = dataEnca.FechaConsulta.ToString().Trim().Replace("  ", " ");
            //DateTime dateTime = DateTime.ParseExact(formattedDateClean, "MMM d yyyy h:mmtt", CultureInfo.InvariantCulture);
            //string formattedDate = dateTime.ToString("yyyy/MM/dd");
            ReportParameter[] parameters = new ReportParameter[32];
            parameters[0] = new ReportParameter("nombre_consulta", dataEnca.NombreConsulta.ToString());
            parameters[1] = new ReportParameter("identificacion_consulta", dataEnca.IdentificacionConsulta.ToString());
            parameters[2] = new ReportParameter("no_consulta", dataEnca.idConsultaEmpresa.ToString());
            parameters[3] = new ReportParameter("nombre_usuario_consultor", dataIndi.Usuario.Nombres.ToString());
            parameters[4] = new ReportParameter("usuario_consultor", dataIndi.Usuario.Usuario.ToString());
            parameters[5] = new ReportParameter("fecha_reporte", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            parameters[6] = new ReportParameter("fecha_consulta", dataEnca.FechaConsulta.ToString("dd/MM/yyyy HH:mm:ss"));

            String ejercito_mensaje = ""; // dataIndi["ejercito_mensaje"] != null ? dataIndi["ejercito_mensaje"].ToString() : "";
            parameters[8] = new ReportParameter("mensaje_ejercito", ejercito_mensaje);
            String super_sociedades_mensaje = ""; //dataIndi["super_sociedades_mensaje"] != null ? dataIndi["super_sociedades_mensaje"].ToString() : "";
            parameters[9] = new ReportParameter("mensaje_super_sociedades", super_sociedades_mensaje);
            String procuraduria_mensaje = ""; //dataIndi["procuraduria_mensaje"] != null ? dataIndi["procuraduria_mensaje"].ToString() : "";
            if (dataIndi.procuraduria != null)
            {
                if (dataIndi.procuraduria.ErrorMessage != null)
                {
                    procuraduria_mensaje = dataIndi.procuraduria.ErrorMessage != null ? dataIndi.procuraduria.ErrorMessage.ToString() : "";
                }
            }

            parameters[10] = new ReportParameter("mensaje_procuraduria", procuraduria_mensaje);
            String rama_judicial_mensaje = ""; //dataIndi["rama_judicial_mensaje"] != null ? dataIndi["rama_judicial_mensaje"].ToString() : "";
            if (dataIndi.ramaJudicial != null)
            {
                if (dataIndi.ramaJudicial.HasError)
                {
                    rama_judicial_mensaje = dataIndi.ramaJudicial.ErrorMessage != null ? dataIndi.ramaJudicial.ErrorMessage.ToString() : "";
                }
            }
            parameters[11] = new ReportParameter("mensaje_rama_judicial", rama_judicial_mensaje);
            parameters[12] = new ReportParameter("tipo_tercero", "");

            String simit_mensaje = ""; //dataIndi["simit_mensaje"] != null ? dataIndi["simit_mensaje"].ToString() : dataIndi["simit_mensaje_new"] != null ? dataIndi["simit_mensaje_new"].ToString() : "";
            parameters[13] = new ReportParameter("mensaje_simit", simit_mensaje);
            String rues_mensaje = ""; //dataIndi["rues_mensaje"] != null ? dataIndi["rues_mensaje"].ToString() : "";
            parameters[14] = new ReportParameter("mensaje_rues", rues_mensaje);
            StringBuilder str_listas_consultadas = new StringBuilder();
            int no_listas_consultadas = listas_seleccionadas.Rows.Count; //JsonMapper.ToObject(dataIndi["listas_seleccionadas"].ToString()).Count;

            foreach (DataRow item in listas_seleccionadas.Rows)
            {
                string nombreTipoLista = item["NombreTipoLista"].ToString().Trim().ToLower();

                if (!nombreTipoLista.Equals("todas"))
                {
                    if (str_listas_consultadas.Length == 0)
                    {
                        str_listas_consultadas.Append(item["NombreTipoLista"].ToString().Trim());
                    }
                    else
                    {
                        str_listas_consultadas.Append(", ").Append(item["NombreTipoLista"].ToString().Trim());
                    }
                }
                else
                {
                    no_listas_consultadas--;
                }
            }
            parameters[7] = new ReportParameter("no_listas_consultadas", no_listas_consultadas.ToString());
            parameters[15] = new ReportParameter("ListasConsultadas", str_listas_consultadas.ToString());

            String rama_judicial_jepms_mensaje = ""; //dataIndi["rama_judicial_jepms_mensaje"] != null ? dataIndi["rama_judicial_jepms_mensaje"].ToString() : "";
            if (dataIndi.ramaJudicialJEPMS != null)
            {
                if (dataIndi.ramaJudicialJEPMS.ErrorMessage != null)
                {
                    rama_judicial_jepms_mensaje = dataIndi.ramaJudicialJEPMS.ErrorMessage != null ? dataIndi.ramaJudicialJEPMS.ErrorMessage.ToString() : "";
                }
            }
            parameters[17] = new ReportParameter("mensaje_rama_judicial_jepms", rama_judicial_jepms_mensaje);

            String contaduria = ""; //dataIndi["contaduria"] != null ? dataIndi["contaduria"].ToString() : "";
            parameters[18] = new ReportParameter("mensaje_contaduria", contaduria);

            String contaduria_incumplimiento = ""; //dataIndi["contaduria_incumplimiento_acuerdos"] != null ? dataIndi["contaduria_incumplimiento_acuerdos"].ToString() : "";
            parameters[19] = new ReportParameter("mensaje_contaduria_incumplimiento_acuerdos", contaduria_incumplimiento);

            string img = image;
            parameters[16] = new ReportParameter("image", img);

            string PPT_mensaje = ""; //dataIndi["PPT_mensaje"] != null ? dataIndi["PPT_mensaje"].ToString() : "";
            parameters[20] = new ReportParameter("mensaje_PPT", PPT_mensaje);

            string mensaje_defRegistraduria = ""; //dataIndi["mensaje_defRegistraduria"] != null ? dataIndi["mensaje_defRegistraduria"].ToString() : "";
            parameters[21] = new ReportParameter("mensaje_defRegistraduria", mensaje_defRegistraduria);

            string EPS_mensaje = ""; //dataIndi["EPS_mensaje"] != null ? dataIndi["EPS_mensaje"].ToString() : "";
            parameters[22] = new ReportParameter("mensaje_EPS", EPS_mensaje);

            String criminal_records_ecuador_mensaje = ""; //dataIndi["CriminalRecordEcuador_mensaje"] != null ? dataIndi["CriminalRecordEcuador_mensaje"].ToString() : "";
            parameters[23] = new ReportParameter("mensaje_criminal_records_ecuador", criminal_records_ecuador_mensaje);

            String judicial_information_mensaje = ""; //dataIndi["JudicialInformationEcuador_mensaje"] != null ? dataIndi["JudicialInformationEcuador_mensaje"].ToString() : "";
            parameters[24] = new ReportParameter("mensaje_judicial_information_ecuador", judicial_information_mensaje);

            String sunat_mensaje = ""; //dataIndi["sunat_mensaje"] != null ? dataIndi["sunat_mensaje"].ToString() : "";
            parameters[25] = new ReportParameter("mensaje_sunat", sunat_mensaje);

            String rethus_mensaje = ""; //dataIndi["rethus_message"] != null ? dataIndi["rethus_message"].ToString() : "";
            parameters[26] = new ReportParameter("mensaje_rethus", rethus_mensaje);

            String MedidasCorrectivas_mensaje = ""; //dataIndi["MedidasCorrectivas_mensaje"] != null ? dataIndi["MedidasCorrectivas_mensaje"].ToString() : "";
            parameters[27] = new ReportParameter("mensaje_medidas_correctivas", MedidasCorrectivas_mensaje);

            String PoNal = ""; //dataIndi["PoNal"] != null ? dataIndi["PoNal"].ToString() : "";
            parameters[28] = new ReportParameter("mensaje_policia", PoNal);

            String DelitosSexuales = ""; //dataIndi["DelitosSexuales"] != null ? dataIndi["DelitosSexuales"].ToString() : "";
            parameters[29] = new ReportParameter("mensaje_delitos_sexuales", DelitosSexuales);

            String Inpec_mensaje = ""; //dataIndi["Inpec_mensaje"] != null ? dataIndi["Inpec_mensaje"].ToString() : "";
            parameters[30] = new ReportParameter("mensaje_inpec", Inpec_mensaje);

            String judicial_information_complete_mensaje = ""; //dataIndi["JudicialInformationCompleteEcuador_mensaje"] != null ? dataIndi["JudicialInformationCompleteEcuador_mensaje"].ToString() : "";
            parameters[31] = new ReportParameter("mensaje_judicial_information_complete_ecuador", judicial_information_complete_mensaje);

            List<Listas> tabla_listas_restrictivas = new List<Listas>();
            List<Listas> listas_la_ft = new List<Listas>();
            List<Listas> tabla_listas_la_lf_admin = new List<Listas>();
            List<Listas> tabla_lista_sanciones_administrativas = new List<Listas>();
            List<Listas> tabla_lista_afectacion_financiera = new List<Listas>();
            List<Listas> tabla_listas_peps = new List<Listas>();
            List<Listas> tabla_listas_informativa_peps = new List<Listas>();
            foreach (var item in dataIndi.Listas)
            {
                switch (item.IdGrupoLista)
                {
                    case 1:
                        tabla_listas_restrictivas.Add(item);
                        break;
                    case 2:
                        listas_la_ft.Add(item);
                        break;
                    case 3:
                        tabla_listas_la_lf_admin.Add(item);
                        break;
                    case 4:
                        tabla_lista_sanciones_administrativas.Add(item);
                        break;
                    case 5:
                        tabla_lista_afectacion_financiera.Add(item);
                        break;
                    case 6:
                        tabla_listas_peps.Add(item);
                        break;
                    case 7:
                        tabla_listas_informativa_peps.Add(item);
                        break;
                }
            }

            var tabla_listas_la_lf_admin_reporte = reporteCompletoGetRowTables();
            if (tabla_listas_la_lf_admin != null && !tabla_listas_la_lf_admin.Equals("[]"))
            {
                foreach (var jsonObject in tabla_listas_la_lf_admin)
                {
                    var row = tabla_listas_la_lf_admin_reporte.NewRow();
                    row["Prioridad"] = jsonObject.Prioridad?.ToString().Trim(); //jsonObject["Prioridad"].ToString().Trim();
                    row["Tipo_documento"] = jsonObject.TipoDocumento?.ToString().Trim(); // jsonObject["TipoDocumento"].ToString().Trim();
                    row["Documento"] = jsonObject.DocumentoIdentidad?.ToString().Trim(); // jsonObject["DocumentoIdentidad"].ToString().Trim();
                    row["Nombre"] = jsonObject.NombreCompleto?.ToString().Trim(); // jsonObject["NombreCompleto"].ToString().Trim();
                    row["Nombre_lista"] = jsonObject.NombreTipoLista?.ToString().Trim(); // jsonObject["NombreTipoLista"].ToString().Trim();
                    row["Cargo"] = jsonObject.Delito?.ToString().Trim(); // jsonObject["Delito"].ToString().Trim();
                    row["Zona"] = jsonObject.Zona?.ToString().Trim(); // jsonObject["Zona"].ToString().Trim();
                    if (jsonObject.OtraInformacion?.ToString().Trim().Length > 300)
                        jsonObject.OtraInformacion = jsonObject.OtraInformacion.ToString().Trim().Substring(0, 300) + "(Ver mas con el numero de radicado)";
                    row["otra_informacion"] = jsonObject.OtraInformacion?.ToString().Trim();
                    tabla_listas_la_lf_admin_reporte.Rows.Add(row);
                }
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "listas_la_ft_admin",
                Value = tabla_listas_la_lf_admin_reporte
            });

            var listas_la_ft_reporte = reporteCompletoGetRowTables();
            if (listas_la_ft != null && !listas_la_ft.Equals("[]"))
            {
                foreach (var jsonObject in listas_la_ft)
                {
                    var row = listas_la_ft_reporte.NewRow();
                    row["Prioridad"] = jsonObject.Prioridad?.ToString().Trim(); //jsonObject["Prioridad"].ToString().Trim();
                    row["Tipo_documento"] = jsonObject.TipoDocumento?.ToString().Trim(); // jsonObject["TipoDocumento"].ToString().Trim();
                    row["Documento"] = jsonObject.DocumentoIdentidad?.ToString().Trim(); // jsonObject["DocumentoIdentidad"].ToString().Trim();
                    row["Nombre"] = jsonObject.NombreCompleto?.ToString().Trim(); // jsonObject["NombreCompleto"].ToString().Trim();
                    row["Nombre_lista"] = jsonObject.NombreTipoLista?.ToString().Trim(); // jsonObject["NombreTipoLista"].ToString().Trim();
                    row["Cargo"] = jsonObject.Delito?.ToString().Trim(); // jsonObject["Delito"].ToString().Trim();
                    row["Zona"] = jsonObject.Zona?.ToString().Trim(); // jsonObject["Zona"].ToString().Trim();
                    if (jsonObject.OtraInformacion?.ToString().Trim().Length > 300)
                        jsonObject.OtraInformacion = jsonObject.OtraInformacion.ToString().Trim().Substring(0, 300) + "(Ver mas con el numero de radicado)";
                    row["otra_informacion"] = jsonObject.OtraInformacion?.ToString().Trim();
                    listas_la_ft_reporte.Rows.Add(row);
                }
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "listas_la_ft",
                Value = listas_la_ft_reporte
            });

            var tabla_lista_afectacion_financiera_reporte = reporteCompletoGetRowTables();
            if (tabla_lista_afectacion_financiera_reporte != null && !tabla_lista_afectacion_financiera_reporte.Equals("[]"))
            {
                foreach (var jsonObject in tabla_lista_afectacion_financiera)
                {
                    var row = tabla_lista_afectacion_financiera_reporte.NewRow();
                    row["Prioridad"] = jsonObject.Prioridad?.ToString().Trim(); //jsonObject["Prioridad"].ToString().Trim();
                    row["Tipo_documento"] = jsonObject.TipoDocumento?.ToString().Trim(); // jsonObject["TipoDocumento"].ToString().Trim();
                    row["Documento"] = jsonObject.DocumentoIdentidad?.ToString().Trim(); // jsonObject["DocumentoIdentidad"].ToString().Trim();
                    row["Nombre"] = jsonObject.NombreCompleto?.ToString().Trim(); // jsonObject["NombreCompleto"].ToString().Trim();
                    row["Nombre_lista"] = jsonObject.NombreTipoLista?.ToString().Trim(); // jsonObject["NombreTipoLista"].ToString().Trim();
                    row["Cargo"] = jsonObject.Delito?.ToString().Trim(); // jsonObject["Delito"].ToString().Trim();
                    row["Zona"] = jsonObject.Zona?.ToString().Trim(); // jsonObject["Zona"].ToString().Trim();
                    if (jsonObject.OtraInformacion?.ToString().Trim().Length > 300)
                        jsonObject.OtraInformacion = jsonObject.OtraInformacion.ToString().Trim().Substring(0, 300) + "(Ver mas con el numero de radicado)";
                    row["otra_informacion"] = jsonObject.OtraInformacion?.ToString().Trim();
                    tabla_lista_afectacion_financiera_reporte.Rows.Add(row);
                }
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "listas_afectacion_financiera",
                Value = tabla_lista_afectacion_financiera_reporte
            });

            var tabla_lista_sanciones_administrativas_reporte = reporteCompletoGetRowTables();
            if (tabla_lista_sanciones_administrativas != null && !tabla_lista_sanciones_administrativas.Equals("[]"))
            {
                foreach (var jsonObject in tabla_lista_sanciones_administrativas)
                {
                    var row = tabla_lista_sanciones_administrativas_reporte.NewRow();
                    row["Prioridad"] = jsonObject.Prioridad?.ToString().Trim(); //jsonObject["Prioridad"].ToString().Trim();
                    row["Tipo_documento"] = jsonObject.TipoDocumento?.ToString().Trim(); // jsonObject["TipoDocumento"].ToString().Trim();
                    row["Documento"] = jsonObject.DocumentoIdentidad?.ToString().Trim(); // jsonObject["DocumentoIdentidad"].ToString().Trim();
                    row["Nombre"] = jsonObject.NombreCompleto?.ToString().Trim(); // jsonObject["NombreCompleto"].ToString().Trim();
                    row["Nombre_lista"] = jsonObject.NombreTipoLista?.ToString().Trim(); // jsonObject["NombreTipoLista"].ToString().Trim();
                    row["Cargo"] = jsonObject.Delito?.ToString().Trim(); // jsonObject["Delito"].ToString().Trim();
                    row["Zona"] = jsonObject.Zona?.ToString().Trim(); // jsonObject["Zona"].ToString().Trim();
                    if (jsonObject.OtraInformacion?.ToString().Trim().Length > 300)
                        jsonObject.OtraInformacion = jsonObject.OtraInformacion.ToString().Trim().Substring(0, 300) + "(Ver mas con el numero de radicado)";
                    row["otra_informacion"] = jsonObject.OtraInformacion?.ToString().Trim();
                    tabla_lista_sanciones_administrativas_reporte.Rows.Add(row);
                }
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "listas_sanciones_administrativas",
                Value = tabla_lista_sanciones_administrativas_reporte
            });

            var tabla_listas_peps_reporte = reporteCompletoGetRowTables();
            if (tabla_listas_peps != null && !tabla_listas_peps.Equals("[]"))
            {
                foreach (var jsonObject in tabla_listas_peps)
                {
                    var row = tabla_listas_peps_reporte.NewRow();
                    row["Prioridad"] = jsonObject.Prioridad?.ToString().Trim(); //jsonObject["Prioridad"].ToString().Trim();
                    row["Tipo_documento"] = jsonObject.TipoDocumento?.ToString().Trim(); // jsonObject["TipoDocumento"].ToString().Trim();
                    row["Documento"] = jsonObject.DocumentoIdentidad?.ToString().Trim(); // jsonObject["DocumentoIdentidad"].ToString().Trim();
                    row["Nombre"] = jsonObject.NombreCompleto?.ToString().Trim(); // jsonObject["NombreCompleto"].ToString().Trim();
                    row["Nombre_lista"] = jsonObject.NombreTipoLista?.ToString().Trim(); // jsonObject["NombreTipoLista"].ToString().Trim();
                    row["Cargo"] = jsonObject.Delito?.ToString().Trim(); // jsonObject["Delito"].ToString().Trim();
                    row["Zona"] = jsonObject.Zona?.ToString().Trim(); // jsonObject["Zona"].ToString().Trim();
                    if (jsonObject.OtraInformacion?.ToString().Trim().Length > 300)
                        jsonObject.OtraInformacion = jsonObject.OtraInformacion.ToString().Trim().Substring(0, 300) + "(Ver mas con el numero de radicado)";
                    row["otra_informacion"] = jsonObject.OtraInformacion?.ToString().Trim();
                    tabla_listas_peps_reporte.Rows.Add(row);
                }
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("lista_peps", tabla_listas_peps_reporte));

            var tabla_listas_informativa_peps_reporte = reporteCompletoGetRowTables();
            if (tabla_listas_informativa_peps != null && !tabla_listas_informativa_peps.Equals("[]"))
            {
                foreach (var jsonObject in tabla_listas_informativa_peps)
                {
                    var row = tabla_listas_informativa_peps_reporte.NewRow();
                    row["Prioridad"] = jsonObject.Prioridad?.ToString().Trim(); //jsonObject["Prioridad"].ToString().Trim();
                    row["Tipo_documento"] = jsonObject.TipoDocumento?.ToString().Trim(); // jsonObject["TipoDocumento"].ToString().Trim();
                    row["Documento"] = jsonObject.DocumentoIdentidad?.ToString().Trim(); // jsonObject["DocumentoIdentidad"].ToString().Trim();
                    row["Nombre"] = jsonObject.NombreCompleto?.ToString().Trim(); // jsonObject["NombreCompleto"].ToString().Trim();
                    row["Nombre_lista"] = jsonObject.NombreTipoLista?.ToString().Trim(); // jsonObject["NombreTipoLista"].ToString().Trim();
                    row["Cargo"] = jsonObject.Delito?.ToString().Trim(); // jsonObject["Delito"].ToString().Trim();
                    row["Zona"] = jsonObject.Zona?.ToString().Trim(); // jsonObject["Zona"].ToString().Trim();
                    if (jsonObject.OtraInformacion?.ToString().Trim().Length > 300)
                        jsonObject.OtraInformacion = jsonObject.OtraInformacion.ToString().Trim().Substring(0, 300) + "(Ver mas con el numero de radicado)";
                    row["otra_informacion"] = jsonObject.OtraInformacion?.ToString().Trim();
                    tabla_listas_informativa_peps_reporte.Rows.Add(row);
                }
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "listas_informativas_peps",
                Value = tabla_listas_informativa_peps_reporte
            });

            //var tabla_listas_ofac = reporteCompletoGetRowTables();
            //var tabla_listas_seguridad_onu = reporteCompletoGetRowTables();
            //var tabla_listas_activos_financieros = reporteCompletoGetRowTables();
            //var tabla_listas_terrorista_policial_judicial = reporteCompletoGetRowTables();
            //var tabla_listas_terrorist_organization = reporteCompletoGetRowTables();
            //var tabla_listas_elimanos_terrorista_eu = reporteCompletoGetRowTables();

            List<Listas> tabla_listas_ofac = new List<Listas>();
            List<Listas> tabla_listas_seguridad_onu = new List<Listas>();
            List<Listas> tabla_listas_activos_financieros = new List<Listas>();
            List<Listas> tabla_listas_terrorista_policial_judicial = new List<Listas>();
            List<Listas> tabla_listas_terrorist_organization = new List<Listas>();
            List<Listas> tabla_listas_elimanos_terrorista_eu = new List<Listas>();

            if (tabla_listas_restrictivas != null && !tabla_listas_restrictivas.Equals("[]"))
            {
                foreach (var lista in tabla_listas_restrictivas)
                {
                    switch (lista.IdTipoLista) 
                    {
                    case 4:
                            tabla_listas_ofac.Add(lista);
                        break;
                    case 8:
                            tabla_listas_seguridad_onu.Add(lista);
                        break;
                    case 158:
                            tabla_listas_activos_financieros.Add(lista);
                        break;
                    case 159:
                            tabla_listas_terrorista_policial_judicial.Add(lista);
                        break;
                    case 160:
                            tabla_listas_terrorist_organization.Add(lista);
                        break;
                    case 161:
                            tabla_listas_elimanos_terrorista_eu.Add(lista);
                        break;

                    }

                }
            }

            var tabla_listas_ofac_reporte = reporteCompletoGetRowTables();
            if (tabla_listas_ofac != null && !tabla_listas_ofac.Equals("[]"))
            {
                foreach (var jsonObject in tabla_listas_ofac)
                {
                    var row = tabla_listas_ofac_reporte.NewRow();
                    row["Prioridad"] = jsonObject.Prioridad?.ToString().Trim(); //jsonObject["Prioridad"].ToString().Trim();
                    row["Tipo_documento"] = jsonObject.TipoDocumento?.ToString().Trim(); // jsonObject["TipoDocumento"].ToString().Trim();
                    row["Documento"] = jsonObject.DocumentoIdentidad?.ToString().Trim(); // jsonObject["DocumentoIdentidad"].ToString().Trim();
                    row["Nombre"] = jsonObject.NombreCompleto?.ToString().Trim(); // jsonObject["NombreCompleto"].ToString().Trim();
                    row["Nombre_lista"] = jsonObject.NombreTipoLista?.ToString().Trim(); // jsonObject["NombreTipoLista"].ToString().Trim();
                    row["Cargo"] = jsonObject.Delito?.ToString().Trim(); // jsonObject["Delito"].ToString().Trim();
                    row["Zona"] = jsonObject.Zona?.ToString().Trim(); // jsonObject["Zona"].ToString().Trim();
                    if (jsonObject.OtraInformacion?.ToString().Trim().Length > 300)
                        jsonObject.OtraInformacion = jsonObject.OtraInformacion.ToString().Trim().Substring(0, 300) + "(Ver mas con el numero de radicado)";
                    row["otra_informacion"] = jsonObject.OtraInformacion?.ToString().Trim();
                    tabla_listas_ofac_reporte.Rows.Add(row);
                }
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "lista_ofac",
                Value = tabla_listas_ofac_reporte
            });

            var tabla_listas_seguridad_onu_reporte = reporteCompletoGetRowTables();
            if (tabla_listas_seguridad_onu != null && !tabla_listas_seguridad_onu.Equals("[]"))
            {
                foreach (var jsonObject in tabla_listas_seguridad_onu)
                {
                    var row = tabla_listas_seguridad_onu_reporte.NewRow();
                    row["Prioridad"] = jsonObject.Prioridad?.ToString().Trim(); //jsonObject["Prioridad"].ToString().Trim();
                    row["Tipo_documento"] = jsonObject.TipoDocumento?.ToString().Trim(); // jsonObject["TipoDocumento"].ToString().Trim();
                    row["Documento"] = jsonObject.DocumentoIdentidad?.ToString().Trim(); // jsonObject["DocumentoIdentidad"].ToString().Trim();
                    row["Nombre"] = jsonObject.NombreCompleto?.ToString().Trim(); // jsonObject["NombreCompleto"].ToString().Trim();
                    row["Nombre_lista"] = jsonObject.NombreTipoLista?.ToString().Trim(); // jsonObject["NombreTipoLista"].ToString().Trim();
                    row["Cargo"] = jsonObject.Delito?.ToString().Trim(); // jsonObject["Delito"].ToString().Trim();
                    row["Zona"] = jsonObject.Zona?.ToString().Trim(); // jsonObject["Zona"].ToString().Trim();
                    if (jsonObject.OtraInformacion?.ToString().Trim().Length > 300)
                        jsonObject.OtraInformacion = jsonObject.OtraInformacion.ToString().Trim().Substring(0, 300) + "(Ver mas con el numero de radicado)";
                    row["otra_informacion"] = jsonObject.OtraInformacion?.ToString().Trim();
                    tabla_listas_seguridad_onu_reporte.Rows.Add(row);
                }
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "lista_seguridad_onu",
                Value = tabla_listas_seguridad_onu_reporte
            });

            var tabla_listas_activos_financieros_reporte = reporteCompletoGetRowTables();
            if (tabla_listas_activos_financieros != null && !tabla_listas_activos_financieros.Equals("[]"))
            {
                foreach (var jsonObject in tabla_listas_activos_financieros)
                {
                    var row = tabla_listas_activos_financieros_reporte.NewRow();
                    row["Prioridad"] = jsonObject.Prioridad?.ToString().Trim(); //jsonObject["Prioridad"].ToString().Trim();
                    row["Tipo_documento"] = jsonObject.TipoDocumento?.ToString().Trim(); // jsonObject["TipoDocumento"].ToString().Trim();
                    row["Documento"] = jsonObject.DocumentoIdentidad?.ToString().Trim(); // jsonObject["DocumentoIdentidad"].ToString().Trim();
                    row["Nombre"] = jsonObject.NombreCompleto?.ToString().Trim(); // jsonObject["NombreCompleto"].ToString().Trim();
                    row["Nombre_lista"] = jsonObject.NombreTipoLista?.ToString().Trim(); // jsonObject["NombreTipoLista"].ToString().Trim();
                    row["Cargo"] = jsonObject.Delito?.ToString().Trim(); // jsonObject["Delito"].ToString().Trim();
                    row["Zona"] = jsonObject.Zona?.ToString().Trim(); // jsonObject["Zona"].ToString().Trim();
                    if (jsonObject.OtraInformacion?.ToString().Trim().Length > 300)
                        jsonObject.OtraInformacion = jsonObject.OtraInformacion.ToString().Trim().Substring(0, 300) + "(Ver mas con el numero de radicado)";
                    row["otra_informacion"] = jsonObject.OtraInformacion?.ToString().Trim();
                    tabla_listas_activos_financieros_reporte.Rows.Add(row);
                }
            }
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "lista_terrorista_activo_financiero",
                Value = tabla_listas_activos_financieros_reporte
            });

            var tabla_listas_terrorista_policial_judicial_reporte = reporteCompletoGetRowTables();
            if (tabla_listas_terrorista_policial_judicial != null && !tabla_listas_terrorista_policial_judicial.Equals("[]"))
            {
                foreach (var jsonObject in tabla_listas_terrorista_policial_judicial)
                {
                    var row = tabla_listas_terrorista_policial_judicial_reporte.NewRow();
                    row["Prioridad"] = jsonObject.Prioridad?.ToString().Trim(); //jsonObject["Prioridad"].ToString().Trim();
                    row["Tipo_documento"] = jsonObject.TipoDocumento?.ToString().Trim(); // jsonObject["TipoDocumento"].ToString().Trim();
                    row["Documento"] = jsonObject.DocumentoIdentidad?.ToString().Trim(); // jsonObject["DocumentoIdentidad"].ToString().Trim();
                    row["Nombre"] = jsonObject.NombreCompleto?.ToString().Trim(); // jsonObject["NombreCompleto"].ToString().Trim();
                    row["Nombre_lista"] = jsonObject.NombreTipoLista?.ToString().Trim(); // jsonObject["NombreTipoLista"].ToString().Trim();
                    row["Cargo"] = jsonObject.Delito?.ToString().Trim(); // jsonObject["Delito"].ToString().Trim();
                    row["Zona"] = jsonObject.Zona?.ToString().Trim(); // jsonObject["Zona"].ToString().Trim();
                    if (jsonObject.OtraInformacion?.ToString().Trim().Length > 300)
                        jsonObject.OtraInformacion = jsonObject.OtraInformacion.ToString().Trim().Substring(0, 300) + "(Ver mas con el numero de radicado)";
                    row["otra_informacion"] = jsonObject.OtraInformacion?.ToString().Trim();
                    tabla_listas_terrorista_policial_judicial_reporte.Rows.Add(row);
                }
            }
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "lista_terrorista_policial_judicial",
                Value = tabla_listas_terrorista_policial_judicial_reporte
            });

            var tabla_listas_terrorist_organization_reporte = reporteCompletoGetRowTables();
            if (tabla_listas_terrorist_organization != null && !tabla_listas_terrorist_organization.Equals("[]"))
            {
                foreach (var jsonObject in tabla_listas_terrorist_organization)
                {
                    var row = tabla_listas_terrorista_policial_judicial_reporte.NewRow();
                    row["Prioridad"] = jsonObject.Prioridad?.ToString().Trim(); //jsonObject["Prioridad"].ToString().Trim();
                    row["Tipo_documento"] = jsonObject.TipoDocumento?.ToString().Trim(); // jsonObject["TipoDocumento"].ToString().Trim();
                    row["Documento"] = jsonObject.DocumentoIdentidad?.ToString().Trim(); // jsonObject["DocumentoIdentidad"].ToString().Trim();
                    row["Nombre"] = jsonObject.NombreCompleto?.ToString().Trim(); // jsonObject["NombreCompleto"].ToString().Trim();
                    row["Nombre_lista"] = jsonObject.NombreTipoLista?.ToString().Trim(); // jsonObject["NombreTipoLista"].ToString().Trim();
                    row["Cargo"] = jsonObject.Delito?.ToString().Trim(); // jsonObject["Delito"].ToString().Trim();
                    row["Zona"] = jsonObject.Zona?.ToString().Trim(); // jsonObject["Zona"].ToString().Trim();
                    if (jsonObject.OtraInformacion?.ToString().Trim().Length > 300)
                        jsonObject.OtraInformacion = jsonObject.OtraInformacion.ToString().Trim().Substring(0, 300) + "(Ver mas con el numero de radicado)";
                    row["otra_informacion"] = jsonObject.OtraInformacion?.ToString().Trim();
                    tabla_listas_terrorist_organization_reporte.Rows.Add(row);
                }
            }
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "lista_terrorist_organization",
                Value = tabla_listas_terrorist_organization_reporte
            });

            var tabla_listas_elimanos_terrorista_eu_reporte = reporteCompletoGetRowTables();
            if (tabla_listas_elimanos_terrorista_eu != null && !tabla_listas_elimanos_terrorista_eu.Equals("[]"))
            {
                foreach (var jsonObject in tabla_listas_elimanos_terrorista_eu)
                {
                    var row = tabla_listas_elimanos_terrorista_eu_reporte.NewRow();
                    row["Prioridad"] = jsonObject.Prioridad?.ToString().Trim(); //jsonObject["Prioridad"].ToString().Trim();
                    row["Tipo_documento"] = jsonObject.TipoDocumento?.ToString().Trim(); // jsonObject["TipoDocumento"].ToString().Trim();
                    row["Documento"] = jsonObject.DocumentoIdentidad?.ToString().Trim(); // jsonObject["DocumentoIdentidad"].ToString().Trim();
                    row["Nombre"] = jsonObject.NombreCompleto?.ToString().Trim(); // jsonObject["NombreCompleto"].ToString().Trim();
                    row["Nombre_lista"] = jsonObject.NombreTipoLista?.ToString().Trim(); // jsonObject["NombreTipoLista"].ToString().Trim();
                    row["Cargo"] = jsonObject.Delito?.ToString().Trim(); // jsonObject["Delito"].ToString().Trim();
                    row["Zona"] = jsonObject.Zona?.ToString().Trim(); // jsonObject["Zona"].ToString().Trim();
                    if (jsonObject.OtraInformacion?.ToString().Trim().Length > 300)
                        jsonObject.OtraInformacion = jsonObject.OtraInformacion.ToString().Trim().Substring(0, 300) + "(Ver mas con el numero de radicado)";
                    row["otra_informacion"] = jsonObject.OtraInformacion?.ToString().Trim();
                    tabla_listas_elimanos_terrorista_eu_reporte.Rows.Add(row);
                }
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "listas_eliminados_terroristas_eu",
                Value = tabla_listas_elimanos_terrorista_eu_reporte
            });

            var tabla_listas_propias_reporte = new DataTable();
            tabla_listas_propias_reporte.Columns.Add(new DataColumn("Tipo_documento"));
            tabla_listas_propias_reporte.Columns.Add(new DataColumn("Documento"));
            tabla_listas_propias_reporte.Columns.Add(new DataColumn("Nombre"));
            tabla_listas_propias_reporte.Columns.Add(new DataColumn("Nombre_Tipo_Lista"));
            tabla_listas_propias_reporte.Columns.Add(new DataColumn("Tipo_persona"));
            tabla_listas_propias_reporte.Columns.Add(new DataColumn("Cargo"));
            tabla_listas_propias_reporte.Columns.Add(new DataColumn("Alias"));
            if (dataIndi.Listas_propias != null && !dataIndi.Listas_propias.Equals("[]"))
            {
                foreach (var jsonObject in dataIndi.Listas_propias)
                {
                    var row = tabla_listas_propias_reporte.NewRow();
                    row["Tipo_documento"] = jsonObject.TipoDocumento?.ToString().Trim(); // jsonObject["TipoDocumento"].ToString().Trim();
                    row["Documento"] = jsonObject.DocumentoIdentidad?.ToString().Trim(); // jsonObject["DocumentoIdentidad"].ToString().Trim();
                    row["Nombre"] = jsonObject.NombreCompleto?.ToString().Trim(); // jsonObject["NombreCompleto"].ToString().Trim();
                    row["Nombre_Tipo_Lista"] = jsonObject.NombreTipoLista?.ToString().Trim(); // jsonObject["NombreTipoLista"].ToString().Trim();
                    row["Cargo"] = jsonObject.Delito?.ToString().Trim(); // jsonObject["Delito"].ToString().Trim();
                    row["Alias"] = jsonObject.Alias?.ToString().Trim(); // jsonObject["Zona"].ToString().Trim();
                    tabla_listas_propias_reporte.Rows.Add(row);
                }
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "listas_propias",
                Value = tabla_listas_propias_reporte
            });
            /* Servicios adicionales -----------------------------------------------*/
            /* ecuador -----------------------------------------------*/
            var tabla_criminal_records_ecuador = new DataTable();
            tabla_criminal_records_ecuador.Columns.Add(new DataColumn("criminalRecord"));
            tabla_criminal_records_ecuador.Columns.Add(new DataColumn("documentType"));
            tabla_criminal_records_ecuador.Columns.Add(new DataColumn("document"));
            tabla_criminal_records_ecuador.Columns.Add(new DataColumn("name"));

            //if (dataIndi["CriminalRecordEcuador"] != null && !dataIndi["CriminalRecordEcuador"].ToString().Equals("[]"))
            //{
            //    JsonData jsonObject = JsonMapper.ToObject(dataIndi["CriminalRecordEcuador"].ToJson().ToString());

            //    var row = tabla_criminal_records_ecuador.NewRow();
            //    row["criminalRecord"] = jsonObject["criminalRecord"] != null ? jsonObject["criminalRecord"].ToString() : "";
            //    row["documentType"] = jsonObject["documentType"] != null ? jsonObject["documentType"].ToString() : "";
            //    row["document"] = jsonObject["document"] != null ? jsonObject["document"].ToString() : "";
            //    row["name"] = jsonObject["name"] != null ? jsonObject["name"].ToString() : "";

            //    tabla_criminal_records_ecuador.Rows.Add(row);

            //}
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("criminal_records_ecuador", tabla_criminal_records_ecuador));

            var tabla_judicial_information_ecuador = new DataTable();
            tabla_judicial_information_ecuador.Columns.Add(new DataColumn("date"));
            tabla_judicial_information_ecuador.Columns.Add(new DataColumn("nProccess"));
            tabla_judicial_information_ecuador.Columns.Add(new DataColumn("action"));

            //if (dataIndi["JudicialInformationEcuador"] != null && !dataIndi["JudicialInformationEcuador"].ToJson().ToString().Equals("[]"))
            //{
            //    JsonData jsonObjectJudicialInformationEcuador = JsonMapper.ToObject(dataIndi["JudicialInformationEcuador"].ToString());



            //    foreach (JsonData jsonObject in jsonObjectJudicialInformationEcuador)
            //    {

            //        var row = tabla_judicial_information_ecuador.NewRow();
            //        row["date"] = jsonObject["date"] != null ? jsonObject["date"].ToString() : "";
            //        row["nProccess"] = jsonObject["nProccess"] != null ? jsonObject["nProccess"].ToString() : "";
            //        row["action"] = jsonObject["action"] != null ? jsonObject["action"].ToString() : "";

            //        tabla_judicial_information_ecuador.Rows.Add(row);

            //    }
            //}
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("judicial_information_ecuador", tabla_judicial_information_ecuador));

            var tabla_judicial_information_complete_ecuador = new DataTable();
            tabla_judicial_information_complete_ecuador.Columns.Add(new DataColumn("id"));
            tabla_judicial_information_complete_ecuador.Columns.Add(new DataColumn("fechaIngreso"));
            tabla_judicial_information_complete_ecuador.Columns.Add(new DataColumn("idJuicio"));
            tabla_judicial_information_complete_ecuador.Columns.Add(new DataColumn("nombreDelito"));
            tabla_judicial_information_complete_ecuador.Columns.Add(new DataColumn("link"));
            //if (dataIndi["JudicialInformationCompleteEcuador"] != null && !dataIndi["JudicialInformationCompleteEcuador"].ToJson().ToString().Equals("[]"))
            //{
            //    JsonData jsonObjectJudicialInformationCompleteEcuador = JsonMapper.ToObject(dataIndi["JudicialInformationCompleteEcuador"].ToString());



            //    foreach (JsonData jsonObject in jsonObjectJudicialInformationCompleteEcuador)
            //    {
            //        string date = jsonObject["fechaIngreso"] != null ? jsonObject["fechaIngreso"].ToString() : "";
            //        if (!string.IsNullOrEmpty(date))
            //            date = date.Split(new char[] { 'T' })[0];
            //        var row = tabla_judicial_information_complete_ecuador.NewRow();
            //        row["id"] = jsonObject["id"] != null ? jsonObject["id"].ToString() : "";
            //        row["fechaIngreso"] = date;
            //        row["idJuicio"] = jsonObject["idJuicio"] != null ? jsonObject["idJuicio"].ToString() : "";
            //        row["nombreDelito"] = jsonObject["nombreDelito"] != null ? jsonObject["nombreDelito"].ToString() : "";
            //        row["link"] = jsonObject["link"] != null ? jsonObject["link"].ToString() : "";

            //        tabla_judicial_information_complete_ecuador.Rows.Add(row);

            //    }
            //}
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("judicial_information_complete_ecuador", tabla_judicial_information_complete_ecuador));
            /* ecuador -----------------------------------------------*/
            /* peru -----------------------------------------------*/
            var tabla_sunat = new DataTable();
            tabla_sunat.Columns.Add(new DataColumn("no_ruc"));
            tabla_sunat.Columns.Add(new DataColumn("tipo_cont"));
            tabla_sunat.Columns.Add(new DataColumn("nombre_comercial"));
            tabla_sunat.Columns.Add(new DataColumn("fecha_inscripcion"));
            tabla_sunat.Columns.Add(new DataColumn("fecha_inicio_actividades"));
            tabla_sunat.Columns.Add(new DataColumn("estado"));
            tabla_sunat.Columns.Add(new DataColumn("condicion"));
            tabla_sunat.Columns.Add(new DataColumn("domicilio"));
            tabla_sunat.Columns.Add(new DataColumn("sistema_emicion"));
            tabla_sunat.Columns.Add(new DataColumn("actividad_comercio_exterior"));
            tabla_sunat.Columns.Add(new DataColumn("sistema_contabilidad"));
            tabla_sunat.Columns.Add(new DataColumn("actividades_economicas"));
            tabla_sunat.Columns.Add(new DataColumn("comprobante_pago"));
            tabla_sunat.Columns.Add(new DataColumn("sistema_emision_electronica"));
            tabla_sunat.Columns.Add(new DataColumn("emision_electronica_desde"));
            tabla_sunat.Columns.Add(new DataColumn("comprobantes_electronicos"));
            tabla_sunat.Columns.Add(new DataColumn("afiliado_ple"));
            tabla_sunat.Columns.Add(new DataColumn("padrones"));

            //if (dataIndi["sunat"] != null && !dataIndi["sunat"].ToJson().ToString().Equals("[]"))
            //{
            //    JsonData jsonObject = JsonMapper.ToObject(dataIndi["sunat"].ToJson().ToString());

            //    var row = tabla_sunat.NewRow();
            //    row["no_ruc"] = jsonObject["no_ruc"] != null ? jsonObject["no_ruc"].ToString() : "";
            //    row["tipo_cont"] = jsonObject["tipo_cont"] != null ? jsonObject["tipo_cont"].ToString() : "";
            //    row["nombre_comercial"] = jsonObject["nombre_comercial"] != null ? jsonObject["nombre_comercial"].ToString() : "";
            //    row["fecha_inscripcion"] = jsonObject["fecha_inscripcion"] != null ? jsonObject["fecha_inscripcion"].ToString() : "";
            //    row["fecha_inicio_actividades"] = jsonObject["fecha_inicio_actividades"] != null ? jsonObject["fecha_inicio_actividades"].ToString() : "";
            //    row["estado"] = jsonObject["estado"] != null ? jsonObject["estado"].ToString() : "";
            //    row["condicion"] = jsonObject["condicion"] != null ? jsonObject["condicion"].ToString() : "";
            //    row["domicilio"] = jsonObject["domicilio"] != null ? jsonObject["domicilio"].ToString() : "";
            //    row["sistema_emicion"] = jsonObject["sistema_emicion"] != null ? jsonObject["sistema_emicion"].ToString() : "";
            //    row["actividad_comercio_exterior"] = jsonObject["actividad_comercio_exterior"] != null ? jsonObject["actividad_comercio_exterior"].ToString() : "";
            //    row["sistema_contabilidad"] = jsonObject["sistema_contabilidad"] != null ? jsonObject["sistema_contabilidad"].ToString() : "";
            //    row["actividades_economicas"] = jsonObject["actividades_economicas"] != null ? jsonObject["actividades_economicas"].ToString() : "";
            //    row["comprobante_pago"] = jsonObject["comprobante_pago"] != null ? jsonObject["comprobante_pago"].ToString() : "";
            //    row["sistema_emision_electronica"] = jsonObject["sistema_emision_electronica"] != null ? jsonObject["sistema_emision_electronica"].ToString() : "";
            //    row["emision_electronica_desde"] = jsonObject["emision_electronica_desde"] != null ? jsonObject["emision_electronica_desde"].ToString() : "";
            //    row["comprobantes_electronicos"] = jsonObject["comprobantes_electronicos"] != null ? jsonObject["comprobantes_electronicos"].ToString() : "";
            //    row["afiliado_ple"] = jsonObject["afiliado_ple"] != null ? jsonObject["afiliado_ple"].ToString() : "";
            //    row["padrones"] = jsonObject["padrones"] != null ? jsonObject["padrones"].ToString() : "";

            //    tabla_sunat.Rows.Add(row);
            //}
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("sunat", tabla_sunat));
            /* peru -----------------------------------------------*/

            var tabla_TTP = new DataTable();
            tabla_TTP.Columns.Add(new DataColumn("Name"));
            tabla_TTP.Columns.Add(new DataColumn("IdentificationType"));
            tabla_TTP.Columns.Add(new DataColumn("Identification"));
            tabla_TTP.Columns.Add(new DataColumn("Correo"));
            tabla_TTP.Columns.Add(new DataColumn("Telefono"));
            tabla_TTP.Columns.Add(new DataColumn("Status"));
            //if (dataIndi["PPT"] != null && !dataIndi["PPT"].ToString().Equals("[]"))
            //{
            //    JsonData jsonObject = dataIndi["PPT"];
            //    if (jsonObject["Name"] != null && jsonObject["Identification"] != null)
            //    {
            //        var row = tabla_TTP.NewRow();
            //        row["Name"] = jsonObject["Name"].ToString();
            //        row["IdentificationType"] = jsonObject["IdentificationType"].ToString();
            //        row["Identification"] = jsonObject["Identification"].ToString();
            //        row["Correo"] = jsonObject["Correo"].ToString();
            //        row["Telefono"] = jsonObject["Telefono"].ToString();
            //        row["Status"] = jsonObject["Status"].ToString();
            //        tabla_TTP.Rows.Add(row);
            //    }

            //}
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("PPT", tabla_TTP));

            var tabla_EPS = new DataTable();
            tabla_EPS.Columns.Add(new DataColumn("State"));
            tabla_EPS.Columns.Add(new DataColumn("Entity"));
            tabla_EPS.Columns.Add(new DataColumn("Regime"));
            tabla_EPS.Columns.Add(new DataColumn("EffectiveDate"));
            tabla_EPS.Columns.Add(new DataColumn("EndDate"));
            tabla_EPS.Columns.Add(new DataColumn("AffiliateType"));
            //if (dataIndi["EPS"] != null && !dataIndi["EPS"].ToString().Equals("[]"))
            //{
            //    foreach (JsonData jsonObject in dataIndi["EPS"])
            //    {
            //        var row = tabla_EPS.NewRow();
            //        row["State"] = jsonObject["State"].ToString();
            //        row["Entity"] = jsonObject["Entity"].ToString();
            //        row["Regime"] = jsonObject["Regime"].ToString();
            //        row["EffectiveDate"] = jsonObject["EffectiveDate"].ToString();
            //        row["EndDate"] = jsonObject["EndDate"].ToString();
            //        row["AffiliateType"] = jsonObject["AffiliateType"].ToString();
            //        tabla_EPS.Rows.Add(row);
            //    }

            //}
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("EPS", tabla_EPS));

            var table_contaduria_bme = new DataTable();
            table_contaduria_bme.Columns.Add(new DataColumn("nombre_reportado"));
            table_contaduria_bme.Columns.Add(new DataColumn("numero_obligacion"));
            table_contaduria_bme.Columns.Add(new DataColumn("estado"));
            table_contaduria_bme.Columns.Add(new DataColumn("fecha_corte"));
            //if (dataIndi["contaduria_bme_table_new"] != null && !dataIndi["contaduria_bme_table_new"].ToJson().ToString().Equals("") && !dataIndi["contaduria_bme_table_new"].ToJson().ToString().Equals("[]"))
            //{
            //    foreach (JsonData jsonObject in dataIndi["contaduria_bme_table_new"])
            //    {
            //        var row = table_contaduria_bme.NewRow();
            //        row["nombre_reportado"] = jsonObject["name_reported"].ToString();
            //        row["numero_obligacion"] = jsonObject["no_obligation"].ToString();
            //        row["estado"] = jsonObject["state"].ToString();
            //        row["fecha_corte"] = jsonObject["date"].ToString();

            //        table_contaduria_bme.Rows.Add(row);
            //    }

            //}
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("contaduria_bme", table_contaduria_bme));

            var table_sub_procuraduria = new DataTable();
            table_sub_procuraduria.Columns.Add(new DataColumn("num_siri"));
            table_sub_procuraduria.Columns.Add(new DataColumn("data"));

            if (dataIndi.procuraduria != null && procuraduria_mensaje.Equals(""))
            {

                var TempObjectProcuraduria = dataIndi.procuraduria.Data;
                String html_contaduria = TempObjectProcuraduria.html_response != null ? TempObjectProcuraduria.html_response.ToString() : "";
                if (!html_contaduria.Equals("") && html_contaduria.Contains("Datos del ciudadano") && !html_contaduria.Contains("El ciudadano no presenta antecedentes"))
                {
                    try
                    {
                        html_contaduria = StringBetween(html_contaduria, "Datos del ciudadano", ".  &lt");
                        html_contaduria = html_contaduria.Replace("&lt;", "").Replace("/span&gt;", "").Replace("span&gt;", " ").Replace("/h2&gt;div class=\\&quot;datosConsultado\\&quot;&gt;", "").Trim();
                        html_contaduria += "<br>" + procuraduria_mensaje;
                        parameters[10] = new ReportParameter("mensaje_procuraduria", html_contaduria);
                    }
                    catch (Exception ex) { }
                }
                var TempjsonObjectProcuraduriaString = TempObjectProcuraduria.ToString();
                if (TempObjectProcuraduria != null && !TempjsonObjectProcuraduriaString.Equals(""))
                {
                    foreach (var jsonObjectProcuraduria in TempObjectProcuraduria.data)
                    {
                        var row = table_sub_procuraduria.NewRow();
                        var itemPocuraduria = TempObjectProcuraduria.data[0];
                        row["num_siri"] = itemPocuraduria.num_siri;
                        row["data"] = JsonConvert.SerializeObject(TempObjectProcuraduria.data); //TempObjectProcuraduria.data.ToString();
                        table_sub_procuraduria.Rows.Add(row);
                    }
                }
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "sub_procuraduria",
                Value = table_sub_procuraduria
            });

            var tabla_rethus = new DataTable();
            tabla_rethus.Columns.Add(new DataColumn("TipoIdentificacion"));
            tabla_rethus.Columns.Add(new DataColumn("NroIdentificacion"));
            tabla_rethus.Columns.Add(new DataColumn("PrimerNombre"));
            tabla_rethus.Columns.Add(new DataColumn("SegundoNombre"));
            tabla_rethus.Columns.Add(new DataColumn("PrimerApellido"));
            tabla_rethus.Columns.Add(new DataColumn("SegundoApellido"));
            tabla_rethus.Columns.Add(new DataColumn("EstadoIdentificacion"));
            //if (dataIndi["rethus"] != null && !dataIndi["rethus"].ToJson().ToString().Equals("") && !dataIndi["rethus"].ToJson().ToString().Equals("[]"))
            //{

            //    if (dataIndi["rethus"] != null && !dataIndi["rethus"].ToString().Equals("[]"))
            //    {
            //        foreach (JsonData jsonData in dataIndi["rethus"])
            //        {
            //            var rowRethus = tabla_rethus.NewRow();
            //            rowRethus["TipoIdentificacion"] = jsonData["TipoIdentificacion"]?.ToString();
            //            rowRethus["NroIdentificacion"] = jsonData["NroIdentificacion"]?.ToString();
            //            rowRethus["PrimerNombre"] = jsonData["PrimerNombre"]?.ToString();
            //            rowRethus["SegundoNombre"] = jsonData["SegundoNombre"]?.ToString();
            //            rowRethus["PrimerApellido"] = jsonData["PrimerApellido"]?.ToString();
            //            rowRethus["SegundoApellido"] = jsonData["SegundoApellido"]?.ToString();
            //            rowRethus["EstadoIdentificacion"] = jsonData["EstadoIdentificacion"]?.ToString();

            //            tabla_rethus.Rows.Add(rowRethus);
            //        }
            //    }
            //}
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("rethus", tabla_rethus));

            var tabla_inpec = new DataTable();
            tabla_inpec.Columns.Add(new DataColumn("Identificacion"));
            tabla_inpec.Columns.Add(new DataColumn("numeroUnicoInpec"));
            tabla_inpec.Columns.Add(new DataColumn("nombre"));
            tabla_inpec.Columns.Add(new DataColumn("genero"));
            tabla_inpec.Columns.Add(new DataColumn("estadoDeIngreso"));
            tabla_inpec.Columns.Add(new DataColumn("situacionJuridica"));
            tabla_inpec.Columns.Add(new DataColumn("establecimientoACargo"));


            //if (dataIndi["Inpec"] != null && !dataIndi["Inpec"].ToString().Equals(""))
            //{
            //    var rowInpec = tabla_inpec.NewRow();
            //    rowInpec["Identificacion"] = dataIndi["Inpec"]?["Identificacion"]?.ToString();
            //    rowInpec["numeroUnicoInpec"] = dataIndi["Inpec"]?["numeroUnicoInpec"]?.ToString();
            //    rowInpec["nombre"] = dataIndi["Inpec"]?["nombre"]?.ToString();
            //    rowInpec["genero"] = dataIndi["Inpec"]?["genero"]?.ToString();
            //    rowInpec["estadoDeIngreso"] = dataIndi["Inpec"]?["estadoDeIngreso"]?.ToString();
            //    rowInpec["situacionJuridica"] = dataIndi["Inpec"]?["situacionJuridica"]?.ToString();
            //    rowInpec["establecimientoACargo"] = dataIndi["Inpec"]?["establecimientoACargo"]?.ToString();

            //    tabla_inpec.Rows.Add(rowInpec);

            //}

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("Inpec", tabla_inpec));


            var tabla_MedidasCorrectivas = new DataTable();
            tabla_MedidasCorrectivas.Columns.Add(new DataColumn("Identificacion"));
            tabla_MedidasCorrectivas.Columns.Add(new DataColumn("Expediente"));
            tabla_MedidasCorrectivas.Columns.Add(new DataColumn("Formato"));
            tabla_MedidasCorrectivas.Columns.Add(new DataColumn("Infractor"));
            tabla_MedidasCorrectivas.Columns.Add(new DataColumn("Fecha"));
            tabla_MedidasCorrectivas.Columns.Add(new DataColumn("Departamento"));
            tabla_MedidasCorrectivas.Columns.Add(new DataColumn("Municipio"));


            //if (dataIndi["MedidasCorrectivas"] != null && !dataIndi["MedidasCorrectivas"].ToString().Equals("[]"))
            //{
            //    var rowMedidasCorrectivas = tabla_MedidasCorrectivas.NewRow();
            //    rowMedidasCorrectivas["Identificacion"] = dataIndi["MedidasCorrectivas"]?["Identificacin"]?.ToString();
            //    rowMedidasCorrectivas["Expediente"] = dataIndi["MedidasCorrectivas"]?["Expendiente"]?.ToString();
            //    rowMedidasCorrectivas["Formato"] = dataIndi["MedidasCorrectivas"]?["Formato"]?.ToString();
            //    rowMedidasCorrectivas["Infractor"] = dataIndi["MedidasCorrectivas"]?["Infractor"]?.ToString();
            //    rowMedidasCorrectivas["Fecha"] = dataIndi["MedidasCorrectivas"]?["Fecha"]?.ToString();
            //    rowMedidasCorrectivas["Departamento"] = dataIndi["MedidasCorrectivas"]?["Departamento"]?.ToString();
            //    rowMedidasCorrectivas["Municipio"] = dataIndi["MedidasCorrectivas"]?["Municipio"]?.ToString();

            //    tabla_MedidasCorrectivas.Rows.Add(rowMedidasCorrectivas);

            //}

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("MedidasCorrectivas", tabla_MedidasCorrectivas));


            var tabla_lista_ejercito = new DataTable();
            tabla_lista_ejercito.Columns.Add(new DataColumn("Tipo_reservista"));
            tabla_lista_ejercito.Columns.Add(new DataColumn("Nombre"));
            tabla_lista_ejercito.Columns.Add(new DataColumn("Lugar"));
            tabla_lista_ejercito.Columns.Add(new DataColumn("Direccion"));

            //if (dataIndi["ejercito"] != null && !dataIndi["ejercito"].ToJson().ToString().Equals("") && !dataIndi["ejercito"].ToJson().ToString().Equals("[]"))
            //{
            //    JsonData dataEjercito = JsonMapper.ToObject(dataIndi["ejercito"].ToString());
            //    var rowEjercito = tabla_lista_ejercito.NewRow();
            //    rowEjercito["Tipo_reservista"] = dataEjercito["TipoReservista"];
            //    rowEjercito["Nombre"] = dataEjercito["Nombre"];
            //    rowEjercito["Lugar"] = dataEjercito["Lugar"];
            //    rowEjercito["Direccion"] = dataEjercito["Direccion"];
            //    tabla_lista_ejercito.Rows.Add(rowEjercito);
            //}

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_ejercito", tabla_lista_ejercito));

            var tabla_listas_rama_judicial = new DataTable();
            tabla_listas_rama_judicial.Columns.Add(new DataColumn("NumeroRadicado"));
            tabla_listas_rama_judicial.Columns.Add(new DataColumn("Identificacion"));
            tabla_listas_rama_judicial.Columns.Add(new DataColumn("NombreSujeto"));
            tabla_listas_rama_judicial.Columns.Add(new DataColumn("Juzgado"));
            tabla_listas_rama_judicial.Columns.Add(new DataColumn("Representante"));
            tabla_listas_rama_judicial.Columns.Add(new DataColumn("Ciudad"));

            //if (dataIndi["rama_judicial"] != null && !dataIndi["rama_judicial"].ToJson().ToString().Equals("") && !dataIndi["rama_judicial"].ToJson().ToString().Equals("[]"))
            //{
            //    JsonData JSonObjectRamaJ = JsonMapper.ToObject(dataIndi["rama_judicial"].ToJson().ToString());
            //    JSonObjectRamaJ = JsonMapper.ToObject(JSonObjectRamaJ["data"].ToJson().ToString());
            //    if (!JSonObjectRamaJ.ToJson().ToString().Equals("") && !JSonObjectRamaJ.ToJson().ToString().Equals("[]"))
            //    {
            //        foreach (var prop in JSonObjectRamaJ.PropertyNames)
            //        {

            //            string key = prop.ToString();
            //            JsonData data_temp = JSonObjectRamaJ[prop.ToString()];
            //            foreach (JsonData jsonObject in data_temp)
            //            {
            //                var rowRamaJudicial = tabla_listas_rama_judicial.NewRow();

            //                rowRamaJudicial["NumeroRadicado"] = jsonObject["num_rad"].ToString();
            //                rowRamaJudicial["Identificacion"] = jsonObject["identificacion"].ToString();
            //                rowRamaJudicial["NombreSujeto"] = jsonObject["nombre"].ToString();
            //                rowRamaJudicial["Juzgado"] = jsonObject["juzgado"].ToString();
            //                rowRamaJudicial["Representante"] = jsonObject["representante"].ToString();
            //                rowRamaJudicial["Ciudad"] = key;
            //                tabla_listas_rama_judicial.Rows.Add(rowRamaJudicial);

            //            }
            //        }
            //    }
            //}

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_rama_judicial", tabla_listas_rama_judicial));

            var listas_rama_judicial_jepms = new DataTable();

            listas_rama_judicial_jepms.Columns.Add(new DataColumn("Nombre"));
            listas_rama_judicial_jepms.Columns.Add(new DataColumn("Identificacion"));
            listas_rama_judicial_jepms.Columns.Add(new DataColumn("Ciudad"));
            listas_rama_judicial_jepms.Columns.Add(new DataColumn("Enlace"));
                if (dataIndi.ramaJudicialJEPMS != null && rama_judicial_jepms_mensaje.Equals(""))
                {
                    foreach (var item in dataIndi.ramaJudicialJEPMS.Data)
                    {
                        DataRow rowBranchJudicial = listas_rama_judicial_jepms.NewRow();
                        rowBranchJudicial["Nombre"] = item.NameResult != null ? item.NameResult.ToString() : "";
                        rowBranchJudicial["Identificacion"] = item.IdentificationNumberResult != null ? item.IdentificationNumberResult.ToString() : "";
                        rowBranchJudicial["Ciudad"] = item.CityName != null ? item.CityName.ToString() : "";
                        rowBranchJudicial["Enlace"] = item.Link != null ? item.Link.ToString() : "";
                        listas_rama_judicial_jepms.Rows.Add(rowBranchJudicial);
                    }
                }
                ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
                {
                    Name = "listas_rama_judicial_jepms",
                    Value = listas_rama_judicial_jepms
                });


            var tabla_listas_rama_judicial_new_administrativo = reporteCompletoGetRowTablesRama();
            var tabla_listas_rama_judicial_new_civil = reporteCompletoGetRowTablesRama();
            var tabla_listas_rama_judicial_new_laboral = reporteCompletoGetRowTablesRama();
            var tabla_listas_rama_judicial_new_familia = reporteCompletoGetRowTablesRama();
            var tabla_listas_rama_judicial_new_JEPMS = reporteCompletoGetRowTablesRama();
            var tabla_listas_rama_judicial_new_otros = reporteCompletoGetRowTablesRama();

                if (dataIndi.ramaJudicial != null && rama_judicial_mensaje.Equals(""))
                {
                    if (!string.IsNullOrEmpty(dataIndi.ramaJudicial.ErrorMessage.ToString()))
                    {
                        rama_judicial_mensaje = dataIndi.ramaJudicial.ErrorMessage != null ? dataIndi.ramaJudicial.ErrorMessage.ToString() : "";
                        parameters[11] = new ReportParameter("mensaje_rama_judicial", rama_judicial_mensaje);
                    }

                    foreach (var jsonObject in dataIndi.ramaJudicial.Data)
                    {                                              

                        string despacho = jsonObject.despacho.ToString().ToLower();
                        DataRow rowRamaJudicial;
                        if (despacho.Contains("administrativo"))
                        {
                            rowRamaJudicial = tabla_listas_rama_judicial_new_administrativo.NewRow();
                        }
                        else if (despacho.Contains("civil"))
                        {
                            rowRamaJudicial = tabla_listas_rama_judicial_new_civil.NewRow();
                        }
                        else if (despacho.Contains("laboral"))
                        {
                            rowRamaJudicial = tabla_listas_rama_judicial_new_laboral.NewRow();
                        }
                        else if (despacho.Contains("familia"))
                        {
                            rowRamaJudicial = tabla_listas_rama_judicial_new_familia.NewRow();
                        }
                        else if (despacho.Contains("penal") || despacho.Contains("con función de control de garantías") || despacho.Contains("con función de conocimiento"))
                        {
                            rowRamaJudicial = tabla_listas_rama_judicial_new_JEPMS.NewRow();
                        }
                        else
                        {
                            rowRamaJudicial = tabla_listas_rama_judicial_new_otros.NewRow();
                        }
                        rowRamaJudicial["llaveProceso"] = jsonObject.llaveProceso != null ? jsonObject.llaveProceso.ToString() : "";
                        rowRamaJudicial["fechaProceso"] = jsonObject.fechaProceso != null ? jsonObject.fechaProceso.ToString() : "";
                        rowRamaJudicial["fechaUltimaActuacion"] = jsonObject.fechaUltimaActuacion != null ? jsonObject.fechaUltimaActuacion.ToString() : "";
                        rowRamaJudicial["despacho"] = jsonObject.despacho != null ? jsonObject.despacho.ToString() : "";
                        rowRamaJudicial["departamento"] = jsonObject.departamento != null ? jsonObject.departamento.ToString() : "";

                        if (jsonObject.sujetosProcesales != null && jsonObject.sujetosProcesales.ToString().Length > 300)
                            jsonObject.sujetosProcesales = jsonObject.sujetosProcesales.ToString().Substring(0, 300) + "(Ver mas con el numero de radicado)";
                        rowRamaJudicial["sujetosProcesales"] = jsonObject.sujetosProcesales != null ? jsonObject.sujetosProcesales.ToString() : "";

                        if (despacho.Contains("administrativo"))
                        {
                            tabla_listas_rama_judicial_new_administrativo.Rows.Add(rowRamaJudicial);
                        }
                        else if (despacho.Contains("civil"))
                        {

                            tabla_listas_rama_judicial_new_civil.Rows.Add(rowRamaJudicial);
                        }
                        else if (despacho.Contains("laboral"))
                        {

                            tabla_listas_rama_judicial_new_laboral.Rows.Add(rowRamaJudicial);
                        }
                        else if (despacho.Contains("familia"))
                        {

                            tabla_listas_rama_judicial_new_familia.Rows.Add(rowRamaJudicial);
                        }
                        else if (despacho.Contains("penal") || despacho.Contains("con función de control de garantías") || despacho.Contains("con función de conocimiento"))
                        {

                            tabla_listas_rama_judicial_new_JEPMS.Rows.Add(rowRamaJudicial);
                        }
                        else
                        {
                            tabla_listas_rama_judicial_new_otros.Rows.Add(rowRamaJudicial);
                        }
                    }
                }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_rama_judicial_new_administrativo", tabla_listas_rama_judicial_new_administrativo));
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_rama_judicial_new_civil", tabla_listas_rama_judicial_new_civil));
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_rama_judicial_new_laboral", tabla_listas_rama_judicial_new_laboral));
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_rama_judicial_new_familia", tabla_listas_rama_judicial_new_familia));
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_rama_judicial_new_JEPMS", tabla_listas_rama_judicial_new_JEPMS));
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_rama_judicial_new_otros", tabla_listas_rama_judicial_new_otros));

            var tabla_listas_super_sociedades = new DataTable();

            tabla_listas_super_sociedades.Columns.Add(new DataColumn("ActividadCIIU"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("VersionCIIU"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("DescripcionCIIU"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("DireccionJudicial"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("CiudadJudicial"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("DepartamentoJudicial"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("DireccionDomicilio"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("CiudadDomicilio"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("DepartamentoDomicilio"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("CorreoElectronico"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("Nit"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("RazonSocial"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("Expediente"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("Sigla"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("ObjetoSocial"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("TipoSociedad"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("Estado"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("FechaEstado"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("EtapaSituacion"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("FechaSituacion"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("FechaEtapa"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("Causal"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("Contador"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("RevisorFiscal"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("RepresentanteLegal"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("RepresentanteLegalPrimerSuplente"));

            var tabla_listas_juntas_principal = new DataTable();
            tabla_listas_juntas_principal.Columns.Add(new DataColumn("Nombre"));
            var tabla_listas_juntas_suplente = new DataTable();
            tabla_listas_juntas_suplente.Columns.Add(new DataColumn("Nombre"));

            //if (dataIndi["super_sociedades"] != null && !dataIndi["super_sociedades"].ToJson().ToString().Equals("") && !dataIndi["super_sociedades"].ToJson().ToString().Equals("[]"))
            //{
            //    JsonData dataSuperSociedades = JsonMapper.ToObject(dataIndi["super_sociedades"].ToString());
            //    var rowSuperSociedades = tabla_listas_super_sociedades.NewRow();
            //    rowSuperSociedades["ActividadCIIU"] = dataSuperSociedades["ActividadCIIU"];
            //    rowSuperSociedades["VersionCIIU"] = dataSuperSociedades["VersionCIIU"];
            //    rowSuperSociedades["DescripcionCIIU"] = dataSuperSociedades["DescripcionCIIU"];
            //    rowSuperSociedades["DireccionJudicial"] = dataSuperSociedades["DireccionJudicial"];
            //    rowSuperSociedades["CiudadJudicial"] = dataSuperSociedades["CiudadJudicial"];
            //    rowSuperSociedades["DepartamentoJudicial"] = dataSuperSociedades["DepartamentoJudicial"];
            //    rowSuperSociedades["CiudadDomicilio"] = dataSuperSociedades["CiudadDomicilio"];
            //    rowSuperSociedades["DepartamentoDomicilio"] = dataSuperSociedades["DepartamentoDomicilio"];
            //    rowSuperSociedades["DireccionDomicilio"] = dataSuperSociedades["DireccionDomicilio"];
            //    rowSuperSociedades["CorreoElectronico"] = dataSuperSociedades["CorreoElectronico"];
            //    rowSuperSociedades["Nit"] = dataSuperSociedades["Nit"];
            //    rowSuperSociedades["RazonSocial"] = dataSuperSociedades["RazonSocial"];
            //    rowSuperSociedades["Expediente"] = dataSuperSociedades["Expediente"];
            //    rowSuperSociedades["Sigla"] = dataSuperSociedades["Sigla"];
            //    rowSuperSociedades["ObjetoSocial"] = dataSuperSociedades["ObjetoSocial"];
            //    rowSuperSociedades["TipoSociedad"] = dataSuperSociedades["TipoSociedad"];
            //    rowSuperSociedades["Estado"] = dataSuperSociedades["Estado"];
            //    rowSuperSociedades["FechaEstado"] = dataSuperSociedades["FechaEstado"];
            //    rowSuperSociedades["EtapaSituacion"] = dataSuperSociedades["EtapaSituacion"];
            //    rowSuperSociedades["FechaSituacion"] = dataSuperSociedades["FechaSituacion"];
            //    rowSuperSociedades["FechaEtapa"] = dataSuperSociedades["FechaEtapa"];
            //    rowSuperSociedades["Causal"] = dataSuperSociedades["Causal"];
            //    rowSuperSociedades["Contador"] = dataSuperSociedades["Contador"];
            //    rowSuperSociedades["RevisorFiscal"] = dataSuperSociedades["RevisorFiscal"];
            //    rowSuperSociedades["RepresentanteLegal"] = dataSuperSociedades["RepresentanteLegal"];
            //    rowSuperSociedades["RepresentanteLegalPrimerSuplente"] = dataSuperSociedades["RepresentanteLegalPrimerSuplente"];
            //    tabla_listas_super_sociedades.Rows.Add(rowSuperSociedades);

            //    JsonData jsonJuntaDirectivaPrincipal = JsonMapper.ToObject(dataSuperSociedades["JuntaDirectivaPrincipal"].ToJson().ToString());
            //    foreach (JsonData item in jsonJuntaDirectivaPrincipal)
            //    {
            //        var rowListasJuntaPrincipal = tabla_listas_juntas_principal.NewRow();
            //        rowListasJuntaPrincipal["Nombre"] = item;
            //        tabla_listas_juntas_principal.Rows.Add(rowListasJuntaPrincipal);
            //    }
            //    JsonData jsonJuntaDirectivaSuplente = JsonMapper.ToObject(dataSuperSociedades["JuntaDirectivaSuplente"].ToJson().ToString());
            //    foreach (JsonData item in jsonJuntaDirectivaSuplente)
            //    {
            //        var rowListasJuntaSuplente = tabla_listas_juntas_suplente.NewRow();
            //        rowListasJuntaSuplente["Nombre"] = item;
            //        tabla_listas_juntas_suplente.Rows.Add(rowListasJuntaSuplente);
            //    }

            //}

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_super_sociedades", tabla_listas_super_sociedades));
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_juntas_principal", tabla_listas_juntas_principal));
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_juntas_suplente", tabla_listas_juntas_suplente));

            var tabla_lista_rues = new DataTable();
            var tabla_lista_ruesRM = new DataTable();

            //if ((dataIndi["rues"] != null && !dataIndi["rues"].ToJson().ToString().Equals("") && !dataIndi["rues"].ToJson().ToString().Equals("[]")) ||
            //    (dataIndi["new_rues"] != null && !dataIndi["new_rues"].ToJson().ToString().Equals("") && !dataIndi["new_rues"].ToJson().ToString().Equals("[]")))
            //{
            //    tabla_lista_rues.Columns.Add(new DataColumn("RazonSocialONombre"));
            //    tabla_lista_rues.Columns.Add(new DataColumn("Nit"));
            //    tabla_lista_rues.Columns.Add(new DataColumn("Estado"));
            //    tabla_lista_rues.Columns.Add(new DataColumn("municipio"));
            //    tabla_lista_rues.Columns.Add(new DataColumn("Categoria"));
            //}
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_rues", tabla_lista_rues));
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_ruesRM", tabla_lista_ruesRM));

            var tabla_lista_simit = new DataTable();
            tabla_lista_simit.Columns.Add(new DataColumn("Resolucion"));
            tabla_lista_simit.Columns.Add(new DataColumn("FechaResolucion"));
            tabla_lista_simit.Columns.Add(new DataColumn("Comparendo"));
            tabla_lista_simit.Columns.Add(new DataColumn("FechaComparendo"));
            tabla_lista_simit.Columns.Add(new DataColumn("Secretaria"));
            tabla_lista_simit.Columns.Add(new DataColumn("NombreInfractor"));
            tabla_lista_simit.Columns.Add(new DataColumn("Estado"));
            tabla_lista_simit.Columns.Add(new DataColumn("Infraccion"));
            tabla_lista_simit.Columns.Add(new DataColumn("ValorMulta"));
            tabla_lista_simit.Columns.Add(new DataColumn("InteresMora"));
            tabla_lista_simit.Columns.Add(new DataColumn("ValorAdicional"));
            tabla_lista_simit.Columns.Add(new DataColumn("ValorPagar"));

            //if (dataIndi["simit"] != null && !dataIndi["simit"].ToJson().ToString().Equals("") && !dataIndi["simit"].ToJson().ToString().Equals("[]"))
            //{
            //    JsonData dataSimit = JsonMapper.ToObject(dataIndi["simit"].ToString());
            //    foreach (JsonData item in dataSimit)
            //    {
            //        var rowsimit = tabla_lista_simit.NewRow();
            //        rowsimit["Resolucion"] = item["Resolucion"];
            //        rowsimit["FechaResolucion"] = item["FechaResolucion"];
            //        rowsimit["Comparendo"] = item["Comparendo"];
            //        rowsimit["FechaComparendo"] = item["FechaComparendo"];
            //        rowsimit["Secretaria"] = item["Secretaria"];
            //        rowsimit["NombreInfractor"] = item["NombreInfractor"];
            //        rowsimit["Estado"] = item["Estado"];
            //        rowsimit["Infraccion"] = item["Infraccion"];
            //        rowsimit["ValorMulta"] = item["ValorMulta"];
            //        rowsimit["InteresMora"] = item["InteresMora"];
            //        rowsimit["ValorAdicional"] = item["ValorAdicional"];
            //        rowsimit["ValorPagar"] = item["ValorPagar"];
            //        tabla_lista_simit.Rows.Add(rowsimit);
            //    }
            //}

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_simit", tabla_lista_simit));

            var tabla_lista_simit_new = new DataTable();
            tabla_lista_simit_new.Columns.Add(new DataColumn("type"));
            tabla_lista_simit_new.Columns.Add(new DataColumn("notification"));
            tabla_lista_simit_new.Columns.Add(new DataColumn("plate"));
            tabla_lista_simit_new.Columns.Add(new DataColumn("secretaryship"));
            tabla_lista_simit_new.Columns.Add(new DataColumn("infringement"));
            tabla_lista_simit_new.Columns.Add(new DataColumn("state"));
            tabla_lista_simit_new.Columns.Add(new DataColumn("amount"));
            tabla_lista_simit_new.Columns.Add(new DataColumn("amountToPaid"));

            //if (dataIndi["simit_new"] != null && !dataIndi["simit_new"].ToJson().ToString().Equals("") && !dataIndi["simit_new"].ToJson().ToString().Equals("[]"))
            //{
            //    JsonData dataSimit = JsonMapper.ToObject(dataIndi["simit_new"].ToString());
            //    foreach (JsonData item in dataSimit)
            //    {
            //        var rowsimit = tabla_lista_simit_new.NewRow();
            //        rowsimit["type"] = item["type"];
            //        rowsimit["notification"] = item["notification"];
            //        rowsimit["plate"] = item["plate"];
            //        rowsimit["secretaryship"] = item["secretaryship"];
            //        rowsimit["infringement"] = item["infringement"];
            //        rowsimit["state"] = item["state"];
            //        rowsimit["amount"] = item["amount"];
            //        rowsimit["amountToPaid"] = item["amountToPaid"];
            //        tabla_lista_simit_new.Rows.Add(rowsimit);
            //    }
            //}

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_simit_new", tabla_lista_simit_new));

            /* Servicios adicionales -----------------------------------------------*/

            var matriz_riesgo = new DataTable();
            var rowriesgo = matriz_riesgo.NewRow();
            for (int y = 1; y <= 4; y++)
            {
                for (int x = 1; x <= 5; x++)
                {
                    matriz_riesgo.Columns.Add(new DataColumn("p" + y + x));
                    rowriesgo["p" + y + x] = 0;
                }
            }

            //string[] list_keys = { "GridView3_tabla_listas_restrictivas", "GridView7_tabla_listas_la_lf", "GridViewG1_1_tabla_listas_la_lf_admin", "GridView5_tabla_lista_afectacion_financiera", "GridViewG2_1_tabla_lista_sanciones_administrativas", "GridView6_tabla_listas_informativa_peps" };
            //foreach (string key in list_keys)
            //{
            //    if (dataIndi[key] != null && !dataIndi[key].ToString().Equals("[]"))
            //    {
            //        foreach (JsonData jsonObject in JsonMapper.ToObject(dataIndi[key].ToString()))
            //        {
            //            int orden = Int32.Parse(jsonObject["Orden"].ToString().Trim());
            //            int prioridad = Int32.Parse(jsonObject["Prioridad"].ToString().Trim());
            //            if (orden <= 5 && prioridad <= 4)
            //            {
            //                int value = Int32.Parse(rowriesgo["p" + prioridad + orden].ToString().Trim());
            //                value += 1;
            //                rowriesgo["p" + prioridad + orden] = value;
            //            }
            //        }
            //    }
            //}

            if (dataIndi.Listas != null && !dataIndi.Listas.Equals("[]"))
            {
                foreach (var item in dataIndi.Listas)
                {
                    int order = Int32.Parse(item.Orden.ToString().Trim());
                    int priority = Int32.Parse(item.Prioridad.ToString().Trim());
                    if (order <= 5 && priority <= 4)
                    {
                        int value = Int32.Parse(rowriesgo["p" + priority + order].ToString().Trim());
                        value += 1;
                        rowriesgo["p" + priority + order] = value;
                    }
                }
            }

            matriz_riesgo.Rows.Add(rowriesgo);
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "matriz_riesgo",
                Value = matriz_riesgo
            });

            ReportViewerReporteConsolidadoNew.ReportPath = contentRootPath + "\\Reports\\ReporteConsolidadoNew.rdlc";

            ReportViewerReporteConsolidadoNew.SetParameters(parameters);
            ReportViewerReporteConsolidadoNew.Refresh();

            ReportViewerReporteConsolidadoNew.SubreportProcessing += SubreportProcessingEventHandler;

            return createPdf(ReportViewerReporteConsolidadoNew, response);

            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}
            //Warning[] warnings;
            //string[] streamIds;
            //string mimeType = string.Empty;
            //string encoding = string.Empty;
            //string extension = string.Empty;

            //byte[] bytes = ReportViewerReporteConsolidadoNew.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            //PdfSharp.Pdf.PdfDocument document = new PdfSharp.Pdf.PdfDocument();
            //PdfSharp.Pdf.PdfDocument tempPDFDoc = PdfSharp.Pdf.IO.PdfReader.Open(new MemoryStream(bytes), PdfDocumentOpenMode.Import);

            //for (int i = 0; i < tempPDFDoc.PageCount; i++)
            //{
            //    PdfSharp.Pdf.PdfPage page = tempPDFDoc.Pages[i];
            //    document.AddPage(page);
            //}

            //XFont font = new XFont("Verdana", 8);
            //XBrush brush = XBrushes.Black;

            //// Add the page counter.
            //string noPages = document.Pages.Count.ToString();
            //for (int i = 0; i < document.Pages.Count; ++i)
            //{
            //    PdfSharp.Pdf.PdfPage page = document.Pages[i];

            //    // Make a layout rectangle.
            //    XRect layoutRectangle = new XRect(0/*X*/, page.Height - font.Height/*Y*/, page.Width/*Width*/, font.Height/*Height*/);

            //    using (XGraphics gfx = XGraphics.FromPdfPage(page))
            //    {
            //        gfx.DrawString(
            //            "Pagina " + (i + 1).ToString() + " de " + noPages,
            //            font,
            //            brush,
            //            layoutRectangle,
            //            XStringFormats.Center);
            //    }
            //}

            //MemoryStream stream = new MemoryStream();
            //document.Save(stream, false);
            //var options = document.Options;
            //byte[] buffer = new byte[0];
            //buffer = stream.ToArray();
            //var contentLength = buffer.Length;
            //response.Clear();
            //response.ContentType = "application/pdf";

            //stream.Close();
            //string namepdf = "Reporte_" + System.DateTime.Now.ToString().Replace(" ", "_").Replace(",", "").Replace(".", "").Replace(":", "").Replace("/", "-");
            //Object[] objects = new Object[4];
            //objects[0] = bytes;
            //objects[1] = mimeType;
            //objects[2] = namepdf;
            //objects[3] = extension;
            //return objects;
        }
        public Object[] generarReporteIndividual(JsonData dataIndi, HttpResponse response, string contentRootPath, string image)
        {
            using var ReportViewerReporteConsolidadoNew = new LocalReport();
            ReportViewerReporteConsolidadoNew.DataSources.Clear();
            ReportViewerReporteConsolidadoNew.EnableExternalImages = true;
            ReportViewerReporteConsolidadoNew.EnableHyperlinks = true;
            ReportParameter[] parameters = new ReportParameter[32];
            parameters[0] = new ReportParameter("nombre_consulta", dataIndi["nombre_consulta"].ToString());
            parameters[1] = new ReportParameter("identificacion_consulta", dataIndi["identificacion_consulta"].ToString());
            parameters[2] = new ReportParameter("no_consulta", dataIndi["no_consulta"].ToString());
            parameters[3] = new ReportParameter("nombre_usuario_consultor", dataIndi["nombre_consultor"].ToString());
            parameters[4] = new ReportParameter("usuario_consultor", dataIndi["usuario_consultor"].ToString());
            parameters[5] = new ReportParameter("fecha_reporte", System.DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
            parameters[6] = new ReportParameter("fecha_consulta", dataIndi["fecha_consulta"].ToString());

            String ejercito_mensaje = dataIndi["ejercito_mensaje"] != null ? dataIndi["ejercito_mensaje"].ToString() : "";
            parameters[8] = new ReportParameter("mensaje_ejercito", ejercito_mensaje);
            String super_sociedades_mensaje = dataIndi["super_sociedades_mensaje"] != null ? dataIndi["super_sociedades_mensaje"].ToString() : "";
            parameters[9] = new ReportParameter("mensaje_super_sociedades", super_sociedades_mensaje);
            String procuraduria_mensaje = dataIndi["procuraduria_mensaje"] != null ? dataIndi["procuraduria_mensaje"].ToString() : "";
            parameters[10] = new ReportParameter("mensaje_procuraduria", procuraduria_mensaje);
            String rama_judicial_mensaje = dataIndi["rama_judicial_mensaje"] != null ? dataIndi["rama_judicial_mensaje"].ToString() : "";
            parameters[11] = new ReportParameter("mensaje_rama_judicial", rama_judicial_mensaje);
            parameters[12] = new ReportParameter("tipo_tercero", dataIndi["tipo_tercero"].ToString());

            String simit_mensaje = dataIndi["simit_mensaje"] != null ? dataIndi["simit_mensaje"].ToString() : dataIndi["simit_mensaje_new"] != null ? dataIndi["simit_mensaje_new"].ToString() : "";
            parameters[13] = new ReportParameter("mensaje_simit", simit_mensaje);
            String rues_mensaje = dataIndi["rues_mensaje"] != null ? dataIndi["rues_mensaje"].ToString() : "";
            parameters[14] = new ReportParameter("mensaje_rues", rues_mensaje);
            string str_listas_consultadas = string.Empty;
            int no_listas_consultadas = JsonMapper.ToObject(dataIndi["listas_seleccionadas"].ToString()).Count;

            foreach (JsonData item in JsonMapper.ToObject(dataIndi["listas_seleccionadas"].ToString()))
            {
                if (!item["NombreTipoLista"].ToString().ToLower().Equals("todas"))
                    str_listas_consultadas += str_listas_consultadas.Equals("") ? item["NombreTipoLista"] : ", " + item["NombreTipoLista"];
                else
                    no_listas_consultadas--;
            }
            parameters[7] = new ReportParameter("no_listas_consultadas", no_listas_consultadas.ToString());
            parameters[15] = new ReportParameter("ListasConsultadas", str_listas_consultadas);

            String rama_judicial_jepms_mensaje = dataIndi["rama_judicial_jepms_mensaje"] != null ? dataIndi["rama_judicial_jepms_mensaje"].ToString() : "";
            parameters[17] = new ReportParameter("mensaje_rama_judicial_jepms", rama_judicial_jepms_mensaje);

            String contaduria = dataIndi["contaduria"] != null ? dataIndi["contaduria"].ToString() : "";
            parameters[18] = new ReportParameter("mensaje_contaduria", contaduria);

            String contaduria_incumplimiento = dataIndi["contaduria_incumplimiento_acuerdos"] != null ? dataIndi["contaduria_incumplimiento_acuerdos"].ToString() : "";
            parameters[19] = new ReportParameter("mensaje_contaduria_incumplimiento_acuerdos", contaduria_incumplimiento);

            string img = image;
            parameters[16] = new ReportParameter("image", img);

            string PPT_mensaje = dataIndi["PPT_mensaje"] != null ? dataIndi["PPT_mensaje"].ToString() : "";
            parameters[20] = new ReportParameter("mensaje_PPT", PPT_mensaje);

            string mensaje_defRegistraduria = dataIndi["mensaje_defRegistraduria"] != null ? dataIndi["mensaje_defRegistraduria"].ToString() : "";
            parameters[21] = new ReportParameter("mensaje_defRegistraduria", mensaje_defRegistraduria);

            string EPS_mensaje = dataIndi["EPS_mensaje"] != null ? dataIndi["EPS_mensaje"].ToString() : "";
            parameters[22] = new ReportParameter("mensaje_EPS", EPS_mensaje);

            String criminal_records_ecuador_mensaje = dataIndi["CriminalRecordEcuador_mensaje"] != null ? dataIndi["CriminalRecordEcuador_mensaje"].ToString() : "";
            parameters[23] = new ReportParameter("mensaje_criminal_records_ecuador", criminal_records_ecuador_mensaje);

            String judicial_information_mensaje = dataIndi["JudicialInformationEcuador_mensaje"] != null ? dataIndi["JudicialInformationEcuador_mensaje"].ToString() : "";
            parameters[24] = new ReportParameter("mensaje_judicial_information_ecuador", judicial_information_mensaje);

            String sunat_mensaje = dataIndi["sunat_mensaje"] != null ? dataIndi["sunat_mensaje"].ToString() : "";
            parameters[25] = new ReportParameter("mensaje_sunat", sunat_mensaje);

            String rethus_mensaje = dataIndi["rethus_message"] != null ? dataIndi["rethus_message"].ToString() : "";
            parameters[26] = new ReportParameter("mensaje_rethus", rethus_mensaje);

            String MedidasCorrectivas_mensaje = dataIndi["MedidasCorrectivas_mensaje"] != null ? dataIndi["MedidasCorrectivas_mensaje"].ToString() : "";
            parameters[27] = new ReportParameter("mensaje_medidas_correctivas", MedidasCorrectivas_mensaje);

            String PoNal = dataIndi["PoNal"] != null ? dataIndi["PoNal"].ToString() : "";
            parameters[28] = new ReportParameter("mensaje_policia", PoNal);

            String DelitosSexuales = dataIndi["DelitosSexuales"] != null ? dataIndi["DelitosSexuales"].ToString() : "";
            parameters[29] = new ReportParameter("mensaje_delitos_sexuales", DelitosSexuales);

            String Inpec_mensaje = dataIndi["Inpec_mensaje"] != null ? dataIndi["Inpec_mensaje"].ToString() : "";
            parameters[30] = new ReportParameter("mensaje_inpec", Inpec_mensaje);

            String judicial_information_complete_mensaje = dataIndi["JudicialInformationCompleteEcuador_mensaje"] != null ? dataIndi["JudicialInformationCompleteEcuador_mensaje"].ToString() : "";
            parameters[31] = new ReportParameter("mensaje_judicial_information_complete_ecuador", judicial_information_complete_mensaje);

            var tabla_listas_ofac = reporteCompletoGetRowTables();
            var tabla_listas_seguridad_onu = reporteCompletoGetRowTables();
            var tabla_listas_activos_financieros = reporteCompletoGetRowTables();
            var tabla_listas_terrorista_policial_judicial = reporteCompletoGetRowTables();
            var tabla_listas_terrorist_organization = reporteCompletoGetRowTables();
            var tabla_listas_elimanos_terrorista_eu = reporteCompletoGetRowTables();
            if (dataIndi["GridView3_tabla_listas_restrictivas"] != null && !dataIndi["GridView3_tabla_listas_restrictivas"].ToString().Equals("[]"))
            {
                foreach (JsonData jsonObject in JsonMapper.ToObject(dataIndi["GridView3_tabla_listas_restrictivas"].ToString()))
                {
                    var row = tabla_listas_ofac.NewRow();
                    switch (jsonObject["IdTipoLista"].ToString())
                    {
                        case "4":
                        default:
                            row = tabla_listas_ofac.NewRow();
                            addRowValuesListasRestrictivas(row, jsonObject);
                            tabla_listas_ofac.Rows.Add(row);
                            break;
                        case "8":
                            row = tabla_listas_seguridad_onu.NewRow();
                            addRowValuesListasRestrictivas(row, jsonObject);
                            tabla_listas_seguridad_onu.Rows.Add(row);
                            break;
                        case "158":
                            row = tabla_listas_activos_financieros.NewRow();
                            addRowValuesListasRestrictivas(row, jsonObject);
                            tabla_listas_activos_financieros.Rows.Add(row);
                            break;
                        case "159":
                            row = tabla_listas_terrorista_policial_judicial.NewRow();
                            addRowValuesListasRestrictivas(row, jsonObject);
                            tabla_listas_terrorista_policial_judicial.Rows.Add(row);
                            break;
                        case "160":
                            row = tabla_listas_terrorist_organization.NewRow();
                            addRowValuesListasRestrictivas(row, jsonObject);
                            tabla_listas_terrorist_organization.Rows.Add(row);
                            break;
                        case "161":
                            row = tabla_listas_elimanos_terrorista_eu.NewRow();
                            addRowValuesListasRestrictivas(row, jsonObject);
                            tabla_listas_elimanos_terrorista_eu.Rows.Add(row);
                            break;

                    }

                }
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "lista_ofac",
                Value = tabla_listas_ofac
            });
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "lista_seguridad_onu",
                Value = tabla_listas_seguridad_onu
            });
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "lista_terrorista_activo_financiero",
                Value = tabla_listas_activos_financieros
            });
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "lista_terrorista_policial_judicial",
                Value = tabla_listas_terrorista_policial_judicial
            });
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "lista_terrorist_organization",
                Value = tabla_listas_terrorist_organization
            });
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "listas_eliminados_terroristas_eu",
                Value = tabla_listas_elimanos_terrorista_eu
            });

            var tabla_listas_la_lf_admin = reporteCompletoGetRowTables();
            if (dataIndi["GridViewG1_1_tabla_listas_la_lf_admin"] != null && !dataIndi["GridViewG1_1_tabla_listas_la_lf_admin"].ToString().Equals("[]"))
            {
                foreach (JsonData jsonObject in JsonMapper.ToObject(dataIndi["GridViewG1_1_tabla_listas_la_lf_admin"].ToString()))
                {
                    var row = tabla_listas_la_lf_admin.NewRow();
                    row["Prioridad"] = jsonObject["Prioridad"].ToString().Trim();
                    row["Tipo_documento"] = jsonObject["TipoDocumento"].ToString().Trim();
                    row["Documento"] = jsonObject["DocumentoIdentidad"].ToString().Trim();
                    row["Nombre"] = jsonObject["NombreCompleto"].ToString().Trim();
                    row["Nombre_lista"] = jsonObject["NombreTipoLista"].ToString().Trim();
                    row["Cargo"] = jsonObject["Delito"].ToString().Trim();
                    row["Zona"] = jsonObject["Zona"].ToString().Trim();
                    if (jsonObject["OtraInformacion"].ToString().Trim().Length > 300)
                        jsonObject["OtraInformacion"] = jsonObject["OtraInformacion"].ToString().Trim().Substring(0, 300) + "(Ver mas con el numero de radicado)";
                    row["otra_informacion"] = jsonObject["OtraInformacion"].ToString().Trim();
                    tabla_listas_la_lf_admin.Rows.Add(row);
                }
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "listas_la_ft_admin",
                Value = tabla_listas_la_lf_admin
            });

            var listas_la_ft = reporteCompletoGetRowTables();
            if (dataIndi["GridView7_tabla_listas_la_lf"] != null && !dataIndi["GridView7_tabla_listas_la_lf"].ToString().Equals("[]"))
            {
                foreach (JsonData jsonObject in JsonMapper.ToObject(dataIndi["GridView7_tabla_listas_la_lf"].ToString()))
                {
                    var row = listas_la_ft.NewRow();
                    row["Prioridad"] = jsonObject["Prioridad"].ToString().Trim();
                    row["Tipo_documento"] = jsonObject["TipoDocumento"].ToString().Trim();
                    row["Documento"] = jsonObject["DocumentoIdentidad"].ToString().Trim();
                    row["Nombre"] = jsonObject["NombreCompleto"].ToString().Trim();
                    row["Nombre_lista"] = jsonObject["NombreTipoLista"].ToString().Trim();
                    row["Cargo"] = jsonObject["Delito"].ToString().Trim();
                    row["Zona"] = jsonObject["Zona"].ToString().Trim();
                    if (jsonObject["OtraInformacion"].ToString().Trim().Length > 300)
                        jsonObject["OtraInformacion"] = jsonObject["OtraInformacion"].ToString().Trim().Substring(0, 300) + "(Ver mas con el numero de radicado)";
                    row["otra_informacion"] = jsonObject["OtraInformacion"].ToString().Trim();
                    listas_la_ft.Rows.Add(row);
                }
            }
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "listas_la_ft",
                Value = listas_la_ft
            });

            var tabla_lista_afectacion_financiera = reporteCompletoGetRowTables();
            if (dataIndi["GridView5_tabla_lista_afectacion_financiera"] != null && !dataIndi["GridView5_tabla_lista_afectacion_financiera"].ToString().Equals("[]"))
            {
                foreach (JsonData jsonObject in JsonMapper.ToObject(dataIndi["GridView5_tabla_lista_afectacion_financiera"].ToString()))
                {
                    var row = tabla_lista_afectacion_financiera.NewRow();
                    row["Prioridad"] = jsonObject["Prioridad"].ToString().Trim();
                    row["Tipo_documento"] = jsonObject["TipoDocumento"].ToString().Trim();
                    row["Documento"] = jsonObject["DocumentoIdentidad"].ToString().Trim();
                    row["Nombre"] = jsonObject["NombreCompleto"].ToString().Trim();
                    row["Nombre_lista"] = jsonObject["NombreTipoLista"].ToString().Trim();
                    row["Cargo"] = jsonObject["Delito"].ToString().Trim();
                    row["Zona"] = jsonObject["Zona"].ToString().Trim();
                    if (jsonObject["OtraInformacion"].ToString().Trim().Length > 300)
                        jsonObject["OtraInformacion"] = jsonObject["OtraInformacion"].ToString().Trim().Substring(0, 300) + "(Ver mas con el numero de radicado)";
                    row["otra_informacion"] = jsonObject["OtraInformacion"].ToString().Trim();
                    tabla_lista_afectacion_financiera.Rows.Add(row);
                }
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "listas_afectacion_financiera",
                Value = tabla_lista_afectacion_financiera
            });

            var tabla_lista_sanciones_administrativas = reporteCompletoGetRowTables();
            if (dataIndi["GridViewG2_1_tabla_lista_sanciones_administrativas"] != null && !dataIndi["GridViewG2_1_tabla_lista_sanciones_administrativas"].ToString().Equals("[]"))
            {
                foreach (JsonData jsonObject in JsonMapper.ToObject(dataIndi["GridViewG2_1_tabla_lista_sanciones_administrativas"].ToString()))
                {
                    var row = tabla_lista_sanciones_administrativas.NewRow();
                    row["Prioridad"] = jsonObject["Prioridad"].ToString().Trim();
                    row["Tipo_documento"] = jsonObject["TipoDocumento"].ToString().Trim();
                    row["Documento"] = jsonObject["DocumentoIdentidad"].ToString().Trim();
                    row["Nombre"] = jsonObject["NombreCompleto"].ToString().Trim();
                    row["Nombre_lista"] = jsonObject["NombreTipoLista"].ToString().Trim();
                    row["Cargo"] = jsonObject["Delito"].ToString().Trim();
                    row["Zona"] = jsonObject["Zona"].ToString().Trim();
                    if (jsonObject["OtraInformacion"].ToString().Trim().Length > 300)
                        jsonObject["OtraInformacion"] = jsonObject["OtraInformacion"].ToString().Trim().Substring(0, 300) + "(Ver mas con el numero de radicado)";
                    row["otra_informacion"] = jsonObject["OtraInformacion"].ToString().Trim();
                    tabla_lista_sanciones_administrativas.Rows.Add(row);
                }
            }
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "listas_sanciones_administrativas",
                Value = tabla_lista_sanciones_administrativas
            });

            var tabla_listas_peps = reporteCompletoGetRowTables();
            if (dataIndi["GridView_tabla_peps"] != null && !dataIndi["GridView_tabla_peps"].ToString().Equals("[]"))
            {
                foreach (JsonData jsonObject in JsonMapper.ToObject(dataIndi["GridView_tabla_peps"].ToString()))
                {
                    var row = tabla_listas_peps.NewRow();
                    row["Prioridad"] = jsonObject["Prioridad"].ToString();
                    row["Tipo_documento"] = jsonObject["TipoDocumento"].ToString();
                    row["Documento"] = jsonObject["DocumentoIdentidad"].ToString();
                    row["Nombre"] = jsonObject["NombreCompleto"].ToString();
                    row["Nombre_lista"] = jsonObject["NombreTipoLista"].ToString();
                    row["Cargo"] = jsonObject["Delito"].ToString();
                    row["Zona"] = jsonObject["Zona"].ToString();
                    if (jsonObject["OtraInformacion"].ToString().Length > 300)
                        jsonObject["OtraInformacion"] = jsonObject["OtraInformacion"].ToString().Substring(0, 300) + "(Ver mas con el numero de radicado)";
                    row["otra_informacion"] = jsonObject["OtraInformacion"].ToString();
                    tabla_listas_peps.Rows.Add(row);
                }
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("lista_peps", tabla_listas_peps));

            var tabla_listas_informativa_peps = reporteCompletoGetRowTables();
            if (dataIndi["GridView6_tabla_listas_informativa_peps"] != null && !dataIndi["GridView6_tabla_listas_informativa_peps"].ToString().Equals("[]"))
            {
                foreach (JsonData jsonObject in JsonMapper.ToObject(dataIndi["GridView6_tabla_listas_informativa_peps"].ToString()))
                {
                    var row = tabla_listas_informativa_peps.NewRow();
                    row["Prioridad"] = jsonObject["Prioridad"].ToString().Trim();
                    row["Tipo_documento"] = jsonObject["TipoDocumento"].ToString().Trim();
                    row["Documento"] = jsonObject["DocumentoIdentidad"].ToString().Trim();
                    row["Nombre"] = jsonObject["NombreCompleto"].ToString().Trim();
                    row["Nombre_lista"] = jsonObject["NombreTipoLista"].ToString().Trim();
                    row["Cargo"] = jsonObject["Delito"].ToString().Trim();
                    row["Zona"] = jsonObject["Zona"].ToString().Trim();
                    if (jsonObject["OtraInformacion"].ToString().Trim().Length > 300)
                        jsonObject["OtraInformacion"] = jsonObject["OtraInformacion"].ToString().Trim().Substring(0, 300) + "(Ver mas con el numero de radicado)";
                    row["otra_informacion"] = jsonObject["OtraInformacion"].ToString();
                    tabla_listas_informativa_peps.Rows.Add(row);
                }
            }
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "listas_informativas_peps",
                Value = tabla_listas_informativa_peps
            });

            var tabla_listas_propias = new DataTable();
            tabla_listas_propias.Columns.Add(new DataColumn("Tipo_documento"));
            tabla_listas_propias.Columns.Add(new DataColumn("Documento"));
            tabla_listas_propias.Columns.Add(new DataColumn("Nombre"));
            tabla_listas_propias.Columns.Add(new DataColumn("Nombre_Tipo_Lista"));
            tabla_listas_propias.Columns.Add(new DataColumn("Tipo_persona"));
            tabla_listas_propias.Columns.Add(new DataColumn("Cargo"));
            tabla_listas_propias.Columns.Add(new DataColumn("Alias"));
            if (dataIndi["GridView2_tabla_listas_propias"] != null && !dataIndi["GridView2_tabla_listas_propias"].ToString().Equals("[]"))
            {
                foreach (JsonData jsonObject in JsonMapper.ToObject(dataIndi["GridView2_tabla_listas_propias"].ToString()))
                {
                    var row = tabla_listas_propias.NewRow();
                    row["Tipo_documento"] = jsonObject["TipoDocumento"] != null ? jsonObject["TipoDocumento"].ToString().Trim() : "";
                    row["Documento"] = jsonObject["DocumentoIdentidad"].ToString().Trim();
                    row["Nombre"] = jsonObject["NombreCompleto"].ToString().Trim();
                    row["Nombre_tipo_lista"] = jsonObject["NombreTipoLista"] != null ? jsonObject["NombreTipoLista"].ToString().Trim() : "";
                    row["Tipo_persona"] = jsonObject["TipoPersona"] != null ? jsonObject["TipoPersona"].ToString().Trim() : "";
                    row["Cargo"] = jsonObject["Delito"].ToString().Trim();
                    row["Alias"] = jsonObject["Alias"] != null ? jsonObject["Alias"].ToString().Trim() : "";
                    tabla_listas_propias.Rows.Add(row);
                }
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "listas_propias",
                Value = tabla_listas_propias
            });
            /* Servicios adicionales -----------------------------------------------*/
            /* ecuador -----------------------------------------------*/
            var tabla_criminal_records_ecuador = new DataTable();
            tabla_criminal_records_ecuador.Columns.Add(new DataColumn("criminalRecord"));
            tabla_criminal_records_ecuador.Columns.Add(new DataColumn("documentType"));
            tabla_criminal_records_ecuador.Columns.Add(new DataColumn("document"));
            tabla_criminal_records_ecuador.Columns.Add(new DataColumn("name"));

            if (dataIndi["CriminalRecordEcuador"] != null && !dataIndi["CriminalRecordEcuador"].ToString().Equals("[]"))
            {
                JsonData jsonObject = JsonMapper.ToObject(dataIndi["CriminalRecordEcuador"].ToJson().ToString());

                var row = tabla_criminal_records_ecuador.NewRow();
                row["criminalRecord"] = jsonObject["criminalRecord"] != null ? jsonObject["criminalRecord"].ToString() : "";
                row["documentType"] = jsonObject["documentType"] != null ? jsonObject["documentType"].ToString() : "";
                row["document"] = jsonObject["document"] != null ? jsonObject["document"].ToString() : "";
                row["name"] = jsonObject["name"] != null ? jsonObject["name"].ToString() : "";

                tabla_criminal_records_ecuador.Rows.Add(row);

            }
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("criminal_records_ecuador", tabla_criminal_records_ecuador));

            var tabla_judicial_information_ecuador = new DataTable();
            tabla_judicial_information_ecuador.Columns.Add(new DataColumn("date"));
            tabla_judicial_information_ecuador.Columns.Add(new DataColumn("nProccess"));
            tabla_judicial_information_ecuador.Columns.Add(new DataColumn("action"));

            if (dataIndi["JudicialInformationEcuador"] != null && !dataIndi["JudicialInformationEcuador"].ToJson().ToString().Equals("[]"))
            {
                JsonData jsonObjectJudicialInformationEcuador = JsonMapper.ToObject(dataIndi["JudicialInformationEcuador"].ToString());



                foreach (JsonData jsonObject in jsonObjectJudicialInformationEcuador)
                {

                    var row = tabla_judicial_information_ecuador.NewRow();
                    row["date"] = jsonObject["date"] != null ? jsonObject["date"].ToString() : "";
                    row["nProccess"] = jsonObject["nProccess"] != null ? jsonObject["nProccess"].ToString() : "";
                    row["action"] = jsonObject["action"] != null ? jsonObject["action"].ToString() : "";

                    tabla_judicial_information_ecuador.Rows.Add(row);

                }
            }
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("judicial_information_ecuador", tabla_judicial_information_ecuador));

            var tabla_judicial_information_complete_ecuador = new DataTable();
            tabla_judicial_information_complete_ecuador.Columns.Add(new DataColumn("id"));
            tabla_judicial_information_complete_ecuador.Columns.Add(new DataColumn("fechaIngreso"));
            tabla_judicial_information_complete_ecuador.Columns.Add(new DataColumn("idJuicio"));
            tabla_judicial_information_complete_ecuador.Columns.Add(new DataColumn("nombreDelito"));
            tabla_judicial_information_complete_ecuador.Columns.Add(new DataColumn("link"));
            if (dataIndi["JudicialInformationCompleteEcuador"] != null && !dataIndi["JudicialInformationCompleteEcuador"].ToJson().ToString().Equals("[]"))
            {
                JsonData jsonObjectJudicialInformationCompleteEcuador = JsonMapper.ToObject(dataIndi["JudicialInformationCompleteEcuador"].ToString());



                foreach (JsonData jsonObject in jsonObjectJudicialInformationCompleteEcuador)
                {
                    string date = jsonObject["fechaIngreso"] != null ? jsonObject["fechaIngreso"].ToString() : "";
                    if (!string.IsNullOrEmpty(date))
                        date = date.Split(new char[] { 'T' })[0];
                    var row = tabla_judicial_information_complete_ecuador.NewRow();
                    row["id"] = jsonObject["id"] != null ? jsonObject["id"].ToString() : "";
                    row["fechaIngreso"] = date;
                    row["idJuicio"] = jsonObject["idJuicio"] != null ? jsonObject["idJuicio"].ToString() : "";
                    row["nombreDelito"] = jsonObject["nombreDelito"] != null ? jsonObject["nombreDelito"].ToString() : "";
                    row["link"] = jsonObject["link"] != null ? jsonObject["link"].ToString() : "";

                    tabla_judicial_information_complete_ecuador.Rows.Add(row);

                }
            }
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("judicial_information_complete_ecuador", tabla_judicial_information_complete_ecuador));
            /* ecuador -----------------------------------------------*/
            /* peru -----------------------------------------------*/
            var tabla_sunat = new DataTable();
            tabla_sunat.Columns.Add(new DataColumn("no_ruc"));
            tabla_sunat.Columns.Add(new DataColumn("tipo_cont"));
            tabla_sunat.Columns.Add(new DataColumn("nombre_comercial"));
            tabla_sunat.Columns.Add(new DataColumn("fecha_inscripcion"));
            tabla_sunat.Columns.Add(new DataColumn("fecha_inicio_actividades"));
            tabla_sunat.Columns.Add(new DataColumn("estado"));
            tabla_sunat.Columns.Add(new DataColumn("condicion"));
            tabla_sunat.Columns.Add(new DataColumn("domicilio"));
            tabla_sunat.Columns.Add(new DataColumn("sistema_emicion"));
            tabla_sunat.Columns.Add(new DataColumn("actividad_comercio_exterior"));
            tabla_sunat.Columns.Add(new DataColumn("sistema_contabilidad"));
            tabla_sunat.Columns.Add(new DataColumn("actividades_economicas"));
            tabla_sunat.Columns.Add(new DataColumn("comprobante_pago"));
            tabla_sunat.Columns.Add(new DataColumn("sistema_emision_electronica"));
            tabla_sunat.Columns.Add(new DataColumn("emision_electronica_desde"));
            tabla_sunat.Columns.Add(new DataColumn("comprobantes_electronicos"));
            tabla_sunat.Columns.Add(new DataColumn("afiliado_ple"));
            tabla_sunat.Columns.Add(new DataColumn("padrones"));

            if (dataIndi["sunat"] != null && !dataIndi["sunat"].ToJson().ToString().Equals("[]"))
            {
                JsonData jsonObject = JsonMapper.ToObject(dataIndi["sunat"].ToJson().ToString());

                var row = tabla_sunat.NewRow();
                row["no_ruc"] = jsonObject["no_ruc"] != null ? jsonObject["no_ruc"].ToString() : "";
                row["tipo_cont"] = jsonObject["tipo_cont"] != null ? jsonObject["tipo_cont"].ToString() : "";
                row["nombre_comercial"] = jsonObject["nombre_comercial"] != null ? jsonObject["nombre_comercial"].ToString() : "";
                row["fecha_inscripcion"] = jsonObject["fecha_inscripcion"] != null ? jsonObject["fecha_inscripcion"].ToString() : "";
                row["fecha_inicio_actividades"] = jsonObject["fecha_inicio_actividades"] != null ? jsonObject["fecha_inicio_actividades"].ToString() : "";
                row["estado"] = jsonObject["estado"] != null ? jsonObject["estado"].ToString() : "";
                row["condicion"] = jsonObject["condicion"] != null ? jsonObject["condicion"].ToString() : "";
                row["domicilio"] = jsonObject["domicilio"] != null ? jsonObject["domicilio"].ToString() : "";
                row["sistema_emicion"] = jsonObject["sistema_emicion"] != null ? jsonObject["sistema_emicion"].ToString() : "";
                row["actividad_comercio_exterior"] = jsonObject["actividad_comercio_exterior"] != null ? jsonObject["actividad_comercio_exterior"].ToString() : "";
                row["sistema_contabilidad"] = jsonObject["sistema_contabilidad"] != null ? jsonObject["sistema_contabilidad"].ToString() : "";
                row["actividades_economicas"] = jsonObject["actividades_economicas"] != null ? jsonObject["actividades_economicas"].ToString() : "";
                row["comprobante_pago"] = jsonObject["comprobante_pago"] != null ? jsonObject["comprobante_pago"].ToString() : "";
                row["sistema_emision_electronica"] = jsonObject["sistema_emision_electronica"] != null ? jsonObject["sistema_emision_electronica"].ToString() : "";
                row["emision_electronica_desde"] = jsonObject["emision_electronica_desde"] != null ? jsonObject["emision_electronica_desde"].ToString() : "";
                row["comprobantes_electronicos"] = jsonObject["comprobantes_electronicos"] != null ? jsonObject["comprobantes_electronicos"].ToString() : "";
                row["afiliado_ple"] = jsonObject["afiliado_ple"] != null ? jsonObject["afiliado_ple"].ToString() : "";
                row["padrones"] = jsonObject["padrones"] != null ? jsonObject["padrones"].ToString() : "";

                tabla_sunat.Rows.Add(row);
            }
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("sunat", tabla_sunat));
            /* peru -----------------------------------------------*/

            var tabla_TTP = new DataTable();
            tabla_TTP.Columns.Add(new DataColumn("Name"));
            tabla_TTP.Columns.Add(new DataColumn("IdentificationType"));
            tabla_TTP.Columns.Add(new DataColumn("Identification"));
            tabla_TTP.Columns.Add(new DataColumn("Correo"));
            tabla_TTP.Columns.Add(new DataColumn("Telefono"));
            tabla_TTP.Columns.Add(new DataColumn("Status"));
            if (dataIndi["PPT"] != null && !dataIndi["PPT"].ToString().Equals("[]"))
            {
                JsonData jsonObject = dataIndi["PPT"];
                if (jsonObject["Name"] != null && jsonObject["Identification"] != null)
                {
                    var row = tabla_TTP.NewRow();
                    row["Name"] = jsonObject["Name"].ToString();
                    row["IdentificationType"] = jsonObject["IdentificationType"].ToString();
                    row["Identification"] = jsonObject["Identification"].ToString();
                    row["Correo"] = jsonObject["Correo"].ToString();
                    row["Telefono"] = jsonObject["Telefono"].ToString();
                    row["Status"] = jsonObject["Status"].ToString();
                    tabla_TTP.Rows.Add(row);
                }

            }
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("PPT", tabla_TTP));

            var tabla_EPS = new DataTable();
            tabla_EPS.Columns.Add(new DataColumn("State"));
            tabla_EPS.Columns.Add(new DataColumn("Entity"));
            tabla_EPS.Columns.Add(new DataColumn("Regime"));
            tabla_EPS.Columns.Add(new DataColumn("EffectiveDate"));
            tabla_EPS.Columns.Add(new DataColumn("EndDate"));
            tabla_EPS.Columns.Add(new DataColumn("AffiliateType"));
            if (dataIndi["EPS"] != null && !dataIndi["EPS"].ToString().Equals("[]"))
            {
                foreach (JsonData jsonObject in dataIndi["EPS"])
                {
                    var row = tabla_EPS.NewRow();
                    row["State"] = jsonObject["State"].ToString();
                    row["Entity"] = jsonObject["Entity"].ToString();
                    row["Regime"] = jsonObject["Regime"].ToString();
                    row["EffectiveDate"] = jsonObject["EffectiveDate"].ToString();
                    row["EndDate"] = jsonObject["EndDate"].ToString();
                    row["AffiliateType"] = jsonObject["AffiliateType"].ToString();
                    tabla_EPS.Rows.Add(row);
                }

            }
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("EPS", tabla_EPS));

            var table_contaduria_bme = new DataTable();
            table_contaduria_bme.Columns.Add(new DataColumn("nombre_reportado"));
            table_contaduria_bme.Columns.Add(new DataColumn("numero_obligacion"));
            table_contaduria_bme.Columns.Add(new DataColumn("estado"));
            table_contaduria_bme.Columns.Add(new DataColumn("fecha_corte"));
            if (dataIndi["contaduria_bme_table_new"] != null && !dataIndi["contaduria_bme_table_new"].ToJson().ToString().Equals("") && !dataIndi["contaduria_bme_table_new"].ToJson().ToString().Equals("[]"))
            {
                foreach (JsonData jsonObject in dataIndi["contaduria_bme_table_new"])
                {
                    var row = table_contaduria_bme.NewRow();
                    row["nombre_reportado"] = jsonObject["name_reported"].ToString();
                    row["numero_obligacion"] = jsonObject["no_obligation"].ToString();
                    row["estado"] = jsonObject["state"].ToString();
                    row["fecha_corte"] = jsonObject["date"].ToString();

                    table_contaduria_bme.Rows.Add(row);
                }

            }
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("contaduria_bme", table_contaduria_bme));

            var table_sub_procuraduria = new DataTable();
            table_sub_procuraduria.Columns.Add(new DataColumn("num_siri"));
            table_sub_procuraduria.Columns.Add(new DataColumn("data"));

            if (dataIndi["procuraduria"] != null && !dataIndi["procuraduria"].ToJson().ToString().Equals("") && !dataIndi["procuraduria"].ToJson().ToString().Equals("[]"))
            {

                JsonData TempjsonObjectProcuraduria = JsonMapper.ToObject(dataIndi["procuraduria"].ToJson().ToString());
                String html_contaduria = TempjsonObjectProcuraduria["html_response"] != null ? TempjsonObjectProcuraduria["html_response"].ToString() : "";
                if (!html_contaduria.Equals("") && html_contaduria.Contains("Datos del ciudadano") && !html_contaduria.Contains("El ciudadano no presenta antecedentes"))
                {
                    try
                    {
                        html_contaduria = StringBetween(html_contaduria, "Datos del ciudadano", ".   &lt");
                        html_contaduria = html_contaduria.Replace("&lt;", "").Replace("/span&gt;", "").Replace("span&gt;", " ").Replace("/h2&gt;div class=\\&quot;datosConsultado\\&quot;&gt;", "").Trim();
                        html_contaduria += "<br>" + procuraduria_mensaje;
                        parameters[10] = new ReportParameter("mensaje_procuraduria", html_contaduria);
                    }
                    catch (Exception ex) { }
                }
                var TempjsonObjectProcuraduriaString = TempjsonObjectProcuraduria["data"].ToJson().ToString();
                if (TempjsonObjectProcuraduria["data"] != null && !TempjsonObjectProcuraduriaString.Equals("") && !TempjsonObjectProcuraduriaString.Equals("[]"))
                {
                    foreach (JsonData jsonObjectProcuraduria in TempjsonObjectProcuraduria["data"])
                    {
                        var row = table_sub_procuraduria.NewRow();
                        row["num_siri"] = jsonObjectProcuraduria["num_siri"].ToString();
                        row["data"] = jsonObjectProcuraduria.ToJson().ToString();
                        table_sub_procuraduria.Rows.Add(row);
                    }
                }
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("sub_procuraduria", table_sub_procuraduria));
            var tabla_rethus = new DataTable();
            tabla_rethus.Columns.Add(new DataColumn("TipoIdentificacion"));
            tabla_rethus.Columns.Add(new DataColumn("NroIdentificacion"));
            tabla_rethus.Columns.Add(new DataColumn("PrimerNombre"));
            tabla_rethus.Columns.Add(new DataColumn("SegundoNombre"));
            tabla_rethus.Columns.Add(new DataColumn("PrimerApellido"));
            tabla_rethus.Columns.Add(new DataColumn("SegundoApellido"));
            tabla_rethus.Columns.Add(new DataColumn("EstadoIdentificacion"));
            if (dataIndi["rethus"] != null && !dataIndi["rethus"].ToJson().ToString().Equals("") && !dataIndi["rethus"].ToJson().ToString().Equals("[]"))
            {

                if (dataIndi["rethus"] != null && !dataIndi["rethus"].ToString().Equals("[]"))
                {
                    foreach (JsonData jsonData in dataIndi["rethus"])
                    {
                        var rowRethus = tabla_rethus.NewRow();
                        rowRethus["TipoIdentificacion"] = jsonData["TipoIdentificacion"]?.ToString();
                        rowRethus["NroIdentificacion"] = jsonData["NroIdentificacion"]?.ToString();
                        rowRethus["PrimerNombre"] = jsonData["PrimerNombre"]?.ToString();
                        rowRethus["SegundoNombre"] = jsonData["SegundoNombre"]?.ToString();
                        rowRethus["PrimerApellido"] = jsonData["PrimerApellido"]?.ToString();
                        rowRethus["SegundoApellido"] = jsonData["SegundoApellido"]?.ToString();
                        rowRethus["EstadoIdentificacion"] = jsonData["EstadoIdentificacion"]?.ToString();

                        tabla_rethus.Rows.Add(rowRethus);
                    }
                }
            }
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("rethus", tabla_rethus));

            var tabla_inpec = new DataTable();
            tabla_inpec.Columns.Add(new DataColumn("Identificacion"));
            tabla_inpec.Columns.Add(new DataColumn("numeroUnicoInpec"));
            tabla_inpec.Columns.Add(new DataColumn("nombre"));
            tabla_inpec.Columns.Add(new DataColumn("genero"));
            tabla_inpec.Columns.Add(new DataColumn("estadoDeIngreso"));
            tabla_inpec.Columns.Add(new DataColumn("situacionJuridica"));
            tabla_inpec.Columns.Add(new DataColumn("establecimientoACargo"));


            if (dataIndi["Inpec"] != null && !dataIndi["Inpec"].ToString().Equals(""))
            {
                var rowInpec = tabla_inpec.NewRow();
                rowInpec["Identificacion"] = dataIndi["Inpec"]?["Identificacion"]?.ToString();
                rowInpec["numeroUnicoInpec"] = dataIndi["Inpec"]?["numeroUnicoInpec"]?.ToString();
                rowInpec["nombre"] = dataIndi["Inpec"]?["nombre"]?.ToString();
                rowInpec["genero"] = dataIndi["Inpec"]?["genero"]?.ToString();
                rowInpec["estadoDeIngreso"] = dataIndi["Inpec"]?["estadoDeIngreso"]?.ToString();
                rowInpec["situacionJuridica"] = dataIndi["Inpec"]?["situacionJuridica"]?.ToString();
                rowInpec["establecimientoACargo"] = dataIndi["Inpec"]?["establecimientoACargo"]?.ToString();

                tabla_inpec.Rows.Add(rowInpec);

            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("Inpec", tabla_inpec));


            var tabla_MedidasCorrectivas = new DataTable();
            tabla_MedidasCorrectivas.Columns.Add(new DataColumn("Identificacion"));
            tabla_MedidasCorrectivas.Columns.Add(new DataColumn("Expediente"));
            tabla_MedidasCorrectivas.Columns.Add(new DataColumn("Formato"));
            tabla_MedidasCorrectivas.Columns.Add(new DataColumn("Infractor"));
            tabla_MedidasCorrectivas.Columns.Add(new DataColumn("Fecha"));
            tabla_MedidasCorrectivas.Columns.Add(new DataColumn("Departamento"));
            tabla_MedidasCorrectivas.Columns.Add(new DataColumn("Municipio"));


            if (dataIndi["MedidasCorrectivas"] != null && !dataIndi["MedidasCorrectivas"].ToString().Equals("[]"))
            {
                var rowMedidasCorrectivas = tabla_MedidasCorrectivas.NewRow();
                rowMedidasCorrectivas["Identificacion"] = dataIndi["MedidasCorrectivas"]?["Identificacin"]?.ToString();
                rowMedidasCorrectivas["Expediente"] = dataIndi["MedidasCorrectivas"]?["Expendiente"]?.ToString();
                rowMedidasCorrectivas["Formato"] = dataIndi["MedidasCorrectivas"]?["Formato"]?.ToString();
                rowMedidasCorrectivas["Infractor"] = dataIndi["MedidasCorrectivas"]?["Infractor"]?.ToString();
                rowMedidasCorrectivas["Fecha"] = dataIndi["MedidasCorrectivas"]?["Fecha"]?.ToString();
                rowMedidasCorrectivas["Departamento"] = dataIndi["MedidasCorrectivas"]?["Departamento"]?.ToString();
                rowMedidasCorrectivas["Municipio"] = dataIndi["MedidasCorrectivas"]?["Municipio"]?.ToString();

                tabla_MedidasCorrectivas.Rows.Add(rowMedidasCorrectivas);

            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("MedidasCorrectivas", tabla_MedidasCorrectivas));


            var tabla_lista_ejercito = new DataTable();
            tabla_lista_ejercito.Columns.Add(new DataColumn("Tipo_reservista"));
            tabla_lista_ejercito.Columns.Add(new DataColumn("Nombre"));
            tabla_lista_ejercito.Columns.Add(new DataColumn("Lugar"));
            tabla_lista_ejercito.Columns.Add(new DataColumn("Direccion"));

            if (dataIndi["ejercito"] != null && !dataIndi["ejercito"].ToJson().ToString().Equals("") && !dataIndi["ejercito"].ToJson().ToString().Equals("[]"))
            {
                JsonData dataEjercito = JsonMapper.ToObject(dataIndi["ejercito"].ToString());
                var rowEjercito = tabla_lista_ejercito.NewRow();
                rowEjercito["Tipo_reservista"] = dataEjercito["TipoReservista"];
                rowEjercito["Nombre"] = dataEjercito["Nombre"];
                rowEjercito["Lugar"] = dataEjercito["Lugar"];
                rowEjercito["Direccion"] = dataEjercito["Direccion"];
                tabla_lista_ejercito.Rows.Add(rowEjercito);
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_ejercito", tabla_lista_ejercito));

            var tabla_listas_rama_judicial = new DataTable();
            tabla_listas_rama_judicial.Columns.Add(new DataColumn("NumeroRadicado"));
            tabla_listas_rama_judicial.Columns.Add(new DataColumn("Identificacion"));
            tabla_listas_rama_judicial.Columns.Add(new DataColumn("NombreSujeto"));
            tabla_listas_rama_judicial.Columns.Add(new DataColumn("Juzgado"));
            tabla_listas_rama_judicial.Columns.Add(new DataColumn("Representante"));
            tabla_listas_rama_judicial.Columns.Add(new DataColumn("Ciudad"));

            if (dataIndi["rama_judicial"] != null && !dataIndi["rama_judicial"].ToJson().ToString().Equals("") && !dataIndi["rama_judicial"].ToJson().ToString().Equals("[]"))
            {
                JsonData JSonObjectRamaJ = JsonMapper.ToObject(dataIndi["rama_judicial"].ToJson().ToString());
                JSonObjectRamaJ = JsonMapper.ToObject(JSonObjectRamaJ["data"].ToJson().ToString());
                if (!JSonObjectRamaJ.ToJson().ToString().Equals("") && !JSonObjectRamaJ.ToJson().ToString().Equals("[]"))
                {
                    foreach (var prop in JSonObjectRamaJ.PropertyNames)
                    {

                        string key = prop.ToString();
                        JsonData data_temp = JSonObjectRamaJ[prop.ToString()];
                        foreach (JsonData jsonObject in data_temp)
                        {
                            var rowRamaJudicial = tabla_listas_rama_judicial.NewRow();

                            rowRamaJudicial["NumeroRadicado"] = jsonObject["num_rad"].ToString();
                            rowRamaJudicial["Identificacion"] = jsonObject["identificacion"].ToString();
                            rowRamaJudicial["NombreSujeto"] = jsonObject["nombre"].ToString();
                            rowRamaJudicial["Juzgado"] = jsonObject["juzgado"].ToString();
                            rowRamaJudicial["Representante"] = jsonObject["representante"].ToString();
                            rowRamaJudicial["Ciudad"] = key;
                            tabla_listas_rama_judicial.Rows.Add(rowRamaJudicial);

                        }
                    }
                }
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_rama_judicial", tabla_listas_rama_judicial));

            var listas_rama_judicial_jepms = new DataTable();

            listas_rama_judicial_jepms.Columns.Add(new DataColumn("Nombre"));
            listas_rama_judicial_jepms.Columns.Add(new DataColumn("Identificacion"));
            listas_rama_judicial_jepms.Columns.Add(new DataColumn("Ciudad"));
            listas_rama_judicial_jepms.Columns.Add(new DataColumn("Enlace"));
            if (dataIndi["rama_judicial_jepms"] != null && !dataIndi["rama_judicial_jepms"].ToJson().ToString().Equals("") && !dataIndi["rama_judicial_jepms"].ToJson().ToString().Equals("[]"))
            {
                JsonData JSonObjectRamaJ = JsonMapper.ToObject(dataIndi["rama_judicial_jepms"].ToJson().ToString());

                foreach (JsonData jsonObject in JSonObjectRamaJ)
                {
                    if (Convert.ToBoolean(jsonObject["inRisk"].ToString()) || !Convert.ToBoolean(jsonObject["available"].ToString()))
                    {
                        string name = jsonObject["NameResult"] != null ? jsonObject["NameResult"].ToString() : "";
                        name = jsonObject["available"] != null && !Convert.ToBoolean(jsonObject["inRisk"].ToString()) ? "JUZGADO NO DISPONIBLE" : name;
                        DataRow rowRamaJudicial = listas_rama_judicial_jepms.NewRow();
                        rowRamaJudicial["Nombre"] = name;
                        rowRamaJudicial["Identificacion"] = jsonObject["IdentificationNumberResult"] != null ? jsonObject["IdentificationNumberResult"].ToString() : "";
                        rowRamaJudicial["Ciudad"] = jsonObject["CityName"] != null ? jsonObject["CityName"].ToString() : "";
                        rowRamaJudicial["Enlace"] = jsonObject["Link"] != null ? jsonObject["Link"].ToString() : "";
                        listas_rama_judicial_jepms.Rows.Add(rowRamaJudicial);
                    }
                }
            }
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_rama_judicial_jepms", listas_rama_judicial_jepms));


            var tabla_listas_rama_judicial_new_administrativo = reporteCompletoGetRowTablesRama();
            var tabla_listas_rama_judicial_new_civil = reporteCompletoGetRowTablesRama();
            var tabla_listas_rama_judicial_new_laboral = reporteCompletoGetRowTablesRama();
            var tabla_listas_rama_judicial_new_familia = reporteCompletoGetRowTablesRama();
            var tabla_listas_rama_judicial_new_JEPMS = reporteCompletoGetRowTablesRama();
            var tabla_listas_rama_judicial_new_otros = reporteCompletoGetRowTablesRama();

            if (dataIndi["rama_judicial_new"] != null && !dataIndi["rama_judicial_new"].ToJson().ToString().Equals("") && !dataIndi["rama_judicial_new"].ToJson().ToString().Equals("[]"))
            {
                JsonData JSonObjectRamaJ = JsonMapper.ToObject(dataIndi["rama_judicial_new"].ToJson().ToString());

                foreach (JsonData jsonObject in JSonObjectRamaJ)
                {
                    if (jsonObject["message"] == null)
                    {
                        string despacho = jsonObject["despacho"].ToString().ToLower();
                        DataRow rowRamaJudicial;
                        if (despacho.Contains("administrativo"))
                        {
                            rowRamaJudicial = tabla_listas_rama_judicial_new_administrativo.NewRow();
                        }
                        else if (despacho.Contains("civil"))
                        {
                            rowRamaJudicial = tabla_listas_rama_judicial_new_civil.NewRow();
                        }
                        else if (despacho.Contains("laboral"))
                        {
                            rowRamaJudicial = tabla_listas_rama_judicial_new_laboral.NewRow();
                        }
                        else if (despacho.Contains("familia"))
                        {
                            rowRamaJudicial = tabla_listas_rama_judicial_new_familia.NewRow();
                        }
                        else if (despacho.Contains("penal") || despacho.Contains("con función de control de garantías") || despacho.Contains("con función de conocimiento"))
                        {
                            rowRamaJudicial = tabla_listas_rama_judicial_new_JEPMS.NewRow();
                        }
                        else
                        {
                            rowRamaJudicial = tabla_listas_rama_judicial_new_otros.NewRow();
                        }
                        rowRamaJudicial["llaveProceso"] = jsonObject["llaveProceso"] != null ? jsonObject["llaveProceso"].ToString() : "";
                        rowRamaJudicial["fechaProceso"] = jsonObject["fechaProceso"] != null ? jsonObject["fechaProceso"].ToString() : "";
                        rowRamaJudicial["fechaUltimaActuacion"] = jsonObject["fechaUltimaActuacion"] != null ? jsonObject["fechaUltimaActuacion"].ToString() : "";
                        rowRamaJudicial["despacho"] = jsonObject["despacho"] != null ? jsonObject["despacho"].ToString() : "";
                        rowRamaJudicial["departamento"] = jsonObject["departamento"] != null ? jsonObject["departamento"].ToString() : "";

                        if (jsonObject["sujetosProcesales"] != null && jsonObject["sujetosProcesales"].ToString().Length > 300)
                            jsonObject["sujetosProcesales"] = jsonObject["sujetosProcesales"].ToString().Substring(0, 300) + "(Ver mas con el numero de radicado)";
                        rowRamaJudicial["sujetosProcesales"] = jsonObject["sujetosProcesales"] != null ? jsonObject["sujetosProcesales"].ToString() : "";

                        if (despacho.Contains("administrativo"))
                        {
                            tabla_listas_rama_judicial_new_administrativo.Rows.Add(rowRamaJudicial);
                        }
                        else if (despacho.Contains("civil"))
                        {

                            tabla_listas_rama_judicial_new_civil.Rows.Add(rowRamaJudicial);
                        }
                        else if (despacho.Contains("laboral"))
                        {

                            tabla_listas_rama_judicial_new_laboral.Rows.Add(rowRamaJudicial);
                        }
                        else if (despacho.Contains("familia"))
                        {

                            tabla_listas_rama_judicial_new_familia.Rows.Add(rowRamaJudicial);
                        }
                        else if (despacho.Contains("penal") || despacho.Contains("con función de control de garantías") || despacho.Contains("con función de conocimiento"))
                        {

                            tabla_listas_rama_judicial_new_JEPMS.Rows.Add(rowRamaJudicial);
                        }
                        else
                        {
                            tabla_listas_rama_judicial_new_otros.Rows.Add(rowRamaJudicial);
                        }
                    }
                }
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_rama_judicial_new_administrativo", tabla_listas_rama_judicial_new_administrativo));
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_rama_judicial_new_civil", tabla_listas_rama_judicial_new_civil));
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_rama_judicial_new_laboral", tabla_listas_rama_judicial_new_laboral));
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_rama_judicial_new_familia", tabla_listas_rama_judicial_new_familia));
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_rama_judicial_new_JEPMS", tabla_listas_rama_judicial_new_JEPMS));
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_rama_judicial_new_otros", tabla_listas_rama_judicial_new_otros));

            var tabla_listas_super_sociedades = new DataTable();

            tabla_listas_super_sociedades.Columns.Add(new DataColumn("ActividadCIIU"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("VersionCIIU"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("DescripcionCIIU"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("DireccionJudicial"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("CiudadJudicial"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("DepartamentoJudicial"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("DireccionDomicilio"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("CiudadDomicilio"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("DepartamentoDomicilio"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("CorreoElectronico"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("Nit"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("RazonSocial"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("Expediente"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("Sigla"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("ObjetoSocial"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("TipoSociedad"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("Estado"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("FechaEstado"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("EtapaSituacion"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("FechaSituacion"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("FechaEtapa"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("Causal"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("Contador"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("RevisorFiscal"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("RepresentanteLegal"));
            tabla_listas_super_sociedades.Columns.Add(new DataColumn("RepresentanteLegalPrimerSuplente"));

            var tabla_listas_juntas_principal = new DataTable();
            tabla_listas_juntas_principal.Columns.Add(new DataColumn("Nombre"));
            var tabla_listas_juntas_suplente = new DataTable();
            tabla_listas_juntas_suplente.Columns.Add(new DataColumn("Nombre"));

            if (dataIndi["super_sociedades"] != null && !dataIndi["super_sociedades"].ToJson().ToString().Equals("") && !dataIndi["super_sociedades"].ToJson().ToString().Equals("[]"))
            {
                JsonData dataSuperSociedades = JsonMapper.ToObject(dataIndi["super_sociedades"].ToString());
                var rowSuperSociedades = tabla_listas_super_sociedades.NewRow();
                rowSuperSociedades["ActividadCIIU"] = dataSuperSociedades["ActividadCIIU"];
                rowSuperSociedades["VersionCIIU"] = dataSuperSociedades["VersionCIIU"];
                rowSuperSociedades["DescripcionCIIU"] = dataSuperSociedades["DescripcionCIIU"];
                rowSuperSociedades["DireccionJudicial"] = dataSuperSociedades["DireccionJudicial"];
                rowSuperSociedades["CiudadJudicial"] = dataSuperSociedades["CiudadJudicial"];
                rowSuperSociedades["DepartamentoJudicial"] = dataSuperSociedades["DepartamentoJudicial"];
                rowSuperSociedades["CiudadDomicilio"] = dataSuperSociedades["CiudadDomicilio"];
                rowSuperSociedades["DepartamentoDomicilio"] = dataSuperSociedades["DepartamentoDomicilio"];
                rowSuperSociedades["DireccionDomicilio"] = dataSuperSociedades["DireccionDomicilio"];
                rowSuperSociedades["CorreoElectronico"] = dataSuperSociedades["CorreoElectronico"];
                rowSuperSociedades["Nit"] = dataSuperSociedades["Nit"];
                rowSuperSociedades["RazonSocial"] = dataSuperSociedades["RazonSocial"];
                rowSuperSociedades["Expediente"] = dataSuperSociedades["Expediente"];
                rowSuperSociedades["Sigla"] = dataSuperSociedades["Sigla"];
                rowSuperSociedades["ObjetoSocial"] = dataSuperSociedades["ObjetoSocial"];
                rowSuperSociedades["TipoSociedad"] = dataSuperSociedades["TipoSociedad"];
                rowSuperSociedades["Estado"] = dataSuperSociedades["Estado"];
                rowSuperSociedades["FechaEstado"] = dataSuperSociedades["FechaEstado"];
                rowSuperSociedades["EtapaSituacion"] = dataSuperSociedades["EtapaSituacion"];
                rowSuperSociedades["FechaSituacion"] = dataSuperSociedades["FechaSituacion"];
                rowSuperSociedades["FechaEtapa"] = dataSuperSociedades["FechaEtapa"];
                rowSuperSociedades["Causal"] = dataSuperSociedades["Causal"];
                rowSuperSociedades["Contador"] = dataSuperSociedades["Contador"];
                rowSuperSociedades["RevisorFiscal"] = dataSuperSociedades["RevisorFiscal"];
                rowSuperSociedades["RepresentanteLegal"] = dataSuperSociedades["RepresentanteLegal"];
                rowSuperSociedades["RepresentanteLegalPrimerSuplente"] = dataSuperSociedades["RepresentanteLegalPrimerSuplente"];
                tabla_listas_super_sociedades.Rows.Add(rowSuperSociedades);

                JsonData jsonJuntaDirectivaPrincipal = JsonMapper.ToObject(dataSuperSociedades["JuntaDirectivaPrincipal"].ToJson().ToString());
                foreach (JsonData item in jsonJuntaDirectivaPrincipal)
                {
                    var rowListasJuntaPrincipal = tabla_listas_juntas_principal.NewRow();
                    rowListasJuntaPrincipal["Nombre"] = item;
                    tabla_listas_juntas_principal.Rows.Add(rowListasJuntaPrincipal);
                }
                JsonData jsonJuntaDirectivaSuplente = JsonMapper.ToObject(dataSuperSociedades["JuntaDirectivaSuplente"].ToJson().ToString());
                foreach (JsonData item in jsonJuntaDirectivaSuplente)
                {
                    var rowListasJuntaSuplente = tabla_listas_juntas_suplente.NewRow();
                    rowListasJuntaSuplente["Nombre"] = item;
                    tabla_listas_juntas_suplente.Rows.Add(rowListasJuntaSuplente);
                }

            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_super_sociedades", tabla_listas_super_sociedades));
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_juntas_principal", tabla_listas_juntas_principal));
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_juntas_suplente", tabla_listas_juntas_suplente));

            var tabla_lista_rues = new DataTable();
            var tabla_lista_ruesRM = new DataTable();

            if ((dataIndi["rues"] != null && !dataIndi["rues"].ToJson().ToString().Equals("") && !dataIndi["rues"].ToJson().ToString().Equals("[]")) ||
                (dataIndi["new_rues"] != null && !dataIndi["new_rues"].ToJson().ToString().Equals("") && !dataIndi["new_rues"].ToJson().ToString().Equals("[]")))
            {
                tabla_lista_rues.Columns.Add(new DataColumn("RazonSocialONombre"));
                tabla_lista_rues.Columns.Add(new DataColumn("Nit"));
                tabla_lista_rues.Columns.Add(new DataColumn("Estado"));
                tabla_lista_rues.Columns.Add(new DataColumn("municipio"));
                tabla_lista_rues.Columns.Add(new DataColumn("Categoria"));
            }
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_rues", tabla_lista_rues));
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_ruesRM", tabla_lista_ruesRM));

            var tabla_lista_simit = new DataTable();
            tabla_lista_simit.Columns.Add(new DataColumn("Resolucion"));
            tabla_lista_simit.Columns.Add(new DataColumn("FechaResolucion"));
            tabla_lista_simit.Columns.Add(new DataColumn("Comparendo"));
            tabla_lista_simit.Columns.Add(new DataColumn("FechaComparendo"));
            tabla_lista_simit.Columns.Add(new DataColumn("Secretaria"));
            tabla_lista_simit.Columns.Add(new DataColumn("NombreInfractor"));
            tabla_lista_simit.Columns.Add(new DataColumn("Estado"));
            tabla_lista_simit.Columns.Add(new DataColumn("Infraccion"));
            tabla_lista_simit.Columns.Add(new DataColumn("ValorMulta"));
            tabla_lista_simit.Columns.Add(new DataColumn("InteresMora"));
            tabla_lista_simit.Columns.Add(new DataColumn("ValorAdicional"));
            tabla_lista_simit.Columns.Add(new DataColumn("ValorPagar"));

            if (dataIndi["simit"] != null && !dataIndi["simit"].ToJson().ToString().Equals("") && !dataIndi["simit"].ToJson().ToString().Equals("[]"))
            {
                JsonData dataSimit = JsonMapper.ToObject(dataIndi["simit"].ToString());
                foreach (JsonData item in dataSimit)
                {
                    var rowsimit = tabla_lista_simit.NewRow();
                    rowsimit["Resolucion"] = item["Resolucion"];
                    rowsimit["FechaResolucion"] = item["FechaResolucion"];
                    rowsimit["Comparendo"] = item["Comparendo"];
                    rowsimit["FechaComparendo"] = item["FechaComparendo"];
                    rowsimit["Secretaria"] = item["Secretaria"];
                    rowsimit["NombreInfractor"] = item["NombreInfractor"];
                    rowsimit["Estado"] = item["Estado"];
                    rowsimit["Infraccion"] = item["Infraccion"];
                    rowsimit["ValorMulta"] = item["ValorMulta"];
                    rowsimit["InteresMora"] = item["InteresMora"];
                    rowsimit["ValorAdicional"] = item["ValorAdicional"];
                    rowsimit["ValorPagar"] = item["ValorPagar"];
                    tabla_lista_simit.Rows.Add(rowsimit);
                }
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_simit", tabla_lista_simit));

            var tabla_lista_simit_new = new DataTable();
            tabla_lista_simit_new.Columns.Add(new DataColumn("type"));
            tabla_lista_simit_new.Columns.Add(new DataColumn("notification"));
            tabla_lista_simit_new.Columns.Add(new DataColumn("plate"));
            tabla_lista_simit_new.Columns.Add(new DataColumn("secretaryship"));
            tabla_lista_simit_new.Columns.Add(new DataColumn("infringement"));
            tabla_lista_simit_new.Columns.Add(new DataColumn("state"));
            tabla_lista_simit_new.Columns.Add(new DataColumn("amount"));
            tabla_lista_simit_new.Columns.Add(new DataColumn("amountToPaid"));

            if (dataIndi["simit_new"] != null && !dataIndi["simit_new"].ToJson().ToString().Equals("") && !dataIndi["simit_new"].ToJson().ToString().Equals("[]"))
            {
                JsonData dataSimit = JsonMapper.ToObject(dataIndi["simit_new"].ToString());
                foreach (JsonData item in dataSimit)
                {
                    var rowsimit = tabla_lista_simit_new.NewRow();
                    rowsimit["type"] = item["type"];
                    rowsimit["notification"] = item["notification"];
                    rowsimit["plate"] = item["plate"];
                    rowsimit["secretaryship"] = item["secretaryship"];
                    rowsimit["infringement"] = item["infringement"];
                    rowsimit["state"] = item["state"];
                    rowsimit["amount"] = item["amount"];
                    rowsimit["amountToPaid"] = item["amountToPaid"];
                    tabla_lista_simit_new.Rows.Add(rowsimit);
                }
            }

            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource("listas_simit_new", tabla_lista_simit_new));

            /* Servicios adicionales -----------------------------------------------*/

            var matriz_riesgo = new DataTable();
            var rowriesgo = matriz_riesgo.NewRow();
            for (int y = 1; y <= 4; y++)
            {
                for (int x = 1; x <= 5; x++)
                {
                    matriz_riesgo.Columns.Add(new DataColumn("p" + y + x));
                    rowriesgo["p" + y + x] = 0;
                }
            }

            string[] list_keys = { "GridView3_tabla_listas_restrictivas", "GridView7_tabla_listas_la_lf", "GridViewG1_1_tabla_listas_la_lf_admin", "GridView5_tabla_lista_afectacion_financiera", "GridViewG2_1_tabla_lista_sanciones_administrativas", "GridView6_tabla_listas_informativa_peps" };
            foreach (string key in list_keys)
            {
                if (dataIndi[key] != null && !dataIndi[key].ToString().Equals("[]"))
                {
                    foreach (JsonData jsonObject in JsonMapper.ToObject(dataIndi[key].ToString()))
                    {
                        int orden = Int32.Parse(jsonObject["Orden"].ToString().Trim());
                        int prioridad = Int32.Parse(jsonObject["Prioridad"].ToString().Trim());
                        if (orden <= 5 && prioridad <= 4)
                        {
                            int value = Int32.Parse(rowriesgo["p" + prioridad + orden].ToString().Trim());
                            value += 1;
                            rowriesgo["p" + prioridad + orden] = value;
                        }
                    }
                }
            }
            matriz_riesgo.Rows.Add(rowriesgo);
            ReportViewerReporteConsolidadoNew.DataSources.Add(new ReportDataSource()
            {
                Name = "matriz_riesgo",
                Value = matriz_riesgo
            });

            ReportViewerReporteConsolidadoNew.ReportPath = contentRootPath + "\\Reports\\ReporteConsolidadoNew.rdlc";

            ReportViewerReporteConsolidadoNew.SetParameters(parameters);
            ReportViewerReporteConsolidadoNew.Refresh();

            ReportViewerReporteConsolidadoNew.SubreportProcessing += SubreportProcessingEventHandler;

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            byte[] bytes = ReportViewerReporteConsolidadoNew.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            PdfSharp.Pdf.PdfDocument document = new PdfSharp.Pdf.PdfDocument();
            PdfSharp.Pdf.PdfDocument tempPDFDoc = PdfSharp.Pdf.IO.PdfReader.Open(new MemoryStream(bytes), PdfDocumentOpenMode.Import);

            for (int i = 0; i < tempPDFDoc.PageCount; i++)
            {
                PdfSharp.Pdf.PdfPage page = tempPDFDoc.Pages[i];
                document.AddPage(page);
            }

            XFont font = new XFont("Verdana", 8);
            XBrush brush = XBrushes.Black;

            // Add the page counter.
            string noPages = document.Pages.Count.ToString();
            for (int i = 0; i < document.Pages.Count; ++i)
            {
                PdfSharp.Pdf.PdfPage page = document.Pages[i];

                // Make a layout rectangle.
                XRect layoutRectangle = new XRect(0/*X*/, page.Height - font.Height/*Y*/, page.Width/*Width*/, font.Height/*Height*/);

                using (XGraphics gfx = XGraphics.FromPdfPage(page))
                {
                    gfx.DrawString(
                        "Pagina " + (i + 1).ToString() + " de " + noPages,
                        font,
                        brush,
                        layoutRectangle,
                        XStringFormats.Center);
                }
            }

            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);
            var options = document.Options;
            byte[] buffer = new byte[0];
            buffer = stream.ToArray();
            var contentLength = buffer.Length;
            response.Clear();
            response.ContentType = "application/pdf";

            stream.Close();
            string namepdf = "Reporte_" + System.DateTime.Now.ToString().Replace(" ", "_").Replace(",", "").Replace(".", "").Replace(":", "").Replace("/", "-");
            Object[] objects = new Object[4];
            objects[0] = bytes;
            objects[1] = mimeType;
            objects[2] = namepdf;
            objects[3] = extension;
            return objects;
        }
        protected DataTable reporteCompletoGetRowTables()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Prioridad"));
            table.Columns.Add(new DataColumn("Tipo_documento"));
            table.Columns.Add(new DataColumn("Documento"));
            table.Columns.Add(new DataColumn("Nombre"));
            table.Columns.Add(new DataColumn("Nombre_lista"));
            table.Columns.Add(new DataColumn("Tipo_persona"));
            table.Columns.Add(new DataColumn("Cargo"));
            table.Columns.Add(new DataColumn("Alias"));
            table.Columns.Add(new DataColumn("Zona"));
            table.Columns.Add(new DataColumn("otra_informacion"));
            return table;
        }
        private void addRowValuesListasRestrictivas(DataRow row, JsonData jsonObject)
        {
            row["Prioridad"] = jsonObject["Prioridad"].ToString().Trim();
            row["Tipo_documento"] = jsonObject["TipoDocumento"].ToString().Trim();
            row["Documento"] = jsonObject["DocumentoIdentidad"].ToString().Trim();
            row["Nombre"] = jsonObject["NombreCompleto"].ToString().Trim();
            row["Nombre_lista"] = jsonObject["NombreTipoLista"].ToString().Trim();
            row["Cargo"] = jsonObject["Delito"].ToString().Trim();
            row["Zona"] = jsonObject["Zona"].ToString().Trim();
            row["otra_informacion"] = jsonObject["OtraInformacion"].ToString().Trim();
        }
        void SubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            string data = e.Parameters["data"].Values[0];

            JsonData jsonObjectProcuraduria = JsonMapper.ToObject(data.ToString());

            var procuraduria = JsonConvert.DeserializeObject<List<ProcuraduriaData>>(data);

            ReportParameter[] parametersProc = new ReportParameter[4];
            string num_siri = procuraduria[0].num_siri.ToString();
            parametersProc[1] = new ReportParameter("num_siri", num_siri);

            var tabla_procuraduria_sanciones = new DataTable();
            tabla_procuraduria_sanciones.Columns.Add(new DataColumn("Sancion"));
            tabla_procuraduria_sanciones.Columns.Add(new DataColumn("Termino"));
            tabla_procuraduria_sanciones.Columns.Add(new DataColumn("Clase"));
            tabla_procuraduria_sanciones.Columns.Add(new DataColumn("Suspendida"));
            tabla_procuraduria_sanciones.Columns.Add(new DataColumn("Suspension_art"));

            var tabla_procuraduria_delitos = new DataTable();
            tabla_procuraduria_delitos.Columns.Add(new DataColumn("Descripcion"));

            var tabla_procuraduria_instancias = new DataTable();
            tabla_procuraduria_instancias.Columns.Add(new DataColumn("Nombre"));
            tabla_procuraduria_instancias.Columns.Add(new DataColumn("Autoridad"));
            tabla_procuraduria_instancias.Columns.Add(new DataColumn("Fecha_provincia"));
            tabla_procuraduria_instancias.Columns.Add(new DataColumn("Fecha_efecto_juridicos"));

            var tabla_procuraduria_eventos = new DataTable();
            tabla_procuraduria_eventos.Columns.Add(new DataColumn("Nombre_causa"));
            tabla_procuraduria_eventos.Columns.Add(new DataColumn("Entidad"));
            tabla_procuraduria_eventos.Columns.Add(new DataColumn("Tipo_acto"));
            tabla_procuraduria_eventos.Columns.Add(new DataColumn("Fecha_acto"));

            var tabla_procuraduria_inhabilidades = new DataTable();
            tabla_procuraduria_inhabilidades.Columns.Add(new DataColumn("Siri"));
            tabla_procuraduria_inhabilidades.Columns.Add(new DataColumn("Modulo"));
            tabla_procuraduria_inhabilidades.Columns.Add(new DataColumn("Inhabilidad_legal"));
            tabla_procuraduria_inhabilidades.Columns.Add(new DataColumn("Fecha_inicio"));
            tabla_procuraduria_inhabilidades.Columns.Add(new DataColumn("Fecha_fin"));
            tabla_procuraduria_inhabilidades.Columns.Add(new DataColumn("Suspension_art"));

            var tabla_perdida_investidura = new DataTable();
            tabla_perdida_investidura.Columns.Add(new DataColumn("Sancion"));
            tabla_perdida_investidura.Columns.Add(new DataColumn("Termino"));
            tabla_perdida_investidura.Columns.Add(new DataColumn("Clase_sancion"));
            tabla_perdida_investidura.Columns.Add(new DataColumn("Entidad"));
            tabla_perdida_investidura.Columns.Add(new DataColumn("Cargo"));

            try
            {
                if (procuraduria[0].sanciones != null)
                {

                    foreach (var jsonObject in procuraduria[0].sanciones)
                    {
                        var rowSanciones = tabla_procuraduria_sanciones.NewRow();

                        rowSanciones["Sancion"] = jsonObject.sancion.ToString();
                        rowSanciones["Termino"] = DecodeFromUtf8(jsonObject.termino.ToString());
                        rowSanciones["Clase"] = DecodeFromUtf8(jsonObject.clase.ToString());
                        rowSanciones["Suspendida"] = jsonObject.suspendida.ToString();
                        if (jsonObject.Suspension_art != null)
                            rowSanciones["Suspension_art"] = DecodeFromUtf8(jsonObject.Suspension_art.ToString());
                        tabla_procuraduria_sanciones.Rows.Add(rowSanciones);
                    }
                }
                //if (jsonObjectProcuraduria["sanciones"] != null)
                //{

                //    foreach (JsonData jsonObject in jsonObjectProcuraduria["sanciones"])
                //    {
                //        var rowSanciones = tabla_procuraduria_sanciones.NewRow();

                //        rowSanciones["Sancion"] = jsonObject["sancion"].ToString();
                //        rowSanciones["Termino"] = DecodeFromUtf8(jsonObject["termino"].ToString());
                //        rowSanciones["Clase"] = DecodeFromUtf8(jsonObject["clase"].ToString());
                //        rowSanciones["Suspendida"] = jsonObject["suspendida"].ToString();
                //        if (jsonObject["suspension_art"] != null)
                //            rowSanciones["Suspension_art"] = DecodeFromUtf8(jsonObject["suspension_art"].ToString());
                //        tabla_procuraduria_sanciones.Rows.Add(rowSanciones);
                //    }
                //}
                if (procuraduria[0].delitos != null)
                {
                    foreach (var jsonObject in procuraduria[0].delitos)
                    {
                        var rowDelitos = tabla_procuraduria_delitos.NewRow();

                        rowDelitos["Descripcion"] = DecodeFromUtf8(jsonObject.descripcion.ToString());
                        tabla_procuraduria_delitos.Rows.Add(rowDelitos);
                    }
                }
                if (procuraduria[0].instancias != null)
                {
                    foreach (var jsonObject in procuraduria[0].instancias)
                    {
                        var rowInstancias = tabla_procuraduria_instancias.NewRow();

                        rowInstancias["Nombre"] = DecodeFromUtf8(jsonObject.nombre.ToString());
                        rowInstancias["Autoridad"] = DecodeFromUtf8(jsonObject.autoridad.ToString());
                        rowInstancias["Fecha_provincia"] = DecodeFromUtf8(jsonObject.fecha_provincia.ToString());
                        rowInstancias["Fecha_efecto_juridicos"] = DecodeFromUtf8(jsonObject.fecha_efecto_juridicos.ToString());
                        tabla_procuraduria_instancias.Rows.Add(rowInstancias);
                    }
                }
                if (procuraduria[0].eventos != null)
                {
                    foreach (var jsonObject in procuraduria[0].eventos)
                    {
                        var rowEventos = tabla_procuraduria_eventos.NewRow();

                        rowEventos["Nombre_causa"] = jsonObject.nombre_causa.ToString();
                        rowEventos["Entidad"] = jsonObject.entidad.ToString();
                        rowEventos["Tipo_acto"] = jsonObject.tipo_acto.ToString();
                        rowEventos["Fecha_acto"] = jsonObject.fecha_acto.ToString();
                        tabla_procuraduria_eventos.Rows.Add(rowEventos);
                    }
                }
                if (procuraduria[0].inhabilidades != null)
                {
                    foreach (var jsonObject in procuraduria[0].inhabilidades)
                    {
                        var rowInhabilidad = tabla_procuraduria_inhabilidades.NewRow();

                        rowInhabilidad["Siri"] = DecodeFromUtf8(jsonObject.siri.ToString());
                        rowInhabilidad["Modulo"] = DecodeFromUtf8(jsonObject.modulo.ToString());
                        rowInhabilidad["Inhabilidad_legal"] = jsonObject.inhabilidad_legal.ToString();
                        rowInhabilidad["Fecha_inicio"] = DecodeFromUtf8(jsonObject.fecha_inicio.ToString());
                        rowInhabilidad["Fecha_fin"] = DecodeFromUtf8(jsonObject.fecha_fin.ToString());
                        rowInhabilidad["Suspension_art"] = jsonObject.Suspension_art != null ? DecodeFromUtf8(jsonObject.Suspension_art.ToString()) : null;
                        tabla_procuraduria_inhabilidades.Rows.Add(rowInhabilidad);
                    }
                }

                if (procuraduria[0].investiduras != null)
                {
                    foreach (var jsonObject in procuraduria[0].investiduras)
                    {
                        var rowPerdidaI = tabla_perdida_investidura.NewRow();

                        rowPerdidaI["Sancion"] = DecodeFromUtf8(jsonObject.sancion.ToString());
                        rowPerdidaI["Termino"] = DecodeFromUtf8(jsonObject.termino.ToString());
                        rowPerdidaI["Clase_sancion"] = DecodeFromUtf8(jsonObject.clase_sancion.ToString());
                        rowPerdidaI["Entidad"] = DecodeFromUtf8(jsonObject.entidad.ToString());
                        rowPerdidaI["Cargo"] = DecodeFromUtf8(jsonObject.cargo.ToString());
                        tabla_perdida_investidura.Rows.Add(rowPerdidaI);
                    }
                }

                e.DataSources.Add(new ReportDataSource("procuraduria_sanciones", tabla_procuraduria_sanciones));
                e.DataSources.Add(new ReportDataSource("procuraduria_delitos", tabla_procuraduria_delitos));
                e.DataSources.Add(new ReportDataSource("procuraduria_instancias", tabla_procuraduria_instancias));
                e.DataSources.Add(new ReportDataSource("procuraduria_eventos", tabla_procuraduria_eventos));
                e.DataSources.Add(new ReportDataSource("procuraduria_inhabilidades", tabla_procuraduria_inhabilidades));
                e.DataSources.Add(new ReportDataSource("perdida_investidura", tabla_perdida_investidura));
            }
            catch (Exception ex)
            {

                throw;
            }

            
        }
        protected string DecodeFromUtf8(string utf8String)
        {
            byte[] utf8Bytes = new byte[utf8String.Length];
            for (int i = 0; i < utf8String.Length; ++i)
            {
                utf8Bytes[i] = (byte)utf8String[i];
            }

            return Encoding.UTF8.GetString(utf8Bytes, 0, utf8Bytes.Length);
        }
        protected DataTable reporteCompletoGetRowTablesRama()
        {
            DataTable tabla_listas_rama_judicial_new = new DataTable();
            tabla_listas_rama_judicial_new.Columns.Add(new DataColumn("llaveProceso"));
            tabla_listas_rama_judicial_new.Columns.Add(new DataColumn("fechaProceso"));
            tabla_listas_rama_judicial_new.Columns.Add(new DataColumn("fechaUltimaActuacion"));
            tabla_listas_rama_judicial_new.Columns.Add(new DataColumn("despacho"));
            tabla_listas_rama_judicial_new.Columns.Add(new DataColumn("departamento"));
            tabla_listas_rama_judicial_new.Columns.Add(new DataColumn("sujetosProcesales"));
            return tabla_listas_rama_judicial_new;
        }
        public static string StringBetween(string STR, string FirstString, string LastString)
        {
            string FinalString;
            int Pos1 = STR.IndexOf(FirstString) + FirstString.Length;
            int Pos2 = STR.IndexOf(LastString);
            FinalString = STR.Substring(Pos1, Pos2 - Pos1);
            return FinalString;
        }
        private Object[] createPdf(LocalReport report, HttpResponse response)
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            byte[] bytes = report.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            PdfDocument document = new PdfDocument();
            PdfDocument tempPDFDoc = PdfReader.Open(new MemoryStream(bytes), PdfDocumentOpenMode.Import);

            for (int i = 0; i < tempPDFDoc.PageCount; i++)
            {
                PdfPage page = tempPDFDoc.Pages[i];
                document.AddPage(page);
            }

            XFont font = new XFont("Verdana", 8);
            XBrush brush = XBrushes.Black;

            // Add the page counter.
            string noPages = document.Pages.Count.ToString();
            for (int i = 0; i < document.Pages.Count; ++i)
            {
                PdfPage page = document.Pages[i];

                // Make a layout rectangle.
                XRect layoutRectangle = new XRect(0/*X*/, page.Height - font.Height/*Y*/, page.Width/*Width*/, font.Height/*Height*/);

                using (XGraphics gfx = XGraphics.FromPdfPage(page))
                {
                    gfx.DrawString(
                        "Pagina " + (i + 1).ToString() + " de " + noPages,
                        font,
                        brush,
                    layoutRectangle,
                        XStringFormats.Center);
                }
            }

            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);
            var options = document.Options;
            byte[] buffer = new byte[0];
            buffer = stream.ToArray();
            var contentLength = buffer.Length;
            response.Clear();
            response.ContentType = "application/pdf";
            stream.Close();
            string namepdf = "Reporte_" + System.DateTime.Now.ToString().Replace(" ", "_").Replace(",", "").Replace(".", "").Replace(":", "").Replace("/", "-");
            Object[] objects = new Object[4];
            objects[0] = bytes;
            objects[1] = mimeType;
            objects[2] = namepdf;
            objects[3] = extension;
            return objects;
        }
    }
}
