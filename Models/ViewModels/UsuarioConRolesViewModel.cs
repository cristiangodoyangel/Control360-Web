using System.Collections.Generic;

namespace Inventario360.Web.Models.ViewModels
{
    public class UsuarioConRolesViewModel
    {
        public string Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
