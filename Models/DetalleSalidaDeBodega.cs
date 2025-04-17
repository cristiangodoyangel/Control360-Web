using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Inventario360.Models;

namespace Inventario360.Web.Models
{
    public class DetalleSalidaDeBodega
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("SalidaDeBodega")]
        public int SalidaDeBodegaID { get; set; }
        public virtual SalidaDeBodega SalidaDeBodega { get; set; } // Propiedad de navegación

        [ForeignKey("Producto")]
        public int ProductoID { get; set; }
        public virtual Producto Producto { get; set; } // Relación con Producto

        public int Cantidad { get; set; }

        // Propiedad auxiliar no mapeada a base de datos
        [NotMapped]
        public string Categoria
        {
            get { return Producto != null ? Producto.Categoria : string.Empty; }
        }
    }
}
