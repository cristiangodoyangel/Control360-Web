using System.ComponentModel.DataAnnotations;

namespace Inventario360.Web.ViewModels
{
    public class PerfilViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo electrónico no válido")]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }

        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }
    }
}
