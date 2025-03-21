using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class Empresa
    {
        public Empresa()
        {
            Consultum1s = new HashSet<Consultum1>();
            CorreosDestinos = new HashSet<CorreosDestino>();
            CorreosEnviados = new HashSet<CorreosEnviado>();
            EmpresaTipoLista = new HashSet<EmpresaTipoLista>();
            ListaBlancas = new HashSet<ListaBlanca>();
            SubmenusRolesEmpresas = new HashSet<SubmenusRolesEmpresa>();
            TipoTerceros = new HashSet<TipoTercero>();
            Usuario = new HashSet<Usuario>();
        }

        public int IdEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public bool Bloqueado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string NumeroDocumento { get; set; }
        public int IdPlan { get; set; }
        public DateTime? FechaContrato { get; set; }
        public string DuracionMeses { get; set; }
        public string CorreoElectronico { get; set; }
        public int IdUsuario { get; set; }
        public bool RenovacionAut { get; set; }
        public Guid? IdUniqueEmpresa { get; set; }
        public byte[] Imagen { get; set; }
        public int? Consecutivo { get; set; }
        public bool? HasMasivaProcuraduria { get; set; }

        public virtual ICollection<Consultum1> Consultum1s { get; set; }
        public virtual ICollection<CorreosDestino> CorreosDestinos { get; set; }
        public virtual ICollection<CorreosEnviado> CorreosEnviados { get; set; }
        public virtual ICollection<EmpresaTipoLista> EmpresaTipoLista { get; set; }
        public virtual ICollection<ListaBlanca> ListaBlancas { get; set; }
        public virtual ICollection<SubmenusRolesEmpresa> SubmenusRolesEmpresas { get; set; }
        public virtual ICollection<TipoTercero> TipoTerceros { get; set; }
        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
