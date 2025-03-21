using System;
using Inspektor_API_REST.Models.Auth;
using Inspektor_API_REST.Models.ServicesAdditional;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class listasrestrictivas_riskconsultingcolombia_comContext : DbContext
    {
        public listasrestrictivas_riskconsultingcolombia_comContext()
        {
        }

        public listasrestrictivas_riskconsultingcolombia_comContext(DbContextOptions<listasrestrictivas_riskconsultingcolombia_comContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Consultas> Consultas { get; set; }
        public virtual DbSet<Listas> Listas { get; set; }
        public virtual DbSet<TipoLista> TipoLista { get; set; }
        public virtual DbSet<GrupoLista> GrupoLista { get; set; }
        public virtual DbSet<ListasPropias> ListasPropias { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<Empresas> Empresas { get; set; }
        public virtual DbSet<DetalleConsulta> DetalleConsulta { get; set; }
        public virtual DbSet<TipoTerceros> TipoTerceros { get; set; }
        public virtual DbSet<Servicios> Servicios { get; set; }
        public virtual DbSet<TokenJWTUsuario> TokenJWTUsuarios { get; set; }
        public virtual DbSet<CorreosDestino> CorreosDestinos { get; set; }
        public virtual DbSet<CorreosEnviado> CorreosEnviados { get; set; }
        public virtual DbSet<DatosCantidadConsultas> DatosCantidadConsultas { get; set; }
        public virtual DbSet<LogAdditionalServices> LogAdditionalServices { get; set; }
        public virtual DbSet<Permisos> Permisos { get; set; }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
