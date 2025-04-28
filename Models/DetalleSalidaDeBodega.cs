using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario360.Web.Models
{
    [Table("DetalleSalidaDeBodega")]
    public class DetalleSalidaDeBodega
    {
        [Key]
        public int ID { get; set; }

        public int SalidaDeBodegaID { get; set; }

        [ForeignKey("SalidaDeBodegaID")]
        public virtual SalidaDeBodega SalidaDeBodega { get; set; }

        public int ProductoID { get; set; }

        [ForeignKey("ProductoID")]
        public virtual Producto Producto { get; set; }

        public int Cantidad { get; set; }

        [NotMapped]
        public string Categoria
        {
            get { return Producto != null ? Producto.Categoria : null; }
        }
    }
}
