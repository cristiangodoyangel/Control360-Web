using System.Collections.Generic;

namespace Inventario360.Web.Models.ViewModels
{
    public class RolesViewModel
    {
        public string UserId { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public List<string> RolesAsignados { get; set; } = new List<string>();
        public List<string> TodosLosRoles { get; set; } = new List<string>();
    }
}
