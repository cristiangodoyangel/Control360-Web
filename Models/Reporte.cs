using System;

namespace Inventario360.Web.Models
{
    public class Reporte
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
    }
}
