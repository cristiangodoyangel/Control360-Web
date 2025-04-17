using System;
using System.ComponentModel.DataAnnotations;

namespace Inventario360.Web.Models
{
    public class Reporte
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Nombre { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [StringLength(1000)]
        public string Descripcion { get; set; }
    }
}
