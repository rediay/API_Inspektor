using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class UsuariosLog
    {
        public int IdUsuarioLog { get; set; }
        public int IdUsuario { get; set; }
        public int IdEmpresa { get; set; }
        public int IdRol { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public bool Bloqueado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool? WsUser { get; set; }
        public string CorreoElectronico { get; set; }
        public bool ResetContrasena { get; set; }
        public bool Terminos { get; set; }
        public string GetIp { get; set; }
        public Guid? IdUniqueUsuario { get; set; }
        public DateTime? FechaLog { get; set; }
    }
}
