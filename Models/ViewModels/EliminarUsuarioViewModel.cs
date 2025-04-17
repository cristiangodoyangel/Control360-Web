using System.ComponentModel.DataAnnotations;

namespace Inventario360.Web.ViewModels
{
    public class EliminarUsuarioViewModel
    {
        [Required]
        public string Id { get; set; }
    }
}
