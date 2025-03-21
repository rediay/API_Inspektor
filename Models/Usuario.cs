using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Consulta = new HashSet<Consultum>();
            Consultum1s = new HashSet<Consultum1>();
            EmpresaTipoLista = new HashSet<EmpresaTipoLista>();
            Planes = new HashSet<Plane>();
            TipoDocumentos = new HashSet<TipoDocumento>();
            TipoTerceros = new HashSet<TipoTercero>();
            TokenJwts = new HashSet<TokenJWTUsuario>();
        }

        public int IdUsuario { get; set; }
        public int IdEmpresa { get; set; }
        public int IdRol { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Usuario1 { get; set; }
        public string Contrasena { get; set; }
        public bool Bloqueado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool? WsUser { get; set; }
        public string CorreoElectronico { get; set; }
        public bool ResetContrasena { get; set; }
        public bool Terminos { get; set; }
        public Guid? IdUniqueUsuario { get; set; }
        public string GetIp { get; set; }
        public DateTime? FechaContrasena { get; set; }
        public byte[] PasswordHash { get; set; }
        public bool HasNewContrasena { get; set; }
        public byte? CambiarEmpresa { get; set; }

        public virtual Empresa IdEmpresaNavigation { get; set; }
        public virtual Role IdRolNavigation { get; set; }
        public virtual ICollection<Consultum> Consulta { get; set; }
        public virtual ICollection<Consultum1> Consultum1s { get; set; }
        public virtual ICollection<EmpresaTipoLista> EmpresaTipoLista { get; set; }
        public virtual ICollection<Plane> Planes { get; set; }
        public virtual ICollection<TipoDocumento> TipoDocumentos { get; set; }
        public virtual ICollection<TipoTercero> TipoTerceros { get; set; }
        public virtual ICollection<TokenJWTUsuario> TokenJwts { get; set; }
    }
}
