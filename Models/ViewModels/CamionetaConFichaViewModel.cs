using System.Web.Mvc;
using Inventario360.Web.Models;

namespace Inventario360.Web.Models.ViewModels
{
    public class CamionetaConFichaViewModel
    {
        public Camioneta Camioneta { get; set; }
        public FichaCamioneta FichaCamioneta { get; set; }
        public SelectList Empleados { get; set; }
    }
}
