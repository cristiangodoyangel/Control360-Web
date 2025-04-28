using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario360.Web.Models
{
    [Table("Empleado")]
    public class Empleado
    {
        [Key]
        public int ID { get; set; }

        [StringLength(255)]
        public string Nombre { get; set; }

        [StringLength(255)]
        public string Cargo { get; set; }
    }
}
