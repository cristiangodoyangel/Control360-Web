using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario360.Web.Models
{
    [Table("SalidaDeBodega")]
    public class SalidaDeBodega
    {
        [Key]
        public int ID { get; set; }

        public DateTime? Fecha { get; set; }

        public int? Solicitante { get; set; }

        [ForeignKey("Solicitante")]
        public virtual Empleado SolicitanteObj { get; set; }

        public int? ResponsableEntrega { get; set; }

        [ForeignKey("ResponsableEntrega")]
        public virtual Empleado ResponsableEntregaObj { get; set; }

        public int? ProyectoAsignado { get; set; }

        [ForeignKey("ProyectoAsignado")]
        public virtual Proyecto ProyectoObj { get; set; }

        public virtual ICollection<DetalleSalidaDeBodega> Detalles { get; set; }

        public SalidaDeBodega()
        {
            Detalles = new HashSet<DetalleSalidaDeBodega>();
        }
    }
}
