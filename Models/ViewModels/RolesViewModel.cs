using System.Collections.Generic;

namespace Inventario360.Web.ViewModels
{
    public class RolesViewModel
    {
        public string UserId { get; set; }
        public string NombreUsuario { get; set; }  // ← esta propiedad estaba faltando
        public string Email { get; set; }
        public List<string> RolesAsignados { get; set; } = new List<string>();
        public List<string> TodosLosRoles { get; set; } = new List<string>();
    }
}
