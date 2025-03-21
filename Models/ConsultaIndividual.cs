using System;
using System.Collections.Generic;

namespace Inspektor_API_REST.Models
{
    public class ConsultaIndividual
    {
        public decimal IdConsulta { get; set; }
        public decimal IdConsultaEmpresa { get; set; }
        public int IdEmpresa { get; set; }
        public string? Identification { get; set; }
        public string? Name { get; set; }        
        public int IdTipoTercero { get; set; }
        public Boolean HasProioridad4 { get; set; }
        public Boolean HasProcuraduria { get; set; }        
        public string? TipoDocumentoProcuraduria { get; set; }
        public Consultas? Consulta { get; set; }
        public IEnumerable<TipoTerceros>? tipoTerceros { get; set; }
        public IEnumerable<Listas>? listas_restrictivas { get; set; }
        public IEnumerable<Listas>? listas_LAFT_Admin { get; set; }
        public IEnumerable<Listas>? listas_LAFT_Penal { get; set; }
        public IEnumerable<Listas>? listas_Sanciones_Afectacion { get; set; }
        public IEnumerable<Listas>? listas_Peps { get; set; }
        public IEnumerable<ListasPropias>? listas_Propias { get; set; }
        public Procuraduria? Procuraduria { get; set; }        
    }
}
