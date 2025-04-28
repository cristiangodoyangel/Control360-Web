using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario360.Web.Models
{
    [Table("Proyecto")]
    public class Proyecto
    {
        [Key]
        public int ID { get; set; }

        [StringLength(255)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }
    }
}
