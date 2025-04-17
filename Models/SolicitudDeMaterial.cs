using System;
using System.ComponentModel.DataAnnotations;

namespace Inventario360.Web.Models
{
    public class SolicitudDeMaterial
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int ITEM { get; set; }  // Relación con Producto (si aplica)

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [StringLength(255)]
        public string NombreTecnico { get; set; }

        [StringLength(100)]
        public string Medida { get; set; }

        [StringLength(100)]
        public string UnidadMedida { get; set; }

        [StringLength(100)]
        public string Marca { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [StringLength(255)]
        public string Imagen { get; set; }

        [StringLength(255)]
        public string PosibleProveedor { get; set; }

        [Required]
        [StringLength(255)]
        public string Solicitante { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; }

        [StringLength(50)]
        public string Estado { get; set; }

        public SolicitudDeMaterial()
        {
            Fecha = DateTime.Now;
            Estado = "Pendiente";
        }
    }
}
