using System.ComponentModel.DataAnnotations;

namespace Inventario360.Web.Models
{
    public class Empleado
    {
        [Key]
        public int ID { get; set; } // Clave primaria

        [StringLength(255)]
        public string Nombre { get; set; } // varchar(255), permite NULL

        [StringLength(255)]
        public string Cargo { get; set; } // varchar(255), permite NULL
    }
}
