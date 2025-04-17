using System.ComponentModel.DataAnnotations;

namespace Inventario360.Web.Models
{
    public class Proyecto
    {
        [Key]
        public int ID { get; set; } // Clave primaria

        [StringLength(255)]
        public string Nombre { get; set; } // varchar(255), permite NULL

        public string Descripcion { get; set; } // text, permite NULL
    }
}
