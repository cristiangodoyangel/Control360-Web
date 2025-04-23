using System.Data.Entity;
using Inventario360.Web.Models;

namespace Inventario360.Web.Data
{
    public class InventarioDbContext : DbContext
    {
        public InventarioDbContext() : base("DefaultConnection") { }

        public DbSet<Producto> Producto { get; set; }

        /*public DbSet<SalidaDeBodega> SalidaDeBodega { get; set; }
        public DbSet<DetalleSalidaDeBodega> DetalleSalidaDeBodega { get; set; }
       
        public DbSet<SolicitudDeMaterial> SolicitudDeMaterial { get; set; }
        public DbSet<Empleado> Empleado { get; set; }
        public DbSet<Proveedor> Proveedor { get; set; }
        public DbSet<Proyecto> Proyecto { get; set; }
        public DbSet<FichaEmpleado> FichaEmpleado { get; set; }
        public DbSet<Camioneta> Camionetas { get; set; }
        public DbSet<FichaCamioneta> FichaCamionetas { get; set; }*/

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Relaciones personalizadas si las necesitas
           /* modelBuilder.Entity<SalidaDeBodega>()
                .HasRequired(s => s.SolicitanteObj)
                .WithMany()
                .HasForeignKey(s => s.Solicitante);

            modelBuilder.Entity<SalidaDeBodega>()
                .HasRequired(s => s.ResponsableEntregaObj)
                .WithMany()
                .HasForeignKey(s => s.ResponsableEntrega);

            modelBuilder.Entity<SalidaDeBodega>()
                .HasRequired(s => s.ProyectoObj)
                .WithMany()
                .HasForeignKey(s => s.ProyectoAsignado);*/
        }
    }
}
